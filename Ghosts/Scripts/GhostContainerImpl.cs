using System.Collections.Generic;
using Game.Levels;
using Game.Player;
using Godot;

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
    }
}