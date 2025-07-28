using Game.Levels;
using Godot;
using Util.ExtensionMethods;

namespace Game.Ghosts
{

    public abstract class IdleGhost : GhostImpl
    {
        [Export]
        private NodePath _visualNodePath;
        private Node2D _visualNodeReference;
        [Export]
        private NodePath _idleAnimationPlayerPath;
        private AnimationPlayer _idleAnimationPlayer;
        protected AnimationPlayer IdleAnimationPlayer
        {
            get { return _idleAnimationPlayer; }
        }
        protected const string IDLE_ANIMATION_NAME = "Idle";

        [Export]
        private NodePath _idleStatePath;
        private IdleState _idleStateReference;

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            base._Ready();
            SetNodeReferences();
            CheckNodeReferences();
            _idleStateReference.IdleAnimationPlayer = _idleAnimationPlayer;
            _idleStateReference.Movement = MovementReference;
            _idleStateReference.VisualComponentReference = _visualNodeReference;
            _idleStateReference.GhostCollision = GhostCollision;
        }

        private void SetNodeReferences()
        {
            _visualNodeReference = GetNode<Node2D>(_visualNodePath);
            _idleAnimationPlayer = GetNode<AnimationPlayer>(_idleAnimationPlayerPath);
            _idleStateReference = GetNode<IdleState>(_idleStatePath);
        }

        private void CheckNodeReferences()
        {
            if (!_visualNodeReference.IsValid())
            {
                GD.PrintErr("ERROR: Idle Ghost Visual Node Reference is not valid!");
            }
            if (!_idleAnimationPlayer.IsValid())
            {
                GD.PrintErr("ERROR: Idle Ghost Idle Animation Player is not valid!");
            }
        }

        public override void StartGhost()
        {
            StateMachineReference.SetIsMachineActive(true);
            BodyVisual.Play("move");
        }


        public override void StopGhost()
        {
            base.StopGhost();
            _idleAnimationPlayer.Stop(false);
        }

        public override void ResetGhost()
        {
            base.ResetGhost();
            _visualNodeReference.Position = Vector2.Zero;
        }

        public override void SetLevelReference(Level level)
        {
            base.SetLevelReference(level);
            _idleStateReference.CurrentLevel = level;
        }
    }
}