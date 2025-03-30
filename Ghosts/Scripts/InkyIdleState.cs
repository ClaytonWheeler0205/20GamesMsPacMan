using Game.Bus;
using Godot;

namespace Game.Ghosts
{

    public class InkyIdleState : IdleState
    {
        public override void EnterState()
        {
            if (!GhostEventBus.Instance.IsConnected("InkyReleased", this, nameof(OnInkyReleased)))
            {
                GhostEventBus.Instance.Connect("InkyReleased", this, nameof(OnInkyReleased));
            }
            base.EnterState();
        }

        public void OnInkyReleased()
        {
            HasBeenReleased = true;
            IdleAnimationPlayer.Stop();
            Movement.BodyToMove.GlobalPosition = VisualComponentReference.GlobalPosition;
            VisualComponentReference.Position = Vector2.Zero;
            Movement.ChangeDirection(Vector2.Right);
        }

        public override void ExitState()
        {
            base.ExitState();
            GhostEventBus.Instance.Disconnect("InkyReleased", this, nameof(OnInkyReleased));
        }


    }
}