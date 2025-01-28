using Godot;
using Util.ExtensionMethods;

namespace Game.Bus
{

    public class LevelEventBus : Node
    {
        public static LevelEventBus Instance { get; private set; }

        [Signal]
        public delegate void LevelCleared();

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