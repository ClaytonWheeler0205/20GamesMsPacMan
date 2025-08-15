using Godot;

namespace Game.UI
{

    public abstract class LivesManager : Control
    {
        private int _livesRemaining = 2;
        protected int LivesRemaining
        {
            get { return _livesRemaining; }
            set { _livesRemaining = value; }
        }
        public abstract void LoseLife();
        public abstract int GetLivesRemaining();
        public abstract void OnLifeGained();
    }
}