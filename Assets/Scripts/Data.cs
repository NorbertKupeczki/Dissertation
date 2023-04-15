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

    public enum EffectImpact
    {
        LOW_IMPACT = 0,
        MEDIUM_IMPACT = 1,
        HIGH_IMPACT = 2
    }


    public class Data
    {
        public const short NUMBER_OF_STATS = 5;
        public const short MAX_STAT_VALUE = 100;
        public const float UPDATE_REFRESH_IN_SEC = 0.1f;
        public const float AOE_DETECTION_RADIUS = 4.0f;
    }

    public class InputData
    {
        public const int LEFT_MOUSE_BUTTON = 0;
        public const int RIGHT_MOUSE_BUTTON = 1;
        public const int MIDDLE_MOUSE_BUTTON = 2;

        public static readonly Vector3 LEFT_VECTOR = new(-1.0f, 0.0f, 0.0f);
        public static readonly Vector3 RIGHT_VECTOR = new(1.0f, 0.0f, 0.0f);
        public static readonly Vector3 UP_VECTOR = new(0.0f, 0.0f, 1.0f);
        public static readonly Vector3 DOWN_VECTOR = new(0.0f, 0.0f, -1.0f);

        public const float LEFT_ROTATION = -1.0f;
        public const float RIGHT_ROTATION = 1.0f;
    }

    public class ParticleData
    {
        public static readonly ParticleSystem.MinMaxCurve LOW_PARTICLE_AMOUNT = 2;
        public static readonly ParticleSystem.MinMaxCurve MEDIUM_PARTICLE_AMOUNT = 4;
        public static readonly ParticleSystem.MinMaxCurve HIGH_PARTICLE_AMOUNT = 6;
    }

}



