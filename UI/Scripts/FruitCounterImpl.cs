using Godot;
using Util.ExtensionMethods;

namespace Game.UI
{

    public class FruitCounterImpl : FruitCounter
    {
        private int _fruitCounter = 5;
        [Export]
        private NodePath _fruitContainerPath;
        private Node _fruitContainerReference;

        public override void _Ready()
        {
            SetNodeReferences();
            CheckNodeReferences();
        }

        private void SetNodeReferences()
        {
            _fruitContainerReference = GetNode<Node>(_fruitContainerPath);
        }

        private void CheckNodeReferences()
        {
            if (!_fruitContainerReference.IsValid())
            {
                GD.PrintErr("ERROR: Fruit UI Fruit Container Reference is not valid!");
            }
        }

        public override void IncreaseFruitCounter()
        {
            if (_fruitCounter >= 0)
            {
                CanvasItem fruitIcon = _fruitContainerReference.GetChild(_fruitCounter) as CanvasItem;
                fruitIcon.Visible = true;
                _fruitCounter--;
            }
        }
    }
}