using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Policies;

public class DroneAcademy : MonoBehaviour
{
    private void Start()
    {
        Academy.Instance.EnvironmentParameters.RegisterCallback("wind_strength", OnWindStrengthChanged);
        Academy.Instance.OnEnvironmentReset += EnvironmentReset;
    }

    private void OnDestroy()
    {
        Academy.Instance.EnvironmentParameters.RegisterCallback("wind_strength", OnWindStrengthChanged);
        Academy.Instance.OnEnvironmentReset -= EnvironmentReset;
    }

    private void EnvironmentReset()
    {
        Debug.Log("Environment Reset Triggered");
        // Implement any global reset logic here if needed
    }

    private void OnWindStrengthChanged(float windStrength)
    {
        Debug.Log($"Wind Strength Changed: {windStrength}");
        // Apply wind strength adjustments to the environment
    }
}