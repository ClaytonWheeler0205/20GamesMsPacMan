using Game.Bus;

public class GhostFleeingPlayerImpl : GhostFleeingPlayer
{
    private int _ghostsFleeing = 0;

    public override void _Ready()
    {
        SetNodeConnections();
    }

    private void SetNodeConnections()
    {
        GhostEventBus.Instance.Connect("AnyGhostEntersHouse", this, nameof(OnAnyGhostEntersHouse));
    }

    public override void PlayFleeingSound()
    {
        _ghostsFleeing++;
        if (_ghostsFleeing == 1)
        {
            Play();
        }
    }

    public override void StopFleeingSound()
    {
        Stop();
        _ghostsFleeing = 0;
    }

    public override void OnAnyGhostEntersHouse()
    {
        _ghostsFleeing--;
        if (_ghostsFleeing == 0)
        {
            Stop();
        }
    }

}
