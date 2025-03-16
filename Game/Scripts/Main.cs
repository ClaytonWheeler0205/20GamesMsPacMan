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
        private PackedScene _worldOne = GD.Load<PackedScene>("res://Levels/Scenes/Level1.tscn");
        private Level _currentLevel;
        private int _currentLevelNumber = 1;

        [Export]
        private NodePath _ghostContainerPath;
        private Node2D _ghostContainer;

        [Export]
        private NodePath _startJinglePath;
        private AudioStreamPlayer _startJingle;
        [Export]
        private NodePath _deathJinglePath;
        private AudioStreamPlayer _deathJingle;


        public override void _Ready()
        {
            SetNodeReferences();
            CheckNodeReferences();
            SetNodeConnections();
            SetupPlayer();
            SetLevel(_currentLevelNumber);
            SetupGhosts();
            _startJingle.Play();
        }

        private void SetNodeReferences()
        {
            _player = GetNode<MsPacMan>(_msPacManPath);
            _controller = GetNode<PlayerController>(_playerControllerPath);
            _levelContainer = GetNode<Node2D>(_levelContainerPath);
            _ghostContainer = GetNode<Node2D>(_ghostContainerPath);
            _startJingle = GetNode<AudioStreamPlayer>(_startJinglePath);
            _deathJingle = GetNode<AudioStreamPlayer>(_deathJinglePath);
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
            if (!_startJingle.IsValid())
            {
                GD.PrintErr("ERROR: Main Start Jingle Player is not valid!");
            }
            if (!_deathJingle.IsValid())
            {
                GD.PrintErr("ERROR: Main Death Jingle is not valid!");
            }
        }

        private void SetNodeConnections()
        {
            LevelEventBus.Instance.Connect("LevelCleared", this, nameof(OnLevelCleared));
            _startJingle.Connect("finished", this, nameof(OnStartJingleFinished));
            PlayerEventBus.Instance.Connect("PlayerHit", this, nameof(OnPlayerHit));
            _deathJingle.Connect("finished", this, nameof(OnDeathJingleFinished));
        }

        public async void OnPlayerHit()
        {
            _controller.IsControllerActive = false;
            _player.Stop();
            StopGhosts();
            await ToSignal(GetTree().CreateTimer(0.5f), "timeout");
            _player.PlayDeathAnimation();
            _deathJingle.Play();
        }

        public void OnDeathJingleFinished()
        {
            ResetPlayer();
            ResetGhosts();
            _startJingle.Play();
        }

        private void SetupPlayer()
        {
            _controller.PlayerToControl = _player;
            _playerStartPosition = _player.GlobalPosition;
            _controller.IsControllerActive = false;
        }

        private void SetLevel(int levelNumber)
        {
            if (levelNumber == 1)
            {
                _currentLevel = _worldOne.Instance<Level>();
                _levelContainer.AddChild(_currentLevel);
                _currentLevel.Connect("LevelFlashFinished", this, nameof(OnLevelFlashFinished));
                IntersectionDetector.CurrentLevel = _currentLevel;
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
                    ghost.SetPlayerReference(_player);
                }
            }
        }

        public void OnStartJingleFinished()
        {
            _controller.IsControllerActive = true;
            StartGhosts();
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
            StopGhosts();
            await ToSignal(GetTree().CreateTimer(0.5f), "timeout");
            if (_currentLevel.IsValid())
            {
                _currentLevel.PlayLevelFlash();
            }
        }

        private void StopGhosts()
        {
            for (int i = 0; i < _ghostContainer.GetChildCount(); i++)
            {
                if (_ghostContainer.GetChild(i) is Ghost ghost)
                {
                    ghost.StopGhost();
                }
            }
        }

        public async void OnLevelFlashFinished()
        {
            await ToSignal(GetTree().CreateTimer(1.0f), "timeout");
            _currentLevelNumber++;
            ResetPlayer();
            ResetGhosts();
            SetLevel(_currentLevelNumber);
            _startJingle.Play();
        }

        private void ResetPlayer()
        {
            _player.GlobalPosition = _playerStartPosition;
            _player.ResetOrientation();
        }

        private void ResetGhosts()
        {
            for (int i = 0; i < _ghostContainer.GetChildCount(); i++)
            {
                if (_ghostContainer.GetChild(i) is Ghost ghost)
                {
                    ghost.ResetGhost();
                }
            }
        }

    }
}