using Godot;
using Game.Levels;
using Util.ExtensionMethods;
using Game.Player;
using Game.Bus;

namespace Game.Ghosts
{

    public abstract class ChaseState : GhostState
    {
        private Level _currentLevel;
        public Level CurrentLevel
        {
            get { return _currentLevel; }
            set
            {
                if (value.IsValid())
                {
                    _currentLevel = value;
                }
            }
        }
        private MovementComponent _movement;
        public MovementComponent Movement
        {
            get { return _movement; }
            set
            {
                if (value.IsValid())
                {
                    _movement = value;
                }
            }
        }
        private MsPacMan _player;
        public MsPacMan Player
        {
            get { return _player; }
            set
            {
                if (value.IsValid())
                {
                    _player = value;
                }
            }
        }
        private GhostCollisionHandler _ghostCollision;
        public GhostCollisionHandler GhostCollision
        {
            get { return _ghostCollision; }
            set
            {
                if (value.IsValid())
                {
                    _ghostCollision = value;
                }
            }
        }

        [Export]
        private NodePath _chaseTimerPath;
        private Timer _chaseTimer;
        protected Timer ChaseTimer
        {
            get { return _chaseTimer; }
        }
        private float _chaseTime = 20.0f;

        public override void _Ready()
        {
            SetNodeReferences();
            CheckNodeReferences();
            SetNodeConnections();
        }

        private void SetNodeReferences()
        {
            _chaseTimer = GetNode<Timer>(_chaseTimerPath);
        }

        private void CheckNodeReferences()
        {
            if (!_chaseTimer.IsValid())
            {
                GD.PrintErr("ERROR: Ghost Chase Timer is not valid!");
            }
        }

        private void SetNodeConnections()
        {
            LevelEventBus.Instance.Connect("LevelCleared", this, nameof(OnLevelCleared));
            _chaseTimer.Connect("timeout", this, nameof(OnChaseTimerTimeout));
        }


        public void OnLevelCleared()
        {
            _chaseTimer.Paused = false;
            _chaseTimer.Stop();
        }

        public void OnChaseTimerTimeout()
        {
            EmitSignal("Transitioned", this, "ScatterState");
        }

        public override void EnterState()
        {
            if (!_chaseTimer.Paused)
            {
                _chaseTimer.Start(_chaseTime);
            }
            else
            {
                _chaseTimer.Paused = false;
                _chaseTimer.Start();
            }
        }

        public override void UpdateState(float delta)
        {
            if (GhostCollision.Vulnerable)
            {
                _chaseTimer.Paused = true;
                EmitSignal("Transitioned", this, "FrightenedState");
            }
        }

        public override void ExitState()
        {
            _chaseTimer.Stop();
            DirectionReverser.ReverseDirection(_movement);
        }
    }
}