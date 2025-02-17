using UnityEngine;
using System.Collections;

public class CameraLookAt : MonoBehaviour
{
    public Transform drone;
    private Quaternion originalRotation;
    public Vector3 offset;

    void Start()
    {
        if (drone == null)
        {
            Debug.LogError("Drone transform not assigned in CameraLookAt!");
            return;
        }
        originalRotation = transform.rotation;
    }

    void LateUpdate()
    {
        if (drone == null) return;

        float yaw = drone.eulerAngles.y;
        Vector3 relativeOffset = Quaternion.Euler(0, yaw, 0) * offset;
        transform.position = drone.position + relativeOffset;
        transform.rotation = Quaternion.Euler(0, yaw, 0) * originalRotation;
    }
}