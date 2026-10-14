using UnityEngine;

public class SmoothMover : MonoBehaviour
{
    public Vector3 targetPosition;
    public float moveSpeed = 5.0f;
    public float wait_Time;
    private bool start = false;

    private void Start() {
        Invoke("StartBool", wait_Time);
    }

    void StartBool()
    {
        start = true;
    }

    private void Update()
    {
        if(start)
        {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            
            if (transform.position == targetPosition)
            {
                Destroy(this); // Remove this script component when the target is reached
            }
        }
    }
}
