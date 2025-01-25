using Game.Player;
using Godot;
using Util.ExtensionMethods;

namespace Game
{

    public class Main : Node
    {
        [Export]
        private NodePath _msPacManPath;
        private MsPacMan _player;
        [Export]
        private NodePath _playerControllerPath;
        private PlayerController _controller;

        public override void _Ready()
        {
            SetNodeReferences();
            CheckNodeReferences();
            _controller.PlayerToControl = _player;
        }

        private void SetNodeReferences()
        {
            _player = GetNode<MsPacMan>(_msPacManPath);
            _controller = GetNode<PlayerController>(_playerControllerPath);
        }

        private void CheckNodeReferences()
        {
            if (!_player.IsValid())
            {
                GD.PrintErr("ERROR: Main player is not valid!");
            }
            if (!_controller.IsValid())
            {
                GD.PrintErr("ERROR: Main controller is not valid!");
            }
        }

    }
}