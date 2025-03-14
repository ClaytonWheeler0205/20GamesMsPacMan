using Godot;
using System;
using Util.ExtensionMethods;

public class PelletEventBus : Node
{
    public static PelletEventBus Instance { get; private set; }

    [Signal]
    public delegate void PowerPelletCollected();

    public override void _Ready()
    {
        if (Instance != null && Instance != this)
        {
            this.SafeQueueFree();
        }
        else
        {
            Instance = this;
        }
    }

}
