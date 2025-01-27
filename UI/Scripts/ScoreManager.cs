using Game.Bus;
using Godot;
using Util.ExtensionMethods;

namespace Game.UI
{

    public class ScoreManager : Control
    {
        [Export]
        private NodePath _scoreDisplayPath;
        private Label _scoreDisplay;
        private int _score = 0;

        public override void _Ready()
        {
            SetNodeReferences();
            CheckNodeReferences();
            SetNodeConnections();
        }

        private void SetNodeReferences()
        {
            _scoreDisplay = GetNode<Label>(_scoreDisplayPath);
        }

        private void CheckNodeReferences()
        {
            if (!_scoreDisplay.IsValid())
            {
                GD.PrintErr("ERROR: Score UI Score Display is not valid!");
            }
        }

        private void SetNodeConnections()
        {
            ScoreEventBus.Instance.Connect("AwardPoints", this, "OnAwardPoints");
        }

        public void OnAwardPoints(int pointsToGive)
        {
            _score += pointsToGive;
            UpdateScore();
        }

        private void UpdateScore()
        {
            _scoreDisplay.Text = $"{_score}";
        }

    }
}