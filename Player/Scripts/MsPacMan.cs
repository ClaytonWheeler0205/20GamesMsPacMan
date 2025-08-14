using Godot;

namespace Game.Player
{

    public abstract class MsPacMan : KinematicBody2D
    {
        public abstract void Move(Vector2 direction);
        public abstract void Stop();
        public abstract void Pause();
        public abstract void Resume();
        public abstract void ResetOrientation();
        public abstract void PlayDeathAnimation();
        public abstract Vector2 GetPlayerDirection();
        public abstract void ResetPlayerSpeed();
    }
}