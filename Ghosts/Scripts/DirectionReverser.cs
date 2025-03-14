using Godot;

namespace Game.Ghosts
{
    public class DirectionReverser
    {
        public static void ReverseDirection(MovementComponent movement)
        {
            if (movement.GetCurrentDirection() == Vector2.Up)
            {
                movement.ChangeDirection(Vector2.Down);
            }
            else if (movement.GetCurrentDirection() == Vector2.Down)
            {
                movement.ChangeDirection(Vector2.Up);
            }
            else if (movement.GetCurrentDirection() == Vector2.Right)
            {
                movement.ChangeDirection(Vector2.Left);
            }
            else if (movement.GetCurrentDirection() == Vector2.Left)
            {
                movement.ChangeDirection(Vector2.Right);
            }
        }
    }
}