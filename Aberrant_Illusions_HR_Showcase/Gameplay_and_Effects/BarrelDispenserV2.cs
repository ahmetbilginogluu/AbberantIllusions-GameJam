using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelDispenserV2 : MonoBehaviour
{
    public GameObject prefabToSpawn; // Assign your prefab in the inspector
    public Transform[] points; // Assign points A, B, C, D in the inspector
    public float spawnRate = 2f; // Time between spawns
    public float spawnOffset = 0.25f; // Time between spawns

    private float nextSpawnTime;

    [SerializeField] private GameObject prefabToSpawn1; // Assign this in the Inspector
    [SerializeField] private GameObject prefabToSpawn2; // Assign this in the Inspector

    private GameObject lastPrefabSpawned;
    private int samePrefabCount = 0;



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
            GameObject prefabToSpawn;

            // Check if we should force switching to the other prefab
            if (samePrefabCount >= 3)
            {
                // Switch to the other prefab
                prefabToSpawn = lastPrefabSpawned == prefabToSpawn1 ? prefabToSpawn2 : prefabToSpawn1;
                samePrefabCount = 1; // Reset count as we are spawning the other prefab now
            }
            else
            {
                // Choose randomly if not forced to switch
                prefabToSpawn = Random.Range(0, 2) == 0 ? prefabToSpawn1 : prefabToSpawn2;

                // Check if we are spawning the same prefab as last time
                if (prefabToSpawn == lastPrefabSpawned)
                {
                    samePrefabCount++; // Increment counter if the same prefab is being spawned
                }
                else
                {
                    samePrefabCount = 1; // Reset count if a different prefab is being spawned
                }
            }

            lastPrefabSpawned = prefabToSpawn; // Update the last spawned prefab

            // Instantiate the chosen prefab at the first point's position with the chosen prefab's rotation
            GameObject instance = Instantiate(prefabToSpawn, points[0].position, prefabToSpawn.transform.rotation);
            BarrelMove mover = instance.GetComponent<BarrelMove>();
            if (mover != null)
            {
                mover.points = points; // Pass the points to the instantiated prefab
                mover.StartMoving();
            }
        }
}
