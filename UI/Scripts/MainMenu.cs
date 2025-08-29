using Godot;

namespace Game.UI
{

    public class MainMenu : Node
    {
        public override void _UnhandledInput(InputEvent @event)
        {
            if (@event.IsActionPressed("ui_select"))
            {
                GetTree().ChangeScene("res://Game/Scenes/Main.tscn");
            }
        }

    }
}