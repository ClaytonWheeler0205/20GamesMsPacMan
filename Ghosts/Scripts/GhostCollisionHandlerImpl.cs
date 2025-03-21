using Game.Bus;
using Godot;

namespace Game.Ghosts
{

    public class GhostCollisionHandlerImpl : GhostCollisionHandler
    {
        private const string PLAYER_NODE_GROUP = "Player";

        public override void _Ready()
        {
            SetNodeConnections();
        }

        private void SetNodeConnections()
        {
            Connect("area_entered", this, nameof(OnAreaEntered));
        }

        public void OnAreaEntered(Area2D area)
        {
            if (area.IsInGroup(PLAYER_NODE_GROUP))
            {
                if (!Vulnerable && !Fleeing)
                {
                    PlayerEventBus.Instance.EmitSignal("PlayerHit");
                }
                else if (!Fleeing)
                {
                    EmitSignal("GhostEaten");
                }
            }
        }
    }
}