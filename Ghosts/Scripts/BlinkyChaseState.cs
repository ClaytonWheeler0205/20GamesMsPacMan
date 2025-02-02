using Godot;
using Util.ExtensionMethods;

namespace Game.Ghosts
{

    public class BlinkyChaseState : ChaseState
    {
        private bool _inIntersectionTile = false;
        private const int TURN_TILE_CELL_NUMBER = 1;
        private const int PATH_TILE_CELL_NUMBER = 2;
        private const int SPECIAL_TURN_TILE_CELL_NUMBER = 3;
        [Export]
        private NodePath _chasetimerPath;
        private Timer _chaseTimer;
        private float _chaseTime = 20.0f;

        public override void _Ready()
        {
            SetNodeReferences();
            CheckNodeReferences();
        }

        private void SetNodeReferences()
        {
            _chaseTimer = GetNode<Timer>(_chasetimerPath);
        }

        private void CheckNodeReferences()
        {
            if (!_chaseTimer.IsValid())
            {
                GD.PrintErr("ERROR: Chase State Chase Timer is not valid!");
            }
        }

        public override void EnterState()
        {
            _chaseTimer.Start(_chaseTime);
        }

        public override void UpdateState(float delta)
        {
            if (CurrentLevel.IsValid())
            {
                if (IsAtIntersection() && !_inIntersectionTile)
                {
                    _inIntersectionTile = true;
                    Movement.ChangeDirection(FindShortestPathToPlayer());
                }
                if (!IsAtIntersection())
                {
                    _inIntersectionTile = false;
                }
            }
        }

        private bool IsAtIntersection()
        {
            Vector2 localPosition = CurrentLevel.ToLocal(Movement.BodyToMove.GlobalPosition);
            Vector2 mapPosition = CurrentLevel.WorldToMap(localPosition);
            int cellNumber = CurrentLevel.GetCell((int)mapPosition.x, (int)mapPosition.y);
            return cellNumber == TURN_TILE_CELL_NUMBER || cellNumber == SPECIAL_TURN_TILE_CELL_NUMBER;
        }

        private void ReverseDirection()
        {
            if (Movement.GetCurrentDirection() == Vector2.Up)
            {
                Movement.ChangeDirection(Vector2.Down);
            }
            else if (Movement.GetCurrentDirection() == Vector2.Down)
            {
                Movement.ChangeDirection(Vector2.Up);
            }
            else if (Movement.GetCurrentDirection() == Vector2.Right)
            {
                Movement.ChangeDirection(Vector2.Left);
            }
            else if (Movement.GetCurrentDirection() == Vector2.Left)
            {
                Movement.ChangeDirection(Vector2.Right);
            }
        }

        private Vector2 FindShortestPathToPlayer()
        {
            Vector2 localPosition = CurrentLevel.ToLocal(Movement.BodyToMove.GlobalPosition);
            Vector2 mapPosition = CurrentLevel.WorldToMap(localPosition);
            Vector2 localPlayerPosition = CurrentLevel.ToLocal(Player.GlobalPosition);
            Vector2 playerMapPosition = CurrentLevel.WorldToMap(localPlayerPosition);
            int cellNumber = CurrentLevel.GetCell((int)mapPosition.x, (int)mapPosition.y);
            float minDistance = float.PositiveInfinity;
            Vector2 newDirection = Vector2.Zero;
            // Priority is Up, Left, Down, Right
            if (Movement.GetCurrentDirection() != Vector2.Down && cellNumber != SPECIAL_TURN_TILE_CELL_NUMBER)
            {
                Vector2 mapPositionUp = new Vector2(mapPosition.x, mapPosition.y - 1);
                int cellNumberUp = CurrentLevel.GetCell((int)mapPositionUp.x, (int)mapPositionUp.y);
                if (cellNumberUp == PATH_TILE_CELL_NUMBER)
                {
                    float distance = playerMapPosition.DistanceTo(mapPositionUp);
                    if (distance < minDistance)
                    {
                        newDirection = Vector2.Up;
                        minDistance = distance;
                    }
                }
            }
            if (Movement.GetCurrentDirection() != Vector2.Right)
            {
                Vector2 mapPositionLeft = new Vector2(mapPosition.x - 1, mapPosition.y);
                int cellNumberLeft = CurrentLevel.GetCell((int)mapPositionLeft.x, (int)mapPositionLeft.y);
                if (cellNumberLeft == PATH_TILE_CELL_NUMBER)
                {
                    float distance = playerMapPosition.DistanceTo(mapPositionLeft);
                    if (distance < minDistance)
                    {
                        newDirection = Vector2.Left;
                        minDistance = distance;
                    }
                }
            }
            if (Movement.GetCurrentDirection() != Vector2.Up)
            {
                Vector2 mapPositionDown = new Vector2(mapPosition.x, mapPosition.y + 1);
                int cellNumberDown = CurrentLevel.GetCell((int)mapPositionDown.x, (int)mapPositionDown.y);
                if (cellNumberDown == PATH_TILE_CELL_NUMBER)
                {
                    float distance = playerMapPosition.DistanceTo(mapPositionDown);
                    if (distance < minDistance)
                    {
                        newDirection = Vector2.Down;
                        minDistance = distance;
                    }
                }
            }
            if (Movement.GetCurrentDirection() != Vector2.Left)
            {
                Vector2 mapPositionRight = new Vector2(mapPosition.x + 1, mapPosition.y);
                int cellNumberRight = CurrentLevel.GetCell((int)mapPositionRight.x, (int)mapPositionRight.y);
                if (cellNumberRight == PATH_TILE_CELL_NUMBER)
                {
                    float distance = playerMapPosition.DistanceTo(mapPositionRight);
                    if (distance < minDistance)
                    {
                        newDirection = Vector2.Right;
                    }
                }
            }

            return newDirection;
        }

        public override void PhysicsUpdateState(float delta)
        {

        }

        public override void ExitState()
        {
            ReverseDirection();
            _inIntersectionTile = false;
        }

        public void OnTimerTimeout()
        {
            EmitSignal("Transitioned", this, "ScatterState");
        }

    }
}