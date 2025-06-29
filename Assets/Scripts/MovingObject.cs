using Assets.Scripts.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEditor;

namespace Assets.Scripts
{
    public class MovingObject : MonoBehaviour
    {
        [SerializeField]
        float mass = 10;
        [SerializeField]
        float sleepingTime = 5;
        [SerializeField]
        float movingSpeed = 8;
        [SerializeField]
        int level = 0;
        [SerializeField]
        public string objName = "";

        [SerializeField]
        protected Color textColor;
        [SerializeField]
        float fontSize = 1;

        public bool atTargetLocation = false;
        Rigidbody2D rb;
        ObjectPathSeeker pathSeeker;

        protected CountdownText countdownText;

        protected SpriteRenderer outLine;

        float timeLastSeen = 0;
        float timeStartMove = 0;
        public ObjectState state = ObjectState.SLEEPING;

        public bool spotted = false;
        public bool isMoving = false;

        FaceManager faceManager = new();

        protected virtual void Awake()
        {
            countdownText = CountdownText.makeText(gameObject);
        }

        protected virtual void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            pathSeeker = GetComponentInChildren<ObjectPathSeeker>();
            countdownText.SetColor(textColor);
            countdownText.SetFontSize(fontSize);
            outLine = transform.Find("OutLine").GetComponent<SpriteRenderer>();
            outLine.color = textColor;
            outLine.gameObject.SetActive(true);
            countdownText.gameObject.SetActive(true);
        }

        protected virtual void Update()
        {
            UnityEngine.Debug.Log("Update state:" + state);

            if (state == ObjectState.DONE)
            {
                // never move if Done
                countdownText.Hide();
                outLine.gameObject.SetActive(false);
                return;
            }
            
            if (GameManager.lightOn)
            {
                OnSpot();
            }

            if (state == ObjectState.ALIVE)
            {
                if (!isMoving)
                {
                    GameManager.AudioManager.PlaySFX("drag" + UnityEngine.Random.Range(0, 5));
                    UpdateFace(-1);
                    isMoving = true;
                    timeStartMove = Time.time;
                }

                Move();
            }
            else if (CheckCanMove() && state == ObjectState.SLEEPING)
            {
                // move it
                UnityEngine.Debug.Log("Object " + name + " Start seeking path");
                state = ObjectState.SEEKING_PATH;
                pathSeeker.StartSeekingPath();
            }

            if (state == ObjectState.SLEEPING && !spotted)
            {
                float rstTime = sleepingTime - (Time.time - timeLastSeen);
                UpdateFace(rstTime);

            }

            
        }

        protected virtual void OnDestroy()
        {
            if (countdownText.gameObject != null)
            {
                Destroy(countdownText.gameObject);
            }
        }

        public virtual void UpdateFace(float rstTime)
        {
            if (rstTime < 0)
            {
                countdownText.SetText(FaceManager.getRandomWakingFace());
                return;
            }

            float timeOne = Config.OBJECT_WAKE_TIME / faceManager.TotalStages - 0.01f;
            if (faceManager.TotalStages - rstTime / timeOne - 1 > faceManager.curStages)
            {
                faceManager.Wake();
            }

            countdownText.Show();
            countdownText.SetText(faceManager.getFace());

            outLine.gameObject.SetActive(true);

        }

        public void OnSpot()
        {
            if (countdownText != null)
            {
                countdownText.Hide();
            }

            if (outLine != null)
            {
                outLine.gameObject.SetActive(false);
            }
            
            if (state == ObjectState.DONE)
            {
                return;
            }

            timeLastSeen = Time.time;
            spotted = true;
            isMoving = false;
            
            faceManager.Reset();

            if (state == ObjectState.PICKED_UP)
            {
                return;
            }

            UnityEngine.Debug.Log("Spotted " + name);

            Freeze();
            
        }

        public void OnUnspot()
        {
            if (state == ObjectState.PICKED_UP) return;

            spotted = false;
            outLine.gameObject.SetActive(true);
            countdownText.Show();
            UpdateFace(float.PositiveInfinity);
            UnityEngine.Debug.Log("Unspotted " + name);
        }


        bool CheckCanMove()
        {
            if (state == ObjectState.PICKED_UP)
            {
                // never move if picked up
                return false;
            }
            if (state == ObjectState.DONE)
            {
                // never move if Done
                return false;
            }
            return Time.time - timeLastSeen > sleepingTime;
        }

        public void PickUp(Player player)
        {
            if (state == ObjectState.PICKED_UP)
            {
                return;
            }
            UnityEngine.Debug.Log("Picked up object: " + gameObject.name);

            //GameManager.AudioManager.PlaySFX("carry_short");

            Free();
            state = ObjectState.PICKED_UP;
            
            var joint = gameObject.AddComponent<FixedJoint2D>();
            joint.connectedBody = player.GetComponent<Rigidbody2D>();
        }

        public void Release(Player player)
        {
            UnityEngine.Debug.Log("Released object: " + gameObject.name);

            rb.velocity = Vector2.zero;
            rb.mass = float.MaxValue;
            state = ObjectState.SLEEPING;
            timeLastSeen = Time.time;
            GameManager.AudioManager.PlaySFX("drop_short");
            Destroy(GetComponent<FixedJoint2D>());

        }

        public void Done()
        {
            UnityEngine.Debug.Log(objName + " before Done: " + state);
            state = ObjectState.DONE;
            if (countdownText != null)
            {
                countdownText.Hide();
            }

            if (outLine != null)
            {
                outLine.gameObject.SetActive(false);
            }

            UnityEngine.Debug.Log(objName + " Done: " + state);
        }

        public void Move()
        {
            if (rb.mass > 100000)
            {
                Free();
            }

            if (Time.time - timeStartMove > Config.OBJECT_MOVE_TIME)
            {
                // move time is over, seek another path
                Freeze();
                UnityEngine.Debug.Log("Object " + name + " Start seeking path");
                state = ObjectState.SEEKING_PATH;
                pathSeeker.StartSeekingPath();
                return;
            }

            var curTarget = pathSeeker.GetCurWaypoint(transform);
            if (curTarget != null && Vector2.Distance(transform.position, curTarget) < 0.1f)
            {
                // reached the wayPoint, get a new one
                if (pathSeeker.GetNextWaypoint(transform, out var wayPoint)) {
                    curTarget = wayPoint;
                } else
                {
                    // reached the end of the path
                    Freeze();
                    UnityEngine.Debug.Log("Object " + name + " Start seeking path");
                    state = ObjectState.SEEKING_PATH;
                    pathSeeker.StartSeekingPath();
                    return;
                }
            } else
            {
                rb.AddForce((curTarget.toVec3() - transform.position).toVec2().normalized * movingSpeed);
            }
        }

        public void Freeze()
        {
            isMoving = false;
            state = ObjectState.SLEEPING;
            rb.velocity = Vector2.zero;
            rb.mass = float.MaxValue; // freeze the object

        }

        public void Free()
        {
            rb.velocity = Vector2.zero;
            rb.mass = mass; 
        }

        public enum ObjectState
        {
            SLEEPING,
            SEEKING_PATH,
            ALIVE,
            PICKED_UP,
            DONE,
        }

    }
}
