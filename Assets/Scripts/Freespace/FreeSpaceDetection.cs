using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class FreeSpaceDetection : MonoBehaviour
{
    public int numHorizontalPoints = 10;
    public int numBins = 5;
    public float fovDegrees = 90f;
    public float maxDist = 100f;
    public bool displayFreespace = false;

    private NativeArray<RaycastCommand> commands;

    void Start()
    {
        if (numBins > numHorizontalPoints)
        {
            Debug.LogError("numBins must be less than or equal to numHorizontalPoints!");
        }
        commands = new NativeArray<RaycastCommand>(numHorizontalPoints, Allocator.Persistent);
    }

    void OnDestroy()
    {
        if (commands.IsCreated)
        {
            commands.Dispose();
        }
    }

    public float[] BatchRaycast()
    {
        var results = new NativeArray<RaycastHit>(numHorizontalPoints, Allocator.Temp);
        Vector3 origin = transform.position;

        // Create a QueryParameters instance
        QueryParameters queryParams = new QueryParameters
        {
            layerMask = LayerMask.GetMask("Default"),
            hitTriggers = QueryTriggerInteraction.Ignore,
            hitBackfaces = false,
            hitMultipleFaces = false
        };

        for (int i = 0; i < numHorizontalPoints; i++)
        {
            float theta = Mathf.Lerp(-fovDegrees / 2.0f, fovDegrees / 2.0f, i / (float)(numHorizontalPoints - 1));
            Vector3 direction = Quaternion.Euler(0, theta, 0) * transform.forward;
            // Use the constructor that includes QueryParameters
            commands[i] = new RaycastCommand(origin, direction, queryParams, maxDist);
        }

        var handle = RaycastCommand.ScheduleBatch(commands, results, 1);
        handle.Complete();

        float[] output = new float[numBins];
        for (int i = 0; i < numBins; i++)
        {
            output[i] = 0.0f;
        }

        int elsPerBin = numHorizontalPoints / numBins;
        for (int i = 0; i < numHorizontalPoints; i++)
        {
            int bin = i / elsPerBin;
            output[bin] += results[i].distance / elsPerBin;
        }

        results.Dispose();
        return output;
    }

}
