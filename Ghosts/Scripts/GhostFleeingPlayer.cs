using Godot;

namespace Game.Ghosts
{

    public abstract class GhostFleeingPlayer : AudioStreamPlayer
    {
        [Signal]
        public delegate void SoundStarted();
        [Signal]
        public delegate void SoundStopped();
        public abstract void PlayFleeingSound();
        public abstract void StopFleeingSound();
        public abstract void OnAnyGhostEntersHouse();
    }
}