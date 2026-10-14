using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stairs : MonoBehaviour
{
    public float scaleSpeed = 0.5f; // Adjust this to scale faster or slower

    private void OnTriggerEnter(Collider other)
    {
        StartCoroutine(ScaleDownAndDestroy(transform.parent.gameObject));
    }

    private IEnumerator ScaleDownAndDestroy(GameObject target)
    {
        Vector3 originalScale = target.transform.localScale;
        Vector3 targetScale = new Vector3(originalScale.x, 0, originalScale.z);

        float currentTime = 0.0f;

        while (target.transform.localScale.y > 0)
        {
            currentTime += Time.deltaTime;
            float scaleProgress = currentTime / scaleSpeed;
            target.transform.localScale = Vector3.Lerp(originalScale, targetScale, scaleProgress);
            yield return null;
        }

        Destroy(target);
    }
}
