using System.Runtime.InteropServices;
using Godot;

namespace Game.Pellets
{

    public abstract class PelletContainer : Node
    {
        private int _pelletsNeededForFirstElroy = 20;
        public int PelletsNeededForFirstElroy
        {
            get { return _pelletsNeededForFirstElroy; }
            set { _pelletsNeededForFirstElroy = value; }
        }
        private int _pelletsNeededForSecondElroy = 10;
        public int PelletsNeededForSecondElroy
        {
            get { return _pelletsNeededForSecondElroy; }
            set { _pelletsNeededForSecondElroy = value; }
        }

        public abstract void ResetPellets();
    }
}