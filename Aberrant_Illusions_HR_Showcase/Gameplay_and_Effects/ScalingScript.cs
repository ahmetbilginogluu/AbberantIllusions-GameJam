using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScalingScript : MonoBehaviour
{
    public Vector3 scaleOffset;
    private Vector3 startScale;
    private Vector3 endScale;
    public float timeToScale = 1.0f; // Duration of the scaling in seconds
    private float timeStartedScaling;
    private bool shouldScale = false;
    public GameObject prefab;
    private static ScalingScript instance = null;
    public Vector3 instantiationPosition;
    public GameObject prefab_player;
    public GameObject spinnies;

    void Awake()
    {
        // Check if instance already exists
        if (instance == null)
        {
            // If not, set instance to this
            instance = this;

            // Makes the object not be destroyed automatically when loading a new scene.
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Instantiate(prefab_player);
            spinnies.GetComponent<StartMove>().enabled = false;
            spinnies.transform.position += new Vector3(0, 0.6f, 0);
            // If instance already exists and it's not this, then destroy this to enforce the singleton pattern.
            Destroy(gameObject);
        }
    }



    void Start()
    {
        startScale = transform.localScale;
        endScale = startScale + scaleOffset;
        timeStartedScaling = Time.time;
        shouldScale = true;
    }

    void Update()
    {
        if (shouldScale)
        {
            float timeSinceStarted = Time.time - timeStartedScaling;
            float percentageComplete = timeSinceStarted / timeToScale;

            transform.localScale = Vector3.Lerp(startScale, endScale, percentageComplete);

            if (percentageComplete >= 1.0f)
            {
                Instantiate(prefab);
                shouldScale = false;
            }
        }
    }
}