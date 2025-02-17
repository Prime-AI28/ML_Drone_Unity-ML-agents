using UnityEngine;
using System.Collections;

public class BasicControl : MonoBehaviour
{
    [Header("Control")]
    public Controller Controller;
    public float ThrottleIncrease = 1.0f;

    [Header("Motors")]
    public Motor[] Motors;
    public float ThrottleValue;

    [Header("Internal")]
    public ComputerModule Computer;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody missing from BasicControl!");
        }
    }

    void FixedUpdate()
    {
        if (Controller == null || Computer == null) return;

        Computer.UpdateComputer(Controller.Pitch, Controller.Roll, Controller.Throttle * ThrottleIncrease);
        ThrottleValue = Computer.HeightCorrection;
        ApplyMotorForces();
    }

    private void ApplyMotorForces()
    {
        if (Motors.Length == 0) return;

        float yawTorque = 0.0f;
        foreach (Motor motor in Motors)
        {
            motor.UpdateForceValues();
            yawTorque += motor.SideForce;
            rb.AddForceAtPosition(transform.up * motor.UpForce, motor.transform.position, ForceMode.Force);
        }
        rb.AddTorque(Vector3.up * yawTorque, ForceMode.Force);
    }
}