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

        private KinematicBody2D _bodyToMove;
        public KinematicBody2D BodyToMove
        {
            get { return _bodyToMove; }
            set{
                if (value.IsValid())
                {
                    _bodyToMove = value;
                }
            }
        }

        public abstract void ChangeDirection(Vector2 newDirection);
        public abstract void StopMoving();

    }
}