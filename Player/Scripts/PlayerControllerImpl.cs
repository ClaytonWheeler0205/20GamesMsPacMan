using Godot;

namespace Game.Player
{

	public class PlayerControllerImpl : PlayerController
	{
		public override void _Input(InputEvent @event)
		{
			if (IsControllerActive)
			{
				if (@event.IsActionPressed("move_up"))
				{
					GetPlayerToControl().Move(Vector2.Up);
				}
				else if (@event.IsActionPressed("move_down"))
				{
					GetPlayerToControl().Move(Vector2.Down);
				}
				else if (@event.IsActionPressed("move_right"))
				{
					GetPlayerToControl().Move(Vector2.Right);
				}
				else if (@event.IsActionPressed("move_left"))
				{
					GetPlayerToControl().Move(Vector2.Left);
				}
			}
		}

	}
}
