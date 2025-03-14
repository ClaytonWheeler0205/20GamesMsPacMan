using Godot;
using System;
using Game.Levels;
using Util.ExtensionMethods;

namespace Game.Ghosts
{

    public class IntersectionDetector
    {
        private static Level _currentLevel;
        public static Level CurrentLevel
        {
            set
            {
                if (value.IsValid())
                {
                    _currentLevel = value;
                }
            }
        }
        private const int TURN_TILE_CELL_NUMBER = 1;
        private const int SPECIAL_TURN_TILE_CELL_NUMBER = 3;

        public static bool IsAtIntersection(Vector2 position)
        {
            Vector2 localPosition = _currentLevel.ToLocal(position);
            Vector2 mapPosition = _currentLevel.WorldToMap(localPosition);
            int cellNumber = _currentLevel.GetCell((int)mapPosition.x, (int)mapPosition.y);
            return cellNumber == TURN_TILE_CELL_NUMBER || cellNumber == SPECIAL_TURN_TILE_CELL_NUMBER;
        }
    }
}