using Game.Levels;
using Godot;

namespace Game.Ghosts
{

    public class Blinky : GhostImpl
    {

        public override void StartGhost()
        {
            MovementReference.ChangeDirection(Vector2.Left);
            StateMachineReference.SetIsMachineActive(true);
        }

        public override void ResetGhost()
        {
            base.ResetGhost();
            Eyes.Play("look_left");
        }

        public override void SetLevelReference(Level level)
        {
            base.SetLevelReference(level);
            ScatterStateReference.HomeTilePosition = level.BlinkyHomeTilePosition;
        }
    }
}