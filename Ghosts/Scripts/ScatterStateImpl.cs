using Game;
using Game.Ghosts;
using Godot;
using Util.ExtensionMethods;

public class ScatterStateImpl : ScatterState
{

    public override void EnterState()
    {
        EmitSignal("SpeedChangeRequested", Movement.BaseSpeed * SpeedupFactor);
    }

    public override void UpdateState(float delta)
    {
        if (GhostCollision.Vulnerable)
        {
            EmitSignal("Transitioned", this, "FrightenedState");
        }
        else if (!ScatterChaseTracker.Instance.InScatterState)
        {
            EmitSignal("Transitioned", this, "ChaseState");
        }
        else
        {
            if (CurrentLevel.IsValid())
            {
                if (IntersectionDetector.IsAtIntersection(Movement.BodyToMove.GlobalPosition) && !InIntersectionTile)
                {
                    InIntersectionTile = true;
                    Movement.ChangeDirection(FindShortestPathToHome());
                }
                if (!IntersectionDetector.IsAtIntersection(Movement.BodyToMove.GlobalPosition))
                {
                    InIntersectionTile = false;
                }
            }
        }
    }

    public override void ResetTileDetection()
    {
        InIntersectionTile = false;
    }

    public override void IncreaseScatterSpeed()
    {
        if (SpeedupFactor >= 0.85f)
        {
            SpeedupFactor = 0.95f;
        }
        else if (SpeedupFactor >= 0.75f)
        {
            SpeedupFactor = 0.85f;
        }
    }
}
