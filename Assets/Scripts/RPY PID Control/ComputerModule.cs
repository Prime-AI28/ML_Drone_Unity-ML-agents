using UnityEngine;
using System.Collections;

public class ComputerModule : MonoBehaviour
{
    [Header("Settings")]
    [Range(0, 90)] public float PitchLimit = 45f;
    [Range(0, 90)] public float RollLimit = 45f;

    [Header("PID Controllers")]
    public PID PidThrottle;
    public PID PidPitch;
    public PID PidRoll;

    [Header("Sensors")]
    public BasicGyro Gyro;

    [Header("Computed Values")]
    public float PitchCorrection;
    public float RollCorrection;
    public float HeightCorrection;

    void Start()
    {
        if (Gyro == null)
        {
            Debug.LogError("Gyro not assigned in ComputerModule!");
        }
    }

    public void UpdateComputer(float controlPitch, float controlRoll, float controlHeight)
    {
        if (Gyro == null) return;

        UpdateGyro();
        PitchCorrection = PidPitch.Update(controlPitch * PitchLimit, Gyro.Pitch, Time.deltaTime);
        RollCorrection = PidRoll.Update(Gyro.Roll, controlRoll * RollLimit, Time.deltaTime);
        HeightCorrection = PidThrottle.Update(controlHeight, Gyro.VelocityVector.y, Time.deltaTime);
    }

    private void UpdateGyro()
    {
        Gyro.UpdateGyro(transform);
    }
}
