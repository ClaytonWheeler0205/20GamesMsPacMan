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
                    if (IntersectionDetector.IsAtIntersection(Movement.BodyToMove.GlobalPosition) && !InIntersectionTile)
                    {
                        InIntersectionTile = true;
                        if (InElroyPhaseOne() || InElroyPhaseTwo())
                        {
                            Movement.ChangeDirection(FindShortestPathToPlayer());
                        }
                        else
                        {
                            Movement.ChangeDirection(FindShortestPathToHome());
                        }
                    }
                    if (!IntersectionDetector.IsAtIntersection(Movement.BodyToMove.GlobalPosition))
                    {
                        InIntersectionTile = false;
                    }
                }
            }
        }

        private Vector2 FindShortestPathToPlayer()
        {
            Vector2 localPosition = CurrentLevel.ToLocal(Movement.BodyToMove.GlobalPosition);
            Vector2 mapPosition = CurrentLevel.WorldToMap(localPosition);
            Vector2 localPlayerPosition = CurrentLevel.ToLocal(Player.GlobalPosition);
            Vector2 playerMapPosition = CurrentLevel.WorldToMap(localPlayerPosition);
            int cellNumber = CurrentLevel.GetCell((int)mapPosition.x, (int)mapPosition.y);
            float minDistance = float.PositiveInfinity;
            Vector2 newDirection = Vector2.Zero;
            Vector2 currentDirection = Movement.GetCurrentDirection();

            // Priority is Up, Left, Down, Right
            if (currentDirection != Vector2.Down && cellNumber != SpecialTurnTileCellNumber)
            {
                Vector2 mapPositionUp = new Vector2(mapPosition.x, mapPosition.y - 1);
                int cellNumberUp = CurrentLevel.GetCell((int)mapPositionUp.x, (int)mapPositionUp.y);
                if (cellNumberUp == PathTileCellNumber)
                {
                    float distance = playerMapPosition.DistanceTo(mapPositionUp);
                    if (distance < minDistance)
                    {
                        newDirection = Vector2.Up;
                        minDistance = distance;
                    }
                }
            }
            if (currentDirection != Vector2.Right)
            {
                Vector2 mapPositionLeft = new Vector2(mapPosition.x - 1, mapPosition.y);
                int cellNumberLeft = CurrentLevel.GetCell((int)mapPositionLeft.x, (int)mapPositionLeft.y);
                if (cellNumberLeft == PathTileCellNumber)
                {
                    float distance = playerMapPosition.DistanceTo(mapPositionLeft);
                    if (distance < minDistance)
                    {
                        newDirection = Vector2.Left;
                        minDistance = distance;
                    }
                }
            }
            if (currentDirection != Vector2.Up)
            {
                Vector2 mapPositionDown = new Vector2(mapPosition.x, mapPosition.y + 1);
                int cellNumberDown = CurrentLevel.GetCell((int)mapPositionDown.x, (int)mapPositionDown.y);
                if (cellNumberDown == PathTileCellNumber)
                {
                    float distance = playerMapPosition.DistanceTo(mapPositionDown);
                    if (distance < minDistance)
                    {
                        newDirection = Vector2.Down;
                        minDistance = distance;
                    }
                }
            }
            if (currentDirection != Vector2.Left)
            {
                Vector2 mapPositionRight = new Vector2(mapPosition.x + 1, mapPosition.y);
                int cellNumberRight = CurrentLevel.GetCell((int)mapPositionRight.x, (int)mapPositionRight.y);
                if (cellNumberRight == PathTileCellNumber)
                {
                    float distance = playerMapPosition.DistanceTo(mapPositionRight);
                    if (distance < minDistance)
                    {
                        newDirection = Vector2.Right;
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
            };
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