using Godot;
using Util.ExtensionMethods;

namespace Game.Ghosts
{

    public class PinkyChaseState : ChaseState
    {
        private bool _inIntersectionTile = false;
        private const int PLAYER_TILE_OFFSET = 4;

        public override void UpdateState(float delta)
        {
            base.UpdateState(delta);
            if (CurrentLevel.IsValid())
            {
                Vector2 positionInLevel = CurrentLevel.GetPositionInLevel(Movement.BodyToMove);
                if (CurrentLevel.IsAtIntersectionTile(positionInLevel) && !_inIntersectionTile)
                {
                    _inIntersectionTile = true;
                    Movement.ChangeDirection(FindShortestPathToTarget(positionInLevel));
                }
                if (!CurrentLevel.IsAtIntersectionTile(positionInLevel))
                {
                    _inIntersectionTile = false;
                }
            }

        }

        private Vector2 FindShortestPathToTarget(Vector2 ghostPosition)
        {
            Vector2 targetPosition = GetTargetMapPosition();
            float minDistance = float.PositiveInfinity;
            Vector2 newDirection = Vector2.Zero;
            Vector2 currentDirection = Movement.GetCurrentDirection();

            // Priority is Up, Left, Down, Right
            if (currentDirection != Vector2.Down)
            {
                if (CurrentLevel.IsAtPathTile(ghostPosition + Vector2.Up))
                {
                    float distance = targetPosition.DistanceTo(ghostPosition + Vector2.Up);
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
                    float distance = targetPosition.DistanceTo(ghostPosition + Vector2.Left);
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
                    float distance = targetPosition.DistanceTo(ghostPosition + Vector2.Down);
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
                    float distance = targetPosition.DistanceTo(ghostPosition + Vector2.Right);
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
            Vector2 playerMapPosition = CurrentLevel.GetPositionInLevel(Player);
            return playerMapPosition + (PLAYER_TILE_OFFSET * Player.GetPlayerDirection()); ;
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