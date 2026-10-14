using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGem : MonoBehaviour
{
    public float rotationSpeed = 90f; // Rotation speed in degrees per second
    private Canvas childCanvas;

    void Start()
    {
        // Attempt to find a Canvas component in the children of this GameObject
        childCanvas = GetComponentInChildren<Canvas>(true);

        // Optionally, deactivate the canvas at start if it should only be visible after the trigger
        if (childCanvas != null)
        {
            childCanvas.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        // Rotate around the Y axis every frame, based on rotationSpeed and deltaTime
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        // When another object enters the trigger, activate the child canvas
        if (childCanvas != null && other.tag == "Player")
        {
            childCanvas.gameObject.SetActive(true);
            other.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        }
        
    }
}
