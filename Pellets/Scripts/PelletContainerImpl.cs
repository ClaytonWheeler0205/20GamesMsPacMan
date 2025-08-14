using Game.Bus;
using Util.ExtensionMethods;

namespace Game.Pellets
{

    public class PelletContainerImpl : PelletContainer
    {
        private Godot.Collections.Array<Pellet> _pellets;
        private int _pelletCount;

        public override void _Ready()
        {
            _pellets = new Godot.Collections.Array<Pellet>();
            _pelletCount = GetChildCount();
            ConstructPelletsArray();
            SetNodeConnections();
        }

        private void ConstructPelletsArray()
        {
            for (int i = 0; i < _pelletCount; i++)
            {
                Pellet pellet = GetChild(i) as Pellet;
                if (pellet.IsValid())
                {
                    _pellets.Add(pellet);
                }
            }
        }

        private void SetNodeConnections()
        {
            foreach (Pellet pellet in _pellets)
            {
                pellet.Connect("PelletCollected", this, "OnPelletCollected");
            }
        }

        public override void ResetPellets()
        {
            foreach (Pellet pellet in _pellets)
            {
                pellet.ResetPellet();
            }
            _pelletCount = GetChildCount();
        }

        public void OnPelletCollected()
        {
            _pelletCount -= 1;
            if (_pelletCount == 0)
            {
                LevelEventBus.Instance.EmitSignal("LevelCleared");
            }
        }
    }
}