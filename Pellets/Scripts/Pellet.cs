using Game.Bus;
using Godot;
using Util.ExtensionMethods;

namespace Game.Pellets
{

    public abstract class Pellet : Area2D
    {
        [Signal]
        public delegate void PelletCollected();
        [Export]
        private NodePath _visualComponentPath;
        private Sprite _visualComponent;
        protected Sprite VisualComponent
        {
            get { return _visualComponent; }
        }
        [Export]
        private NodePath _collisionComponentPath;
        private CollisionShape2D _collisionComponent;
        protected CollisionShape2D CollisionComponent
        {
            get { return _collisionComponent; }
        }
        [Export]
        private NodePath _audioComponentPath;
        private AudioStreamPlayer _audioComponent;
        protected AudioStreamPlayer AudioComponent
        {
            get { return _audioComponent; }
        }
        [Export]
        private int _pointValue;
        private const string PLAYER_NODE_GROUP = "Player";
        protected string PlayerNodeGroup
        {
            get { return PLAYER_NODE_GROUP;}
        }
        public override void _Ready()
        {
            SetNodeReferences();
            CheckNodeReferences();
        }

        private void SetNodeReferences()
        {
            _visualComponent = GetNode<Sprite>(_visualComponentPath);
            _collisionComponent = GetNode<CollisionShape2D>(_collisionComponentPath);
            _audioComponent = GetNode<AudioStreamPlayer>(_audioComponentPath);
        }

        private void CheckNodeReferences()
        {
            if (!_visualComponent.IsValid())
            {
                GD.PrintErr("ERROR: Pellet Visual Component is not valid!");
            }
            if (!_collisionComponent.IsValid())
            {
                GD.PrintErr("ERROR: Pellet Collision Component is not valid!");
            }
            if (!_audioComponent.IsValid())
            {
                GD.PrintErr("ERROR: Pellet Audio Component is not valid!");
            }
        }

        public abstract void ResetPellet();

        protected void CollectPellet()
        {
            _visualComponent.Visible = false;
            _collisionComponent.SetDeferred("disabled", true);
            _audioComponent.Play();
            ScoreEventBus.Instance.EmitSignal("AwardPoints", _pointValue);
            EmitSignal("PelletCollected");
        }

    }
}