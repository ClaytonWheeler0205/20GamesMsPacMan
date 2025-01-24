using Godot;
using Util.ExtensionMethods;

namespace Game.Player
{
    public abstract class MovementComponent : Node2D
    {
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