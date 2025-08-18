using System;
using Game.Fruits;
using Godot;
using Util;
using Util.ExtensionMethods;

namespace Game.Levels
{

    public class LevelImpl : Level
    {
        private const string LEVEL_FLASH_ANIMATION_NAME = "LevelFlash";
        [Export]
        private NodePath _tunnelContainerPath;
        private Node _tunnelContainerReference;

        private int _fruitEntranceIndex;
        private int _fruitExitIndex;
        [Export]
        private NodePath _fruitEntrancesPath;
        private Node _fruitEntrancesReference;
        [Export]
        private NodePath _fruitLoopsPath;
        private Node _fruitLoopsReference;
        [Export]
        private NodePath _loopsToExitsPath;
        private Node _loopsToExitsReference;
        [Export]
        private NodePath _fruitExitsPath;
        private Node _fruitExitsReference;
        // 0 for entrance, 1 for loop, 2 for loop-to-exit, 3 for exit
        private int _pathNumber = 0;
        private Fruit _fruit;
        private bool _fruitExists = false;

        public override void _Ready()
        {
            base._Ready();
            SetNodeReferences();
            CheckNodeReferences();
        }

        private void SetNodeReferences()
        {
            _tunnelContainerReference = GetNode<Node>(_tunnelContainerPath);
            _fruitEntrancesReference = GetNode<Node>(_fruitEntrancesPath);
            _fruitLoopsReference = GetNode<Node>(_fruitLoopsPath);
            _loopsToExitsReference = GetNode<Node>(_loopsToExitsPath);
            _fruitExitsReference = GetNode<Node>(_fruitExitsPath);
        }

        private void CheckNodeReferences()
        {
            if (!_tunnelContainerReference.IsValid())
            {
                GD.PrintErr("ERROR: Level Tunnel Container Reference is not valid!");
            }
        }

        public override void PlayLevelFlash()
        {
            LevelFlashPlayer.Play(LEVEL_FLASH_ANIMATION_NAME);
        }

        public override void ResetLevel()
        {
            Pellets.ResetPellets();
        }

        public override void DestroyTunnels()
        {
            _tunnelContainerReference.SafeQueueFree();
        }

        public override void SpawnFruit(int fruitIndex)
        {
            if (!_fruitExists)
            {
                _pathNumber = 0;
                _fruitEntranceIndex = GDRandom.RandiRange(0, _fruitEntrancesReference.GetChildCount() - 1);
                _fruitExitIndex = GDRandom.RandiRange(0, _fruitExitsReference.GetChildCount() - 1);
                _fruit = GetFruit(fruitIndex);
                _fruitEntrancesReference.GetChild(_fruitEntranceIndex).AddChild(_fruit);
                _fruit.Connect("PathCompleted", this, nameof(OnFruitPathCompleted));
                _fruit.Connect("FruitDestroyed", this, nameof(OnFruitCollected));
                _fruitExists = true;
            }
        }

        private Fruit GetFruit(int fruitIndex)
        {
            switch (fruitIndex)
            {
                case 1:
                    PackedScene cherryScene = GD.Load<PackedScene>("res://Fruits/Scenes/Cherry.tscn");
                    return cherryScene.Instance<Fruit>();
                case 2:
                    PackedScene strawberryScene = GD.Load<PackedScene>("res://Fruits/Scenes/Strawberry.tscn");
                    return strawberryScene.Instance<Fruit>();
                case 3:
                    PackedScene orangeScene = GD.Load<PackedScene>("res://Fruits/Scenes/Orange.tscn");
                    return orangeScene.Instance<Fruit>();
                case 4:
                    PackedScene pretzelScene = GD.Load<PackedScene>("res://Fruits/Scenes/Pretzel.tscn");
                    return pretzelScene.Instance<Fruit>();
                case 5:
                    PackedScene appleScene = GD.Load<PackedScene>("res://Fruits/Scenes/Apple.tscn");
                    return appleScene.Instance<Fruit>();
                case 6:
                    PackedScene pearScene = GD.Load<PackedScene>("res://Fruits/Scenes/Pear.tscn");
                    return pearScene.Instance<Fruit>();
                case 7:
                    PackedScene bananaScene = GD.Load<PackedScene>("res://Fruits/Scenes/Banana.tscn");
                    return bananaScene.Instance<Fruit>();
                default:
                    return GetFruit(GDRandom.RandiRange(1, 7));
            }
        }

        public override void OnFruitPathCompleted()
        {
            _pathNumber++;
            _fruit.Offset = 0.0f;
            _fruit.UnitOffset = 0.0f;

            switch (_pathNumber)
            {
                // loop
                case 1:
                    _fruitEntrancesReference.GetChild(_fruitEntranceIndex).RemoveChild(_fruit);
                    _fruitLoopsReference.GetChild(_fruitEntranceIndex).AddChild(_fruit);
                    break;
                // loop-to-exit
                case 2:
                    _fruitLoopsReference.GetChild(_fruitEntranceIndex).RemoveChild(_fruit);
                    _loopsToExitsReference.GetChild(_fruitExitIndex).GetChild(_fruitEntranceIndex).AddChild(_fruit);
                    break;
                // exit
                case 3:
                    _loopsToExitsReference.GetChild(_fruitExitIndex).GetChild(_fruitEntranceIndex).RemoveChild(_fruit);
                    _fruitExitsReference.GetChild(_fruitExitIndex).AddChild(_fruit);
                    break;
                default:
                    _fruit.SafeQueueFree();
                    _fruitExists = false;
                    break;
            }
        }

        public override void OnFruitCollected()
        {
            _fruitExists = false;
        }

        public void OnAnimationFinished(string anim_name)
        {
            if (anim_name == LEVEL_FLASH_ANIMATION_NAME)
            {
                LevelFlashPlayer.Stop();
                LevelFlash.Visible = false;
                EmitSignal("LevelFlashFinished");
            }
        }
    }
}