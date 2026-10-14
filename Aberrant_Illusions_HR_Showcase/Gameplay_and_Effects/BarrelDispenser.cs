using UnityEngine;
using System.Collections;

public class BarrelDispenser : MonoBehaviour
{
    public GameObject prefabToSpawn; // Assign your prefab in the inspector
    public Transform[] points; // Assign points A, B, C, D in the inspector
    public float spawnRate = 2f; // Time between spawns
    public float spawnOffset = 0.25f; // Time between spawns

    private float nextSpawnTime;



    private void Update()
    {
        if (Time.time >= nextSpawnTime)
        {
            SpawnPrefab();
            nextSpawnTime = Time.time + Random.Range(spawnRate - spawnOffset, spawnRate + spawnOffset);
        }
    }

    private void SpawnPrefab()
    {
        GameObject instance = Instantiate(prefabToSpawn, points[0].position, prefabToSpawn.transform.rotation);
        BarrelMove mover = instance.GetComponent<BarrelMove>();
        if (mover != null)
        {
            mover.points = points; // Pass the points to the instantiated prefab
            mover.StartMoving();
        }
    }
}
