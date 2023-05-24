using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;

public class AIAgent : Agent
{
    private const string HORIZONTAL = "Horizontal";
    private const string VERTICAL = "Vertical";

    public Rigidbody rb;
    public Vector3 newCenterOfMass;
    public Transform startPosition;

    private float horizontalInput;
    private float verticalInput;
    [SerializeField] private float brakeForce = 90000f;
    [SerializeField] private bool isBraking;
    private float currentSteerAngle;
    private float rotationSpeed = 10000f;

    [SerializeField] private float motorForce;
    [SerializeField] private float maxSteerAngle;

    public List<WheelCollider> wheels;
    public List<Transform> wheelTransforms;

    public override void Initialize()
    {
        rb.centerOfMass = newCenterOfMass;
    }

    public override void OnEpisodeBegin()
    {
        // Reset the car's position and rotation
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        transform.position = startPosition.position;
        transform.rotation = startPosition.rotation;
    }
    public override void CollectObservations(VectorSensor sensor)
    {
        // Add observations about the current state of the car here
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(transform.rotation);
        sensor.AddObservation(rb.velocity);
    }

    public void OnActionReceived(float[] vectorAction)
    {
        // Apply the ML agent's actions to control the car
        horizontalInput = vectorAction[0];
        verticalInput = vectorAction[1];
        isBraking = vectorAction[2] > 0.5f;

        // Execute the car controls
        HandleMotor();
        HandleSteering();
        UpdateWheels();
        RotateWheel();

        // Set a reward or penalty based on the car's behavior
        // ...
    }

    public void HandleMotor()
    {
        float motorTorque = verticalInput * motorForce;

        foreach (WheelCollider wheel in wheels)
        {
            wheel.motorTorque = motorTorque;
        }
    }

    public void HandleSteering()
    {
        currentSteerAngle = maxSteerAngle * horizontalInput;

        // Apply steering to the front wheels
        wheels[0].steerAngle = currentSteerAngle;
        wheels[1].steerAngle = currentSteerAngle;

        // Apply braking to the rear wheels
        foreach (WheelCollider wheel in wheels)
        {
            wheel.brakeTorque = isBraking ? brakeForce : 0f;
        }
    }

    public void UpdateWheels()
    {
        for (int i = 0; i < wheels.Count; i++)
        {
            UpdateSingleWheel(wheels[i], wheelTransforms[i]);
        }
    }

    public void UpdateSingleWheel(WheelCollider wheel, Transform wheelTransform)
    {
        Vector3 position;
        Quaternion rotation;
        wheel.GetWorldPose(out position, out rotation);
        wheelTransform.rotation = rotation;
        wheelTransform.position = position;
    }

    public void RotateWheel()
    {
        float wheelRotation = horizontalInput * rotationSpeed * Time.deltaTime;
        GameObject wheelObject = GameObject.Find("sport_car_1_steering_wheel");

        if (wheelObject != null)
        {
            Vector3 currentRotation = wheelObject.transform.localEulerAngles;
            Vector3 desiredRotation = new Vector3(currentRotation.x, currentRotation.y, wheelRotation);
            wheelObject.transform.localEulerAngles = desiredRotation;
        }
        else
        {
            Debug.LogWarning("Stuurobject niet gevonden!");
        }
    }
}