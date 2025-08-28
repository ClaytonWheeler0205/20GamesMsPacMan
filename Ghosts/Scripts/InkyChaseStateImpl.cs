using Godot;
using Util.ExtensionMethods;

namespace Game.Ghosts
{

    public class InkyChaseStateImpl : InkyChaseState
    {
        private bool _inIntersectionTile = false;
        private const int PLAYER_TILE_OFFSET = 2;
        // Priority is Up, Left, Down, Right
        private static readonly Vector2[] _movementDirections = { Vector2.Up, Vector2.Left, Vector2.Down, Vector2.Right };

        public override void UpdateState(float delta)
        {
            base.UpdateState(delta);
            if (CurrentLevel.IsValid())
            {
                Vector2 positionInLevel = CurrentLevel.GetPositionInLevel(Movement.BodyToMove);
                if (CurrentLevel.IsAtIntersectionTile(positionInLevel) && !_inIntersectionTile)
                {
                    _inIntersectionTile = true;
                    Movement.ChangeDirection(GetShortestPathToTarget(positionInLevel));
                }
                if (!CurrentLevel.IsAtIntersectionTile(positionInLevel))
                {
                    _inIntersectionTile = false;
                }
            }
        }

        private Vector2 GetShortestPathToTarget(Vector2 ghostPosition)
        {
            Vector2 targetPosition = GetTargetMapPosition();
            float minDistance = float.PositiveInfinity;
            Vector2 newDirection = Vector2.Zero;
            Vector2 currentDirection = Movement.GetCurrentDirection();

            foreach (Vector2 direction in _movementDirections)
            {
                if (currentDirection != (-1 * direction) && CurrentLevel.IsAtPathTile(ghostPosition + direction))
                {
                    float distance = targetPosition.DistanceTo(ghostPosition + direction);
                    if (distance < minDistance)
                    {
                        newDirection = direction;
                        minDistance = distance;
                    }
                }
            }
            return newDirection;
        }

        private Vector2 GetTargetMapPosition()
        {
            Vector2 playerMapPosition = CurrentLevel.GetPositionInLevel(Player);
            Vector2 mapPositionInFrontOfPlayer = playerMapPosition + (PLAYER_TILE_OFFSET * Player.GetPlayerDirection());

            Vector2 blinkyMapPosition = CurrentLevel.GetPositionInLevel(BlinkyReference);
            Vector2 playerBlinkyDifference = mapPositionInFrontOfPlayer - blinkyMapPosition;
            return blinkyMapPosition + (2 * playerBlinkyDifference);
        }

        public override void ResetTileDetection()
        {
            _inIntersectionTile = false;
        }

        public override void IncreaseChaseSpeed()
        {
            if (SpeedupFactor >= 0.85f)
            {
                SpeedupFactor = 0.95f;
            }
            else if (SpeedupFactor >= 0.75f)
            {
                SpeedupFactor = 0.85f;
            }
        }
    }
}