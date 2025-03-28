using Game.Levels;

namespace Game.Ghosts
{
    public class Clyde : IdleGhost
    {
        public override void ResetGhost()
        {
            base.ResetGhost();
            Eyes.Play("look_up");
        }

        public override void SetLevelReference(Level level)
        {
            base.SetLevelReference(level);
            ScatterStateReference.HomeTilePosition = level.ClydeHomeTilePosition;
        }
    }
}