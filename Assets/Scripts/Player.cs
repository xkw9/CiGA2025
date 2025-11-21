using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Assets.Scripts.Utils;
using Assets.Scripts;
using Unity.VisualScripting;

public class Player : MonoBehaviour
{

    Rigidbody2D rb;
    Light2D light2d;

    ObjectDetector objDetector;

    MovingObject objCarried;

    CountdownText leftEye, rightEye;

    static Vector2 leftEyeOffset = new Vector2(-0.3f, 0);
    static Vector2 rightEyeOffset = new Vector2(0.3f, 0);

    static float eyeRadius = 0.1f;

    PlayerState state;

    float deg = 0;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        light2d = GetComponentInChildren<Light2D>();
        objDetector = GetComponentInChildren<ObjectDetector>();

        leftEye = CountdownText.makeText(gameObject);
        rightEye = CountdownText.makeText(gameObject);
        leftEye.SetText("o");
        leftEye.SetColor(Color.cyan);
        rightEye.SetText("o");
        rightEye.SetColor(Color.cyan);
    }

    // Update is called once per frame
    void Update()
    {
        //GatherInput();
        if (GameManager.isTransitioning)
        {
            state = PlayerState.FREEZE;
            rb.velocity = Vector2.zero;
            return;
        } else
        {
            HandleInput();
        }
    }

    void HandleInput()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector2 inputVec = new Vector2(horizontal, vertical).normalized;

        rb.velocity = inputVec * Config.PLAYER_SPEED;

        Vector3 mousePos = Input.mousePosition;
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);

        if (Input.GetKey(KeyCode.N))
        {
            deg += 2 * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.M))
        {
            deg -= 2 * Time.deltaTime;
        }

        //Vector2 playerToMouse = new Vector2(Mathf.Sin(deg), Mathf.Cos(deg));

        Vector2 playerToMouse = (transform.position.toVec2() - worldPos.toVec2()).normalized;

        Vector3.Angle(playerToMouse, Vector2.up);

        light2d.transform.rotation = Quaternion.Euler(0, 0, 180 + Vector2.SignedAngle(Vector2.up, playerToMouse));

        // rotate eyes

        Vector2 leftEyeCenterToMouse = playerToMouse - leftEyeOffset;
        Vector2 rightEyeCenterToMouse = playerToMouse - rightEyeOffset;

        leftEye.SetOffset(leftEyeOffset - leftEyeCenterToMouse.normalized * eyeRadius);
        rightEye.SetOffset(rightEyeOffset - rightEyeCenterToMouse.normalized * eyeRadius);

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

            } else if (state == PlayerState.FREEZE)
            {
                state = PlayerState.NORMAL;
            }
        }


    }

    public void ForceRelease()
    {
        if (objCarried != null)
        {
            // already carrying a object
            // release it
            objCarried.Release(this);
            objCarried = null;
            state = PlayerState.NORMAL;
        }
    }

    private void OnDisable()
    {
        leftEye.gameObject.SetActive(false);
        rightEye.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        leftEye.gameObject.SetActive(true);
        rightEye.gameObject.SetActive(true);
    }

    public enum PlayerState
    {

        NORMAL,
        CARRYING,
        FREEZE,

    }
}
