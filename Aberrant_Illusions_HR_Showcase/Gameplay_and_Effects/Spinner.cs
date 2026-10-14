using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spinner : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> objectsToSpin = new List<GameObject>();

    [SerializeField]
    private float spinSpeedSelf = 50f; // Speed of self-spinning around Y-axis

    [SerializeField]
    private float spinSpeedObjects = 50f; // Speed of spinning objects in list around Z-axis

    private void Start()
    {
        // Set a random initial Y-rotation from 90, 180, 270, 360 degrees
        float[] angles = { 45f, 90f, 135f, 180f, 225f, 270f, 315f, 360f };
        int randomIndex = Random.Range(0, angles.Length);
        transform.rotation = Quaternion.Euler(0, angles[randomIndex], 0);
    }


    private void Update()
    {
        // Spin this GameObject around its Y-axis
        transform.Rotate(0, spinSpeedSelf * Time.deltaTime, 0, Space.World);

        // Spin each object in the list around its Z-axis
        foreach (GameObject obj in objectsToSpin)
        {
            if (obj != null)
            {
                obj.transform.Rotate(0, 0, spinSpeedObjects * Time.deltaTime, Space.Self);
            }
        }
    }
}
