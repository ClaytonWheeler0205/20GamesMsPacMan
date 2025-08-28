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
        // Priority is Up, Left, Down, Right
        private static readonly Vector2[] _movementDirections = { Vector2.Up, Vector2.Left, Vector2.Down, Vector2.Right };

        protected Vector2 FindShortestPathToHome(Vector2 ghostPosition)
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