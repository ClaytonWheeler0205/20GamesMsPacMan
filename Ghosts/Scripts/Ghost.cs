using Game.Levels;
using Game.Player;
using Godot;
using Util.ExtensionMethods;

namespace Game.Ghosts
{

    public abstract class Ghost : KinematicBody2D
    {

        [Export]
        private NodePath _movementPath;
        private MovementComponent _movementReference;
        protected MovementComponent MovementReference
        {
            get { return _movementReference; }
        }
        [Export]
        private NodePath _stateMachinePath;
        private GhostStateMachine _stateMachineReference;
        protected GhostStateMachine StateMachineReference
        {
            get { return _stateMachineReference; }
        }
        [Export]
        private NodePath _scatterStatePath;
        private ScatterState _scatterStateReference;
        protected ScatterState ScatterStateReference
        {
            get { return _scatterStateReference; }
        }
        [Export]
        private NodePath _eyesPath;
        private AnimatedSprite _eyes;

        private MsPacMan _playerReference;
        public MsPacMan PlayerReference
        {
            set
            {
                if (value.IsValid())
                {
                    _playerReference = value;
                }
            }
        }


        public override void _Ready()
        {
            SetNodeReferences();
            CheckNodeReferences();
            SetNodeConnections();
            _movementReference.BodyToMove = this;
            _scatterStateReference.Movement = _movementReference;
        }

        private void SetNodeReferences()
        {
            _movementReference = GetNode<MovementComponent>(_movementPath);
            _stateMachineReference = GetNode<GhostStateMachine>(_stateMachinePath);
            _scatterStateReference = GetNode<ScatterState>(_scatterStatePath);
            _eyes = GetNode<AnimatedSprite>(_eyesPath);
        }

        private void CheckNodeReferences()
        {
            if (!_movementReference.IsValid())
            {
                GD.PrintErr("ERROR: Ghost Movement Reference is not valid!");
            }
            if (!_stateMachineReference.IsValid())
            {
                GD.PrintErr("ERROR: Ghost State Machine Reference is not valid!");
            }
            if (!_scatterStateReference.IsValid())
            {
                GD.PrintErr("ERROR: Ghost Scatter State Reference is not valid!");
            }
            if (!_eyes.IsValid())
            {
                GD.PrintErr("ERROR: Ghost Eyes is not valid!");
            }
        }

        private void SetNodeConnections()
        {
            _movementReference.Connect("DirectionChanged", this, "OnDirectionChanged");
        }

        public abstract void StartGhost();
        public abstract void SetLevelReference(Level level);

        public void OnDirectionChanged(Vector2 newDirection)
        {
            if (newDirection == Vector2.Up)
            {
                _eyes.Play("look_up");
            }
            else if (newDirection == Vector2.Down)
            {
                _eyes.Play("look_down");
            }
            else if (newDirection == Vector2.Left)
            {
                _eyes.Play("look_left");
            }
            else if (newDirection == Vector2.Right)
            {
                _eyes.Play("look_right");
            }
        }

    }
}