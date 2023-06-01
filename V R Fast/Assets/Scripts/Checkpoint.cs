using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public Canvas checkpointCanvas;

    private static int lastCheckpointIndex = -1; // Index of the last triggered checkpoint

    public int checkpointIndex; // Index of this checkpoint (starts at 0)

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (checkpointIndex == lastCheckpointIndex + 1) // Check if this is the next checkpoint
            {
                Debug.Log("Checkpoint triggered!");
                lastCheckpointIndex = checkpointIndex;
                HideMissedCheckpointCanvas(); // Hide the "Wrong Checkpoint" canvas if shown
            }
            else if (checkpointIndex == 0/** && lastCheckpointIndex == 2*/) // Check if this is checkpoint 1 after passing through checkpoint 2
            {
                Debug.Log("Checkpoint 1 triggered!");
                lastCheckpointIndex = checkpointIndex;
                HideMissedCheckpointCanvas();
            }
            else
            {
                Debug.Log("Missed checkpoint! Please go back...");
                ShowMissedCheckpointCanvas();
            }
        }
    }

    private void ShowMissedCheckpointCanvas()
    {
        checkpointCanvas.gameObject.SetActive(true);
    }

    private void HideMissedCheckpointCanvas()
    {
        checkpointCanvas.gameObject.SetActive(false);
    }
}




