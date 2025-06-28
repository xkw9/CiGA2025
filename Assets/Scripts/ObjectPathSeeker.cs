using Assets.Scripts.Utils;
using Pathfinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public class ObjectPathSeeker : MonoBehaviour
    {

        MovingObject movingObject;

        Path path;

        int currentWayPoint = 0;

        bool reachedEnd = false;

        Seeker seeker;

        [SerializeField]
        Vector2 target;

        private void Start()
        {
            movingObject = GetComponentInParent<MovingObject>();
            seeker = GetComponent<Seeker>();
        }

        Vector2 GetValidTarget()
        {
            Vector2 guess;
            //Vector2 dst = GameManager.Level.GetDestination(movingObject).transform.position.toVec2();
            while (true)
            {
                guess = new Vector2(UnityEngine.Random.Range(-10, 10), UnityEngine.Random.Range(-10, 10));
                break;
                // dont make the target too close to player or origin point
                //if ((guess - GameManager.player.transform.position.toVec2()).sqrMagnitude > Config.CLOSEST_DIS_TO_PLAYER_SQR
                //    && (guess - dst).sqrMagnitude > Config.CLOSEST_DIS_TO_TARGET_SQR))
                //{
                //    break;
                //}

            }

            return guess;
        }

        public void StartSeekingPath()
        {
            // summon a random target point
            target = GetValidTarget();
            // create a path
            seeker.StartPath(transform.position, target, OnPathComplete);
        }

        private void OnPathComplete(Path p)
        {
            if (p.error)
            {
                return;

            }

            path = p;
            currentWayPoint = 0;

            if (movingObject.state == MovingObject.ObjectState.SEEKING_PATH)
            {
                movingObject.state = MovingObject.ObjectState.ALIVE;
            }

        }

        public bool GetNextWaypoint(Transform transform, out Vector2 wayPoint)
        {
            if (path == null)
            {
                wayPoint = transform.position;
                return false;
            } else if (reachedEnd)
            {
                wayPoint = path.vectorPath[path.vectorPath.Count - 1];
                return false;
            }

            Vector2 nextWaypoint = path.vectorPath[currentWayPoint];

            // check if we reached the waypoint
            if (Vector2.Distance(transform.position, nextWaypoint) < 0.1f)
            {
                currentWayPoint++;
                if (currentWayPoint >= path.vectorPath.Count)
                {
                    reachedEnd = true;
                }
            }

            wayPoint = nextWaypoint;
            return true;
        }

        public Vector2 GetCurWaypoint(Transform transform)
        {
            if (path == null)
            {
                return transform.position;
            } else if (reachedEnd)
            {
                return path.vectorPath[path.vectorPath.Count - 1];
            }

            return path.vectorPath[currentWayPoint];
        }

    }
}
