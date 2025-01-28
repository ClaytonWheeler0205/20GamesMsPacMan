using Godot;
using Util.ExtensionMethods;

namespace Game.Player
{

    public class MsPacManImpl : MsPacMan
    {
        [Export]
        private NodePath _movementComponentPath;
        private MovementComponent _movement;
        [Export]
        private NodePath _spritePath;
        private AnimatedSprite _visual;
        private const string MOVE_ANIMATION_NAME = "move";

        public override void _Ready()
        {
            SetNodeReferences();
            CheckNodeReferences();
            _movement.BodyToMove = this;
        }

        private void SetNodeReferences()
        {
            _movement = GetNode<MovementComponent>(_movementComponentPath);
            _visual = GetNode<AnimatedSprite>(_spritePath);
        }

        private void CheckNodeReferences()
        {
            if (!_movement.IsValid())
            {
                GD.PrintErr("ERROR: MsPacMan movement component is not valid!");
            }
            if (!_visual.IsValid())
            {
                GD.PrintErr("ERROR: MsPacMan visual is not valid!");
            }
        }

        public override void Move(Vector2 direction)
        {
            _movement.ChangeDirection(direction);
            _visual.Play(MOVE_ANIMATION_NAME);
        }

        public override void Stop()
        {
            _movement.StopMoving();
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
    }
}