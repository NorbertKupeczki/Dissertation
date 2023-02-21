using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utility
{
    public static class Utility
    {
        public static Color GetRandomColour()
        {
            return new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
        }
    }
}

