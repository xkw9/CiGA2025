using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public class MovingObjectWithText : MovingObject
    {

        CountdownText text;

        [SerializeField]
        string textToShow = "new";

        protected override void Awake()
        {
            base.Awake();
            text = CountdownText.makeText(gameObject);
        }

        protected override void Start()
        {
            base.Start();
            text.Hide();
            text.SetText(textToShow);
            text.SetColor(textColor);
        }

        protected override void Update()
        {
            base.Update();
            if (state == ObjectState.SLEEPING && spotted)
            {
                text.gameObject.SetActive(false);
            } else if (state == ObjectState.ALIVE && !text.gameObject.activeSelf)
            {
                text.gameObject.SetActive(true);
            }
        }

        public override void UpdateFace(float rstTime)
        {
            // DO NOTHING
        }

    }
}
