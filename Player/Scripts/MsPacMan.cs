using Godot;

namespace Game.Player
{

    public abstract class MsPacMan : KinematicBody2D
    {
        public abstract void Move(Vector2 direction);
        public abstract void Stop();
        public abstract void ResetOrientation();
        public abstract void PlayDeathAnimation();
    }
}