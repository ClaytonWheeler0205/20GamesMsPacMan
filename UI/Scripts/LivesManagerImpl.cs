using System.Collections.Generic;
using Game.Bus;
using Godot;
using Util.ExtensionMethods;

namespace Game.UI
{

    public class LivesManagerImpl : LivesManager
    {
        [Export]
        private NodePath _iconContainerPath;
        private HBoxContainer _iconContainerReference;
        private List<TextureRect> _lifeIcons;
        [Export]
        private NodePath _lifeGainedSoundPath;
        private AudioStreamPlayer _lifeGainedSoundReference;

        public override void _Ready()
        {
            SetNodeReferences();
            CheckNodeReferences();
            SetupLifeIconArray();
            SetNodeConnections();
        }

        private void SetNodeReferences()
        {
            _iconContainerReference = GetNode<HBoxContainer>(_iconContainerPath);
            _lifeGainedSoundReference = GetNode<AudioStreamPlayer>(_lifeGainedSoundPath);
        }

        private void CheckNodeReferences()
        {
            if (!_iconContainerReference.IsValid())
            {
                GD.PrintErr("ERROR: Lives Manager Icon Container Reference is not valid!");
            }
            if (!_lifeGainedSoundReference.IsValid())
            {
                GD.PrintErr("ERROR: Lives Manager Life Gained Sound Reference is not valid!");
            }
        }

        private void SetupLifeIconArray()
        {
            _lifeIcons = new List<TextureRect>();
            _lifeIcons.Capacity = _iconContainerReference.GetChildCount();
            for (int i = 0; i < _iconContainerReference.GetChildCount(); i++)
            {
                if (_iconContainerReference.GetChild(i) is TextureRect textureRect)
                {
                    _lifeIcons.Insert(i, textureRect);
                }
            }
        }

        private void SetNodeConnections()
        {
            LifeEventBus.Instance.Connect("LifeGained", this, nameof(OnLifeGained));
        }

        public override void LoseLife()
        {
            LivesRemaining--;
            if (LivesRemaining >= 0)
            {
                _lifeIcons[LivesRemaining].Visible = false;
            }
        }

        public override int GetLivesRemaining()
        {
            return LivesRemaining;
        }

        public override void OnLifeGained()
        {
            _lifeIcons[LivesRemaining].Visible = true;
            LivesRemaining++;
            _lifeGainedSoundReference.Play();
        }
    }
}