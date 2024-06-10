using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Target")]
    public Transform target; // The target that the camera will follow

    [Header("Bounds")]
    public float minX;
    public float maxX;
    public float minY;
    public float maxY;

    [Header("Camera Offset")]
    public Vector3 offset; // The offset at which the camera follows the target

    private void LateUpdate()
    {

        if (target != null)
        {
            // Calculate the desired position of the camera
            Vector3 desiredPosition = target.position + offset;

            // Clamp the camera's x and y positions within the specified bounds
            //float clampedX = Mathf.Clamp(desiredPosition.x, minX, maxX);
            float clampedY = Mathf.Clamp(desiredPosition.y, minY, maxY);

            // Set the camera's position to the clamped values
            transform.position = new Vector3(desiredPosition.x, clampedY, transform.position.z);
        }
    }
}
