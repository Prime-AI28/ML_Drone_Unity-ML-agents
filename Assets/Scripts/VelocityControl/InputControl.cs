using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputControl : MonoBehaviour
{
    public VelocityControl velocityControl;
    private float absoluteHeight = -0.0f;

    void Start()
    {
        if (velocityControl == null)
        {
            Debug.LogError("VelocityControl not assigned!");
        }
    }

    void FixedUpdate()
    {

        if (velocityControl == null) return;

        float pitchInput = Input.GetAxis("Pitch") * 15.0f;
        float rollInput = Input.GetAxis("Roll") * 15.0f;
        float yawInput = Input.GetAxis("Yaw") * 0.5f;
        absoluteHeight += Input.GetAxis("Throttle") * 0.1f;

        velocityControl.desiredVX = pitchInput;
        velocityControl.desiredVY = rollInput;
        velocityControl.desiredYaw = yawInput;
        velocityControl.desiredHeight = Mathf.Clamp(absoluteHeight, -7.5f, 3.5f);
    }
}