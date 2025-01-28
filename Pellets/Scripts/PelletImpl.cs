using Godot;

namespace Game.Pellets
{

    public class PelletImpl : Pellet
    {
        public override void ResetPellet()
        {
            VisualComponent.Visible = true;
            CollisionComponent.SetDeferred("disabled", false);

        }

        public void OnAreaEntered(Area2D area)
        {
            if (area.IsInGroup(PlayerNodeGroup))
            {
                CollectPellet();
            }
        }
    }
}