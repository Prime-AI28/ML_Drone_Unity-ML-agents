using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StateFinder : MonoBehaviour
{
    public float Altitude;
    public Vector3 Angles;
    public Vector3 VelocityVector;
    public Vector3 AngularVelocityVector;
    public Vector3 Inertia;
    public float Mass;

    private bool initialized = false;
    private Rigidbody rb;


    public void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            Inertia = rb.inertiaTensor;
            Mass = rb.mass;
            initialized = true;
            rb.velocity = Vector3.zero; // Reset any pre-existing velocity
            rb.angularVelocity = Vector3.zero; // Prevent unintentional rotation
        }
        else
        {
            Debug.LogError("Rigidbody component missing on object!");
        }
    }

    public void GetState()
    {
        if (!initialized) return;

        Altitude = transform.position.y;
        VelocityVector = transform.InverseTransformDirection(rb.velocity);
        AngularVelocityVector = transform.InverseTransformDirection(rb.angularVelocity);

        Vector3 worldDown = transform.InverseTransformDirection(Vector3.down);
        float pitch = worldDown.z;
        float roll = -worldDown.x;
        float yaw = transform.eulerAngles.y;

        Angles = new Vector3(pitch, yaw, roll);
    }

    public void Reset()
    {
        Altitude = 0.0f;
        VelocityVector = Vector3.zero;
        AngularVelocityVector = Vector3.zero;
        Angles = Vector3.zero;
    }
}
