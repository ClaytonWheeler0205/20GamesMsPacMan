using Game.Levels;
using Game.Player;

namespace Game.Ghosts
{

    public abstract class GhostImpl : Ghost
    {
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
        }

        public override void SetPlayerReference(MsPacMan player)
        {
            ChaseStateReference.Player = player;
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
            if (!GhostCollision.Fleeing)
            {
                previousDirection = MovementReference.GetCurrentDirection();
                StopGhost();
            }
        }

        public override void ResumeGhost()
        {
            if (!GhostCollision.Fleeing)
            {
                MovementReference.OverrideDirection(previousDirection);
                StateMachineReference.SetIsMachineActive(true);
            }
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
            BodyVisual.Play(MOVE_ANIMATION);
        }

        public override void SetGhostFlash()
        {
            if (GhostCollision.Vulnerable)
            {
                FrightenedFlashVisual.Visible = !FrightenedFlashVisual.Visible;
            }
        }
    }
}