using Godot;

namespace Game.Pellets
{

    public abstract class PelletCounter : Node
    {
        [Signal]
        public delegate void PelletCountForFruitMet();
        
        public abstract void StartCounting();
        public abstract void SetDotLimits(int inky, int clyde);
        public abstract void ResetCounter();
    }
}