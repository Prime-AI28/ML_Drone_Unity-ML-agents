using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowScript : MonoBehaviour
{
    public Transform drone;
    public Vector3 offset = new Vector3(0, 2, -4);
    public float followSpeed = 5f;
    public float rotationSpeed = 3f;

    private Vector3 velocity = Vector3.zero;

    void LateUpdate()
    {
        if (drone == null)
        {
            Debug.LogError("Drone transform not assigned in CameraFollowScript!");
            return;
        }

        Vector3 targetPosition = drone.position + drone.TransformDirection(offset);
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, 1f / followSpeed);

        Quaternion targetRotation = Quaternion.LookRotation(drone.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }
}
