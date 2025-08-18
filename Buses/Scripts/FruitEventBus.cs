using Game.Fruits;
using Godot;
using Util.ExtensionMethods;

namespace Game.Bus
{

    public class FruitEventBus : Node
    {
        public static FruitEventBus Instance { get; private set; }

        [Signal]
        public delegate void FruitCollected(Fruit fruit);

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