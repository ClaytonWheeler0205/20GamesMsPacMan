using Game.Levels;
using Godot;
using System;

namespace Game.Ghosts
{

    public class Pinky : IdleGhost
    {
        public override void StartGhost()
        {
            StateMachineReference.SetIsMachineActive(true);
        }

        public override void ResetGhost()
        {
            base.ResetGhost();
            Eyes.Play("look_down");
        }

        public override void SetLevelReference(Level level)
        {
            base.SetLevelReference(level);
            ScatterStateReference.HomeTilePosition = level.PinkyHomeTilePosition;
        }
    }
}