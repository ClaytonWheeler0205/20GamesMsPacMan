using Game.Bus;
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
        private NodePath _frightenedStatePath;
        private FrightenedState _frightenedStateReference;
        protected FrightenedState FrightenedStateReference
        {
            get { return _frightenedStateReference; }
        }
        [Export]
        private NodePath _returnStatePath;
        private ReturnState _returnStateReference;
        protected ReturnState ReturnStateReference
        {
            get { return _returnStateReference; }
        }

        [Export]
        private NodePath _eyesPath;
        private AnimatedSprite _eyes;
        protected AnimatedSprite Eyes
        {
            get { return _eyes; }
        }
        [Export]
        private NodePath _bodyVisualPath;
        private AnimatedSprite _bodyVisual;
        protected AnimatedSprite BodyVisual
        {
            get { return _bodyVisual; }
        }
        protected const string MOVE_ANIMATION = "move";
        [Export]
        private NodePath _frightenedBodyVisualPath;
        private AnimatedSprite _frightenedBodyVisual;
        protected AnimatedSprite FrightenedBodyVisual
        {
            get { return _frightenedBodyVisual; }
        }
        protected const string FRIGHTENED_MOVE_ANIMATION = "frightened_move";
        [Export]
        private NodePath _frightenedFlashVisualPath;
        private AnimatedSprite _frightenedFlashVisual;
        protected AnimatedSprite FrightenedFlashVisual
        {
            get { return _frightenedFlashVisual; }
        }
        protected const string FRIGHTENED_FLASH_MOVE_ANIMATION = "frightened_flash_move";

        [Export]
        private NodePath _ghostCollisionPath;
        private GhostCollisionHandler _ghostCollision;
        protected GhostCollisionHandler GhostCollision
        {
            get { return _ghostCollision; }
        }

        protected Vector2 startPosition;
        protected Vector2 previousDirection = Vector2.Zero;

        public override void _Ready()
        {
            SetNodeReferences();
            CheckNodeReferences();
            SetNodeConnections();
            _movementReference.BodyToMove = this;
            _scatterStateReference.Movement = _movementReference;
            _scatterStateReference.GhostCollision = _ghostCollision;
            _chaseStateReference.Movement = _movementReference;
            _chaseStateReference.GhostCollision = _ghostCollision;
            _frightenedStateReference.Movement = _movementReference;
            _frightenedStateReference.GhostCollision = _ghostCollision;
            _returnStateReference.Movement = _movementReference;
            startPosition = GlobalPosition;
        }

        private void SetNodeReferences()
        {
            _movementReference = GetNode<MovementComponent>(_movementPath);

            _stateMachineReference = GetNode<GhostStateMachine>(_stateMachinePath);
            _scatterStateReference = GetNode<ScatterState>(_scatterStatePath);
            _chaseStateReference = GetNode<ChaseState>(_chaseStatePath);
            _frightenedStateReference = GetNode<FrightenedState>(_frightenedStatePath);
            _returnStateReference = GetNode<ReturnState>(_returnStatePath);

            _eyes = GetNode<AnimatedSprite>(_eyesPath);
            _bodyVisual = GetNode<AnimatedSprite>(_bodyVisualPath);
            _frightenedBodyVisual = GetNode<AnimatedSprite>(_frightenedBodyVisualPath);
            _frightenedFlashVisual = GetNode<AnimatedSprite>(_frightenedFlashVisualPath);

            _ghostCollision = GetNode<GhostCollisionHandler>(_ghostCollisionPath);
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
            if (!_frightenedStateReference.IsValid())
            {
                GD.PrintErr("ERROR: Ghost Frightened State Reference is not valid!");
            }
            if (!_returnStateReference.IsValid())
            {
                GD.PrintErr("ERROR: Ghost Return State Reference is not valid!");
            }
            if (!_eyes.IsValid())
            {
                GD.PrintErr("ERROR: Ghost Eyes is not valid!");
            }
            if (!_bodyVisual.IsValid())
            {
                GD.PrintErr("ERROR: Ghost Body Visual is not valid!");
            }
            if (!_frightenedBodyVisual.IsValid())
            {
                GD.PrintErr("ERROR Ghost Frightened Body Visual is not valid!");
            }
            if (!_frightenedFlashVisual.IsValid())
            {
                GD.PrintErr("ERROR: Ghost Frightened Flash Visual is not valid!");
            }
            if (!_ghostCollision.IsValid())
            {
                GD.PrintErr("ERROR: Ghost Collision is not valid!");
            }
        }

        private void SetNodeConnections()
        {
            _movementReference.Connect("DirectionChanged", this, nameof(OnDirectionChanged));
            _movementReference.Connect("MovementStopped", this, nameof(OnMovementStopped));
            _ghostCollision.Connect("GhostEaten", this, nameof(OnGhostEaten));
            _returnStateReference.Connect("ReturnStateEntered", this, nameof(OnReturnStateEntered));
            _returnStateReference.Connect("ReturnStateExited", this, nameof(OnReturnStateExited));
            _returnStateReference.Connect("GhostHouseEntered", this, nameof(OnGhostHouseEntered));
        }

        public abstract void StartGhost();
        public abstract void StopGhost();
        public abstract void ResetGhost();
        public abstract void SetLevelReference(Level level);
        public abstract void SetPlayerReference(MsPacMan player);
        public abstract void PauseGhost();
        public abstract void ResumeGhost();
        public abstract void SetGhostVulnerability();
        public abstract void SetGhostFleeing();
        public abstract void SetGhostInvulnerable();
        public abstract void SetGhostFlash();
        public abstract void SlowDownGhost();
        public abstract void SpeedupGhost();
        public abstract void OnSpeedChangeRequested(float newSpeed);
        public abstract void OnDirectionChanged(Vector2 newDirection);
        public abstract void OnMovementStopped();
        public abstract void OnGhostEaten();
        public abstract void OnReturnStateEntered();
        public abstract void OnGhostHouseEntered();
        public abstract void OnReturnStateExited();
    }
}