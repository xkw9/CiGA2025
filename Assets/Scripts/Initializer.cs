﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public class Initializer : MonoBehaviour
    {

        private void Awake()
        {
            GameManager.Init();
        }

        private void Start()
        {
            GameManager.ShowFirstScreen();
        }

    }
}
