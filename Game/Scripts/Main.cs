using Game.Bus;
using Game.Player;
using Game.Levels;
using Godot;
using Util.ExtensionMethods;
using Game.Ghosts;
using Game.Pellets;

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
        private GhostContainer _ghostContainer;

        [Export]
        private NodePath _startJinglePath;
        private AudioStreamPlayer _startJingle;
        [Export]
        private NodePath _deathJinglePath;
        private AudioStreamPlayer _deathJingle;
        private bool _playerDying = false;

        private int _ghostPointValue = 200;
        [Export]
        private NodePath _200PointsVisualPath;
        private Sprite _200PointsVisual;
        [Export]
        private NodePath _400PointsVisualPath;
        private Sprite _400PointsVisual;
        [Export]
        private NodePath _800PointsVisualPath;
        private Sprite _800PointsVisual;
        [Export]
        private NodePath _1600PointsVisualPath;
        private Sprite _1600PointsVisual;
        [Export]
        private NodePath _frightenedTimerPath;
        private Timer _frightenedTimerReference;
        private float _frightenedTimerDuration = 7.0f;
        [Export]
        private NodePath _frightenedFlashTimerPath;
        private Timer _frightenedFlashTimerReference;
        private float _frightenedFlashTimerDuration = 3.0f;
        [Export]
        private NodePath _frightenedFlashingTimerPath;
        private Timer _frightenedFlashingTimerReference;
        private float _frightenedFlashingTimerDuration = 0.25f;
        [Export]
        private NodePath _scatterTimerPath;
        private Timer _scatterTimerReference;
        private float _scatterTimerDuration = 7.0f;
        [Export]
        private NodePath _chaseTimerPath;
        private Timer _chaseTimerReference;
        private float _chaseTimerDuration = 20.0f;
        [Export]
        private NodePath _pelletCounterPath;
        private PelletCounter _pelletCounterReference;

        private bool _isPaused = false;


        public override void _Ready()
        {
            SetNodeReferences();
            CheckNodeReferences();
            SetNodeConnections();
            SetupPlayer();
            SetLevel(_currentLevelNumber);
            _ghostContainer.SetupGhosts(_currentLevel, _player);
            _startJingle.Play();
        }

        private void SetNodeReferences()
        {
            _player = GetNode<MsPacMan>(_msPacManPath);
            _controller = GetNode<PlayerController>(_playerControllerPath);
            _levelContainer = GetNode<Node2D>(_levelContainerPath);
            _ghostContainer = GetNode<GhostContainer>(_ghostContainerPath);
            _startJingle = GetNode<AudioStreamPlayer>(_startJinglePath);
            _deathJingle = GetNode<AudioStreamPlayer>(_deathJinglePath);
            _200PointsVisual = GetNode<Sprite>(_200PointsVisualPath);
            _400PointsVisual = GetNode<Sprite>(_400PointsVisualPath);
            _800PointsVisual = GetNode<Sprite>(_800PointsVisualPath);
            _1600PointsVisual = GetNode<Sprite>(_1600PointsVisualPath);
            _frightenedTimerReference = GetNode<Timer>(_frightenedTimerPath);
            _frightenedFlashTimerReference = GetNode<Timer>(_frightenedFlashTimerPath);
            _frightenedFlashingTimerReference = GetNode<Timer>(_frightenedFlashingTimerPath);
            _scatterTimerReference = GetNode<Timer>(_scatterTimerPath);
            _chaseTimerReference = GetNode<Timer>(_chaseTimerPath);
            _pelletCounterReference = GetNode<PelletCounter>(_pelletCounterPath);
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
            if (!_200PointsVisual.IsValid())
            {
                GD.PrintErr("ERROR: Main 200 Points Visual is not valid!");
            }
            if (!_400PointsVisual.IsValid())
            {
                GD.PrintErr("ERROR: Main 400 Points Visual is not valid!");
            }
            if (!_800PointsVisual.IsValid())
            {
                GD.PrintErr("ERROR: Main 800 Points Visual is not valid!");
            }
            if (!_1600PointsVisual.IsValid())
            {
                GD.PrintErr("ERROR: Main 1600 Points Visual is not valid!");
            }
            if (!_frightenedTimerReference.IsValid())
            {
                GD.PrintErr("ERROR: Main Frightened Timer Reference is not valid!");
            }
            if (!_frightenedFlashTimerReference.IsValid())
            {
                GD.PrintErr("ERROR: Main Frightened Flash Timer Reference is not valid!");
            }
            if (!_frightenedFlashingTimerReference.IsValid())
            {
                GD.PrintErr("ERROR: Main Frightened Flashing Timer is not valid!");
            }
            if (!_scatterTimerReference.IsValid())
            {
                GD.PrintErr("ERROR: Main Scatter Timer Reference is not valid!");
            }
            if (!_chaseTimerReference.IsValid())
            {
                GD.PrintErr("ERROR: Main Chase Timer Reference is not valid!");
            }
            if (!_pelletCounterReference.IsValid())
            {
                GD.PrintErr("ERROR: Main Pellet Counter Reference is not valid!");
            }
        }

        private void SetNodeConnections()
        {
            LevelEventBus.Instance.Connect("LevelCleared", this, nameof(OnLevelCleared));
            _startJingle.Connect("finished", this, nameof(OnStartJingleFinished));
            PlayerEventBus.Instance.Connect("PlayerHit", this, nameof(OnPlayerHit));
            _deathJingle.Connect("finished", this, nameof(OnDeathJingleFinished));
            GhostEventBus.Instance.Connect("GhostEaten", this, nameof(OnGhostEaten));
            PelletEventBus.Instance.Connect("PowerPelletCollected", this, nameof(OnPowerPelledCollected));
            _frightenedTimerReference.Connect("timeout", this, nameof(OnFrightenedTimerTimeout));
            _frightenedFlashTimerReference.Connect("timeout", this, nameof(OnFrightenedFlashTimerTimeout));
            _frightenedFlashingTimerReference.Connect("timeout", this, nameof(OnFrightenedFlashingTimerTimeout));
            _scatterTimerReference.Connect("timeout", this, nameof(OnScatterTimerTimeout));
            _chaseTimerReference.Connect("timeout", this, nameof(OnChaseTimerTimeout));
        }

        public async void OnPlayerHit()
        {
            if (!_playerDying)
            {
                _playerDying = true;
                _controller.IsControllerActive = false;
                _player.Stop();
                _ghostContainer.StopGhosts();
                _scatterTimerReference.Stop();
                _chaseTimerReference.Stop();
                _frightenedFlashingTimerReference.Stop();
                _frightenedFlashTimerReference.Stop();
                _frightenedTimerReference.Stop();
                await ToSignal(GetTree().CreateTimer(0.5f), "timeout");
                _player.PlayDeathAnimation();
                _deathJingle.Play();
            }
        }

        public void OnDeathJingleFinished()
        {
            ResetPlayer();
            _ghostContainer.ResetGhosts();
            _startJingle.Play();
            _playerDying = false;
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

        public void OnStartJingleFinished()
        {
            _controller.IsControllerActive = true;
            _ghostContainer.StartGhosts();
            ScatterChaseTracker.Instance.InScatterState = true;
            _scatterTimerReference.Paused = false;
            _chaseTimerReference.Paused = false;
            _frightenedTimerReference.Paused = false;
            _frightenedFlashTimerReference.Paused = false;
            _frightenedFlashingTimerReference.Paused = false;
            _scatterTimerReference.Start(_scatterTimerDuration);
            _pelletCounterReference.StartCounting();
        }

        public async void OnLevelCleared()
        {
            _controller.IsControllerActive = false;
            _player.Stop();
            _ghostContainer.StopGhosts();
            _frightenedTimerReference.Stop();
            _frightenedFlashTimerReference.Stop();
            _frightenedFlashingTimerReference.Stop();
            _scatterTimerReference.Stop();
            _chaseTimerReference.Stop();
            _ghostPointValue = 200;
            _pelletCounterReference.ResetCounter();
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
            _ghostContainer.ResetGhosts();
            SetLevel(_currentLevelNumber);
            _startJingle.Play();
        }

        private void ResetPlayer()
        {
            _player.GlobalPosition = _playerStartPosition;
            _player.ResetOrientation();
        }

        public async void OnGhostEaten(Ghost ghostEaten)
        {
            _player.Visible = false;
            _player.Pause();
            _controller.IsControllerActive = false;
            _ghostContainer.PauseGhosts();
            _frightenedTimerReference.Paused = true;
            _frightenedFlashTimerReference.Paused = true;
            _frightenedFlashingTimerReference.Paused = true;

            switch (_ghostPointValue)
            {
                case 200:
                    _200PointsVisual.GlobalPosition = ghostEaten.GlobalPosition;
                    _200PointsVisual.Visible = true;
                    break;
                case 400:
                    _400PointsVisual.GlobalPosition = ghostEaten.GlobalPosition;
                    _400PointsVisual.Visible = true;
                    break;
                case 800:
                    _800PointsVisual.GlobalPosition = ghostEaten.GlobalPosition;
                    _800PointsVisual.Visible = true;
                    break;
                default:
                    _1600PointsVisual.GlobalPosition = ghostEaten.GlobalPosition;
                    _1600PointsVisual.Visible = true;
                    break;
            }
            ScoreEventBus.Instance.EmitSignal("AwardPoints", _ghostPointValue);
            await ToSignal(GetTree().CreateTimer(1.0f), "timeout");
            _200PointsVisual.Visible = false;
            _400PointsVisual.Visible = false;
            _800PointsVisual.Visible = false;
            _1600PointsVisual.Visible = false;
            _player.Visible = true;
            _player.Resume();
            _controller.IsControllerActive = true;
            _ghostContainer.ResumeGhosts();
            _frightenedTimerReference.Paused = false;
            _frightenedFlashTimerReference.Paused = false;
            _frightenedFlashingTimerReference.Paused = false;
            ghostEaten.Visible = true;
            ghostEaten.SetGhostFleeing();
            _ghostPointValue *= 2;
        }

        public void OnPowerPelledCollected()
        {
            _frightenedTimerReference.Start(_frightenedTimerDuration);
            _frightenedFlashTimerReference.Stop();
            _frightenedFlashingTimerReference.Stop();
            _ghostContainer.SetGhostsVulnerability();
            _scatterTimerReference.Paused = true;
            _chaseTimerReference.Paused = true;
        }

        public void OnFrightenedTimerTimeout()
        {
            _frightenedFlashTimerReference.Start(_frightenedFlashTimerDuration);
            _frightenedFlashingTimerReference.Start(_frightenedFlashingTimerDuration);
        }

        public void OnFrightenedFlashTimerTimeout()
        {
            _frightenedFlashingTimerReference.Stop();
            _ghostContainer.SetGhostsInvulnerable();
            _ghostPointValue = 200;
            _scatterTimerReference.Paused = false;
            _chaseTimerReference.Paused = false;
        }

        public void OnFrightenedFlashingTimerTimeout()
        {
            _ghostContainer.SetGhostsFlash();
        }

        public void OnScatterTimerTimeout()
        {
            ScatterChaseTracker.Instance.InScatterState = false;
            _chaseTimerReference.Start(_chaseTimerDuration);
        }

        public void OnChaseTimerTimeout()
        {
            ScatterChaseTracker.Instance.InScatterState = true;
            _scatterTimerReference.Start(_scatterTimerDuration);
        }

        public override void _UnhandledInput(InputEvent @event)
        {
            if (@event.IsActionPressed("pause"))
            {
                if (_isPaused)
                {
                    _isPaused = false;
                    _player.Resume();
                    _ghostContainer.ResumeGhosts();
                    _controller.IsControllerActive = true;
                }
                else
                {
                    _isPaused = true;
                    _player.Pause();
                    _ghostContainer.PauseGhosts();
                    _controller.IsControllerActive = false;
                }
            }
        }
    }
}