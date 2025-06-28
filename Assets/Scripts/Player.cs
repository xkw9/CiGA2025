using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Assets.Scripts.Utils;
using Assets.Scripts;

public class Player : MonoBehaviour
{

    Rigidbody2D rb;
    Light2D light2d;

    ObjectDetector objDetector;

    MovingObject objCarried;

    PlayerState state;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        light2d = GetComponentInChildren<Light2D>();
        objDetector = GetComponentInChildren<ObjectDetector>();
    }

    // Update is called once per frame
    void Update()
    {
        //GatherInput();
        HandleInput();
    }

    void HandleInput()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector2 inputVec = new Vector2(horizontal, vertical).normalized;

        rb.velocity = inputVec * Config.PLAYER_SPEED;

        Vector3 mousePos = Input.mousePosition;
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);

        Vector2 playerToMouse = (transform.position.toVec2() - worldPos.toVec2()).normalized;

        Vector3.Angle(playerToMouse, Vector2.up);

        light2d.transform.rotation = Quaternion.Euler(0, 0, 180 + Vector2.SignedAngle(Vector2.up, playerToMouse));

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (state == PlayerState.CARRYING)
            {
                // already carrying a object
                // release it
                objCarried.Release(this);
                objCarried = null;
                state = PlayerState.NORMAL;

            } else if (state == PlayerState.NORMAL)
            {

                if (objDetector.detectedAny)
                {
                    // pick up the object
                    objCarried = objDetector.firstDetected;
                    objCarried.PickUp(this);
                    state = PlayerState.CARRYING;
                }

            }
        }
    }

    public enum PlayerState
    {

        NORMAL,
        CARRYING,

    }
}
