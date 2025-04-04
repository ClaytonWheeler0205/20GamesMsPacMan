using Game.Levels;

namespace Game.Ghosts
{
    public class Pinky : IdleGhost
    {
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