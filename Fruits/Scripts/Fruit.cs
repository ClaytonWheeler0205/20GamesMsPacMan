using Godot;

namespace Game.Fruits
{

    public abstract class Fruit : PathFollow2D
    {
        [Export]
        private int _pointValue = 100;
        public int PointValue
        {
            get { return _pointValue; }
        }

        [Signal]
        public delegate void PathCompleted();
        [Signal]
        public delegate void FruitCollected();

        public abstract void CheckParentPath();
        public abstract void OnAreaEntered(Area2D area);
        public abstract void Pause();
        public abstract void Resume();
    }
}