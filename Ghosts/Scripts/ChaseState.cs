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

        public void OnChaseTimerTimeout()
        {
            EmitSignal("Transitioned", this, "ScatterState");
        }

        public override void EnterState()
        {
            if (!GhostEventBus.Instance.IsConnected("ScatterStateEntered", this, nameof(OnScatterStateEntered)))
            {
                GhostEventBus.Instance.Connect("ScatterStateEntered", this, nameof(OnScatterStateEntered));
            }
        }

        public void OnScatterStateEntered()
        {
            EmitSignal("Transitioned", this, "ScatterState");
        }

        public override void UpdateState(float delta)
        {
            if (GhostCollision.Vulnerable)
            {
                EmitSignal("Transitioned", this, "FrightenedState");
            }
        }

        public override void ExitState()
        {
            GhostEventBus.Instance.Disconnect("ScatterStateEntered", this, nameof(OnScatterStateEntered));
            DirectionReverser.ReverseDirection(_movement);
        }
    }
}