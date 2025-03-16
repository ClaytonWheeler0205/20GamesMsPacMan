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
        private NodePath _eyesPath;
        private AnimatedSprite _eyes;
        protected AnimatedSprite Eyes
        {
            get { return _eyes; }
        }
        [Export]
        private NodePath _bodyVisualPath;
        private AnimatedSprite _bodyVisual;
        [Export]
        private NodePath _frightenedBodyVisualPath;
        private AnimatedSprite _frightenedBodyVisual;
        [Export]
        private NodePath _frightenedFlashVisualPath;
        private AnimatedSprite _frightenedFlashVisual;
        [Export]
        private NodePath _frightenedFlashTimerPath;
        private Timer _frightenedFlashTimer;
        private float _frightenedFlashTime;

        [Export]
        private NodePath _ghostCollisionPath;
        private GhostCollisionHandler _ghostCollision;

        protected Vector2 startPosition;

        public override void _Ready()
        {
            SetNodeReferences();
            CheckNodeReferences();
            SetNodeConnections();
            _movementReference.BodyToMove = this;
            _scatterStateReference.Movement = _movementReference;
            _chaseStateReference.Movement = _movementReference;
            _frightenedStateReference.Movement = _movementReference;
            startPosition = GlobalPosition;
        }

        private void SetNodeReferences()
        {
            _movementReference = GetNode<MovementComponent>(_movementPath);
            _stateMachineReference = GetNode<GhostStateMachine>(_stateMachinePath);
            _scatterStateReference = GetNode<ScatterState>(_scatterStatePath);
            _chaseStateReference = GetNode<ChaseState>(_chaseStatePath);
            _frightenedStateReference = GetNode<FrightenedState>(_frightenedStatePath);
            _eyes = GetNode<AnimatedSprite>(_eyesPath);
            _bodyVisual = GetNode<AnimatedSprite>(_bodyVisualPath);
            _frightenedBodyVisual = GetNode<AnimatedSprite>(_frightenedBodyVisualPath);
            _frightenedFlashVisual = GetNode<AnimatedSprite>(_frightenedFlashVisualPath);
            _frightenedFlashTimer = GetNode<Timer>(_frightenedFlashTimerPath);
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
            if (!_frightenedFlashTimer.IsValid())
            {
                GD.PrintErr("ERROR: Ghost Frightened Flash Timer is not valid!");
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
            _frightenedStateReference.Connect("FrightenedStateEntered", this, nameof(OnFrightenedStateEntered));
            _frightenedStateReference.Connect("FrightenedFlashStarted", this, nameof(OnFrightenedFlashStarted));
            _frightenedStateReference.Connect("FrightenedStateExited", this, nameof(OnFrightenedStateExited));
            _frightenedFlashTimer.Connect("timeout", this, nameof(OnFrightenedFlashTimerTimeout));
        }

        public abstract void StartGhost();
        public abstract void StopGhost();
        public abstract void ResetGhost();
        public abstract void SetLevelReference(Level level);
        public abstract void SetPlayerReference(MsPacMan player);

        public void OnDirectionChanged(Vector2 newDirection)
        {
            if (_bodyVisual.Visible)
            {
                _bodyVisual.Play("move");
            }
            else if (_frightenedBodyVisual.Visible)
            {
                _frightenedBodyVisual.Play("frightened_move");
            }

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
            _frightenedBodyVisual.Stop();
        }

        public void OnFrightenedStateEntered()
        {
            _bodyVisual.Stop();
            _bodyVisual.Visible = false;
            _eyes.Visible = false;
            _frightenedBodyVisual.Frame = _bodyVisual.Frame;
            _frightenedFlashVisual.Frame = _bodyVisual.Frame;
            _frightenedBodyVisual.Visible = true;
            _frightenedFlashVisual.Visible = false;
            _frightenedBodyVisual.Play("frightened_move");
            _frightenedFlashVisual.Play("frightened_flash_move");
            _frightenedFlashTimer.Stop();
            _ghostCollision.Vulnerable = true;
        }

        public void OnFrightenedFlashStarted()
        {
            _frightenedFlashVisual.Visible = true;
            _frightenedFlashTimer.Start(_frightenedFlashTime);
        }

        public void OnFrightenedFlashTimerTimeout()
        {
            _frightenedFlashVisual.Visible = !_frightenedFlashVisual.Visible;
        }

        public void OnFrightenedStateExited()
        {
            _frightenedBodyVisual.Stop();
            _frightenedFlashVisual.Stop();
            _frightenedBodyVisual.Visible = false;
            _frightenedFlashVisual.Visible = false;
            _frightenedFlashTimer.Stop();
            _bodyVisual.Frame = _frightenedBodyVisual.Frame;
            _bodyVisual.Visible = true;
            _eyes.Visible = true;
            _bodyVisual.Play("move");
            _ghostCollision.Vulnerable = false;
        }

    }
}