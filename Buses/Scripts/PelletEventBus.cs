using Godot;
using Util.ExtensionMethods;

namespace Game.Bus
{

    public class PelletEventBus : Node
    {
        public static PelletEventBus Instance { get; private set; }

        [Signal]
        public delegate void PelletCollected();
        [Signal]
        public delegate void PowerPelletCollected();

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