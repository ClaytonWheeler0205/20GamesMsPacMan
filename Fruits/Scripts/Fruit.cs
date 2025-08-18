using Godot;

namespace Game.Fruits
{

    public abstract class Fruit : PathFollow2D
    {
        [Signal]
        public delegate void PathCompleted();
        [Signal]
        public delegate void FruitDestroyed();
    }
}