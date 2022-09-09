using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformScript : MonoBehaviour
{
    [SerializeField] GameObject[] waypoints;
    int currentWaypointIndex = 0;

    [SerializeField] float speed = 3f;
    public LeverScript lS;


    // Update is called once per frame
    void Update()
    {
        if (lS.leverPulled)
       {
            if (Vector3.Distance(transform.position, waypoints[currentWaypointIndex].transform.position) < .1f)
            {
                if (currentWaypointIndex == 1)
                {
                    currentWaypointIndex = 0;
                }
                else
                {
                currentWaypointIndex = 1;
                }
            }
            // smoothly moves platform towards waypoint
            transform.position = Vector3.MoveTowards(transform.position, waypoints[currentWaypointIndex].transform.position, speed * Time.deltaTime);
       }

    }
}
