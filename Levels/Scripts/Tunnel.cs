using Game.Ghosts;
using Godot;
using System;

public class Tunnel : Area2D
{
    public void OnTunnelEntered(Node body)
    {
        if (body is Ghost ghost)
        {
            ghost.SlowDownGhost();
        }
    }

    public void OnTunnelExited(Node body)
    {
        if (body is Ghost ghost)
        {
            ghost.SpeedupGhost();
        }
    }
}
