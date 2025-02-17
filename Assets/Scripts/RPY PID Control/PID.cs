using UnityEngine;

[System.Serializable]
public class PID
{
    public float pFactor, iFactor, dFactor;
    private float integral;
    private float lastError;
    private float maxIntegral = 10.0f;

    public PID(float pFactor, float iFactor, float dFactor)
    {
        this.pFactor = pFactor;
        this.iFactor = iFactor;
        this.dFactor = dFactor;
    }

    public float Update(float setpoint, float actual, float timeFrame)
    {
        float error = setpoint - actual;
        integral = Mathf.Clamp(integral + error * timeFrame, -maxIntegral, maxIntegral);
        float derivative = (error - lastError) / timeFrame;
        lastError = error;

        float output = (error * pFactor) + (integral * iFactor) + (derivative * dFactor);
        return Mathf.Clamp(output, -1.0f, 1.0f);
    }
}
