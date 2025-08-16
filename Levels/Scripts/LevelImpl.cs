using System;
using Godot;
using Util.ExtensionMethods;

namespace Game.Levels
{

    public class LevelImpl : Level
    {
        private const string LEVEL_FLASH_ANIMATION_NAME = "LevelFlash";
        [Export]
        private NodePath _tunnelContainerPath;
        private Node2D _tunnelContainerReference;

        public override void _Ready()
        {
            base._Ready();
            SetNodeReferences();
            CheckNodeReferences();
        }

        private void SetNodeReferences()
        {
            _tunnelContainerReference = GetNode<Node2D>(_tunnelContainerPath);
        }

        private void CheckNodeReferences()
        {
            if (!_tunnelContainerReference.IsValid())
            {
                GD.PrintErr("ERROR: Level Tunnel Container Reference is not valid!");
            }
        }

        public override void PlayLevelFlash()
        {
            LevelFlashPlayer.Play(LEVEL_FLASH_ANIMATION_NAME);
        }

        public override void ResetLevel()
        {
            Pellets.ResetPellets();
        }

        public override void DestroyTunnels()
        {
            _tunnelContainerReference.SafeQueueFree();
        }

        public void OnAnimationFinished(string anim_name)
        {
            if (anim_name == LEVEL_FLASH_ANIMATION_NAME)
            {
                LevelFlashPlayer.Stop();
                LevelFlash.Visible = false;
                EmitSignal("LevelFlashFinished");
            }
        }
    }
}