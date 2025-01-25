using Godot;
using Util.ExtensionMethods;

namespace Game.Player
{

    public class MsPacManImpl : MsPacMan
    {
        [Export]
        private NodePath _movementComponentPath;
        private MovementComponent _movement;

        public override void _Ready()
        {
            SetNodeReferences();
            CheckNodeReferences();
            _movement.BodyToMove = this;
        }

        private void SetNodeReferences()
        {
            _movement = GetNode<MovementComponent>(_movementComponentPath);
        }

        private void CheckNodeReferences()
        {
            if (!_movement.IsValid())
            {
                GD.PrintErr("ERROR: MsPacMan movement component is not valid!");
            }
        }

        public override void Move(Vector2 direction)
        {
            _movement.ChangeDirection(direction);
        }
    }
}