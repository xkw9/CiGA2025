using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;
public class OriginLocation : MonoBehaviour
{
    [SerializeField]
    string targetSqTag = "";
    [SerializeField]
    int level = 0;
    
    Collider2D col;
    // Start is called before the first frame update
    void Start()
    {
        col = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 进入触发区域
    void OnTriggerEnter2D(Collider2D other) {
        MovingObject obj = other.GetComponent<MovingObject>();
        if (obj != null){
            string objName = obj.objName;
            if (targetSqTag == obj.objName){
                //obj.atTargetLocation = true;
                //GameManager.addFinishObject(objName, obj);
                Debug.Log($"enter {objName} 的触发区域");
            }
        }
    }
    
    // 停留在触发区域
    void OnTriggerStay2D(Collider2D other) {
        // 每帧调用
        MovingObject obj = other.GetComponent<MovingObject>();
        if (obj != null){
            string objName = obj.objName;
            if (targetSqTag == obj.objName){
                obj.atTargetLocation = true;
                
                Bounds boundsA = col.bounds;
                Bounds boundsB = obj.GetComponent<Collider2D>().bounds;


                Bounds intersection = new Bounds();
                intersection.SetMinMax(
                    Vector3.Max(boundsA.min, boundsB.min),
                    Vector3.Min(boundsA.max, boundsB.max));

                float intersectionVolume = intersection.size.x * intersection.size.y;
                float volumeA = boundsA.size.x * boundsA.size.y;

                float factor = intersectionVolume / volumeA;

                if (factor > 0.8)
                {
                    obj.atTargetLocation = true;
                    
                    GameManager.addFinishObject(objName, obj);
                } else
                {
                    obj.atTargetLocation = false;
                    GameManager.removeFinishObject(objName, obj);
                }


            }
        }
    }
    
    // 离开触发区域
    void OnTriggerExit2D(Collider2D other) {
        var obj = other.GetComponentInParent<MovingObject>();
        if (obj != null && (obj.state == MovingObject.ObjectState.PICKED_UP
                || obj.state == MovingObject.ObjectState.ALIVE))
        {
            string objName = obj.objName;
            if (targetSqTag == obj.objName){
                Debug.Log($"命中 {obj.objName} exit");
                //obj.atTargetLocation = false;
                //GameManager.removeFinishObject(objName, obj);
                Debug.Log($"exit {objName} 的触发区域");
            }

        }
    }

    void OnEnter(MovingObject obj)
    {

    }

    void OnLeave(MovingObject obj)
    {

    }

}
