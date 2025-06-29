using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts
{
    public class FaceManager
    {
        static List<string> eyes = new() { "-", "o", "0" };

        static List<string> wakingFaces = new() { "0v0", "0w0", "0A0", "030" };

        int leftLv = 0, rightLv = 0;
        int lv;

        public void Wake()
        {
            if (leftLv >= eyes.Count - 1 && rightLv >= eyes.Count - 1)
            {
                return;
            }
            
            bool left = Random.Range(0, 2) < 1;

            if (leftLv >= eyes.Count - 1)
            {
                left = false;

            }
            else if (rightLv >= eyes.Count - 1)
            {
                left = true;
            }


            if (left)
            {
                leftLv++;
            } else
            {
                rightLv++;
            }

            lv++;

            return;


        }

        public string getFace()
        {
            return eyes[leftLv] + "_" + eyes[rightLv];
        }

        public void Reset()
        {
            leftLv = 0;
            rightLv = 0;
            lv = 0;
        }

        public bool Waked()
        {
            return leftLv >= eyes.Count - 1 && rightLv >= eyes.Count - 1;
        }

        public static string getRandomWakingFace()
        {
            return wakingFaces[Random.Range(0, wakingFaces.Count)];
        }

        public int TotalStages => eyes.Count * 2;
        public int curStages => lv;
    }
}
