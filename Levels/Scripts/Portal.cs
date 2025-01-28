using Godot;
using System;
using Util.ExtensionMethods;

namespace Game.Levels
{

    public class Portal : Area2D
    {
        [Export]
        private NodePath _exitPointPath;
        private Position2D _exitPoint;
        public Position2D ExitPoint
        {
            get { return _exitPoint; }
        }
        [Export]
        private NodePath _destinationPath;
        private Portal _destination;

        private const string PLAYER_NODE_GROUP = "Player";

        public override void _Ready()
        {
            SetNodeReferences();
            CheckNodeReferences();
        }

        private void SetNodeReferences()
        {
            _exitPoint = GetNode<Position2D>(_exitPointPath);
            _destination = GetNode<Portal>(_destinationPath);
        }

        private void CheckNodeReferences()
        {
            if (!_exitPoint.IsValid())
            {
                GD.PrintErr("ERROR Portal Exit Point is not valid!");
            }
            if (!_destination.IsValid())
            {
                GD.PrintErr("ERROR: Portal Destination is not valid!");
            }
        }

        public void OnBodyEntered(Node body)
        {
            if (body.IsInGroup(PLAYER_NODE_GROUP))
            {
                if (body is Node2D body2D)
                {
                    body2D.GlobalPosition = _destination.ExitPoint.GlobalPosition;
                }
            }
        }
    }
}