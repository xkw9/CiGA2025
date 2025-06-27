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


    }
}
