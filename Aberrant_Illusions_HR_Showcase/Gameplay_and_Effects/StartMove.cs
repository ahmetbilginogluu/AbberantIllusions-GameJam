using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMove : MonoBehaviour
{
    public Vector3 offset;
    private Vector3 startPosition;
    private Vector3 endPosition;
    [SerializeField] float timeToMove = 1.0f; // Duration of the move in seconds
    private float timeStartedLerping;
    private bool shouldMove = false;
    [SerializeField] GameObject prefab;

    void Start()
    {
        startPosition = transform.position;
        endPosition = startPosition + offset;
        timeStartedLerping = Time.time;
        shouldMove = true;
    }

    void Update()
    {
        if (shouldMove)
        {
            float timeSinceStarted = Time.time - timeStartedLerping;
            float percentageComplete = timeSinceStarted / timeToMove;

            transform.position = Vector3.Lerp(startPosition, endPosition, percentageComplete);

            if (percentageComplete >= 1.0f)
            {
                if(prefab != null)
                {
                    Instantiate(prefab, transform.position, transform.rotation, transform.parent).transform.localScale = transform.localScale;
                    DestroyObject(gameObject);
                }
                shouldMove = false;
            }
        }
    }
}
