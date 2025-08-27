using Game.Bus;

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
        private const int INKY_GLOBAL_PELLET_LIMIT = 17;
        private const int CLYDE_GLOBAL_PELLET_LIMIT = 32;
        private int _pelletsCollectedForFruit = 0;
        private const int PELLETS_NEEDED_FOR_FIRST_FRUIT = 64;
        private const int PELLETS_NEDDED_FOR_SECOND_FRUIT = 176;

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
            _pelletsCollectedForFruit++;
            CheckIfFruitShouldSpawn();
            if (!_useGlobalCounter)
            {
                IncreaseLocalPelletCounter();
            }
            else
            {
                IncreaseGlobalPelletCounter();
            }
        }

        private void IncreaseLocalPelletCounter()
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

        private void IncreaseGlobalPelletCounter()
        {
            _globalPelletsCollected++;
            if (_globalPelletsCollected == PINKY_GLOBAL_PELLET_LIMIT)
            {
                GhostEventBus.Instance.EmitSignal("PinkyReleased");
            }
            else if (_globalPelletsCollected == INKY_GLOBAL_PELLET_LIMIT)
            {
                GhostEventBus.Instance.EmitSignal("InkyReleased");
            }
            else if (_globalPelletsCollected == CLYDE_GLOBAL_PELLET_LIMIT)
            {
                GhostEventBus.Instance.EmitSignal("ClydeReleased");
            }
        }

        public void OnPowerPelledCollected()
        {
            _pelletsCollectedForFruit++;
            CheckIfFruitShouldSpawn();
            if (!_useGlobalCounter)
            {
                IncreaseLocalPelletCounter();
            }
            else
            {
                IncreaseGlobalPelletCounter();
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
                if (_clydeDotLimit == 0)
                {
                    GhostEventBus.Instance.EmitSignal("ClydeReleased");
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
            _pelletsCollectedForFruit = 0;
        }

        private void CheckIfFruitShouldSpawn()
        {
            if (_pelletsCollectedForFruit == PELLETS_NEEDED_FOR_FIRST_FRUIT || _pelletsCollectedForFruit == PELLETS_NEDDED_FOR_SECOND_FRUIT)
            {
                EmitSignal("PelletCountForFruitMet");
            }
        }
    }
}