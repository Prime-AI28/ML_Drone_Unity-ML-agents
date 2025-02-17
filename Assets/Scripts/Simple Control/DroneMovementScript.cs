using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneMovementScript : MonoBehaviour
{
    private Rigidbody droneRb;
    public float upForce = 98.1f;
    private float movementSpeed = 5.0f;
    private float rotationSpeed = 2.5f;
    private float maxTiltAngle = 20f;

    void Awake()
    {
        droneRb = GetComponent<Rigidbody>();
        if (droneRb == null)
        {
            Debug.LogError("Rigidbody missing from DroneMovementScript!");
        }
    }

    void FixedUpdate()
    {
        HandleMovement();
        HandleRotation();
        ApplyForces();
    }

    private void HandleMovement()
    {
        float verticalInput = Input.GetAxis("Throttle");
        float forwardInput = Input.GetAxis("Vertical");
        float sidewaysInput = Input.GetAxis("Horizontal");

        Vector3 movement = new Vector3(sidewaysInput * movementSpeed, verticalInput * upForce, forwardInput * movementSpeed);
        droneRb.AddRelativeForce(movement, ForceMode.Force);
    }

    private void HandleRotation()
    {
        float yawInput = Input.GetAxis("Yaw") * rotationSpeed;
        droneRb.AddTorque(Vector3.up * yawInput, ForceMode.Force);
    }

    private void ApplyForces()
    {
        Vector3 clampedVelocity = Vector3.ClampMagnitude(droneRb.velocity, movementSpeed);
        droneRb.velocity = clampedVelocity;

        float tiltX = Mathf.Clamp(-droneRb.velocity.z * maxTiltAngle / movementSpeed, -maxTiltAngle, maxTiltAngle);
        float tiltZ = Mathf.Clamp(droneRb.velocity.x * maxTiltAngle / movementSpeed, -maxTiltAngle, maxTiltAngle);

        Quaternion targetRotation = Quaternion.Euler(tiltX, transform.eulerAngles.y, tiltZ);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 2f);
    }
}
