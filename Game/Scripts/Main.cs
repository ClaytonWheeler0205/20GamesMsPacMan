using Game.Bus;
using Game.Player;
using Game.Levels;
using Godot;
using Util.ExtensionMethods;
using Game.Ghosts;

namespace Game
{

    public class Main : Node
    {
        [Export]
        private NodePath _msPacManPath;
        private MsPacMan _player;
        private Vector2 _playerStartPosition;
        [Export]
        private NodePath _playerControllerPath;
        private PlayerController _controller;

        [Export]
        private NodePath _levelContainerPath;
        private Node2D _levelContainer;
        [Export]
        private NodePath _ghostContainerPath;
        private Node2D _ghostContainer;
        private PackedScene _worldOne = GD.Load<PackedScene>("res://Levels/Scenes/Level1.tscn");
        private Level _currentLevel;
        private int _currentLevelNumber = 1;

        public override void _Ready()
        {
            SetNodeReferences();
            CheckNodeReferences();
            SetNodeConnections();
            _controller.PlayerToControl = _player;
            _playerStartPosition = _player.GlobalPosition;
            SetLevel(_currentLevelNumber);
            SetupGhosts();
            StartGhosts();
        }

        private void SetNodeReferences()
        {
            _player = GetNode<MsPacMan>(_msPacManPath);
            _controller = GetNode<PlayerController>(_playerControllerPath);
            _levelContainer = GetNode<Node2D>(_levelContainerPath);
            _ghostContainer = GetNode<Node2D>(_ghostContainerPath);
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
            if (!_levelContainer.IsValid())
            {
                GD.PrintErr("ERROR: Main Level Container is not valid!");
            }
            if (!_ghostContainer.IsValid())
            {
                GD.PrintErr("ERROR: Main Ghost Container is not valid!");
            }
        }

        private void SetNodeConnections()
        {
            LevelEventBus.Instance.Connect("LevelCleared", this, "OnLevelCleared");
        }

        private void SetLevel(int levelNumber)
        {
            if (levelNumber == 1)
            {
                _currentLevel = _worldOne.Instance<Level>();
                _levelContainer.AddChild(_currentLevel);
                _currentLevel.Connect("LevelFlashFinished", this, "OnLevelFlashFinished");
            }
            else
            {
                _currentLevel.ResetLevel();
            }
        }

        private void SetupGhosts()
        {
           for (int i = 0; i < _ghostContainer.GetChildCount(); i++)
            {
                if (_ghostContainer.GetChild(i) is Ghost ghost)
                {
                    ghost.SetLevelReference(_currentLevel);
                    ghost.PlayerReference = _player;
                }
            } 
        }

        private void StartGhosts()
        {
            for (int i = 0; i < _ghostContainer.GetChildCount(); i++)
            {
                if (_ghostContainer.GetChild(i) is Ghost ghost)
                {
                    ghost.StartGhost();
                }
            }
        }

        public async void OnLevelCleared()
        {
            _controller.IsControllerActive = false;
            _player.Stop();
            await ToSignal(GetTree().CreateTimer(0.5f), "timeout");
            if (_currentLevel.IsValid())
            {
                _currentLevel.PlayLevelFlash();
            }
        }

        public async void OnLevelFlashFinished()
        {
            await ToSignal(GetTree().CreateTimer(1.0f), "timeout");
            _currentLevelNumber++;
            ResetPlayer();
            SetLevel(_currentLevelNumber);
        }

        private void ResetPlayer()
        {
            _player.GlobalPosition = _playerStartPosition;
            _controller.IsControllerActive = true;
        }

    }
}