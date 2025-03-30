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
            HasBeenReleased = true;
            IdleAnimationPlayer.Stop();
            Movement.BodyToMove.GlobalPosition = VisualComponentReference.GlobalPosition;
            VisualComponentReference.Position = Vector2.Zero;
            Movement.ChangeDirection(Vector2.Left);
        }

        public override void ExitState()
        {
            base.ExitState();
            GhostEventBus.Instance.Disconnect("ClydeReleased", this, nameof(OnClydeReleased));
        }
    }
}