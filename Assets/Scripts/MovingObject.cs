using Assets.Scripts.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public class MovingObject : MonoBehaviour
    {

        [SerializeField]
        float sleepingTime = 5;
        [SerializeField]
        float movingSpeed = 8;
        [SerializeField]
        public string objName = "";
        public bool atTargetLocation = false;
        Rigidbody2D rb;
        ObjectPathSeeker pathSeeker;

        CountdownText countdownText;

        float timeLastSeen = 0;
        float timeStartMove = 0;
        public ObjectState state = ObjectState.SLEEPING;

        public bool spotted = false;
        public bool isMoving = false;

        FaceManager faceManager = new();

        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            pathSeeker = GetComponentInChildren<ObjectPathSeeker>();
            countdownText = CountdownText.makeText(gameObject);
        }

        private void Update()
        {

            if (state == ObjectState.ALIVE)
            {
                if (!isMoving)
                {
                    isMoving = true;
                    timeStartMove = Time.time;
                }

                Move();
                countdownText.Show();
                countdownText.SetText("OAO");
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

                if (rstTime <= Config.OBJECT_WAKE_TIME)
                {
                    UpdateFace(rstTime);
                } else
                {
                    countdownText.Hide();
                }

            }

            
        }

        public void UpdateFace(float rstTime)
        {
            float timeOne = Config.OBJECT_WAKE_TIME / faceManager.TotalStages - 0.01f;
            if (faceManager.TotalStages - rstTime / timeOne - 1 > faceManager.curStages)
            {
                faceManager.Wake();
            }

            countdownText.Show();
            countdownText.SetText(faceManager.getFace());
        
        }

        public void OnSpot()
        {
            countdownText.Hide();
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
            spotted = false;
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
            UnityEngine.Debug.Log("Picked up object: " + gameObject.name);

            GameManager.AudioManager.PlaySFX("ui_start");

            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.velocity = Vector2.zero;
            state = ObjectState.PICKED_UP;
            
            var joint = gameObject.AddComponent<FixedJoint2D>();
            joint.connectedBody = player.GetComponent<Rigidbody2D>();
        }

        public void Release(Player player)
        {
            UnityEngine.Debug.Log("Released object: " + gameObject.name);
            
            rb.bodyType = RigidbodyType2D.Static;
            state = ObjectState.SLEEPING;
            timeLastSeen = Time.time;
            if (atTargetLocation){
                GameManager.addFinishObject(objName, this);
            }
            Destroy(GetComponent<FixedJoint2D>());

        }

        public void Done()
        {
            UnityEngine.Debug.Log(objName + " before Done: " + state);
            state = ObjectState.DONE;
            UnityEngine.Debug.Log(objName + " Done: " + state);
        }

        public void Move()
        {
            rb.bodyType = RigidbodyType2D.Dynamic; // make sure we can move

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
            rb.bodyType = RigidbodyType2D.Static; // stop moving

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
