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
            Eyes.Play("look_left");
            StateMachineReference.ResetMachine();
        }

        public override void SetLevelReference(Level level)
        {
            ScatterStateReference.CurrentLevel = level;
            ChaseStateReference.CurrentLevel = level;
            FrightenedStateReference.CurrentLevel = level;
            ReturnStateReference.CurrentLevel = level;
            ScatterStateReference.HomeTilePosition = level.BlinkyHomeTilePosition;
            ReturnStateReference.GhostHouseTilePosition = level.GhostHousePosition;
        }

        public override void SetPlayerReference(MsPacMan player)
        {
            ChaseStateReference.Player = player;
        }

        public override void ReturnGhost()
        {
            Visible = true;
            FrightenedStateReference.TransitionToReturnState();
        }

        public override void PauseGhost()
        {
            if (!GhostCollision.Fleeing)
            {
                previousDirection = MovementReference.GetCurrentDirection();
                StopGhost();
            }
        }

        public override void ResumeGhost()
        {
            if (!GhostCollision.Fleeing)
            {
                MovementReference.OverrideDirection(previousDirection);
                StateMachineReference.SetIsMachineActive(true);
            }
        }

    }
}