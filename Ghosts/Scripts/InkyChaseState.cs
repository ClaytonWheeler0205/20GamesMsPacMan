using Godot;
using Util.ExtensionMethods;

namespace Game.Ghosts
{

    public abstract class InkyChaseState : ChaseState
    {
        private Node2D _blinkyReference;
        public Node2D BlinkyReference
        {
            get { return _blinkyReference;}
            set
            {
                if (value.IsValid())
                {
                    _blinkyReference = value;
                }
            }
        }
    }
}