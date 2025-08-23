using Game.Bus;
using Game.Player;
using Game.Levels;
using Godot;
using Util.ExtensionMethods;
using Game.Ghosts;
using Game.Pellets;
using Game.UI;
using Game.Fruits;

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
        private PackedScene _worldTwo = GD.Load<PackedScene>("res://Levels/Scenes/Level2.tscn");
        private PackedScene _worldThree = GD.Load<PackedScene>("res://Levels/Scenes/Level3.tscn");
        private PackedScene _worldFour = GD.Load<PackedScene>("res://Levels/Scenes/Level4.tscn");
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
        private Node2D _200PointsVisual;
        [Export]
        private NodePath _400PointsVisualPath;
        private Node2D _400PointsVisual;
        [Export]
        private NodePath _800PointsVisualPath;
        private Node2D _800PointsVisual;
        [Export]
        private NodePath _1600PointsVisualPath;
        private Node2D _1600PointsVisual;
        [Export]
        private NodePath _frightenedTimerPath;
        private Timer _frightenedTimerReference;
        private float _frightenedTimerDuration = 4.75f;
        [Export]
        private NodePath _frightenedFlashTimerPath;
        private Timer _frightenedFlashTimerReference;
        private float _frightenedFlashTimerDuration = 1.25f;
        [Export]
        private NodePath _frightenedFlashingTimerPath;
        private Timer _frightenedFlashingTimerReference;
        private float _frightenedFlashingTimerDuration = 0.125f;
        private bool _useFrightenedTimers = true;
        [Export]
        private NodePath _scatterTimerPath;
        private Timer _scatterTimerReference;
        private float _scatterTimerDuration = 7.0f;
        [Export]
        private NodePath _pelletCounterPath;
        private PelletCounter _pelletCounterReference;
        [Export]
        private NodePath _ghostEatenSoundPath;
        private AudioStreamPlayer _ghostEatenSoundReference;
        [Export]
        private NodePath _ghostFleeingSoundPath;
        private GhostFleeingPlayer _ghostFleeingSoundReference;
        [Export]
        private NodePath _livesManagerPath;
        private LivesManager _livesManagerReference;
        [Export]
        private NodePath _gameOverLabelPath;
        private Control _gameOverLabelReference;

        [Export]
        private NodePath _fruitEatenSoundPath;
        private AudioStreamPlayer _fruitEatenSoundReference;
        [Export]
        private NodePath _100PointsVisualPath;
        private Node2D _100PointsVisualReference;
        [Export]
        private NodePath _200FruitPointsVisualPath;
        private Node2D _200FruitPointsVisualReference;
        [Export]
        private NodePath _500PointsVisualPath;
        private Node2D _500PointsVisualReference;
        [Export]
        private NodePath _700PointsVisualPath;
        private Node2D _700PointsVisualReference;
        [Export]
        private NodePath _1000PointsVisualPath;
        private Node2D _1000PointsVisualReference;
        [Export]
        private NodePath _2000PointsVisualPath;
        private Node2D _2000PointsVisualReference;
        [Export]
        private NodePath _5000PointsVisualPath;
        private Node2D _5000PointsVisualReference;

        [Export]
        private NodePath _fruitCounterPath;
        private FruitCounter _fruitCounterReference;

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
            _200PointsVisual = GetNode<Node2D>(_200PointsVisualPath);
            _400PointsVisual = GetNode<Node2D>(_400PointsVisualPath);
            _800PointsVisual = GetNode<Node2D>(_800PointsVisualPath);
            _1600PointsVisual = GetNode<Node2D>(_1600PointsVisualPath);
            _frightenedTimerReference = GetNode<Timer>(_frightenedTimerPath);
            _frightenedFlashTimerReference = GetNode<Timer>(_frightenedFlashTimerPath);
            _frightenedFlashingTimerReference = GetNode<Timer>(_frightenedFlashingTimerPath);
            _scatterTimerReference = GetNode<Timer>(_scatterTimerPath);
            _pelletCounterReference = GetNode<PelletCounter>(_pelletCounterPath);
            _ghostEatenSoundReference = GetNode<AudioStreamPlayer>(_ghostEatenSoundPath);
            _ghostFleeingSoundReference = GetNode<GhostFleeingPlayer>(_ghostFleeingSoundPath);
            _livesManagerReference = GetNode<LivesManager>(_livesManagerPath);
            _gameOverLabelReference = GetNode<Control>(_gameOverLabelPath);
            _fruitEatenSoundReference = GetNode<AudioStreamPlayer>(_fruitEatenSoundPath);
            _100PointsVisualReference = GetNode<Node2D>(_100PointsVisualPath);
            _200FruitPointsVisualReference = GetNode<Node2D>(_200FruitPointsVisualPath);
            _500PointsVisualReference = GetNode<Node2D>(_500PointsVisualPath);
            _700PointsVisualReference = GetNode<Node2D>(_700PointsVisualPath);
            _1000PointsVisualReference = GetNode<Node2D>(_1000PointsVisualPath);
            _2000PointsVisualReference = GetNode<Node2D>(_2000PointsVisualPath);
            _5000PointsVisualReference = GetNode<Node2D>(_5000PointsVisualPath);
            _fruitCounterReference = GetNode<FruitCounter>(_fruitCounterPath);
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
            if (!_pelletCounterReference.IsValid())
            {
                GD.PrintErr("ERROR: Main Pellet Counter Reference is not valid!");
            }
            if (!_ghostEatenSoundReference.IsValid())
            {
                GD.PrintErr("ERROR: Main Ghost Eaten Sound Player Reference is not valid!");
            }
            if (!_ghostFleeingSoundReference.IsValid())
            {
                GD.PrintErr("ERROR: Main Ghost Fleeing Sound Reference is not valid!");
            }
            if (!_livesManagerReference.IsValid())
            {
                GD.PrintErr("ERROR: Main Lives Manager Reference is not valid!");
            }
            if (!_gameOverLabelReference.IsValid())
            {
                GD.PrintErr("ERROR: Main Game Over Label Reference is not valid!");
            }
            if (!_100PointsVisualReference.IsValid())
            {
                GD.PrintErr("ERROR: Main 100 Points Visual Reference is not valid!");
            }
            if (!_200FruitPointsVisualReference.IsValid())
            {
                GD.PrintErr("ERROR: Main 200 Fruit Points Visual Reference is not valid!");
            }
            if (!_500PointsVisualReference.IsValid())
            {
                GD.PrintErr("ERROR: Main 500 Points Visual Reference is not valid!");
            }
            if (!_700PointsVisualReference.IsValid())
            {
                GD.PrintErr("ERROR: Main 700 Points Visual Reference is not valid!");
            }
            if (!_1000PointsVisualReference.IsValid())
            {
                GD.PrintErr("ERROR: Main 1000 Points Visual Reference is not valid!");
            }
            if (!_2000PointsVisualReference.IsValid())
            {
                GD.PrintErr("ERROR: Main 2000 Points Visual Reference is not valid!");
            }
            if (!_5000PointsVisualReference.IsValid())
            {
                GD.PrintErr("ERROR: Main 5000 Points Visual Reference is not valid!");
            }
            if (!_fruitCounterReference.IsValid())
            {
                GD.PrintErr("ERROR: Main Fruit Counter Reference is not valid!");
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
            FruitEventBus.Instance.Connect("FruitCollected", this, nameof(OnFruitCollected));
            _pelletCounterReference.Connect("PelletCountForFruitMet", this, nameof(OnPelletCountForFruitMet));
        }

        public async void OnPlayerHit()
        {
            if (!_playerDying)
            {
                _playerDying = true;
                _controller.IsControllerActive = false;
                _player.Stop();
                _ghostContainer.StopGhosts();
                _currentLevel.PauseFruit();
                _scatterTimerReference.Stop();
                _frightenedFlashingTimerReference.Stop();
                _frightenedFlashTimerReference.Stop();
                _frightenedTimerReference.Stop();
                _ghostFleeingSoundReference.StopFleeingSound();
                _ghostPointValue = 200;
                await ToSignal(GetTree().CreateTimer(0.5f), "timeout");
                _currentLevel.DestroyFruit();
                _ghostContainer.SetGhostsInvisible();
                _player.PlayDeathAnimation();
                _deathJingle.Play();
            }
        }

        public void OnDeathJingleFinished()
        {
            _livesManagerReference.LoseLife();
            if (_livesManagerReference.GetLivesRemaining() >= 0)
            {
                ResetPlayer();
                _ghostContainer.ResetGhosts();
                _ghostContainer.SetGhostsVisible();
                _ghostContainer.ResetGhostHomeTiles(_currentLevel);
                ScatterChaseTracker.Instance.InScatterState = true;
                _startJingle.Play();
                _playerDying = false;
            }
            else
            {
                _player.Visible = false;
                _gameOverLabelReference.Visible = true;
            }
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
            else if (levelNumber == 3)
            {
                _currentLevel.SafeQueueFree();
                _currentLevel = _worldTwo.Instance<Level>();
                _levelContainer.AddChild(_currentLevel);
                _currentLevel.Connect("LevelFlashFinished", this, nameof(OnLevelFlashFinished));
                IntersectionDetector.CurrentLevel = _currentLevel;
                _ghostContainer.SetupGhosts(_currentLevel, _player);
            }
            else if (levelNumber == 4)
            {
                _currentLevel.DestroyTunnels();
                _currentLevel.ResetLevel();
            }
            else if (levelNumber == 6)
            {
                _currentLevel.SafeQueueFree();
                _currentLevel = _worldThree.Instance<Level>();
                _levelContainer.AddChild(_currentLevel);
                _currentLevel.Connect("LevelFlashFinished", this, nameof(OnLevelFlashFinished));
                IntersectionDetector.CurrentLevel = _currentLevel;
                _ghostContainer.SetupGhosts(_currentLevel, _player);
            }
            else if (levelNumber == 10)
            {
                _currentLevel.SafeQueueFree();
                _currentLevel = _worldFour.Instance<Level>();
                _levelContainer.AddChild(_currentLevel);
                _currentLevel.Connect("LevelFlashFinished", this, nameof(OnLevelFlashFinished));
                IntersectionDetector.CurrentLevel = _currentLevel;
                _ghostContainer.SetupGhosts(_currentLevel, _player);
            }
            else
            {
                _currentLevel.ResetLevel();
            }
            switch (levelNumber)
            {
                case 1:
                    _player.Movement.Speed = _player.Movement.BaseSpeed * 0.8f;
                    break;
                case 2:
                    _player.Movement.Speed = _player.Movement.BaseSpeed * 0.9f;
                    _player.IncreaseSpeedupFactor();
                    _ghostContainer.IncreaseGhostSpeed();
                    _frightenedTimerDuration = 3.75f;
                    _currentLevel.Pellets.PelletsNeededForFirstElroy = 30;
                    _currentLevel.Pellets.PelletsNeededForSecondElroy = 15;
                    _pelletCounterReference.SetDotLimits(0, 50);
                    break;
                case 3:
                    _frightenedTimerDuration = 2.75f;
                    _currentLevel.Pellets.PelletsNeededForFirstElroy = 40;
                    _currentLevel.Pellets.PelletsNeededForSecondElroy = 20;
                    _pelletCounterReference.SetDotLimits(0, 0);
                    break;
                case 4:
                    _frightenedTimerDuration = 1.75f;
                    break;
                case 5:
                    _player.Movement.Speed = _player.Movement.BaseSpeed;
                    _player.IncreaseSpeedupFactor();
                    _ghostContainer.IncreaseGhostSpeed();
                    _frightenedTimerDuration = 0.75f;
                    _scatterTimerDuration = 5.0f;
                    break;
                case 6:
                    _frightenedTimerDuration = 3.75f;
                    _currentLevel.Pellets.PelletsNeededForFirstElroy = 50;
                    _currentLevel.Pellets.PelletsNeededForSecondElroy = 25;
                    break;
                case 7:
                    _frightenedTimerDuration = 0.75f;
                    break;
                case 9:
                    _frightenedFlashTimerDuration = 0.75f;
                    _frightenedTimerDuration = 0.25f;
                    _currentLevel.Pellets.PelletsNeededForFirstElroy = 60;
                    _currentLevel.Pellets.PelletsNeededForSecondElroy = 30;
                    break;
                case 10:
                    _frightenedFlashTimerDuration = 1.25f;
                    _frightenedTimerDuration = 3.75f;
                    break;
                case 11:
                    _frightenedTimerDuration = 0.25f;
                    break;
                case 12:
                    _frightenedFlashTimerDuration = 0.75f;
                    _frightenedTimerDuration = 0.25f;
                    _currentLevel.Pellets.PelletsNeededForFirstElroy = 80;
                    _currentLevel.Pellets.PelletsNeededForSecondElroy = 40;
                    break;
                case 14:
                    _frightenedFlashTimerDuration = 1.25f;
                    _frightenedTimerDuration = 1.75f;
                    break;
                case 15:
                    _frightenedFlashTimerDuration = 0.75f;
                    _frightenedTimerDuration = 0.25f;
                    _currentLevel.Pellets.PelletsNeededForFirstElroy = 100;
                    _currentLevel.Pellets.PelletsNeededForSecondElroy = 50;
                    break;
                case 17:
                    _useFrightenedTimers = false;
                    _player.UseSpeedBoost = false;
                    break;
                case 18:
                    _useFrightenedTimers = true;
                    _player.UseSpeedBoost = true;
                    break;
                case 19:
                    _useFrightenedTimers = false;
                    _player.UseSpeedBoost = false;
                    _currentLevel.Pellets.PelletsNeededForFirstElroy = 120;
                    _currentLevel.Pellets.PelletsNeededForSecondElroy = 60;
                    break;
                case 21:
                    _player.Movement.Speed = _player.Movement.BaseSpeed * 0.9f;
                    break;
            }
        }

        public void OnStartJingleFinished()
        {
            _controller.IsControllerActive = true;
            _ghostContainer.StartGhosts();
            ScatterChaseTracker.Instance.InScatterState = true;
            _scatterTimerReference.Paused = false;
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
            _currentLevel.PauseFruit();
            _frightenedTimerReference.Stop();
            _frightenedFlashTimerReference.Stop();
            _frightenedFlashingTimerReference.Stop();
            _scatterTimerReference.Stop();
            _ghostPointValue = 200;
            _pelletCounterReference.ResetCounter();
            _ghostFleeingSoundReference.StopFleeingSound();
            await ToSignal(GetTree().CreateTimer(0.5f), "timeout");
            if (_currentLevel.IsValid())
            {
                _currentLevel.PlayLevelFlash();
            }
            _ghostContainer.SetGhostsInvisible();
            _currentLevel.DestroyFruit();
        }

        public async void OnLevelFlashFinished()
        {
            await ToSignal(GetTree().CreateTimer(1.0f), "timeout");
            _currentLevelNumber++;
            ResetPlayer();
            _ghostContainer.HardResetGhosts();
            _ghostContainer.SetGhostsVisible();
            SetLevel(_currentLevelNumber);
            _fruitCounterReference.IncreaseFruitCounter();
            _startJingle.Play();
        }

        private void ResetPlayer()
        {
            _player.GlobalPosition = _playerStartPosition;
            _player.ResetOrientation();
            _player.ResetPlayerSpeed();
        }

        public async void OnGhostEaten(Ghost ghostEaten)
        {
            _ghostEatenSoundReference.Play();
            _player.Visible = false;
            _player.Pause();
            _controller.IsControllerActive = false;
            _ghostContainer.PauseGhosts();
            _currentLevel.PauseFruit();
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
                    _ghostPointValue = 1600;
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
            _currentLevel.ResumeFruit();
            _frightenedTimerReference.Paused = false;
            _frightenedFlashTimerReference.Paused = false;
            _frightenedFlashingTimerReference.Paused = false;
            ghostEaten.Visible = true;
            ghostEaten.SetGhostFleeing();
            _ghostFleeingSoundReference.PlayFleeingSound();
            _ghostPointValue *= 2;
        }

        public void OnPowerPelledCollected()
        {
            if (_useFrightenedTimers)
            {
                _frightenedTimerReference.Start(_frightenedTimerDuration);
                _frightenedFlashTimerReference.Stop();
                _frightenedFlashingTimerReference.Stop();
                _ghostContainer.SetGhostsVulnerability();
                _scatterTimerReference.Paused = true;
            }
            else
            {
                _ghostContainer.RevreseDirections();
            }
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
            _player.ResetPlayerSpeed();
        }

        public void OnFrightenedFlashingTimerTimeout()
        {
            _ghostContainer.SetGhostsFlash();
        }

        public void OnScatterTimerTimeout()
        {
            ScatterChaseTracker.Instance.InScatterState = false;
        }

        public async void OnFruitCollected(Fruit fruit)
        {
            _fruitEatenSoundReference.Play();
            switch (fruit.PointValue)
            {
                case 100:
                    _100PointsVisualReference.Visible = true;
                    _100PointsVisualReference.GlobalPosition = fruit.GlobalPosition;
                    await ToSignal(GetTree().CreateTimer(3.0f), "timeout");
                    _100PointsVisualReference.Visible = false;
                    break;
                case 200:
                    _200FruitPointsVisualReference.Visible = true;
                    _200FruitPointsVisualReference.GlobalPosition = fruit.GlobalPosition;
                    await ToSignal(GetTree().CreateTimer(3.0f), "timeout");
                    _200FruitPointsVisualReference.Visible = false;
                    break;
                case 500:
                    _500PointsVisualReference.Visible = true;
                    _500PointsVisualReference.GlobalPosition = fruit.GlobalPosition;
                    await ToSignal(GetTree().CreateTimer(3.0f), "timeout");
                    _500PointsVisualReference.Visible = false;
                    break;
                case 700:
                    _700PointsVisualReference.Visible = true;
                    _700PointsVisualReference.GlobalPosition = fruit.GlobalPosition;
                    await ToSignal(GetTree().CreateTimer(3.0f), "timeout");
                    _700PointsVisualReference.Visible = false;
                    break;
                case 1000:
                    _1000PointsVisualReference.Visible = true;
                    _1000PointsVisualReference.GlobalPosition = fruit.GlobalPosition;
                    await ToSignal(GetTree().CreateTimer(3.0f), "timeout");
                    _1000PointsVisualReference.Visible = false;
                    break;
                case 2000:
                    _2000PointsVisualReference.Visible = true;
                    _2000PointsVisualReference.GlobalPosition = fruit.GlobalPosition;
                    await ToSignal(GetTree().CreateTimer(3.0f), "timeout");
                    _2000PointsVisualReference.Visible = false;
                    break;
                case 5000:
                    _5000PointsVisualReference.Visible = true;
                    _5000PointsVisualReference.GlobalPosition = fruit.GlobalPosition;
                    await ToSignal(GetTree().CreateTimer(3.0f), "timeout");
                    _5000PointsVisualReference.Visible = false;
                    break;
            }
        }

        public void OnPelletCountForFruitMet()
        {
            _currentLevel.SpawnFruit(_currentLevelNumber);
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