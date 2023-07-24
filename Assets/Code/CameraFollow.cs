using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // The object that the camera will follow
    public Vector3 offset = Vector3.zero; // The offset between the target and the camera
    public float smoothSpeed = 0.125f; // The smoothness of camera movement. A value closer to 1 is smoother.

    private Vector3 desiredPosition;

    void LateUpdate()
    {
        if (target == null)
            return;

        // Calculate the desired position the camera should move to
        desiredPosition = target.position + offset;

        // Use SmoothDamp to interpolate between the current position and the desired position
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // Apply the new position to the camera
        transform.position = smoothedPosition;

        // Make the camera look at the target object
        transform.LookAt(target);
    }
}

