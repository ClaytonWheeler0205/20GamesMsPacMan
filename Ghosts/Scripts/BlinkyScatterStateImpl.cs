using Godot;
using System;
using Util.ExtensionMethods;

namespace Game.Ghosts
{

    public class BlinkyScatterStateImpl : BlinkyScatterState
    {
        private bool _inFirstRoundOfLevel = true;
        private bool _inElroyPhaseOne = false;
        private bool _inElroyPhaseTwo = false;
        private bool _clydeReleased = false;
        private float _elroyPhaseOneSpeed = 0.8f;
        private float _elroyPhaseTwoSpeed = 0.85f;
        // Priority is Up, Left, Down, Right
        private static readonly Vector2[] _movementDirections = { Vector2.Up, Vector2.Left, Vector2.Down, Vector2.Right };

        public override void EnterState()
        {
            if (InElroyPhaseTwo())
            {
                EmitSignal("SpeedChangeRequested", Movement.BaseSpeed * _elroyPhaseTwoSpeed);
            }
            else if (InElroyPhaseOne())
            {
                EmitSignal("SpeedChangeRequested", Movement.BaseSpeed * _elroyPhaseOneSpeed);
            }
            else
            {
                EmitSignal("SpeedChangeRequested", Movement.BaseSpeed * SpeedupFactor);
            }
        }

        private bool InElroyPhaseOne()
        {
            return (_inFirstRoundOfLevel && _inElroyPhaseOne) || (!_inFirstRoundOfLevel && _inElroyPhaseOne && _clydeReleased);
        }

        private bool InElroyPhaseTwo()
        {
            return (_inFirstRoundOfLevel && _inElroyPhaseTwo) || (!_inFirstRoundOfLevel && _inElroyPhaseTwo && _clydeReleased);
        }

        public override void UpdateState(float delta)
        {
            if (GhostCollision.Vulnerable)
            {
                EmitSignal("Transitioned", this, "FrightenedState");
            }
            else if (!ScatterChaseTracker.Instance.InScatterState)
            {
                EmitSignal("Transitioned", this, "ChaseState");
            }
            else
            {
                if (CurrentLevel.IsValid())
                {
                    Vector2 positionInLevel = CurrentLevel.GetPositionInLevel(Movement.BodyToMove);
                    if (CurrentLevel.IsAtIntersectionTile(positionInLevel) && !InIntersectionTile)
                    {
                        InIntersectionTile = true;
                        if (InElroyPhaseOne() || InElroyPhaseTwo())
                        {
                            Movement.ChangeDirection(FindShortestPathToPlayer(positionInLevel));
                        }
                        else
                        {
                            Movement.ChangeDirection(FindShortestPathToHome(positionInLevel));
                        }
                    }
                    if (!CurrentLevel.IsAtIntersectionTile(positionInLevel))
                    {
                        InIntersectionTile = false;
                    }
                }
            }
        }

        private Vector2 FindShortestPathToPlayer(Vector2 ghostPosition)
        {
            Vector2 playerMapPosition = CurrentLevel.GetPositionInLevel(Player);
            float minDistance = float.PositiveInfinity;
            Vector2 newDirection = Vector2.Zero;
            Vector2 currentDirection = Movement.GetCurrentDirection();

            foreach (Vector2 direction in _movementDirections)
            {
                if (currentDirection != (-1 * direction) && CurrentLevel.IsAtPathTile(ghostPosition + direction))
                {
                    float distance = playerMapPosition.DistanceTo(ghostPosition + direction);
                    if (distance < minDistance)
                    {
                        newDirection = direction;
                        minDistance = distance;
                    }
                }
            }

            return newDirection;
        }

        public override void ResetTileDetection()
        {
            InIntersectionTile = false;
        }

        public override void IncreaseScatterSpeed()
        {
            if (SpeedupFactor >= 0.85f)
            {
                SpeedupFactor = 0.95f;
                _elroyPhaseOneSpeed = 1.0f;
                _elroyPhaseTwoSpeed = 1.05f;
            }
            else if (SpeedupFactor >= 0.75f)
            {
                SpeedupFactor = 0.85f;
                _elroyPhaseOneSpeed = 0.9f;
                _elroyPhaseTwoSpeed = 0.95f;
            }
        }

        public override void IncreaseElroyLevel()
        {
            if (_inElroyPhaseOne)
            {
                _inElroyPhaseTwo = true;
            }
            else
            {
                _inElroyPhaseOne = true;
            }
        }

        public override void ResetElroyLevel()
        {
            _inElroyPhaseOne = false;
            _inElroyPhaseTwo = false;
            _inFirstRoundOfLevel = true;
            _clydeReleased = false;
            Movement.Speed = Movement.BaseSpeed * SpeedupFactor;
        }

        public override void ApplyElroySpeed()
        {
            _clydeReleased = false;
            _inFirstRoundOfLevel = false;
        }
    }
}