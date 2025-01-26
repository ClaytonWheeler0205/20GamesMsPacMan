using Godot;
using System;
using Util.ExtensionMethods;

namespace Game.Bus
{

    public class ScoreEventBus : Node
    {
        public static ScoreEventBus Instance { get; private set; }

        // Score signals

        [Signal]
        public delegate void AwardPoints(int pointsToGive);

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