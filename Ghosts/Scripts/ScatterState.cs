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
    }
}