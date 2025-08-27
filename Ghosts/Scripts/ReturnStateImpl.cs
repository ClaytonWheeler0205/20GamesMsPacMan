using Game.Bus;
using Godot;
using Util.ExtensionMethods;

namespace Game.Ghosts
{

    public class ReturnStateImpl : ReturnState
    {
        private bool _inIntersectionTile = false;
        private bool _inUpTile = false;
        private bool _transitioning = false;
        private bool _inGhostHouse = false;
        private const int DOWN_TILE_CELL_NUMBER = 4;
        private const int UP_TILE_CELL_NUMBER = 6;

        private float _returnSpeed = 100.0f;
        private float _speedupFactor = 0.75f;

        public override void EnterState()
        {
            if (!InReturnState)
            {
                EmitSignal("ReturnStateEntered");
                InReturnState = true;
                Movement.Speed = _returnSpeed;
            }
        }

        public override void UpdateState(float delta)
        {
            if (CurrentLevel.IsValid())
            {
                Vector2 positionInLevel = CurrentLevel.GetPositionInLevel(Movement.BodyToMove);
                if (CurrentLevel.IsAtIntersectionTile(positionInLevel) && !_inIntersectionTile)
                {
                    _inIntersectionTile = true;
                    Movement.ChangeDirection(FindShortestPathToHome(positionInLevel));
                }
                if (!CurrentLevel.IsAtIntersectionTile(positionInLevel))
                {
                    _inIntersectionTile = false;
                }
                if (IsAtDownTile(positionInLevel))
                {
                    if (Movement.GetCurrentDirection() == Vector2.Up)
                    {
                        _transitioning = true;
                        EmitSignal("Transitioned", this, "ScatterState");
                    }
                    else
                    {
                        Movement.ChangeDirection(Vector2.Down);
                    }
                }
                if (IsAtUpTile(positionInLevel) && !_inUpTile)
                {
                    if (!_inGhostHouse)
                    {
                        GhostEventBus.Instance.EmitSignal("AnyGhostEntersHouse");
                        _inGhostHouse = true;
                    }
                    Movement.ChangeDirection(Vector2.Up);
                    _inUpTile = true;
                    Movement.Speed = Movement.BaseSpeed * _speedupFactor;
                    EmitSignal("GhostHouseEntered");
                }
            }
        }

        private Vector2 FindShortestPathToHome(Vector2 ghostPosition)
        {
            float minDistance = float.PositiveInfinity;
            Vector2 newDirection = Vector2.Zero;
            Vector2 currentDirection = Movement.GetCurrentDirection();

            // Priority is Up, Left, Down, Right
            if (currentDirection != Vector2.Down)
            {
                if (CurrentLevel.IsAtPathTile(ghostPosition + Vector2.Up))
                {
                    float distance = GhostHouseTilePosition.DistanceTo(ghostPosition + Vector2.Up);
                    if (distance < minDistance)
                    {
                        newDirection = Vector2.Up;
                        minDistance = distance;
                    }
                }
            }
            if (currentDirection != Vector2.Right)
            {
                if (CurrentLevel.IsAtPathTile(ghostPosition + Vector2.Left))
                {
                    float distance = GhostHouseTilePosition.DistanceTo(ghostPosition + Vector2.Left);
                    if (distance < minDistance)
                    {
                        newDirection = Vector2.Left;
                        minDistance = distance;
                    }
                }
            }
            if (currentDirection != Vector2.Up)
            {
                if (CurrentLevel.IsAtPathTile(ghostPosition + Vector2.Down))
                {
                    float distance = GhostHouseTilePosition.DistanceTo(ghostPosition + Vector2.Down);
                    if (distance < minDistance)
                    {
                        newDirection = Vector2.Down;
                        minDistance = distance;
                    }
                }
            }
            if (currentDirection != Vector2.Left)
            {
                if (CurrentLevel.IsAtPathTile(ghostPosition + Vector2.Right))
                {
                    float distance = GhostHouseTilePosition.DistanceTo(ghostPosition + Vector2.Right);
                    if (distance < minDistance)
                    {
                        newDirection = Vector2.Right;
                    }
                }
            }

            return newDirection;
        }

        private bool IsAtDownTile(Vector2 ghostPosition)
        {
            int cellNumber = CurrentLevel.GetCell((int)ghostPosition.x, (int)ghostPosition.y);
            return cellNumber == DOWN_TILE_CELL_NUMBER;
        }

        private bool IsAtUpTile(Vector2 ghostPosition)
        {
            int cellNumber = CurrentLevel.GetCell((int)ghostPosition.x, (int)ghostPosition.y);
            return cellNumber == UP_TILE_CELL_NUMBER;
        }

        public override void ExitState()
        {
            if (_transitioning)
            {
                Movement.OverrideDirection(Vector2.Left);
            }
            else
            {
                EmitSignal("ReturnStateExited");
            }
            _inIntersectionTile = false;
            _inUpTile = false;
            _transitioning = false;
            InReturnState = false;
            _inGhostHouse = false;
        }

        public override float GetStateSpeed()
        {
            return _returnSpeed;
        }

        public override void ResetTileDetection()
        {
            _inUpTile = false;
            _inIntersectionTile = false;
        }

        public override void IncreaseReturnExitSpeed()
        {
            if (_speedupFactor >= 0.85f)
            {
                _speedupFactor = 0.95f;
            }
            else if (_speedupFactor >= 0.75f)
            {
                _speedupFactor = 0.85f;
            }
        }
    }
}