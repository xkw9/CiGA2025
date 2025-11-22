using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Scripts
{
    public class GameStartWatcher : MonoBehaviour
    {

        bool started = false;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Return) && ! started && GameManager.ready)
            {
                started = true;
                GameManager.StartGame();
                Destroy(gameObject);
            }

        }


    }
}
