using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public float speed = 2f;

    public Transform platform;
    public Transform startPoint;
    public Transform endPoint;

    private int direction = 1;

    private void Update()
    {
        Vector2 target = CurrentMoveTarget();

        platform.position = Vector2.Lerp(platform.position, target, speed * Time.deltaTime);

        float distance = (target - (Vector2)platform.position).magnitude;

        if (distance <= 0.1)
        {
            direction *= -1;
        }
    }

    private Vector2 CurrentMoveTarget()
    {
        if (direction == 1)
            return startPoint.position;
        else
            return endPoint.position;
    }

    private void OnDrawGizmos()
    {
        if (platform != null && startPoint != null && endPoint != null)
        {
            Gizmos.DrawLine(platform.transform.position, startPoint.position);
            Gizmos.DrawLine(platform.transform.position, endPoint.position);
        }
    }
}
