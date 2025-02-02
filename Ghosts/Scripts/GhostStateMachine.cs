using Godot;

namespace Game.Ghosts
{

    public abstract class GhostStateMachine : Node
    {
        private bool _isMachineActive = false;
        protected bool IsMachineActive
        {
            get { return _isMachineActive;}
            set { _isMachineActive = value; }
        }
        
        public abstract void SetIsMachineActive(bool isActive);
        public abstract void ResetMachine();
    }
}