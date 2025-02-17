using UnityEngine;
using System.Collections;

public class Motor : MonoBehaviour {
    public float UpForce = 0.0f;
    public float SideForce = 0.0f;
    public float Power = 2.0f;
    public float ExceedForce = 0.0f;

    public float YawFactor = 0.0f;
    public bool InvertDirection;
    public float PitchFactor = 0.0f;
    public float RollFactor = 0.0f;
    public float Mass = 0.0f;

    public BasicControl mainController;
    public GameObject Propeller;
    private float SpeedPropeller = 0.0f;

    public void UpdateForceValues() {
        if (mainController == null) {
            Debug.LogError("Main Controller not assigned in Motor!");
            return;
        }

        float UpForceThrottle = Mathf.Clamp(mainController.ThrottleValue, 0, 1) * Power;
        float UpForceTotal = UpForceThrottle;
        UpForceTotal -= mainController.Computer.PitchCorrection * PitchFactor;
        UpForceTotal -= mainController.Computer.RollCorrection * RollFactor;

        UpForce = Mathf.Max(UpForceTotal, 0.0f);
        SideForce = ApplyYawFactor(mainController.Controller.Yaw);

        SpeedPropeller = Mathf.Lerp(SpeedPropeller, UpForce * 2500.0f, Time.deltaTime);
        UpdatePropeller(SpeedPropeller);
    }

    private float ApplyYawFactor(float yawInput) {
        float finalValue = InvertDirection ? Mathf.Clamp(yawInput, -1, 0) : Mathf.Clamp(yawInput, 0, 1);
        return finalValue * YawFactor;
    }

    public void UpdatePropeller(float speed) {
        if (Propeller != null) {
            Propeller.transform.Rotate(0.0f, speed * 2 * Time.deltaTime, 0.0f);
        }
    }
}