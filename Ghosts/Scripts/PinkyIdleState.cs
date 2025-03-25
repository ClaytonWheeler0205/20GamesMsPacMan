using Game.Bus;
using Godot;
using System;

namespace Game.Ghosts
{

    public class PinkyIdleState : IdleState
    {
        public override void EnterState()
        {
            if (!GhostEventBus.Instance.IsConnected("PinkyReleased", this, nameof(OnPinkyReleased)))
            {
                GhostEventBus.Instance.Connect("PinkyReleased", this, nameof(OnPinkyReleased));
            }
            base.EnterState();
        }

        public void OnPinkyReleased()
        {
            IdleAnimationPlayer.Stop();
            Movement.BodyToMove.GlobalPosition = VisualComponentReference.GlobalPosition;
            VisualComponentReference.Position = Vector2.Zero;
            Movement.ChangeDirection(Vector2.Up);
        }

        public override void ExitState()
        {
            GhostEventBus.Instance.Disconnect("PinkyReleased", this, nameof(OnPinkyReleased));
        }
    }
}