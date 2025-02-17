using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPulseNoise : MonoBehaviour
{
    public Rigidbody drone;
    public bool applyForce = true;
    public float strengthCoefficient = 0.0015f;
    public float strengthMean = 30.0f;
    public float strengthVariance = 20.0f;
    public float pulsePeriodMean = 7;
    public float pulsePeriodVariance = 5;
    public float pulseDurationMean = 10;
    public float pulseDurationVariance = 2;

    private System.Random random;
    private float pulseTimer = 0.0f;
    private float pulsePeriod;
    private float pulseDuration;
    private float currentStrength = 0.0f;
    private int pulseMode = 0;

    void Start()
    {
        random = new System.Random();
        SetNewPulsePeriod();
    }

    void FixedUpdate()
    {
        HandlePulse();
    }

    private void HandlePulse()
    {
        if (!applyForce || drone == null) return;

        pulseTimer += Time.deltaTime;

        if (pulseMode == 0 && pulseTimer >= pulsePeriod)
        {
            StartPulse();
        }
        else if (pulseMode == 1 && pulseTimer >= pulseDuration)
        {
            EndPulse();
        }

        ApplyRandomForce();
    }

    private void StartPulse()
    {
        pulseTimer = 0.0f;
        pulseDuration = SamplePositive(pulseDurationMean, pulseDurationVariance);
        currentStrength = SamplePositive(strengthMean, strengthVariance);
        pulseMode = 1;
    }

    private void EndPulse()
    {
        pulseTimer = 0.0f;
        SetNewPulsePeriod();
        currentStrength = 0.0f;
        pulseMode = 0;
    }

    private void ApplyRandomForce()
    {
        Vector3 forceDirection = Random.insideUnitSphere;
        drone.AddForce(forceDirection * currentStrength * strengthCoefficient, ForceMode.Impulse);
    }

    private void SetNewPulsePeriod()
    {
        pulsePeriod = SamplePositive(pulsePeriodMean, pulsePeriodVariance);
    }

    private float SamplePositive(float mean, float variance)
    {
        return Mathf.Abs((float)random.NextDouble() * Mathf.Sqrt(variance) + mean);
    }
}