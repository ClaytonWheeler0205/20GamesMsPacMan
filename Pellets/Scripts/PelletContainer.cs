using Godot;

namespace Game.Pellets
{

    public abstract class PelletContainer : Node
    {
        [Signal]
        public delegate void PelletsCleared();

        public abstract void ResetPellets();
    }
}