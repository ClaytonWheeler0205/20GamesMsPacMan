using Godot;
using System;

namespace Util
{
    public static class GDRandom
    {
        /// <summary>
        /// Generates an integer between a minimum (inclusive) and maximium (inclusive) range
        /// </summary>
        /// <param name="min">the minimum number (inclusive) to be generated</param>
        /// <param name="max">the maximum number (inclusive) to be generated</param>
        /// <returns>a random integer between the two values</returns>
        public static int RandiRange(int min, int max)
        {
            if (min >= max)
            {
                throw new ArgumentException("min range is greater than or equal to max range!");
            }
            GD.Randomize();
            return (int)(GD.Randi() % (max - min + 1)) + min;
        }
    }
}