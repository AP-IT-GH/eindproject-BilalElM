using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeMovement : MonoBehaviour
{
    public float speed = 5f; // Speed of movement

    void Update()
    {
        // Get the horizontal and vertical input axes
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Calculate the direction vector
        Vector3 direction = new Vector3(horizontalInput, 0f, verticalInput);

        // Normalize the direction vector to maintain consistent speed in all directions
        direction.Normalize();

        // Move the cube in the specified direction
        transform.Translate(direction * speed * Time.deltaTime);
    }
}

