using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    private const string HORIZONTAL = "Horizontal";
    private const string VERTICAL = "Vertical";

    public Rigidbody rb;
    public Vector3 newCenterOfMass;
    public Transform startPosition;

    private float horizontalInput;
    private float verticalInput;
    [SerializeField] private float brakeForce = 100000f;
    [SerializeField] private bool isBraking;
    private float currentSteerAngle;
    private float rotationSpeed = 10000f;

    [SerializeField] private float motorForce;
    [SerializeField] private float maxSteerAngle;

    public List<WheelCollider> wheels;
    public List<Transform> wheelTransforms;

    private void Start()
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

    private void FixedUpdate()
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

    private void HandleMotor()
    {
        float motorTorque = verticalInput * motorForce;

        foreach (WheelCollider wheel in wheels)
        {
            wheel.motorTorque = motorTorque;

        }
    }

    private void HandleSteering()
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

    private void UpdateWheels()
    {
        for (int i = 0; i < wheels.Count; i++)
        {
            UpdateSingleWheel(wheels[i], wheelTransforms[i]);
        }
    }

    private void UpdateSingleWheel(WheelCollider wheel, Transform wheelTransform)
    {
        Vector3 position;
        Quaternion rotation;
        wheel.GetWorldPose(out position, out rotation);
        wheelTransform.rotation = rotation;
        wheelTransform.position = position;
    }

    private void RotateWheel()
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
