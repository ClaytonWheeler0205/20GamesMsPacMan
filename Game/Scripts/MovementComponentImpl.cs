using Godot;
using Util.ExtensionMethods;

namespace Game
{

    public class MovementComponentImpl : MovementComponent
    {
        [Export]
        private NodePath _wallDetectorNodePath;
        [Export]
        private NodePath _secondaryWallDetectorNodePath;
        private RayCast2D _wallDetector;
        private RayCast2D _secondaryWallDetector;
        [Export]
        private float _wallDetectorDistance = 16.0f;


        private Vector2 _currentDirection = Vector2.Zero;
        private Vector2 _targetDirection = Vector2.Zero;
        [Export]
        private Vector2 _bodyUpPosition;
        [Export]
        private Vector2 _bodyDownPosition;
        [Export]
        private Vector2 _bodyRightPosition;
        [Export]
        private Vector2 _bodyLeftPosition;
        private bool _stoppedMoving = false;


        public override void _Ready()
        {
            base._Ready();
            SetNodeReferences();
            CheckNodeReferences();
            SetWallDetectorPosition(Vector2.Left);
        }

        private void SetNodeReferences()
        {
            _wallDetector = GetNode<RayCast2D>(_wallDetectorNodePath);
            _secondaryWallDetector = GetNode<RayCast2D>(_secondaryWallDetectorNodePath);
        }

        private void CheckNodeReferences()
        {
            if (!_wallDetector.IsValid())
            {
                GD.PrintErr("ERROR: Movement Component Wall Detector is not valid!");
            }
            if (!_secondaryWallDetector.IsValid())
            {
                GD.PrintErr("ERROR: Movement Component Secondary Wall Detector is not valid!");
            }
        }

        private void SetWallDetectorPosition(Vector2 targetDirection)
        {
            if (targetDirection == Vector2.Down)
            {
                _wallDetector.Position = _bodyUpPosition;
            }
            else if (targetDirection == Vector2.Up)
            {
                _wallDetector.Position = _bodyDownPosition;
            }
            else if (targetDirection == Vector2.Left)
            {
                _wallDetector.Position = _bodyRightPosition;
            }
            else if (targetDirection == Vector2.Right)
            {
                _wallDetector.Position = _bodyLeftPosition;
            }
        }

        public override void _PhysicsProcess(float delta)
        {
            AttemptDirectionChange();
            if (BodyToMove.IsValid())
            {
                Vector2 velocity = _currentDirection * Speed;
                velocity = BodyToMove.MoveAndSlide(velocity);
                if (velocity == Vector2.Zero && !_stoppedMoving)
                {
                    _stoppedMoving = true;
                    EmitSignal("MovementStopped");
                }
                else if (velocity != Vector2.Zero)
                {
                    _stoppedMoving = false;
                }
            }
        }

        private void AttemptDirectionChange()
        {
            if (!_wallDetector.IsColliding() && !_secondaryWallDetector.IsColliding())
            {
                if (_currentDirection != _targetDirection)
                {
                    _currentDirection = _targetDirection;
                    SetWallDetectorPosition(_currentDirection);
                    EmitSignal("DirectionChanged", _currentDirection);
                }
            }
        }

        public override void ChangeDirection(Vector2 newDirection)
        {
            _targetDirection = newDirection;
            _wallDetector.CastTo = newDirection * _wallDetectorDistance;
            _secondaryWallDetector.CastTo = newDirection * _wallDetectorDistance;
        }

        public override void StopMoving()
        {
            _currentDirection = Vector2.Zero;
            _targetDirection = Vector2.Zero;
            EmitSignal("MovementStopped");
        }

        public override Vector2 GetCurrentDirection()
        {
            return _currentDirection;
        }

        public override void OverrideDirection(Vector2 newDirection)
        {
            _currentDirection = newDirection;
            ChangeDirection(newDirection);
            SetWallDetectorPosition(newDirection);
            EmitSignal("DirectionChanged", _currentDirection);
        }

    }
}