using Game.Levels;
using Game.Player;
using Godot;

namespace Game.Ghosts
{

    public abstract class GhostContainer : Node2D
    {
        public abstract void SetupGhosts(Level level, MsPacMan player);
        public abstract void StartGhosts();
        public abstract void StopGhosts();
        public abstract void ResetGhosts();
        public abstract void HardResetGhosts();
        public abstract void PauseGhosts();
        public abstract void ResumeGhosts();
        public abstract void SetGhostsVulnerability();
        public abstract void SetGhostsInvulnerable();
        public abstract void SetGhostsFlash();
        public abstract void SetGhostsInvisible();
        public abstract void SetGhostsVisible();
        public abstract void ResetGhostHomeTiles(Level level);
        public abstract void RevreseDirections();
        public abstract void IncreaseGhostSpeed();
    }
}