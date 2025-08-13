using Godot;

namespace Game.Ghosts
{

    public abstract class GhostState : Node
    {
        [Signal]
        public delegate void Transitioned(GhostState currentState, string newStateName);
        [Signal]
        public delegate void SpeedChangeRequested(float speed);

        public abstract void EnterState();
        public abstract void UpdateState(float delta);
        public abstract void ExitState();
        public abstract float GetStateSpeed();
    }
}