using UnityEngine;
using System.Collections;

public class BarrelMove : MonoBehaviour
{
    public Transform[] points; // Points A, B, C, D
    public float speed = 5f;
    public float rotationSpeed = 2f;

    // This method starts the movement coroutine
    public void StartMoving()
    {
        StartCoroutine(MoveThroughPoints());
    }

    private IEnumerator MoveThroughPoints()
    {
        for (int i = 0; i < points.Length; i++)
        {
            Transform point = points[i];
            Quaternion targetRotation = Quaternion.LookRotation(point.position - transform.position);
            targetRotation = Quaternion.Euler(0, targetRotation.eulerAngles.y + 90, 0);
            if (i == points.Length - 1)
            {
                targetRotation = Quaternion.Euler(0, targetRotation.eulerAngles.y, 0);
            }
            while (Vector3.Distance(transform.position, point.position) > 0.001f)
            {
                transform.position = Vector3.MoveTowards(transform.position, point.position, speed * Time.deltaTime);
                if(i != points.Length - 3)
                {transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);}
                yield return null;
            }

            if (i == points.Length - 1)
            {
                Destroy(gameObject);
            }
        }
    }
}
