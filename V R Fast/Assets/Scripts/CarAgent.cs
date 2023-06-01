using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using UnityEngine;

 class CarAgent : Agent
{
    public CarController carController;

    public override void OnEpisodeBegin()
    {
        // Reset the car's position and rotation
        carController.transform.position = carController.startPosition.position;
        carController.transform.rotation = carController.startPosition.rotation;
    }

    public void CollectObservations(VectorSensor sensor)
    {
        // Add observations to the sensor

        // Car's velocity
        sensor.AddObservation(carController.rb.velocity.magnitude);

        // Car's position
        sensor.AddObservation(carController.transform.position);

        // Car's rotation
        sensor.AddObservation(carController.transform.rotation);
    }

    public void OnActionReceived(float[] vectorAction)
    {
        // Apply actions to the car controller

        float horizontalInput = vectorAction[0];
        float verticalInput = vectorAction[1];
        bool isBraking = vectorAction[2] > 0f;

        carController.horizontalInput = horizontalInput;
        carController.verticalInput = verticalInput;
        carController.isBraking = isBraking;
    }

    public void Heuristic(float[] actionsOut)
    {
        // Implement heuristics for manual control of the car

        actionsOut[0] = Input.GetAxis("Horizontal");
        actionsOut[1] = Input.GetAxis("Vertical");
        actionsOut[2] = Input.GetKey(KeyCode.Space) ? 1f : 0f;
    }
}
