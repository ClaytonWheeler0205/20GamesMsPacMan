using Game.Levels;
using Godot;

namespace Game.Ghosts
{

    public class Blinky : Ghost
    {

        public override void StartGhost()
        {
            MovementReference.ChangeDirection(Vector2.Left);
            StateMachineReference.SetIsMachineActive(true);
            GD.Print(MovementReference.GetCurrentDirection());
        }

        public override void SetLevelReference(Level level)
        {
            ScatterStateReference.CurrentLevel = level;
            ScatterStateReference.HomeTilePosition = level.BlinkyHomeTilePosition;
        }

    }
}