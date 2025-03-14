using Godot;
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

        public override void _Ready()
        {
            SetNodeConnections();
        }

        private void SetNodeConnections()
        {
            PelletEventBus.Instance.Connect("PowerPelletCollected", this, nameof(OnPowerPelletCollected));
        }

        public void OnPowerPelletCollected()
        {
            GD.Print("Power Pellet Collected! Blinky enters frightened state.");
            //EmitSignal("Transitioned", this, "FrightenedState");
        }
    }
}