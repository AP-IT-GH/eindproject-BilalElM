using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using System.Diagnostics;

public class CarController : MonoBehaviour
{
    public const string HORIZONTAL = "Horizontal";
    public const string VERTICAL = "Vertical";

    public Rigidbody rb;
    public Vector3 newCenterOfMass;
    public Transform startPosition;

    public float horizontalInput;
    public float verticalInput;
    [SerializeField] public float brakeForce = 90000f;
    [SerializeField] public bool isBraking;
    public float currentSteerAngle;
    public float rotationSpeed = 10000f;

    [SerializeField] public float motorForce;
    [SerializeField] public float maxSteerAngle;

    public List<WheelCollider> wheels;
    public List<Transform> wheelTransforms;

    public void Start()
    {
        rb.centerOfMass = newCenterOfMass;

        // Assign wheels automatically using the wheelTransforms list
        for (int i = 0; i < wheels.Count; i++)
        {
            WheelCollider wheel = wheels[i];
            Transform wheelTransform = wheelTransforms[i];

            // Assign the wheel collider and transform to the corresponding element in the list
            wheelTransform.rotation = wheel.transform.rotation;
            wheelTransform.position = wheel.transform.position;
        }
    }

    public void FixedUpdate()
    {
        GetInput();
        HandleMotor();
        HandleSteering();
        UpdateWheels();
        RotateWheel();
    }

    public void GetInput()
    {
        horizontalInput = Input.GetAxis(HORIZONTAL);
        verticalInput = Input.GetAxis(VERTICAL);
        isBraking = Input.GetKey(KeyCode.Space);
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
        wheels[0].brakeTorque = isBraking ? brakeForce : 0f;
        wheels[1].brakeTorque = isBraking ? brakeForce : 0f;
        wheels[2].brakeTorque = isBraking ? brakeForce : 0f;
        wheels[3].brakeTorque = isBraking ? brakeForce : 0f;
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
            //Debug.LogWarning("Stuurobject niet gevonden!");
        }
    }
}