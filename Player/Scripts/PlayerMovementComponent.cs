using Game.Bus;
using Godot;

namespace Game.Player
{

    public class PlayerMovementComponent : MovementComponentImpl
    {
        private const int PELLET_PAUSE_IN_FRAMES = 1;
        private const int POWER_PELLET_PAUSE_IN_FRAMES = 3;
        private int _frameCounter = 0;

        public override void _Ready()
        {
            base._Ready();
            SetNodeConnections();
        }

        public override void _PhysicsProcess(float delta)
        {
            if (_frameCounter == 0)
            {
                base._PhysicsProcess(delta);
            }
            else
            {
                _frameCounter--;
            }
        }


        private void SetNodeConnections()
        {
            PelletEventBus.Instance.Connect("PelletCollected", this, nameof(OnPelletCollected));
            PelletEventBus.Instance.Connect("PowerPelletCollected", this, nameof(OnPowerPelledCollected));
        }

        private void OnPelletCollected()
        {
            _frameCounter = PELLET_PAUSE_IN_FRAMES;
        }

        private void OnPowerPelledCollected()
        {
            _frameCounter = POWER_PELLET_PAUSE_IN_FRAMES;
        }

    }
}