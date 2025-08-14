using Game.Levels;
using Util.ExtensionMethods;
using Game.Player;

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

        public override void EnterState()
        {
            EmitSignal("SpeedChangeRequested", Movement.BaseSpeed);
        }

        public override void UpdateState(float delta)
        {
            if (GhostCollision.Vulnerable)
            {
                EmitSignal("Transitioned", this, "FrightenedState");
            }
            else if (ScatterChaseTracker.Instance.InScatterState)
            {
                EmitSignal("Transitioned", this, "ScatterState");
            }
        }

        public override void ExitState()
        {
            DirectionReverser.ReverseDirection(_movement);
        }

        public override float GetStateSpeed()
        {
            return Movement.BaseSpeed;
        }

        public abstract void ResetTileDetection();
    }
}