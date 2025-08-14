namespace Game.Levels
{

    public class LevelImpl : Level
    {
        private const string LEVEL_FLASH_ANIMATION_NAME = "LevelFlash";

        public override void PlayLevelFlash()
        {
            LevelFlashPlayer.Play(LEVEL_FLASH_ANIMATION_NAME);
        }

        public override void ResetLevel()
        {
            Pellets.ResetPellets();
        }

        public void OnAnimationFinished(string anim_name)
        {
            if (anim_name == LEVEL_FLASH_ANIMATION_NAME)
            {
                LevelFlashPlayer.Stop();
                LevelFlash.Visible = false;
                EmitSignal("LevelFlashFinished");
            }
        }
    }
}