using Game.Levels;
using Godot;
using Util.ExtensionMethods;

namespace Game.Ghosts
{

    public abstract class ReturnState : GhostState
    {
        [Signal]
        public delegate void ReturnStateEntered();
        [Signal]
        public delegate void GhostHouseEntered();
        [Signal]
        public delegate void ReturnStateExited();

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
        public Vector2 _ghostHouseTilePosition;
        public Vector2 GhostHouseTilePosition
        {
            get { return _ghostHouseTilePosition; }
            set { _ghostHouseTilePosition = value; }
        }

        public abstract void ResetTileDetection();
        public abstract void IncreaseReturnExitSpeed();
    }
}