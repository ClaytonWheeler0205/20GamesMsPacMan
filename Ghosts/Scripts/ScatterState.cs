using Game.Levels;
using Godot;
using Util.ExtensionMethods;

namespace Game.Ghosts
{

    public abstract class ScatterState : GhostState
    {
        private Level _currentLevel;
        public Level CurrentLevel
        {
            get { return _currentLevel; }
            set
            {
                if (value.IsValid())
                {
                    _currentLevel = value;
                }
            }
        }
        private MovementComponent _movement;
        public MovementComponent Movement
        {
            get { return _movement; }
            set
            {
                if (value.IsValid())
                {
                    _movement = value;
                }
            }
        }
        private Vector2 _homeTilePosition;
        public Vector2 HomeTilePosition
        {
            get { return _homeTilePosition; }
            set { _homeTilePosition = value; }
        }
        private GhostCollisionHandler _ghostCollision;
        public GhostCollisionHandler GhostCollision
        {
            get { return _ghostCollision; }
            set
            {
                if (value.IsValid())
                {
                    _ghostCollision = value;
                }
            }
        }
        private float _speedupFactor = 0.75f;
        protected float SpeedupFactor
        {
            get { return _speedupFactor; }
            set { _speedupFactor = value; }
        }
        private bool _inIntersectionTile = false;
        protected bool InIntersectionTile
        {
            get { return _inIntersectionTile; }
            set { _inIntersectionTile = value; }
        }

        protected Vector2 FindShortestPathToHome(Vector2 ghostPosition)
        {
            float minDistance = float.PositiveInfinity;
            Vector2 newDirection = Vector2.Zero;
            Vector2 currentDirection = Movement.GetCurrentDirection();

            // Priority is Up, Left, Down, Right
            if (currentDirection != Vector2.Down)
            {
                if (CurrentLevel.IsAtPathTile(ghostPosition + Vector2.Up))
                {
                    float distance = HomeTilePosition.DistanceTo(ghostPosition + Vector2.Up);
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
                    float distance = HomeTilePosition.DistanceTo(ghostPosition + Vector2.Left);
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
                    float distance = HomeTilePosition.DistanceTo(ghostPosition + Vector2.Down);
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
                    float distance = HomeTilePosition.DistanceTo(ghostPosition + Vector2.Right);
                    if (distance < minDistance)
                    {
                        newDirection = Vector2.Right;
                    }
                }
            }

            return newDirection;
        }

        public override void ExitState()
        {
            DirectionReverser.ReverseDirection(Movement);
            _inIntersectionTile = false;
        }

        public override float GetStateSpeed()
        {
            return Movement.BaseSpeed * _speedupFactor;
        }

        public abstract void ResetTileDetection();
        public abstract void IncreaseScatterSpeed();
    }
}