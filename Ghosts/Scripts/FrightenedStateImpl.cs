using Godot;
using Util;
using Util.ExtensionMethods;

namespace Game.Ghosts
{

    public class FrightenedStateImpl : FrightenedState
    {
        private bool _inIntersectionTile = false;
        private const int PATH_TILE_CELL_NUMBER = 2;

        public override void EnterState()
        {

        }

        public override void UpdateState(float delta)
        {
            if (GhostCollision.Fleeing)
            {
                EmitSignal("Transitioned", this, "ReturnState");
            }
            else if (!GhostCollision.Vulnerable)
            {
                EmitSignal("Transitioned", this, "PreviousState");
            }
            else
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
            GhostCollision.Vulnerable = false;
            _inIntersectionTile = false;
        }
    }
}