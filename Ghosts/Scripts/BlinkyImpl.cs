using Game.Bus;
using Game.Ghosts;
using Godot;


public class BlinkyImpl : Blinky
{
    public override void _Ready()
    {
        base._Ready();
        SetNodeConnections();
    }

    private void SetNodeConnections()
    {
        GhostEventBus.Instance.Connect("PelletsForElroyMet", this, nameof(OnPelletsForElroyMet));
    }

    private void IncreaseElroyLevel()
    {
        if (ChaseStateReference is BlinkyChaseState blinkyChaseState)
        {
            blinkyChaseState.IncreaseElroyLevel();
            if (StateMachineReference.GetCurrentState() == ChaseStateReference)
            {
                blinkyChaseState.ApplyElroySpeed();
            }
        }
        if (ScatterStateReference is BlinkyScatterState blinkyScatterState)
        {
            blinkyScatterState.IncreaseElroyLevel();
            if (StateMachineReference.GetCurrentState() == ScatterStateReference)
            {
                blinkyScatterState.ApplyElroySpeed();
            }
        }
    }

    public override void ResetElroyLevel()
    {
        if (ChaseStateReference is BlinkyChaseState blinkyChaseState)
        {
            blinkyChaseState.ResetElroyLevel();
        }
        if (ScatterStateReference is BlinkyScatterState blinkyScatterState)
        {
            blinkyScatterState.ResetElroyLevel();
        }
    }

    public override void OnPelletsForElroyMet()
    {
        IncreaseElroyLevel();
    }

}
