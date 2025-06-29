using Assets.Scripts.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Assets.Scripts
{
    public class LightDetector : MonoBehaviour
    {

        Light2D light2d;
        List<MovingObject> detectedObjects = new();
        List<MovingObject> newDetectedObjects = new();

        private void Start()
        {
            light2d = GetComponent<Light2D>();
        }

        private void Update()
        {
            if (GameManager.isTransitioning)
            {
                detectedObjects.Clear();
                newDetectedObjects.Clear();
                return;
            }

            Check();   
        }

        void Check()
        {

            Vector2 facingDir = Vector2.Perpendicular(transform.right);

            Vector2 rightDir = facingDir.rotate(-Config.PLAYER_SPOT_ANGLE / 2);


            for (int i = 0; i < 20; i++)
            {
                Vector2 dir = rightDir.rotate(i * Config.PLAYER_SPOT_ANGLE / 20); 
                var result = Physics2D.RaycastAll(transform.position, dir, Config.PLAYER_SPOT_DISTANCE);

                foreach (var rayhit in result)
                {
                    var col = rayhit.collider;
                    if (col == null) continue;
                    if (col.CompareTag(Config.TAG_PLAYER) || col.CompareTag(Config.TAG_ORIGINLOCATION))
                    {
                        continue;
                    } else if (col.CompareTag(Config.TAG_MOVING_OBJECT))
                    {
                        // spot a object
                        var obj = col.GetComponent<MovingObject>();
                        obj.OnSpot();
                        newDetectedObjects.Add(obj);

                        break;
                    } else
                    {
                        break;
                    }
                } 
            }

            foreach (var obj in detectedObjects)
            {
                if (obj != null && !newDetectedObjects.Contains(obj))
                {
                    // object is no longer detected
                    obj.OnUnspot();
                }
            }

            detectedObjects.Clear();
            detectedObjects.AddRange(newDetectedObjects);
            newDetectedObjects.Clear();

        }

    }
}
