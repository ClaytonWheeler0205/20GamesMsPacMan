using Godot;
using Util.ExtensionMethods;

namespace Game.Ghosts
{

    public class InkyChaseStateImpl : InkyChaseState
    {
        private bool _inIntersectionTile = false;
        private const int PATH_TILE_CELL_NUMBER = 2;
        private const int SPECIAL_TURN_TILE_CELL_NUMBER = 3;
        private const int PLAYER_TILE_OFFSET = 2;

        public override void UpdateState(float delta)
        {
            base.UpdateState(delta);
            if (CurrentLevel.IsValid())
            {
                if (IntersectionDetector.IsAtIntersection(Movement.BodyToMove.GlobalPosition) && !_inIntersectionTile)
                {
                    _inIntersectionTile = true;
                    Movement.ChangeDirection(GetShortestPathToTarget());
                }
                if (!IntersectionDetector.IsAtIntersection(Movement.BodyToMove.GlobalPosition))
                {
                    _inIntersectionTile = false;
                }
            }
        }

        private Vector2 GetShortestPathToTarget()
        {
            Vector2 localPosition = CurrentLevel.ToLocal(Movement.BodyToMove.GlobalPosition);
            Vector2 mapPosition = CurrentLevel.WorldToMap(localPosition);
            Vector2 targetPosition = GetTargetMapPosition();
            int cellNumber = CurrentLevel.GetCell((int)mapPosition.x, (int)mapPosition.y);
            float minDistance = float.PositiveInfinity;
            Vector2 newDirection = Vector2.Zero;
            Vector2 currentDirection = Movement.GetCurrentDirection();

            // Priority is Up, Left, Down, Right
            if (currentDirection != Vector2.Down && cellNumber != SPECIAL_TURN_TILE_CELL_NUMBER)
            {
                Vector2 mapPositionUp = new Vector2(mapPosition.x, mapPosition.y - 1);
                int cellNumberUp = CurrentLevel.GetCell((int)mapPositionUp.x, (int)mapPositionUp.y);
                if (cellNumberUp == PATH_TILE_CELL_NUMBER)
                {
                    float distance = targetPosition.DistanceTo(mapPositionUp);
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
                    float distance = targetPosition.DistanceTo(mapPositionLeft);
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
                    float distance = targetPosition.DistanceTo(mapPositionDown);
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
                    float distance = targetPosition.DistanceTo(mapPositionRight);
                    if (distance < minDistance)
                    {
                        newDirection = Vector2.Right;
                    }
                }
            }

            return newDirection;
        }

        private Vector2 GetTargetMapPosition()
        {
            Vector2 localPlayerPosition = CurrentLevel.ToLocal(Player.GlobalPosition);
            Vector2 playerMapPosition = CurrentLevel.WorldToMap(localPlayerPosition);
            Vector2 mapPositionInFrontOfPlayer = playerMapPosition + (PLAYER_TILE_OFFSET * Player.GetPlayerDirection());

            Vector2 localBlinkyPosition = CurrentLevel.ToLocal(BlinkyReference.GlobalPosition);
            Vector2 blinkyMapPosition = CurrentLevel.WorldToMap(localBlinkyPosition);
            Vector2 playerBlinkyDifference = mapPositionInFrontOfPlayer - blinkyMapPosition;
            return blinkyMapPosition + (2 * playerBlinkyDifference);
        }

        public override void ResetTileDetection()
        {
            _inIntersectionTile = false;
        }

    }
}