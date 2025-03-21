using Godot;
using Util.ExtensionMethods;

namespace Game
{
    public abstract class MovementComponent : Node2D
    {
        [Signal]
        public delegate void DirectionChanged(Vector2 newDirection);
        [Signal]
        public delegate void MovementStopped();

        [Export]
        private float _speed = 200;
        public float Speed
        {
            get { return _speed; }
            set
            {
                if (value > 0)
                {
                    _speed = value;
                }
            }
        }
        private KinematicBody2D _bodyToMove;
        public KinematicBody2D BodyToMove
        {
            get { return _bodyToMove; }
            set
            {
                if (value.IsValid())
                {
                    _bodyToMove = value;
                }
            }
        }

        public abstract void ChangeDirection(Vector2 newDirection);
        public abstract void StopMoving();
        public abstract Vector2 GetCurrentDirection();
        public abstract void OverrideDirection(Vector2 newDirection);

    }
}