using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraScript : MonoBehaviour
{
    public Transform player; // Assign the player's transform in the Inspector
    public Vector3 offset;   // Offset distance between the player and camera

    void Start()
    {
        // If no offset is set in the Inspector, calculate it based on initial positions
        if (offset == Vector3.zero && player != null)
        {
            offset = transform.position - player.position;
        }
    }

    void LateUpdate()
    {
        // Follow the player but keep the camera's original rotation
        if (player != null)
        {
            transform.position = player.position + offset;
        }
    }
}
