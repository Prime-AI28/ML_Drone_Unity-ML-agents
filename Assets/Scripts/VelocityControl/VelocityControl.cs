using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VelocityControl : MonoBehaviour
{
    public StateFinder state;
    public GameObject propFL, propFR, propRR, propRL;

    private float gravity = 9.81f;
    private float maxPitch = 0.175f;
    private float maxRoll = 0.175f;
    private float maxAlpha = 10.0f;

    public float desiredHeight = 0.0f;
    public float desiredVX = 0.0f;
    public float desiredVY = 0.0f;
    public float desiredYaw = 0.0f;
    public float initialHeight = 0.15f;

    private float speedScale = 500.0f;

    void Start()
    {
        state.GetState();
        Rigidbody rb = GetComponent<Rigidbody>();
        //rb.AddForce(Vector3.up * gravity * state.Mass, ForceMode.Acceleration);
    }

    void FixedUpdate()
    {
        state.GetState();
        AdjustVelocityAndForces();
    }

    private void AdjustVelocityAndForces()
    {
        float heightError = state.Altitude - desiredHeight;
        Vector3 desiredVelocity = new Vector3(desiredVY, -heightError, desiredVX);
        Vector3 velocityError = state.VelocityVector - desiredVelocity;
        Vector3 desiredAcceleration = velocityError * -1.0f;

        Vector3 desiredTheta = new Vector3(desiredAcceleration.z / gravity, 0.0f, -desiredAcceleration.x / gravity);
        desiredTheta.x = Mathf.Clamp(desiredTheta.x, -maxPitch, maxPitch);
        desiredTheta.z = Mathf.Clamp(desiredTheta.z, -maxRoll, maxRoll);

        Vector3 thetaError = state.Angles - desiredTheta;
        Vector3 desiredOmega = thetaError * -1.0f;
        desiredOmega.y = desiredYaw;

        Vector3 omegaError = state.AngularVelocityVector - desiredOmega;
        Vector3 desiredAlpha = Vector3.ClampMagnitude(omegaError * -1.0f, maxAlpha);

        float desiredThrust = (gravity + desiredAcceleration.y) / (Mathf.Cos(state.Angles.z) * Mathf.Cos(state.Angles.x));
        desiredThrust = Mathf.Clamp(desiredThrust, 0.0f, 2.0f * gravity);

        Rigidbody rb = GetComponent<Rigidbody>();
        rb.AddRelativeTorque(Vector3.Scale(desiredAlpha, state.Inertia), ForceMode.Acceleration);
        rb.AddRelativeForce(Vector3.up * desiredThrust * state.Mass, ForceMode.Acceleration);

        RotatePropellers(desiredThrust);
    }

    private void RotatePropellers(float thrust)
    {
        float rotationSpeed = thrust * speedScale * Time.deltaTime;
        propFL.transform.Rotate(Vector3.up * rotationSpeed);
        propFR.transform.Rotate(Vector3.up * rotationSpeed);
        propRR.transform.Rotate(Vector3.up * rotationSpeed);
        propRL.transform.Rotate(Vector3.up * rotationSpeed);
    }

    public void Reset()
    {
        state.Reset();
        desiredVX = desiredVY = desiredYaw = 0.0f;
        desiredHeight = 0.0f;
        enabled = true;
    }
}