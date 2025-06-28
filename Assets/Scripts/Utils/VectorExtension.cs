using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Utils
{
    public static class VectorExtension
    {
        public static Vector2 toVec2(this Vector3 vector3)
        {
            return new Vector2(vector3.x, vector3.y);
        }

        public static Vector3 toVec3(this Vector2 vector2, float z = 0)
        {
            return new Vector3(vector2.x, vector2.y, 0);
        }

        public static Vector2 rotate(this Vector2 v, float degrees)
        {
            float radians = degrees * Mathf.Deg2Rad;
            float sin = Mathf.Sin(radians);
            float cos = Mathf.Cos(radians);

            return new Vector2(
                cos * v.x - sin * v.y,
                sin * v.x + cos * v.y
            );
        }

    }
}
