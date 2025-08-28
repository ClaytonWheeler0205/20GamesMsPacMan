using Godot;
using Util.ExtensionMethods;

namespace Game.Ghosts
{

    public class ClydeChaseStateImpl : ClydeChaseState
    {
        private bool _inIntersectionTile = false;
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
                    if (GetDistanceFromPlayer(positionInLevel) >= 8.0f)
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
                    _inIntersectionTile = false;
                }
            }
        }

        private float GetDistanceFromPlayer(Vector2 ghostPosition)
        {
            Vector2 playerMapPosition = CurrentLevel.GetPositionInLevel(Player);
            return ghostPosition.DistanceTo(playerMapPosition);
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

        private Vector2 FindShortestPathToHome(Vector2 ghostPosition)
        {
            float minDistance = float.PositiveInfinity;
            Vector2 newDirection = Vector2.Zero;
            Vector2 currentDirection = Movement.GetCurrentDirection();

            foreach (Vector2 direction in _movementDirections)
            {
                if (currentDirection != (-1 * direction) && CurrentLevel.IsAtPathTile(ghostPosition + direction))
                {
                    float distance = HomeTilePosition.DistanceTo(ghostPosition + direction);
                    if (distance < minDistance)
                    {
                        newDirection = direction;
                        minDistance = distance;
                    }
                }
            }
            return newDirection;
        }

        public override void ExitState()
        {
            base.ExitState();
            _inIntersectionTile = false;
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