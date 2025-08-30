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
        private bool _isMoving = true;
        [Export]
        private NodePath _animationPath;
        private AnimationPlayer _animationReference;
        [Export]
        private NodePath _bobbingSoundPath;
        private AudioStreamPlayer _bobbingSoundReference;

        public override void _Ready()
        {
            SetNodeReferences();
            CheckNodeReferences();
        }

        private void SetNodeReferences()
        {
            _animationReference = GetNode<AnimationPlayer>(_animationPath);
            _bobbingSoundReference = GetNode<AudioStreamPlayer>(_bobbingSoundPath);
        }

        private void CheckNodeReferences()
        {
            if (!_animationReference.IsValid())
            {
                GD.PrintErr("ERROR: Fruit Animation Reference is not valid!");
            }
            if (!_bobbingSoundReference.IsValid())
            {
                GD.PrintErr("ERROR: Fruit Bobbing Sound Reference is not valid!");
            }
        }

        public override void _Process(float delta)
        {
            if (_isMoving)
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

        public override void Pause()
        {
            _isMoving = false;
            _animationReference.Stop(false);
            _bobbingSoundReference.Stop();
        }

        public override void Resume()
        {
            _isMoving = true;
            _animationReference.Play();
            _bobbingSoundReference.Play();
        }
    }
}