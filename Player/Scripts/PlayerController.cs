using Godot;
using Util.ExtensionMethods;

namespace Game.Player
{

    public abstract class PlayerController : Node
    {
        private bool _isControllerActive = true;
        public bool IsControllerActive
        {
            get { return _isControllerActive;}
            set { _isControllerActive = value;}
        }
        private MsPacMan _playerToControl;
        public MsPacMan PlayerToControl
        {
            set
            {
                if (value.IsValid())
                {
                    _playerToControl = value;
                }
            }
        }

        protected MsPacMan GetPlayerToControl()
        {
            return _playerToControl;
        }
    }
}