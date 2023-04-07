using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Utility
{
    public static class Utility
    {
        public static Vector3 s_blenderRotation = new(-90.0f, 0.0f, 0.0f);

        private static Dictionary<int, Color> _hairColours = new()
    {
        { 0, new Color(0.17f, 0.13f, 0.17f) },
        { 1, new Color(0.42f, 0.31f, 0.29f) },
        { 2, new Color(0.74f, 0.59f, 0.47f) },
        { 3, new Color(0.55f, 0.29f, 0.26f) },
        { 4, new Color(0.79f, 0.32f, 0.22f) },
        { 5, new Color(0.65f, 0.42f, 0.27f) },
        { 6, new Color(0.84f, 0.77f, 0.76f) }
    };
        private static Dictionary<int, Color> _skinColours = new()
    {
        { 0, new Color(1.00f, 0.86f, 0.45f) },
        { 1, new Color(0.89f, 0.63f, 0.29f) },
        { 2, new Color(0.80f, 0.52f, 0.26f) },
        { 3, new Color(0.73f, 0.42f, 0.29f) },
        { 4, new Color(0.21f, 0.00f, 0.00f) },
        { 5, new Color(0.88f, 0.68f, 0.64f) }
    };

        public enum Gender
        {
            UNDEFINED = 0,
            MALE = 1,
            FEMALE = 2
        }

        public static Color GetRandomColour()
        {
            return new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
        }

        /// <summary>
        /// Rolls an imaginary dice that has faces equal to the provided parameter and returns the result.
        /// </summary>
        /// <param name="diceFaces"></param>
        /// <returns>Int</returns>
        public static int RollADice(int diceFaces)
        {
            return Random.Range(0, diceFaces) + 1;
        }

        /// <summary>
        /// Generates a random Gender
        /// </summary>
        public static Gender GetRandomSex()
        {
            return (Gender)Random.Range(1, Enum.GetNames(typeof(Gender)).Length);
        }

        public static Color GetRandomHairColour()
        {
            return _hairColours[Random.Range(0, _hairColours.Count)];
        }

        public static Color GetRandomSkinColour()
        {
            return _skinColours[Random.Range(0, _skinColours.Count)];
        }
    }
}

