using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] float minHeightDifference = 0f; // Minimum height difference to start scaling
    [SerializeField] float maxHeightDifference = 10f; // Height difference at which the scale will be at its minimum
    [SerializeField] float minScale = 0.5f; // The minimum scale value
    [SerializeField] float maxScale = 0.5f;
    [SerializeField] float minAlpha = 0.5f; // The minimum alpha value
    [SerializeField] float maxAlpha = 0.5f; // The minimum alpha value


    private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        // Get the SpriteRenderer component from the GameObject
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        // Update position to follow the player on X and Z, but keep original Y
        transform.position = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);

        // Calculate the absolute height difference
        float heightDifference = Mathf.Abs(transform.position.y - player.transform.position.y);

        // Scale the GameObject based on the height difference
        float scale = Mathf.Lerp(maxScale, minScale, Mathf.InverseLerp(minHeightDifference, maxHeightDifference, heightDifference));
        transform.localScale = new Vector3(scale, scale, scale);

        float alpha = Mathf.Lerp(maxAlpha, minAlpha, Mathf.InverseLerp(minHeightDifference, maxHeightDifference, heightDifference));
        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, alpha);
    }
}
