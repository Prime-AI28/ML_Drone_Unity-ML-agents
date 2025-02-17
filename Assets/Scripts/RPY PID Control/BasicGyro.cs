using UnityEngine;
using System.Collections;

[System.Serializable]
public class BasicGyro
{
    public float Pitch;
    public float Roll;
    public float Yaw;
    public float Altitude;
    public Vector3 VelocityVector;
    public float VelocityScalar;

    public void UpdateGyro(Transform transform)
    {
        if (transform == null)
        {
            Debug.LogError("Transform is null in BasicGyro!");
            return;
        }

        Pitch = NormalizeAngle(transform.eulerAngles.x);
        Roll = NormalizeAngle(transform.eulerAngles.z);
        Yaw = NormalizeAngle(transform.eulerAngles.y);

        Altitude = transform.position.y;
        VelocityVector = transform.GetComponent<Rigidbody>()?.velocity ?? Vector3.zero;
        VelocityScalar = VelocityVector.magnitude;
    }

    private float NormalizeAngle(float angle)
    {
        return (angle > 180) ? angle - 360 : angle;
    }
}
