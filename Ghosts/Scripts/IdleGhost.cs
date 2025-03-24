using Godot;
using Util.ExtensionMethods;

namespace Game.Ghosts
{

    public abstract class IdleGhost : GhostImpl
    {
        [Export]
        private NodePath _idleAnimationPlayerPath;
        private AnimationPlayer _idleAnimationPlayer;
        protected AnimationPlayer IdleAnimationPlayer
        {
            get { return _idleAnimationPlayer;}
        }
        protected const string IDLE_ANIMATION_NAME = "Idle";

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            base._Ready();
            SetNodeReferences();
            CheckNodeReferences();
        }

        private void SetNodeReferences()
        {
            _idleAnimationPlayer = GetNode<AnimationPlayer>(_idleAnimationPlayerPath);
        }

        private void CheckNodeReferences()
        {
            if (!_idleAnimationPlayer.IsValid())
            {
                GD.PrintErr("ERROR: Idle Ghost Idle Animation Player is not valid!");
            }
        }
    }
}