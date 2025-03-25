using Game.Ghosts;
using Godot;
using System;
using Util.ExtensionMethods;

namespace Game.Bus
{

    public class GhostEventBus : Node
    {

        public static GhostEventBus Instance { get; private set; }

        [Signal]
        public delegate void GhostEaten(Ghost ghostEaten);
        [Signal]
        public delegate void PinkyReleased();

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