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
        
        Rigidbody2D rb;
        ObjectPathSeeker pathSeeker;

        CountdownText countdownText;

        float timeLastSeen = 0;
        public ObjectState state = ObjectState.SLEEPING;

        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            pathSeeker = GetComponentInChildren<ObjectPathSeeker>();
            countdownText = CountdownText.makeText(this);
        }

        private void Update()
        {

            if (state == ObjectState.ALIVE)
            {
                Move();
            }
            else if (CanMove() && state == ObjectState.SLEEPING)
            {
                // move it
                UnityEngine.Debug.Log("Object " + name + " Start seeking path");
                state = ObjectState.SEEKING_PATH;
                pathSeeker.StartSeekingPath();
            }

            if (state == ObjectState.SLEEPING)
            {
                float rstTime = sleepingTime - (Time.time - timeLastSeen);

                if (rstTime > 0.1 && rstTime <= 4)
                {
                    countdownText.gameObject.SetActive(true);
                    countdownText.SetText(Math.Round(rstTime, 1).ToString());
                } else
                {
                    countdownText.gameObject.SetActive(false);
                }

            }

            
        }

        public void OnSpot()
        {
            timeLastSeen = Time.time;

            if (state == ObjectState.PICKED_UP)
            {
                return;
            }

            UnityEngine.Debug.Log("Spotted " + name);

            Freeze();
            
        }


        bool CanMove()
        {
            if (state == ObjectState.PICKED_UP)
            {
                // never move if picked up
                return false;
            }

            return Time.time - timeLastSeen > sleepingTime;
        }

        public void PickUp(Player player)
        {
            UnityEngine.Debug.Log("Picked up object: " + gameObject.name);

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
            
            Destroy(GetComponent<FixedJoint2D>());

        }

        public void Move()
        {
            rb.bodyType = RigidbodyType2D.Dynamic; // make sure we can move
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
                }
            } else
            {
                rb.AddForce((curTarget.toVec3() - transform.position).toVec2().normalized * movingSpeed);
            }
        }

        public void Freeze()
        {
            timeLastSeen = Time.time;
            state = ObjectState.SLEEPING;
            rb.bodyType = RigidbodyType2D.Static; // stop moving

        }

        public enum ObjectState
        {
            SLEEPING,
            SEEKING_PATH,
            ALIVE,
            PICKED_UP,
        }

    }
}
