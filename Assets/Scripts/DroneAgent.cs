using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class DroneAgent : Agent
{
    public VelocityControl velocityControl;
    public StateFinder stateFinder;

    public int startRegionIndex = -1;
    public GameObject[] startRegions;
    public int endRegionIndex = -1;
    public GameObject[] endRegions;

    private GameObject currStartRegion;
    private GameObject currEndRegion;
    private bool collided = false;
    private System.Random rand;

    public float FORWARD_VELOCITY = 5f;
    public float YAW_RATE = 45f;
    public float DONE_DISTANCE = 1f;

    private float targetHeight = 5.0f;  // Height for takeoff
    private float landingHeight = -6.5f; // Height for landing
    private bool reachedTargetHeight = false;
    private bool nearEndRegion = false;

    // Task Phases: 0 = Takeoff, 1 = Navigation, 2 = Landing
    private int taskPhase = 0;

    public override void Initialize()
    {
        rand = new System.Random();
        SelectStartAndEndRegions();
    }

    public override void OnEpisodeBegin()
    {
        // Existing code
        SelectStartAndEndRegions();

        collided = false;
        reachedTargetHeight = false;
        nearEndRegion = false;
        taskPhase = 0;

        velocityControl.Reset();

        Rigidbody rb = GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        // Add rotation reset
        transform.rotation = Quaternion.identity; // Reset GameObject rotation
        rb.rotation = Quaternion.identity; // Ensure Rigidbody rotation matches

        // Set desired height for takeoff
        velocityControl.desiredHeight = targetHeight;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(taskPhase);
        sensor.AddObservation(Vector3.Distance(transform.position, currEndRegion.transform.position));
        sensor.AddObservation(NormalizedHeading());
        sensor.AddObservation(transform.position.y - targetHeight);
        sensor.AddObservation(velocityControl.desiredVX / FORWARD_VELOCITY);
        sensor.AddObservation(velocityControl.desiredYaw / YAW_RATE);
        sensor.AddObservation(collided ? 1.0f : 0.0f);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        float pitchInput = actions.ContinuousActions[0];  // Forward/Backward
        float rollInput = actions.ContinuousActions[1];   // Left/Right
        float yawInput = actions.ContinuousActions[2];    // Rotation
        float throttleInput = actions.ContinuousActions[3]; // Up/Down

        velocityControl.desiredVX = pitchInput * FORWARD_VELOCITY;
        velocityControl.desiredVY = rollInput * FORWARD_VELOCITY;
        velocityControl.desiredYaw = yawInput * YAW_RATE;

        if (taskPhase == 0) // **Step 1: Takeoff**
        {
            velocityControl.desiredHeight = targetHeight; // Drone ascends

            if (transform.position.y >= targetHeight - 0.5f)
            {
                reachedTargetHeight = true;
                taskPhase = 1; // Move to Navigation phase
                AddReward(0.5f); // Reward for reaching altitude
            }
        }
        else if (taskPhase == 1) // **Step 2: Navigation**
        {
            velocityControl.desiredHeight += throttleInput * 0.1f; // Adjust height smoothly

            float distanceToEnd = Vector3.Distance(transform.position, currEndRegion.transform.position);
            if (distanceToEnd < 2.0f)
            {
                nearEndRegion = true;
                taskPhase = 2; // Move to Landing Phase
            }
        }
        else if (taskPhase == 2) // **Step 3: Landing**
        {
            velocityControl.desiredHeight = landingHeight; // Descend slowly

            if (transform.position.y <= landingHeight + 0.5f && nearEndRegion)
            {
                AddReward(1.0f); // Big reward for landing
                EndEpisode();
            }
        }

        // Reward movement toward goal
        float currentDistance = Vector3.Distance(transform.position, currEndRegion.transform.position);
        if (taskPhase == 1 && currentDistance < 10.0f)
        {
            AddReward(0.1f);
        }

        // Collision penalty
        if (collided)
        {
            AddReward(-1.0f);
            EndEpisode();
        }
    }

    private float NormalizedHeading()
    {
        Vector3 targetDirection = (currEndRegion.transform.position - transform.position).normalized;
        Vector3 currentHeading = transform.forward;
        return Vector3.SignedAngle(currentHeading, targetDirection, Vector3.up) / 180f;
    }

    private void SelectStartAndEndRegions()
    {
        startRegionIndex = startRegionIndex == -1 ? rand.Next(startRegions.Length) : startRegionIndex;
        endRegionIndex = endRegionIndex == -1 ? rand.Next(endRegions.Length) : endRegionIndex;

        currStartRegion = startRegions[startRegionIndex];
        currEndRegion = endRegions[endRegionIndex];

        transform.position = currStartRegion.transform.position;
        transform.position += new Vector3(0, 0.2f, 0);
    }

    private void OnCollisionEnter(Collision other)
    {
        collided = true;
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continuousActions = actionsOut.ContinuousActions;
        continuousActions[0] = Input.GetAxis("Pitch");
        continuousActions[1] = Input.GetAxis("Roll");
        continuousActions[2] = Input.GetAxis("Yaw");
        continuousActions[3] = Input.GetAxis("Throttle");
    }
}
