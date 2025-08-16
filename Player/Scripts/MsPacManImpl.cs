using Game.Bus;
using Godot;
using Util.ExtensionMethods;

namespace Game.Player
{

    public class MsPacManImpl : MsPacMan
    {
        [Export]
        private NodePath _movementComponentPath;
        [Export]
        private NodePath _spritePath;
        private AnimatedSprite _visual;
        private const string MOVE_ANIMATION_NAME = "move";
        private const int OPEN_MOUTH_FRAME = 1;

        [Export]
        private NodePath _deathAnimationPath;
        private AnimationPlayer _deathAnimation;

        private Vector2 _previousDirection = Vector2.Zero;

        private float _speedupFactor = 0.9f;

        public override void _Ready()
        {
            SetNodeReferences();
            CheckNodeReferences();
            Movement.BodyToMove = this;
            PelletEventBus.Instance.Connect("PowerPelletCollected", this, nameof(OnPowerPelletCollected));
        }

        private void SetNodeReferences()
        {
            Movement = GetNode<MovementComponent>(_movementComponentPath);
            _visual = GetNode<AnimatedSprite>(_spritePath);
            _deathAnimation = GetNode<AnimationPlayer>(_deathAnimationPath);
        }

        private void CheckNodeReferences()
        {
            if (!Movement.IsValid())
            {
                GD.PrintErr("ERROR: MsPacMan movement component is not valid!");
            }
            if (!_visual.IsValid())
            {
                GD.PrintErr("ERROR: MsPacMan visual is not valid!");
            }
            if (!_deathAnimation.IsValid())
            {
                GD.PrintErr("ERROR: MsPacMan Death Animation is not valid!");
            }
        }

        public override void Move(Vector2 direction)
        {
            Movement.ChangeDirection(direction);
            _visual.Play(MOVE_ANIMATION_NAME);
        }

        public override void Stop()
        {
            Movement.StopMoving();
        }

        public override void Pause()
        {
            _previousDirection = Movement.GetCurrentDirection();
            Movement.StopMoving();
        }

        public override void Resume()
        {
            Movement.OverrideDirection(_previousDirection);
            _visual.Play(MOVE_ANIMATION_NAME);
        }

        public override void ResetOrientation()
        {
            _visual.Rotation = 0;
            _visual.FlipH = false;
            _visual.Offset = Vector2.Zero;
            _visual.Frame = 0;
            _deathAnimation.Play("reset_values");
        }

        public override void PlayDeathAnimation()
        {
            ResetOrientation();
            _visual.Frame = OPEN_MOUTH_FRAME;
            _deathAnimation.Play("death_anim");
        }

        public void OnDirectionChanged(Vector2 newDirection)
        {
            if (newDirection == Vector2.Right)
            {
                _visual.Rotation = 0;
                _visual.FlipH = false;
                _visual.Offset = Vector2.Zero;
            }
            else if (newDirection == Vector2.Left)
            {
                _visual.Rotation = 0;
                _visual.FlipH = true;
                _visual.Offset = new Vector2(1, 0);
            }
            else if (newDirection == Vector2.Up)
            {
                _visual.Rotation = Mathf.Deg2Rad(270);
                _visual.FlipH = false;
                _visual.Offset = new Vector2(-1, 0);
            }
            else if (newDirection == Vector2.Down)
            {
                _visual.Rotation = Mathf.Deg2Rad(270);
                _visual.FlipH = true;
                _visual.Offset = Vector2.Zero;
            }
        }

        public void OnMovementStopped()
        {
            _visual.Stop();
        }

        public override Vector2 GetPlayerDirection()
        {
            return Movement.GetCurrentDirection();
        }

        public override void ResetPlayerSpeed()
        {
            Movement.Speed = Movement.BaseSpeed;
        }


        public void OnPowerPelletCollected()
        {
            if (UseSpeedBoost)
            {
                Movement.Speed = Movement.BaseSpeed * _speedupFactor;
            }
        }

        public override void IncreaseSpeedupFactor()
        {
            if (_speedupFactor < 0.95f)
            {
                _speedupFactor = 0.95f;
            }
            else if (_speedupFactor < 1.0f)
            {
                _speedupFactor = 1.0f;
            }
        }
    }
}