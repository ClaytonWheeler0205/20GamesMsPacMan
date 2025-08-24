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
        private const int PATH_TILE_CELL_NUMBER = 2;
        protected int PathTileCellNumber
        {
            get { return PATH_TILE_CELL_NUMBER; }
        }
        private const int SPECIAL_TURN_TILE_CELL_NUMBER = 3;
        protected int SpecialTurnTileCellNumber
        {
            get { return SPECIAL_TURN_TILE_CELL_NUMBER; }
        }
        private const int DOWN_TILE_CELL_NUMBER = 4;
        protected int DownTileCellNumber
        {
            get { return DOWN_TILE_CELL_NUMBER; }
        }

        protected Vector2 FindShortestPathToHome()
        {
            Vector2 localPosition = CurrentLevel.ToLocal(Movement.BodyToMove.GlobalPosition);
            Vector2 mapPosition = CurrentLevel.WorldToMap(localPosition);
            int cellNumber = CurrentLevel.GetCell((int)mapPosition.x, (int)mapPosition.y);
            float minDistance = float.PositiveInfinity;
            Vector2 newDirection = Vector2.Zero;
            Vector2 currentDirection = Movement.GetCurrentDirection();

            // Priority is Up, Left, Down, Right
            if (currentDirection != Vector2.Down && cellNumber != SPECIAL_TURN_TILE_CELL_NUMBER)
            {
                Vector2 mapPositionUp = new Vector2(mapPosition.x, mapPosition.y - 1);
                int cellNumberUp = CurrentLevel.GetCell((int)mapPositionUp.x, (int)mapPositionUp.y);
                if (cellNumberUp == PATH_TILE_CELL_NUMBER || cellNumberUp == DOWN_TILE_CELL_NUMBER)
                {
                    float distance = HomeTilePosition.DistanceTo(mapPositionUp);
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
                if (cellNumberLeft == PATH_TILE_CELL_NUMBER || cellNumberLeft == DOWN_TILE_CELL_NUMBER)
                {
                    float distance = HomeTilePosition.DistanceTo(mapPositionLeft);
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
                if (cellNumberDown == PATH_TILE_CELL_NUMBER || cellNumberDown == DOWN_TILE_CELL_NUMBER)
                {
                    float distance = HomeTilePosition.DistanceTo(mapPositionDown);
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
                if (cellNumberRight == PATH_TILE_CELL_NUMBER || cellNumberRight == DOWN_TILE_CELL_NUMBER)
                {
                    float distance = HomeTilePosition.DistanceTo(mapPositionRight);
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