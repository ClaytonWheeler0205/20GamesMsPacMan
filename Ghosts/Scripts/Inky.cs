using Game.Levels;
using Godot;
using Util.ExtensionMethods;

namespace Game.Ghosts
{

    public class Inky : IdleGhost
    {
        [Export]
        private NodePath _blinkyPath;
        private Node2D _blinkyReference;

        public override void _Ready()
        {
            base._Ready();
            SetNodeReferences();
            CheckNodeReferences();
            if (ChaseStateReference is InkyChaseState inkyChaseState)
            {
                inkyChaseState.BlinkyReference = _blinkyReference;
            }
        }

        private void SetNodeReferences()
        {
            _blinkyReference = GetNode<Node2D>(_blinkyPath);
        }

        private void CheckNodeReferences()
        {
            if (!_blinkyReference.IsValid())
            {
                GD.PrintErr("ERROR: Inky Blinky Reference is not valid!");
            }
        }
       
        public override void ResetGhost()
        {
            base.ResetGhost();
            Eyes.Play("look_up");
        }

        public override void SetLevelReference(Level level)
        {
            base.SetLevelReference(level);
            ScatterStateReference.HomeTilePosition = level.InkyHomeTilePosition;
        }


    }
}