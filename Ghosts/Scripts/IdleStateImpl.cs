using Godot;

namespace Game.Ghosts
{

    public class IdleStateImpl : IdleState
    {
        public override void IncreaseIdleSpeed()
        {
            if (SpeedupFactor >= 0.85f)
            {
                SpeedupFactor = 0.95f;
            }
            else if (SpeedupFactor >= 0.75f)
            {
                SpeedupFactor = 0.85f;
            }
        }
    }
}