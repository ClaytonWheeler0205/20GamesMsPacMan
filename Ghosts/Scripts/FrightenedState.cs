using Game.Levels;
using Util.ExtensionMethods;
using Godot;

namespace Game.Ghosts
{
    public abstract class FrightenedState : GhostState
    {
        [Signal]
        public delegate void FrightenedStateEntered();
        [Signal]
        public delegate void FrightenedStateExited();

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
    }
}