using Godot;
using Util.ExtensionMethods;

namespace Game.Player
{

    public abstract class MsPacMan : KinematicBody2D
    {
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
        private bool _useSpeedBoost = true;
        public bool UseSpeedBoost
        {
            get { return _useSpeedBoost; }
            set { _useSpeedBoost = value; }
        }

        public abstract void Move(Vector2 direction);
        public abstract void Stop();
        public abstract void Pause();
        public abstract void Resume();
        public abstract void ResetOrientation();
        public abstract void PlayDeathAnimation();
        public abstract Vector2 GetPlayerDirection();
        public abstract void ResetPlayerSpeed();
        public abstract void IncreaseSpeedupFactor();
    }
}