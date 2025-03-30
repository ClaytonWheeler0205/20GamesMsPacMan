using Godot;
using Util.ExtensionMethods;

namespace Game
{

    public class ScatterChaseTracker : Node
    {
        public static ScatterChaseTracker Instance { get; private set; }
        public bool InScatterState = true; // If this is false, we're in the chase state

        // Called when the node enters the scene tree for the first time.
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