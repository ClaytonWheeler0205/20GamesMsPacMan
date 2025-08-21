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
        private bool _isMoving = true;
        public bool IsMoving
        {
            get { return _isMoving; }
            set { _isMoving = value; }
        }

        [Signal]
        public delegate void PathCompleted();
        [Signal]
        public delegate void FruitCollected();

        public abstract void CheckParentPath();
        public abstract void OnAreaEntered(Area2D area);
    }
}