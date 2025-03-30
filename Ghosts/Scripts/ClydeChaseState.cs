using Godot;

namespace Game.Ghosts
{

    public abstract class ClydeChaseState : ChaseState
    {
        private Vector2 _homeTilePosition;
        public Vector2 HomeTilePosition
        {
            get { return _homeTilePosition; }
            set
            {
                _homeTilePosition = value;
            }
        }
    }
}