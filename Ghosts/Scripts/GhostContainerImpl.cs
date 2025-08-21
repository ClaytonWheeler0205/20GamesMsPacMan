using System.Collections.Generic;
using Game.Levels;
using Game.Player;

namespace Game.Ghosts
{

    public class GhostContainerImpl : GhostContainer
    {
        private List<Ghost> _ghosts;

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            _ghosts = new List<Ghost>();
            for (int i = 0; i < GetChildCount(); i++)
            {
                if (GetChild(i) is Ghost ghost)
                {
                    _ghosts.Add(ghost);
                }
            }
        }

        public override void SetupGhosts(Level level, MsPacMan player)
        {
            foreach (Ghost ghost in _ghosts)
            {
                ghost.SetLevelReference(level);
                ghost.SetPlayerReference(player);
            }
        }

        public override void StartGhosts()
        {
            foreach (Ghost ghost in _ghosts)
            {
                ghost.StartGhost();
            }
        }

        public override void StopGhosts()
        {
            foreach (Ghost ghost in _ghosts)
            {
                ghost.StopGhost();
            }
        }

        public override void ResetGhosts()
        {
            foreach (Ghost ghost in _ghosts)
            {
                ghost.ResetGhost();
            }
        }

        public override void PauseGhosts()
        {
            foreach (Ghost ghost in _ghosts)
            {
                ghost.PauseGhost();
            }
        }

        public override void ResumeGhosts()
        {
            foreach (Ghost ghost in _ghosts)
            {
                ghost.ResumeGhost();
            }
        }

        public override void SetGhostsVulnerability()
        {
            foreach (Ghost ghost in _ghosts)
            {
                ghost.SetGhostVulnerability();
            }
        }

        public override void SetGhostsInvulnerable()
        {
            foreach (Ghost ghost in _ghosts)
            {
                ghost.SetGhostInvulnerable();
            }
        }

        public override void SetGhostsFlash()
        {
            foreach (Ghost ghost in _ghosts)
            {
                ghost.SetGhostFlash();
            }
        }

        public override void SetGhostsInvisible()
        {
            foreach (Ghost ghost in _ghosts)
            {
                ghost.Visible = false;
            }
        }

        public override void ResetGhostHomeTiles(Level level)
        {
            foreach (Ghost ghost in _ghosts)
            {
                ghost.SetLevelReference(level);
            }
        }

        public override void RevreseDirections()
        {
            foreach (Ghost ghost in _ghosts)
            {
                DirectionReverser.ReverseDirection(ghost.MovementReference);
            }
        }

        public override void IncreaseGhostSpeed()
        {
            foreach (Ghost ghost in _ghosts)
            {
                ghost.IncreaseGhostSpeed();
            }
        }
    }
}