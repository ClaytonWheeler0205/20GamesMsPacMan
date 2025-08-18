using Godot;

namespace Game.Fruits
{

    public class FruitImpl : Fruit
    {
        [Export]
        private float _speed = 50.0f;
        [Export]
        private float _pointValue = 100;

        public override void _Process(float delta)
        {
            Offset += _speed * delta;
            if (UnitOffset >= 1.0f)
            {
                EmitSignal("PathCompleted");
            }
        }

        public override void CheckParentPath()
        {
            Path2D parentPath = GetParent() as Path2D;
            if (parentPath.Curve == null)
            {
                EmitSignal("PathCompleted");
            }
        }


    }
}