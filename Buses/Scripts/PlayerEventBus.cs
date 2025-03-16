using Godot;
using Util.ExtensionMethods;

namespace Game.Bus
{

    public class PlayerEventBus : Node
    {
        [Signal]
        public delegate void PlayerHit();

        public static PlayerEventBus Instance { get; private set;}

        public override void _Ready()
        {
            if (Instance != null && Instance != this)
            {
                this.SafeQueueFree();
            }
            else
            {
                Instance = this;
            }
        }
    }
}