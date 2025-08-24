using Game.Player;
using Util.ExtensionMethods;

namespace Game.Ghosts
{

    public abstract class BlinkyScatterState : ScatterState
    {
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
        public abstract void IncreaseElroyLevel();
        public abstract void ResetElroyLevel();
        public abstract void ApplyElroySpeed();
    }
}