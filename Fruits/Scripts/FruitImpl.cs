using Game.Bus;
using Godot;
using Util.ExtensionMethods;

namespace Game.Fruits
{

    public class FruitImpl : Fruit
    {
        [Export]
        private float _speed = 50.0f;
        private const string PLAYER_NODE_GROUP = "Player";

        public override void _Process(float delta)
        {
            if (IsMoving)
            {
                Offset += _speed * delta;
                if (UnitOffset >= 1.0f)
                {
                    EmitSignal("PathCompleted");
                }
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

        public override void OnAreaEntered(Area2D area)
        {
            if (area.IsInGroup(PLAYER_NODE_GROUP))
            {
                FruitEventBus.Instance.EmitSignal("FruitCollected", this);
                ScoreEventBus.Instance.EmitSignal("AwardPoints", PointValue);
                this.SafeQueueFree();
            }
        }
    }
}