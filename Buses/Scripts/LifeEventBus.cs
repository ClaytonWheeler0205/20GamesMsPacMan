using Godot;
using Util.ExtensionMethods;

namespace Game.Bus
{

    public class LifeEventBus : Node
    {
        public static LifeEventBus Instance { get; private set; }

        [Signal]
        public delegate void LifeGained();

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