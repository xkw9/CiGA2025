using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;
public class OriginLocation : MonoBehaviour
{
    [SerializeField]
    string targetSqTag = "";
    // Start is called before the first frame update
    void Start()
    {
        
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
                obj.atTargetLocation = true;
                // GameManager.addFinishObject(objName, obj);
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
                // GameManager.addFinishObject(objName, obj);
                Debug.Log($"stay {objName} 的触发区域");
            }
        }
    }
    
    // 离开触发区域
    void OnTriggerExit2D(Collider2D other) {
        MovingObject obj = other.GetComponent<MovingObject>();
        if (obj != null){
            string objName = obj.objName;
            if (targetSqTag == obj.objName){
                Debug.Log($"命中 {obj.objName} exit");
                obj.atTargetLocation = false;
                GameManager.removeFinishObject(objName, obj);
                Debug.Log($"exit {objName} 的触发区域");
            }
            
        }
    }

}
