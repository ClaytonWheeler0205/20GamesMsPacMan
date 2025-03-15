using Godot;

namespace Game.Ghosts
{

    public abstract class GhostState : Node
    {
        [Signal]
        public delegate void Transitioned(GhostState currentState, string newStateName);

        public abstract void EnterState();
        public abstract void UpdateState(float delta);
        public abstract void ExitState();
    }
}