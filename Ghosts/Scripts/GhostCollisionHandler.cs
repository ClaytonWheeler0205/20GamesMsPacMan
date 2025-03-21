using Godot;

namespace Game.Ghosts
{

    public abstract class GhostCollisionHandler : Area2D
    {
        [Signal]
        public delegate void GhostEaten();

        private bool _vulnerable = false;
        public bool Vulnerable
        {
            get { return _vulnerable; }
            set { _vulnerable = value; }
        }
        private bool _fleeing = false;
        public bool Fleeing 
        {
            get { return _fleeing;}
            set { _fleeing = value;}
        }
    }
}