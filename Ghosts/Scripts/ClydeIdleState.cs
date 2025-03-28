using Game.Bus;
using Godot;

namespace Game.Ghosts
{

    public class ClydeIdleState : IdleState
    {
        public override void EnterState()
        {
            if (!GhostEventBus.Instance.IsConnected("ClydeReleased", this, nameof(OnClydeReleased)))
            {
                GhostEventBus.Instance.Connect("ClydeReleased", this, nameof(OnClydeReleased));
            }
            base.EnterState();
        }

        public void OnClydeReleased()
        {
            IdleAnimationPlayer.Stop();
            Movement.BodyToMove.GlobalPosition = VisualComponentReference.GlobalPosition;
            VisualComponentReference.Position = Vector2.Zero;
            Movement.OverrideDirection(Vector2.Left);
            Movement.ChangeDirection(Vector2.Up);
        }

        public override void ExitState()
        {
            GhostEventBus.Instance.Disconnect("ClydeReleased", this, nameof(OnClydeReleased));
        }
    }
}