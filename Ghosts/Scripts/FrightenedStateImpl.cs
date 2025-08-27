using Godot;
using Util;
using Util.ExtensionMethods;

namespace Game.Ghosts
{

    public class FrightenedStateImpl : FrightenedState
    {
        private bool _inIntersectionTile = false;
        private float _slowSpeedFactor = 0.5f;

        public override void EnterState()
        {
            EmitSignal("SpeedChangeRequested", Movement.BaseSpeed * _slowSpeedFactor);
        }

        public override void UpdateState(float delta)
        {
            if (GhostCollision.Fleeing)
            {
                EmitSignal("Transitioned", this, "ReturnState");
            }
            else if (!GhostCollision.Vulnerable)
            {
                if (ScatterChaseTracker.Instance.InScatterState)
                {
                    EmitSignal("Transitioned", this, "ScatterState");
                }
                else
                {
                    EmitSignal("Transitioned", this, "ChaseState");
                }
            }
            else
            {
                if (CurrentLevel.IsValid())
                {
                    Vector2 positionInLevel = CurrentLevel.GetPositionInLevel(Movement.BodyToMove);
                    if (CurrentLevel.IsAtIntersectionTile(positionInLevel) && !_inIntersectionTile)
                    {
                        _inIntersectionTile = true;
                        Movement.ChangeDirection(GetRandomDirection(positionInLevel));
                    }
                    if (!CurrentLevel.IsAtIntersectionTile(positionInLevel))
                    {
                        _inIntersectionTile = false;
                    }
                }
            }
        }

        private Vector2 GetRandomDirection(Vector2 ghostPosition)
        {
            Vector2 newDirection = Vector2.Zero;
            bool directionFound = false;

            while (!directionFound)
            {
                newDirection = PickRandomDirection();
                directionFound = CurrentLevel.IsAtPathTile(ghostPosition + newDirection);
            }
            return newDirection;
        }

        private Vector2 PickRandomDirection()
        {
            Vector2 newDirection = Vector2.Zero;
            int randomIndex = GDRandom.RandiRange(1, 4);
            switch (randomIndex)
            {
                case 1:
                    newDirection = Vector2.Up;
                    break;
                case 2:
                    newDirection = Vector2.Left;
                    break;
                case 3:
                    newDirection = Vector2.Down;
                    break;
                case 4:
                    newDirection = Vector2.Right;
                    break;
            }
            return newDirection;
        }

        public override void ExitState()
        {
            GhostCollision.Vulnerable = false;
            _inIntersectionTile = false;
        }

        public override float GetStateSpeed()
        {
            return Movement.BaseSpeed * _slowSpeedFactor;
        }

        public override void ResetTileDetection()
        {
            _inIntersectionTile = false;
        }

        public override void IncreaseFrightenSpeed()
        {
            if (_slowSpeedFactor >= 0.55f)
            {
                _slowSpeedFactor = 0.6f;
            }
            else if (_slowSpeedFactor >= 0.5f)
            {
                _slowSpeedFactor = 0.55f;
            }
        }
    }
}