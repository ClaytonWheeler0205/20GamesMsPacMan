using System.Runtime.CompilerServices;
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
        private NodePath _chaseStatePath;
        private ChaseState _chaseStateReference;
        protected ChaseState ChaseStateReference
        {
            get { return _chaseStateReference; }
        }
        [Export]
        private NodePath _eyesPath;
        private AnimatedSprite _eyes;
        [Export]
        private NodePath _bodyVisualPath;
        private AnimatedSprite _bodyVisual;

        protected Vector2 startPosition;


        public override void _Ready()
        {
            SetNodeReferences();
            CheckNodeReferences();
            SetNodeConnections();
            _movementReference.BodyToMove = this;
            _scatterStateReference.Movement = _movementReference;
            _chaseStateReference.Movement = _movementReference;
            startPosition = GlobalPosition;
        }

        private void SetNodeReferences()
        {
            _movementReference = GetNode<MovementComponent>(_movementPath);
            _stateMachineReference = GetNode<GhostStateMachine>(_stateMachinePath);
            _scatterStateReference = GetNode<ScatterState>(_scatterStatePath);
            _chaseStateReference = GetNode<ChaseState>(_chaseStatePath);
            _eyes = GetNode<AnimatedSprite>(_eyesPath);
            _bodyVisual = GetNode<AnimatedSprite>(_bodyVisualPath);
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
            if (!_chaseStateReference.IsValid())
            {
                GD.PrintErr("ERROR: Ghost Chase State Reference is not valid!");
            }
            if (!_eyes.IsValid())
            {
                GD.PrintErr("ERROR: Ghost Eyes is not valid!");
            }
            if(!_bodyVisual.IsValid())
            {
                GD.PrintErr("ERROR: Ghost Body Visual is not valid!");
            }
        }

        private void SetNodeConnections()
        {
            _movementReference.Connect("DirectionChanged", this, "OnDirectionChanged");
            _movementReference.Connect("MovementStopped", this, "OnMovementStopped");
        }

        public abstract void StartGhost();
        public abstract void StopGhost();
        public abstract void ResetGhost();
        public abstract void SetLevelReference(Level level);
        public abstract void SetPlayerReference(MsPacMan player);

        public void OnDirectionChanged(Vector2 newDirection)
        {
            _bodyVisual.Play("move");
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

        public void OnMovementStopped()
        {
            _bodyVisual.Stop();
        }

    }
}