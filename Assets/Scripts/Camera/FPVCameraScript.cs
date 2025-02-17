using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPVCameraScript : MonoBehaviour
{
    public Transform droneTransform;
    public Vector3 offset = new Vector3(0, 0.5f, 0);
    [Range(0, 1)] public float smoothingFactor = 0.5f;
    [Range(0, 1)] public float rotationLimitRatio = 0.5f;

    void LateUpdate()
    {
        if (droneTransform == null)
        {
            Debug.LogError("Drone transform not assigned in FPVCameraScript!");
            return;
        }

        transform.position = Vector3.Lerp(transform.position, droneTransform.position + droneTransform.rotation * offset, smoothingFactor);

        Vector3 euler = droneTransform.rotation.eulerAngles;
        float x = NormalizeAngle(euler.x) * rotationLimitRatio;
        float z = NormalizeAngle(euler.z) * rotationLimitRatio;

        Quaternion targetRotation = Quaternion.Euler(x, euler.y, z);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, smoothingFactor);
    }

    private float NormalizeAngle(float angle)
    {
        return (angle > 180.0f) ? angle - 360.0f : angle;
    }
}
