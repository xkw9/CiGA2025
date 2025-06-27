using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public static class GameManager
    {

        public static Player player;

        
        public static void Init()
        {

            Debug.Log("loading new game!");

            player = GameObject.FindObjectOfType<Player>();

        }

    }
}
