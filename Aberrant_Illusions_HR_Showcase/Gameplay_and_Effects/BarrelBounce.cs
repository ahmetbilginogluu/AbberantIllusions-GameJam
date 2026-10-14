using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelBounce : MonoBehaviour
{
    public float initialSpeed = 5f; // Initial speed at which the object starts moving upwards
    public float acceleration = -9.81f; // Acceleration downwards, simulating gravity
    public float minHeight = 0f; // The ground level or minimum height
    public float maxHeight = 5f; // The maximum height the object can reach

    private float currentSpeed; // The current speed of the object
    private bool movingUp = true; // Indicates if the object is moving upwards
    [SerializeField] float start_time = 0.5f;
    [SerializeField] float start_Offset = 0.3f;
    private bool start = false;

    void Start()
    {
        Random.Range(start_time, start_time + start_Offset);
        Invoke("StartBool", start_time);
    }

    private void StartBool()
    {
        currentSpeed = initialSpeed;
        start = true;
    }


    void Update()
    {
        if(start)
        {
                // Update the position based on the current speed
            transform.position += new Vector3(0, currentSpeed * Time.deltaTime, 0);

            // Update the current speed based on acceleration
            currentSpeed += acceleration * Time.deltaTime;

            // Check if the object is moving downwards and has reached the minimum height
            if (transform.position.y <= minHeight)
            {
                // Reset the speed to the initial speed to "bounce" back up
                currentSpeed = initialSpeed;
                movingUp = true;
            }
            // Check if the object has reached the maximum height and starts moving down
            else if (transform.position.y >= maxHeight && movingUp)
            {
                // Reverse the direction (start moving down)
                movingUp = false;
                currentSpeed = acceleration * Time.deltaTime; // Start slowing down immediately
            }
        }
    }
}
