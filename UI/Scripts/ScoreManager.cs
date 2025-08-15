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
        [Export]
        private NodePath _highScoreDisplayPath;
        private Label _highScoreDisplay;
        private int _score = 0;
        private int _highScore = 0;
        private bool _lifeGained = false;
        private const int LIFE_POINT_LIMIT = 10000;

        private ConfigFile _config = new ConfigFile();

        public override void _Ready()
        {
            SetNodeReferences();
            CheckNodeReferences();
            SetNodeConnections();
            LoadHighScore();
        }

        private void SetNodeReferences()
        {
            _scoreDisplay = GetNode<Label>(_scoreDisplayPath);
            _highScoreDisplay = GetNode<Label>(_highScoreDisplayPath);
        }

        private void CheckNodeReferences()
        {
            if (!_scoreDisplay.IsValid())
            {
                GD.PrintErr("ERROR: Score UI Score Display is not valid!");
            }
            if (!_highScoreDisplay.IsValid())
            {
                GD.PrintErr("ERROR Score UI High Score Display is not valid!");
            }
        }

        private void SetNodeConnections()
        {
            ScoreEventBus.Instance.Connect("AwardPoints", this, "OnAwardPoints");
        }

        private void LoadHighScore()
        {
            Error err = _config.Load("user://mspacman_highscore.cfg");

            if (err != Error.Ok)
            {
                _highScore = 0;
                return;
            }

            _highScore = (int)_config.GetValue("MsPacManPlayerScore", "mspacman_high_score");
            UpdateHighScore();
        }

        private void UpdateHighScore()
        {
            if (_highScoreDisplay.IsValid())
            {
                _highScoreDisplay.Text = $"{_highScore}";
            }
        }

        public void OnAwardPoints(int pointsToGive)
        {
            _score += pointsToGive;
            UpdateScore();
            if (_score >= LIFE_POINT_LIMIT && !_lifeGained)
            {
                _lifeGained = true;
                LifeEventBus.Instance.EmitSignal("LifeGained");
            }
            if (_score > _highScore)
            {
                _highScore = _score;
                UpdateHighScore();
                SaveHighScore();
            }
        }

        private void UpdateScore()
        {
            _scoreDisplay.Text = $"{_score}";
        }

        private void SaveHighScore()
        {
            _config.SetValue("MsPacManPlayerScore", "mspacman_high_score", _highScore);
            _config.Save("user://mspacman_highscore.cfg");
        }

    }
}