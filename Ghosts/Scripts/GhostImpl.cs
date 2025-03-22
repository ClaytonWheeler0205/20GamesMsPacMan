using Game.Levels;
using Game.Player;

namespace Game.Ghosts
{

    public abstract class GhostImpl : Ghost
    {
        public override void StopGhost()
        {
            MovementReference.StopMoving();
            StateMachineReference.SetIsMachineActive(false);
        }

        public override void SetPlayerReference(MsPacMan player)
        {
            ChaseStateReference.Player = player;
        }

        public override void SetLevelReference(Level level)
        {
            ScatterStateReference.CurrentLevel = level;
            ChaseStateReference.CurrentLevel = level;
            FrightenedStateReference.CurrentLevel = level;
            ReturnStateReference.CurrentLevel = level;
            ReturnStateReference.GhostHouseTilePosition = level.GhostHousePosition;
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