using Game.Pellets;
using Godot;
using Util.ExtensionMethods;
using Game.Fruits;

namespace Game.Levels
{

    public abstract class Level : TileMap
    {
        [Signal]
        public delegate void LevelFlashFinished();
        [Export]
        private NodePath _levelFlashPath;
        private CanvasItem _levelFlash;
        protected CanvasItem LevelFlash
        {
            get { return _levelFlash; }
        }
        [Export]
        private NodePath _levelFlashPlayerPath;
        private AnimationPlayer _levelFlashPlayer;
        protected AnimationPlayer LevelFlashPlayer
        {
            get { return _levelFlashPlayer; }
        }
        [Export]
        private NodePath _pelletsPath;
        private PelletContainer _pellets;
        protected PelletContainer Pellets
        {
            get { return _pellets; }
        }
        [Export]
        private Vector2 _blinkyHomeTilePosition;
        public Vector2 BlinkyHomeTilePosition
        {
            get { return _blinkyHomeTilePosition; }
        }
        [Export]
        private Vector2 _pinkyHomeTilePosition;
        public Vector2 PinkyHomeTilePosition
        {
            get { return _pinkyHomeTilePosition; }
        }
        [Export]
        private Vector2 _inkyHomeTilePosition;
        public Vector2 InkyHomeTilePosition
        {
            get { return _inkyHomeTilePosition; }
        }
        [Export]
        private Vector2 _clydeHomeTilePosition;
        public Vector2 ClydeHomeTilePosition
        {
            get { return _clydeHomeTilePosition; }
        }

        [Export]
        private Vector2 _ghostHousePosition;
        public Vector2 GhostHousePosition
        {
            get { return _ghostHousePosition; }
        }

        public override void _Ready()
        {
            SetNodeReferences();
            CheckNodeReferences();
        }

        private void SetNodeReferences()
        {
            _levelFlash = GetNode<CanvasItem>(_levelFlashPath);
            _levelFlashPlayer = GetNode<AnimationPlayer>(_levelFlashPlayerPath);
            _pellets = GetNode<PelletContainer>(_pelletsPath);
        }

        private void CheckNodeReferences()
        {
            if (!_levelFlash.IsValid())
            {
                GD.PrintErr("ERROR: Level Level Flash is not valid!");
            }
            if (!_levelFlashPlayer.IsValid())
            {
                GD.PrintErr("ERROR: Level Level Flash Player is not valid!");
            }
        }

        public abstract void PlayLevelFlash();
        public abstract void ResetLevel();
        public abstract void DestroyTunnels();
        public abstract void SpawnFruit(int fruitIndex);
        public abstract void OnFruitPathCompleted();
        public abstract void OnFruitCollected(Fruit fruit);
    }
}