using Game.Levels;
using Util;

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
            int randomTileIndex = GDRandom.RandiRange(1, 4);
            switch (randomTileIndex)
            {
                case 1:
                    ScatterStateReference.HomeTilePosition = level.BlinkyHomeTilePosition;
                    break;
                case 2:
                    ScatterStateReference.HomeTilePosition = level.PinkyHomeTilePosition;
                    break;
                case 3:
                    ScatterStateReference.HomeTilePosition = level.InkyHomeTilePosition;
                    break;
                case 4:
                    ScatterStateReference.HomeTilePosition = level.ClydeHomeTilePosition;
                    break;
            }
        }
    }
}