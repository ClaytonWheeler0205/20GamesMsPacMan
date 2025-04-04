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
        private const int PATH_TILE_CELL_NUMBER = 2;
        private const int DOWN_TILE_CELL_NUMBER = 4;
        private const int UP_TILE_CELL_NUMBER = 5;

        public override void EnterState()
        {
            EmitSignal("ReturnStateEntered");
            _inUpTile = false;
            _transitioning = false;
        }

        public override void UpdateState(float delta)
        {
            if (CurrentLevel.IsValid())
            {
                if (IntersectionDetector.IsAtIntersection(Movement.BodyToMove.GlobalPosition) && !_inIntersectionTile)
                {
                    _inIntersectionTile = true;
                    Movement.ChangeDirection(FindShortestPathToHome());
                }
                if (!IntersectionDetector.IsAtIntersection(Movement.BodyToMove.GlobalPosition))
                {
                    _inIntersectionTile = false;
                }
                if (IsAtDownTile())
                {
                    if (_inUpTile)
                    {
                        _transitioning = true;
                        EmitSignal("Transitioned", this, "ScatterState");
                    }
                    else
                    {
                        Movement.ChangeDirection(Vector2.Down);
                    }
                }
                if (IsAtUpTile())
                {
                    EmitSignal("GhostHouseEntered");
                    Movement.ChangeDirection(Vector2.Up);
                    _inUpTile = true;
                }
            }
        }

        private Vector2 FindShortestPathToHome()
        {
            Vector2 localPosition = CurrentLevel.ToLocal(Movement.BodyToMove.GlobalPosition);
            Vector2 mapPosition = CurrentLevel.WorldToMap(localPosition);
            float minDistance = float.PositiveInfinity;
            Vector2 newDirection = Vector2.Zero;
            Vector2 currentDirection = Movement.GetCurrentDirection();

            // Priority is Up, Left, Down, Right
            if (currentDirection != Vector2.Down)
            {
                Vector2 mapPositionUp = new Vector2(mapPosition.x, mapPosition.y - 1);
                int cellNumberUp = CurrentLevel.GetCell((int)mapPositionUp.x, (int)mapPositionUp.y);
                if (cellNumberUp == PATH_TILE_CELL_NUMBER)
                {
                    float distance = GhostHouseTilePosition.DistanceTo(mapPositionUp);
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
                if (cellNumberLeft == PATH_TILE_CELL_NUMBER)
                {
                    float distance = GhostHouseTilePosition.DistanceTo(mapPositionLeft);
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
                if (cellNumberDown == PATH_TILE_CELL_NUMBER)
                {
                    float distance = GhostHouseTilePosition.DistanceTo(mapPositionDown);
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
                if (cellNumberRight == PATH_TILE_CELL_NUMBER)
                {
                    float distance = GhostHouseTilePosition.DistanceTo(mapPositionRight);
                    if (distance < minDistance)
                    {
                        newDirection = Vector2.Right;
                    }
                }
            }

            return newDirection;
        }

        private bool IsAtDownTile()
        {
            Vector2 localPosition = CurrentLevel.ToLocal(Movement.BodyToMove.GlobalPosition);
            Vector2 mapPosition = CurrentLevel.WorldToMap(localPosition);
            int cellNumber = CurrentLevel.GetCell((int)mapPosition.x, (int)mapPosition.y);
            return cellNumber == DOWN_TILE_CELL_NUMBER;
        }

        private bool IsAtUpTile()
        {
            Vector2 localPosition = CurrentLevel.ToLocal(Movement.BodyToMove.GlobalPosition);
            Vector2 mapPosition = CurrentLevel.WorldToMap(localPosition);
            int cellNumber = CurrentLevel.GetCell((int)mapPosition.x, (int)mapPosition.y);
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
        }

    }
}