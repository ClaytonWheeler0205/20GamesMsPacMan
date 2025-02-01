using Game.Ghosts;
using Godot;
using System;
using Util.ExtensionMethods;

public class ScatterStateImpl : ScatterState
{
    private bool _inIntersectionTile = false;
    private const int TURN_TILE_CELL_NUMBER = 1;
    private const int PATH_TILE_CELL_NUMBER = 2;

    public override void EnterState()
    {

    }

    public override void UpdateState(float delta)
    {
        if (CurrentLevel.IsValid())
        {
            if (IsAtIntersection() && !_inIntersectionTile)
            {
                _inIntersectionTile = true;
                Movement.ChangeDirection(FindShortestPathToHome());
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
        return cellNumber == TURN_TILE_CELL_NUMBER;
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

    private Vector2 FindShortestPathToHome()
    {
        Vector2 localPosition = CurrentLevel.ToLocal(Movement.BodyToMove.GlobalPosition);
        Vector2 mapPosition = CurrentLevel.WorldToMap(localPosition);
        float minDistance = float.PositiveInfinity;
        Vector2 newDirection = Vector2.Zero;
        // Priority is Up, Left, Down, Right
        if (Movement.GetCurrentDirection() != Vector2.Down)
        {
            Vector2 mapPositionUp = new Vector2(mapPosition.x, mapPosition.y - 1);
            int cellNumberUp = CurrentLevel.GetCell((int)mapPositionUp.x, (int)mapPositionUp.y);
            if (cellNumberUp == PATH_TILE_CELL_NUMBER)
            {
                float distance = HomeTilePosition.DistanceTo(mapPositionUp);
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
                float distance = HomeTilePosition.DistanceTo(mapPositionLeft);
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
                float distance = HomeTilePosition.DistanceTo(mapPositionDown);
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
                float distance = HomeTilePosition.DistanceTo(mapPositionRight);
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
}
