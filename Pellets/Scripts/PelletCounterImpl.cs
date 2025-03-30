using Game.Bus;
using Godot;

namespace Game.Pellets
{

    public class PelletCounterImpl : PelletCounter
    {
        private int _pelletsCollected = 0;
        private int _inkyDotLimit = 30;
        private int _clydeDotLimit = 90;
        private int _globalPelletsCollected = 0;
        private bool _useGlobalCounter = false;
        private const int PINKY_GLOBAL_PELLET_LIMIT = 7;
        private const int INKY_GLOBAL_PELLET_LIMIT = 17 ;
        private const int CLYDE_GLOBAL_PELLET_LIMIT = 32;

        public override void _Ready()
        {
            SetNodeConnections();
        }

        private void SetNodeConnections()
        {
            PelletEventBus.Instance.Connect("PelletCollected", this, nameof(OnPelletCollected));
            PelletEventBus.Instance.Connect("PowerPelletCollected", this, nameof(OnPowerPelledCollected));
            PlayerEventBus.Instance.Connect("PlayerHit", this, nameof(OnPlayerHit));

        }

        public void OnPelletCollected()
        {
            if (!_useGlobalCounter)
            {
                _pelletsCollected++;
                if (_pelletsCollected == _inkyDotLimit)
                {
                    GhostEventBus.Instance.EmitSignal("InkyReleased");
                }
                else if (_pelletsCollected == _clydeDotLimit)
                {
                    GhostEventBus.Instance.EmitSignal("ClydeReleased");
                }
            }
            else
            {
                _globalPelletsCollected++;
                if (_globalPelletsCollected == PINKY_GLOBAL_PELLET_LIMIT)
                {
                    GhostEventBus.Instance.EmitSignal("PinkyReleased");
                }
                else if (_globalPelletsCollected == CLYDE_GLOBAL_PELLET_LIMIT)
                {
                    GhostEventBus.Instance.EmitSignal("ClydeReleased");
                }
            }
        }

        public void OnPowerPelledCollected()
        {
            if (!_useGlobalCounter)
            {
                _pelletsCollected++;
                if (_pelletsCollected == _inkyDotLimit)
                {
                    GhostEventBus.Instance.EmitSignal("InkyReleased");
                }
                if (_pelletsCollected == _clydeDotLimit)
                {
                    GhostEventBus.Instance.EmitSignal("ClydeReleased");
                }
            }
            else
            {
                _globalPelletsCollected++;
            }
        }

        public void OnPlayerHit()
        {
            _globalPelletsCollected = 0;
            _useGlobalCounter = true;
        }

        public override void StartCounting()
        {
            if (!_useGlobalCounter)
            {
                GhostEventBus.Instance.EmitSignal("PinkyReleased");
                if (_inkyDotLimit == 0)
                {
                    GhostEventBus.Instance.EmitSignal("InkyReleased");
                }
            }
        }

        public override void SetDotLimits(int inky, int clyde)
        {
            _inkyDotLimit = inky;
            _clydeDotLimit = clyde;
        }


        public override void ResetCounter()
        {
            _useGlobalCounter = false;
            _globalPelletsCollected = 0;
            _pelletsCollected = 0;
        }


    }
}