using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public class MovingObject : MonoBehaviour
    {

        Rigidbody2D rb;

        Stopwatch timer;

        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            if (CanMove())
            {
                // move it
            }
        }

        public void OnSpot()
        {
            UnityEngine.Debug.Log("Spotted");
            //timer.Stop();
            //timer.Reset();
        }

        public void OnUnspot()
        {
            //timer.Restart();
        }

        bool CanMove()
        {
            return false;
        }



    }
}
