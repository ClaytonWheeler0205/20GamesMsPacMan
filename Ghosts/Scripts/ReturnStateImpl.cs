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
        // Priority is Up, Left, Down, Right
        private static readonly Vector2[] _movementDirections = { Vector2.Up, Vector2.Left, Vector2.Down, Vector2.Right };

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

            foreach (Vector2 direction in _movementDirections)
            {
                if (currentDirection != (-1 * direction) && CurrentLevel.IsAtPathTile(ghostPosition + direction))
                {
                    float distance = GhostHouseTilePosition.DistanceTo(ghostPosition + direction);
                    if (distance < minDistance)
                    {
                        newDirection = direction;
                        minDistance = distance;
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