namespace Game.Ghosts
{

    public abstract class BlinkyChaseState : ChaseState
    {
        public abstract void IncreaseElroyLevel();
        public abstract void ResetElroyLevel();
        public abstract void ApplyElroySpeed();
        public abstract void OnClydeReleased();
        public abstract void OnPlayerHit();
    }
}