using System.Collections;
using UnityEngine;

public class LabFixedPlatforms : MonoBehaviour
{
    public Transform[] waypoints;
    private float speed = 1f;
    private int currentWaypointIndex = 0;
    private bool isWaiting = false;

    private void Start()
    {
        StartCoroutine(Patrol());
    }
    IEnumerator Patrol()
    {
        while (true)
        {
            if (!isWaiting)
            {
                MoveTowardsWayPoints();
                if (Vector3.Distance(transform.position, waypoints[currentWaypointIndex].position) < 0.5f)
                {
                    currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
                }
            }
            yield return null;
        }
        
    }
    void MoveTowardsWayPoints()
    {
        if (waypoints.Length == 0) return;

        Vector3 dir = waypoints[currentWaypointIndex].position - transform.position;
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, waypoints[currentWaypointIndex].position, step);
    }
}
