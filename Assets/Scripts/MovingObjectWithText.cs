using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class MovingObjectWithText : MovingObject
    {

        [SerializeField, Multiline]
        string textToShow = "new";


        public override void UpdateFace(float rstTime)
        {
            countdownText.Show();
            countdownText.SetText(textToShow);

            outLine.gameObject.SetActive(true);
        }

    }
}
