using Game.Levels;
using Godot;
using System;
using Util.ExtensionMethods;

namespace Game.Ghosts
{

    public abstract class IdleState : GhostState
    {
        private AnimationPlayer _idleAnimationPlayer;
        public AnimationPlayer IdleAnimationPlayer
        {
            get { return _idleAnimationPlayer; }
            set
            {
                if (value.IsValid())
                {
                    _idleAnimationPlayer = value;
                }
            }
        }
        private const string IDLE_ANIMATION_NAME = "Idle";

        private MovementComponent _movement;
        public MovementComponent Movement
        {
            get { return _movement; }
            set
            {
                if (value.IsValid())
                {
                    _movement = value;
                }
            }
        }
        private const int DOWN_TILE_CELL_NUMBER = 4;
        private Level _currentLevel;
        public Level CurrentLevel
        {
            set
            {
                if (value.IsValid())
                {
                    _currentLevel = value;
                }
            }
        }
        private Node2D _visualComponentReference;
        public Node2D VisualComponentReference
        {
            get { return _visualComponentReference;}
            set
            {
                if (value.IsValid())
                {
                    _visualComponentReference = value;
                }
            }
        }

        public override void EnterState()
        {
            _idleAnimationPlayer.Play(IDLE_ANIMATION_NAME);
        }

        public override void UpdateState(float delta)
        {
            if (HasLeftGhostHouse())
            {
                _movement.ChangeDirection(Vector2.Left);
                EmitSignal("Transitioned", this, "ScatterState");
            }
        }

        private bool HasLeftGhostHouse()
        {
            Vector2 localPosition = _currentLevel.ToLocal(_movement.BodyToMove.GlobalPosition);
            Vector2 mapPosition = _currentLevel.WorldToMap(localPosition);
            int cellNumber = _currentLevel.GetCell((int)mapPosition.x, (int)mapPosition.y);
            return cellNumber == DOWN_TILE_CELL_NUMBER;
        }
    }
}