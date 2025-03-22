using Game.Bus;
using Game.Ghosts;
using Game.Levels;
using Godot;
using Util.ExtensionMethods;

public class ScatterStateImpl : ScatterState
{
    private bool _inIntersectionTile = false;
    private const int PATH_TILE_CELL_NUMBER = 2;
    private const int SPECIAL_TURN_TILE_CELL_NUMBER = 3;
    [Export]
    private NodePath _scatterTimerPath;
    private Timer _scatterTimer;
    private float _scatterTime = 7.0f;

    public override void _Ready()
    {
        SetNodeReferences();
        CheckNodeReferences();
        SetNodeConnections();
    }

    private void SetNodeReferences()
    {
        _scatterTimer = GetNode<Timer>(_scatterTimerPath);
    }

    private void CheckNodeReferences()
    {
        if (!_scatterTimer.IsValid())
        {
            GD.PrintErr("ERROR: Scatter State Scatter Timer is not valid!");
        }
    }

    private void SetNodeConnections()
    {
        LevelEventBus.Instance.Connect("LevelCleared", this, nameof(OnLevelCleared));
        _scatterTimer.Connect("timeout", this, nameof(OnTimerTimeout));
    }

    public void OnLevelCleared()
    {
        _scatterTimer.Paused = false;
        _scatterTimer.Stop();
    }

    public void OnTimerTimeout()
    {
        EmitSignal("Transitioned", this, "ChaseState");
    }

    public override void EnterState()
    {
        if (!_scatterTimer.Paused)
        {
            _scatterTimer.Start(_scatterTime);
        }
        else
        {
            _scatterTimer.Paused = false;
        }
    }

    public override void UpdateState(float delta)
    {
        if (GhostCollision.Vulnerable)
        {
            EmitSignal("Transitioned", this, "FrightenedState");
        }
        else
        {
            if (CurrentLevel.IsValid())
            {
                if (IntersectionDetector.IsAtIntersection(Movement.BodyToMove.GlobalPosition) && !_inIntersectionTile)
                {
                    _inIntersectionTile = true;
                    Movement.ChangeDirection(FindShortestPathToHome());
                }
                if (!IntersectionDetector.IsAtIntersection(Movement.BodyToMove.GlobalPosition))
                {
                    _inIntersectionTile = false;
                }
            }
        }
    }

    private Vector2 FindShortestPathToHome()
    {
        Vector2 localPosition = CurrentLevel.ToLocal(Movement.BodyToMove.GlobalPosition);
        Vector2 mapPosition = CurrentLevel.WorldToMap(localPosition);
        int cellNumber = CurrentLevel.GetCell((int)mapPosition.x, (int)mapPosition.y);
        float minDistance = float.PositiveInfinity;
        Vector2 newDirection = Vector2.Zero;
        Vector2 currentDirection = Movement.GetCurrentDirection();

        // Priority is Up, Left, Down, Right
        if (currentDirection != Vector2.Down && cellNumber != SPECIAL_TURN_TILE_CELL_NUMBER)
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
        if (currentDirection != Vector2.Right)
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
        if (currentDirection != Vector2.Up)
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
        if (currentDirection != Vector2.Left)
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

    public override void ExitState()
    {
        DirectionReverser.ReverseDirection(Movement);
        //_inIntersectionTile = false; <- uncomment this is you notice any weird behavior in exiting states
    }
}
