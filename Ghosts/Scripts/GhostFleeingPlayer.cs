using Godot;

public abstract class GhostFleeingPlayer : AudioStreamPlayer
{
    public abstract void PlayFleeingSound();
    public abstract void StopFleeingSound();
    public abstract void OnAnyGhostEntersHouse();
}
