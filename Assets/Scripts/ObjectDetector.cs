using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public class ObjectDetector : MonoBehaviour
    {
        List<MovingObject> obj_inRange = new();
        Collider2D col;
    
        public bool detectedAny => obj_inRange.Count > 0;
        public MovingObject firstDetected => obj_inRange[0]; 

        private void Start()
        {
            col = GetComponent<Collider2D>();
        }


        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag != Config.TAG_MOVING_OBJECT)
            {
                return;
            }

            var obj = collision.GetComponent<MovingObject>();
            if (! obj_inRange.Contains(obj))
            {
                Debug.Log("Dectected object: " + collision.gameObject.name);
                obj_inRange.Add(obj);
            }
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.gameObject.tag != Config.TAG_MOVING_OBJECT) { return; }

            var obj = collision.GetComponent<MovingObject>();
            obj.OnSpot();
        }   

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.tag != Config.TAG_MOVING_OBJECT) { return; }
            
            var obj = collision.GetComponent<MovingObject>();
            obj.OnUnspot();
            obj_inRange.Remove(obj);    
        }
    }
}
