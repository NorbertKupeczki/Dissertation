using UnityEngine;

namespace GeneralData
{
    public enum StatType
    {
        OPENNESS = 0,
        CONSCIENTIOUSNESS = 1,
        EXTRAVERSION = 2,
        AGREEABLENESS = 3,
        NEUROTICISM = 4
    }

    public class Data
    {
        public const short NUMBER_OF_STATS = 5;
        public const short MAX_STAT_VALUE = 100;
        public const float UPDATE_REFRESH_IN_SEC = 0.1f;

        public const int LEFT_MOUSE_BUTTON = 0;
        public const int RIGHT_MOUSE_BUTTON = 1;
        public const int MIDDLE_MOUSE_BUTTON = 2;
    }
}



