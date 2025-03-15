using Game.Bus;
using Godot;
using Util;
using Util.ExtensionMethods;

namespace Game.Ghosts
{

    public class FrightenedStateImpl : FrightenedState
    {
        private bool _inIntersectionTile = false;
        private const int PATH_TILE_CELL_NUMBER = 2;

        [Export]
        private NodePath _frightenedTimerPath;
        [Export]
        private NodePath _frightenedFlashTimerPath;
        private Timer _frightenedTimer;
        private Timer _frightenedFlashTimer;
        private float _frightenedTime = 7.0f;
        private float _frightenedFlashTime = 3.0f;

        public override void _Ready()
        {
            SetNodeReferences();
            CheckNodeReferences();
            SetNodeConnections();
        }

        private void SetNodeReferences()
        {
            _frightenedTimer = GetNode<Timer>(_frightenedTimerPath);
            _frightenedFlashTimer = GetNode<Timer>(_frightenedFlashTimerPath);
        }

        private void CheckNodeReferences()
        {
            if (!_frightenedTimer.IsValid())
            {
                GD.PrintErr("ERROR: Ghost Frightened Timer is not valid!");
            }
            if (!_frightenedFlashTimer.IsValid())
            {
                GD.PrintErr("ERROR: Ghost Frightened Flash Timer is not valid!");
            }
        }

        private void SetNodeConnections()
        {
            _frightenedTimer.Connect("timeout", this, nameof(OnFrightenedTimerTimeout));
            _frightenedFlashTimer.Connect("timeout", this, nameof(OnFrightenedFlashTimerTimeout));
            PelletEventBus.Instance.Connect("PowerPelletCollected", this, nameof(OnPowerPelletCollected));
        }

        public void OnFrightenedTimerTimeout()
        {
            _frightenedFlashTimer.Start(_frightenedFlashTime);
            EmitSignal("FrightenedFlashStarted");
        }

        public void OnFrightenedFlashTimerTimeout()
        {
            EmitSignal("Transitioned", this, "PreviousState");
        }

        public void OnPowerPelletCollected()
        {
            if (!_frightenedTimer.IsStopped() || !_frightenedFlashTimer.IsStopped())
            {
                EmitSignal("FrightenedStateEntered");
                _frightenedTimer.Start(_frightenedTime);
                _frightenedFlashTimer.Stop();
            }
        }

        public override void EnterState()
        {
            EmitSignal("FrightenedStateEntered");
            _frightenedTimer.Start(_frightenedTime);
        }

        public override void UpdateState(float delta)
        {
            if (CurrentLevel.IsValid())
            {
                if (IntersectionDetector.IsAtIntersection(Movement.BodyToMove.GlobalPosition) && !_inIntersectionTile)
                {
                    _inIntersectionTile = true;
                    Movement.ChangeDirection(GetRandomDirection());
                }
                if (!IntersectionDetector.IsAtIntersection(Movement.BodyToMove.GlobalPosition))
                {
                    _inIntersectionTile = false;
                }
            }
        }

        private Vector2 GetRandomDirection()
        {
            Vector2 localPosition = CurrentLevel.ToLocal(Movement.BodyToMove.GlobalPosition);
            Vector2 mapPosition = CurrentLevel.WorldToMap(localPosition);
            Vector2 newDirection = Vector2.Zero;
            bool directionFound = false;

            while (!directionFound)
            {
                int randomIndex = GDRandom.RandiRange(1, 4);
                switch (randomIndex)
                {
                    case 1:
                        Vector2 mapPositionUp = new Vector2(mapPosition.x, mapPosition.y - 1);
                        int cellNumberUp = CurrentLevel.GetCell((int)mapPositionUp.x, (int)mapPositionUp.y);
                        if (cellNumberUp == PATH_TILE_CELL_NUMBER)
                        {
                            newDirection = Vector2.Up;
                            directionFound = true;
                        }
                        break;
                    case 2:
                        Vector2 mapPositionLeft = new Vector2(mapPosition.x - 1, mapPosition.y);
                        int cellNumberLeft = CurrentLevel.GetCell((int)mapPositionLeft.x, (int)mapPositionLeft.y);
                        if (cellNumberLeft == PATH_TILE_CELL_NUMBER)
                        {
                            newDirection = Vector2.Left;
                            directionFound = true;
                        }
                        break;
                    case 3:
                        Vector2 mapPositionDown = new Vector2(mapPosition.x, mapPosition.y + 1);
                        int cellNumberDown = CurrentLevel.GetCell((int)mapPositionDown.x, (int)mapPositionDown.y);
                        if (cellNumberDown == PATH_TILE_CELL_NUMBER)
                        {
                            newDirection = Vector2.Down;
                            directionFound = true;
                        }
                        break;
                    case 4:
                        Vector2 mapPositionRight = new Vector2(mapPosition.x + 1, mapPosition.y);
                        int cellNumberRight = CurrentLevel.GetCell((int)mapPositionRight.x, (int)mapPositionRight.y);
                        if (cellNumberRight == PATH_TILE_CELL_NUMBER)
                        {
                            newDirection = Vector2.Right;
                            directionFound = true;
                        }
                        break;
                }
            }

            return newDirection;
        }

        public override void ExitState()
        {
            _frightenedTimer.Stop();
            _frightenedFlashTimer.Stop();
            EmitSignal("FrightenedStateExited");
        }
    }
}