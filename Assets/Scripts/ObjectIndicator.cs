using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.U2D.Animation;

namespace Assets.Scripts
{
    public class ObjectIndicator : MonoBehaviour
    {

        SpriteResolver SpriteResolver;
        MovingObject movingObject;
        private void Start()
        {
            SpriteResolver = GetComponent<SpriteResolver>();
            movingObject = GetComponentInParent<MovingObject>();
        }

        private void Update()
        {
            
            if (movingObject.state == MovingObject.ObjectState.PICKED_UP)
            {
                SpriteResolver.SetCategoryAndLabel("New Category", "Carried");
            } else if (movingObject.state == MovingObject.ObjectState.DONE)
            {
                SpriteResolver.SetCategoryAndLabel("New Category", "Check");
            } 
            else
            {
                SpriteResolver.SetCategoryAndLabel("New Category", "None");
            }
        }

    }
}
