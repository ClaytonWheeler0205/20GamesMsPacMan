using Game.Levels;
using Game.Player;
using Godot;

namespace Game.Ghosts
{

    public class Blinky : Ghost
    {

        public override void StartGhost()
        {
            MovementReference.ChangeDirection(Vector2.Left);
            StateMachineReference.SetIsMachineActive(true);
        }

        public override void StopGhost()
        {
            MovementReference.StopMoving();
            StateMachineReference.SetIsMachineActive(false);
        }

        public override void ResetGhost()
        {
            GlobalPosition = startPosition;
            StateMachineReference.ResetMachine();
        }

        public override void SetLevelReference(Level level)
        {
            ScatterStateReference.CurrentLevel = level;
            ChaseStateReference.CurrentLevel = level;
            FrightenedStateReference.CurrentLevel = level;
            ScatterStateReference.HomeTilePosition = level.BlinkyHomeTilePosition;
        }

        public override void SetPlayerReference(MsPacMan player)
        {
            ChaseStateReference.Player = player;
        }

    }
}