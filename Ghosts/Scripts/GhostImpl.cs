using Game.Levels;
using Game.Player;
using Godot;
using Game.Bus;

namespace Game.Ghosts
{

    public abstract class GhostImpl : Ghost
    {
        private float _ghostTunnelSpeedFactor = 0.4f;
        private int _tunnelsEntered = 0;
        private bool _isInTunnel = false;
        private bool _shouldBePaused = false;

        public override void StopGhost()
        {
            MovementReference.StopMoving();
            StateMachineReference.SetIsMachineActive(false);
        }

        public override void ResetGhost()
        {
            GhostCollision.Vulnerable = false;
            GhostCollision.Fleeing = false;
            ReturnGhostVisuals();
            GlobalPosition = startPosition;
            StateMachineReference.ResetMachine();
            _isInTunnel = false;
            _shouldBePaused = false;
        }

        public override void SetPlayerReference(MsPacMan player)
        {
            ChaseStateReference.Player = player;
            if (ScatterStateReference is BlinkyScatterState blinkyScatterState)
            {
                blinkyScatterState.Player = player;
            }
        }

        public override void SetLevelReference(Level level)
        {
            ScatterStateReference.CurrentLevel = level;
            ChaseStateReference.CurrentLevel = level;
            FrightenedStateReference.CurrentLevel = level;
            ReturnStateReference.CurrentLevel = level;
            ReturnStateReference.GhostHouseTilePosition = level.GhostHousePosition;
        }

        public override void PauseGhost()
        {
            previousDirection = MovementReference.GetCurrentDirection();
            if (!GhostCollision.Fleeing)
            {
                StopGhost();
            }
            _shouldBePaused = true;
        }

        public override void ResumeGhost()
        {
            if (!GhostCollision.Fleeing)
            {
                MovementReference.OverrideDirection(previousDirection);
                StateMachineReference.SetIsMachineActive(true);
                ScatterStateReference.ResetTileDetection();
                ChaseStateReference.ResetTileDetection();
                FrightenedStateReference.ResetTileDetection();
                ReturnStateReference.ResetTileDetection();
            }
            _shouldBePaused = false;
        }

        public override void SetGhostVulnerability()
        {
            if (!GhostCollision.Fleeing)
            {
                GhostCollision.Vulnerable = true;
                TurnGhostsBlue();
            }
        }

        private void TurnGhostsBlue()
        {
            BodyVisual.Stop();
            BodyVisual.Visible = false;
            Eyes.Visible = false;
            FrightenedBodyVisual.Frame = BodyVisual.Frame;
            FrightenedFlashVisual.Frame = BodyVisual.Frame;
            FrightenedBodyVisual.Visible = true;
            FrightenedFlashVisual.Visible = false;
            FrightenedBodyVisual.Play(FRIGHTENED_MOVE_ANIMATION);
            FrightenedFlashVisual.Play(FRIGHTENED_FLASH_MOVE_ANIMATION);
        }

        public override void SetGhostFleeing()
        {
            GhostCollision.Vulnerable = false;
            GhostCollision.Fleeing = true;
            SetGhostFleeingVisuals();
        }

        private void SetGhostFleeingVisuals()
        {
            FrightenedBodyVisual.Stop();
            FrightenedFlashVisual.Stop();
            FrightenedBodyVisual.Visible = false;
            FrightenedFlashVisual.Visible = false;
            Eyes.Visible = true;
        }

        public override void SetGhostInvulnerable()
        {
            if (!GhostCollision.Fleeing)
            {
                GhostCollision.Vulnerable = false;
                ReturnGhostVisuals();
            }
        }

        private void ReturnGhostVisuals()
        {
            FrightenedBodyVisual.Stop();
            FrightenedFlashVisual.Stop();
            FrightenedBodyVisual.Visible = false;
            FrightenedFlashVisual.Visible = false;
            BodyVisual.Frame = FrightenedBodyVisual.Frame;
            BodyVisual.Visible = true;
            Eyes.Visible = true;
        }

        public override void SetGhostFlash()
        {
            if (GhostCollision.Vulnerable)
            {
                FrightenedFlashVisual.Visible = !FrightenedFlashVisual.Visible;
            }
        }

        public override void SlowDownGhost()
        {
            _tunnelsEntered++;
            if (_tunnelsEntered > 0)
            {
                MovementReference.Speed = MovementReference.BaseSpeed * _ghostTunnelSpeedFactor;
                _isInTunnel = true;
            }
        }

        public override void SpeedupGhost()
        {
            _tunnelsEntered--;
            if (_tunnelsEntered == 0)
            {
                MovementReference.Speed = StateMachineReference.GetCurrentState().GetStateSpeed();
                _isInTunnel = false;
            }
        }

        public override void IncreaseGhostSpeed()
        {
            ChaseStateReference.IncreaseChaseSpeed();
            ScatterStateReference.IncreaseScatterSpeed();
            FrightenedStateReference.IncreaseFrightenSpeed();
            ReturnStateReference.IncreaseReturnExitSpeed();
            IncreaseTunnelSpeed();
        }

        private void IncreaseTunnelSpeed()
        {
            if (_ghostTunnelSpeedFactor >= 0.45f)
            {
                _ghostTunnelSpeedFactor = 0.5f;
            }
            else if (_ghostTunnelSpeedFactor >= 0.4f)
            {
                _ghostTunnelSpeedFactor = 0.45f;
            }
        }

        public override void OnSpeedChangeRequested(float newSpeed)
        {
            if (!_isInTunnel)
            {
                MovementReference.Speed = newSpeed;
            }
        }

        public override void OnDirectionChanged(Vector2 newDirection)
        {
            if (BodyVisual.Visible)
            {
                BodyVisual.Play("move");
            }
            else if (FrightenedBodyVisual.Visible)
            {
                FrightenedBodyVisual.Play("frightened_move");
                FrightenedFlashVisual.Frame = FrightenedBodyVisual.Frame;
                FrightenedFlashVisual.Play("frightened_flash_move");
            }
            if (newDirection == Vector2.Up)
            {
                Eyes.Play("look_up");
            }
            else if (newDirection == Vector2.Down)
            {
                Eyes.Play("look_down");
            }
            else if (newDirection == Vector2.Left)
            {
                Eyes.Play("look_left");
            }
            else if (newDirection == Vector2.Right)
            {
                Eyes.Play("look_right");
            }
        }

        public override void OnMovementStopped()
        {
            BodyVisual.Stop();
            FrightenedBodyVisual.Stop();
            FrightenedFlashVisual.Stop();
        }

        public override void OnGhostEaten()
        {
            Visible = false;
            GhostEventBus.Instance.EmitSignal("GhostEaten", this);
        }

        public override void OnReturnStateEntered()
        {
            BodyVisual.Stop();
            BodyVisual.Visible = false;
            GhostCollision.Fleeing = true;
        }

        public override void OnGhostHouseEntered()
        {
            BodyVisual.Visible = true;
            BodyVisual.Play("move");
            GhostCollision.Fleeing = false;
            if (_shouldBePaused)
            {
                PauseGhost();
            }
        }

        public override void OnReturnStateExited()
        {
            BodyVisual.Visible = true;
            GhostCollision.Fleeing = false;
        }
    }
}