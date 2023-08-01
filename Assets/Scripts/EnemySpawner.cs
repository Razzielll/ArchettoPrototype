using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    // Event that is triggered when an enemy is spawned
    public event EventHandler<OnEnemySpawnEventArgs> OnEnemySpawn;

    // Custom event arguments class for the enemy spawn event
    public class OnEnemySpawnEventArgs : EventArgs
    {
        public EnemyAI enemyAI;
    }

    // Array of enemy prefabs to be spawned
    [SerializeField] GameObject[] enemyPrefabs;

    // Transform representing the center of the spawn area
    [SerializeField] Transform spawnCenter;

    // Length of the spawn area along the X-axis
    [SerializeField] float spawnLenghtX;

    // Length of the spawn area along the Y-axis
    [SerializeField] float spawnLenghtY;

    // Rate of enemy spawn per second
    [SerializeField] float spawnPerSecond = 1f;

    // Total number of enemies to be spawned
    [SerializeField] float spawnQuantity = 10f;

    // Transform to parent the spawned enemies
    [SerializeField] Transform enemyContainer;

    // Timer to track the time for enemy spawning
    float timer = 0;

    // Start is called before the first frame update
    void Start()
    {
        // Spawn the initial quantity of enemies on start
        for (int i = 0; i < spawnQuantity; i++)
        {
            SpawnEnemy();
        }
    }

    // Method to spawn an enemy
    public void SpawnEnemy()
    {
        // Generate random X and Y values within the spawn area range
        float newRandX = Random.Range(-spawnLenghtX, spawnLenghtX);
        float newRandY = Random.Range(-spawnLenghtY, spawnLenghtY);

        // Calculate the final spawn position for the enemy
        float newX = newRandX + spawnCenter.position.x;
        float newZ = newRandY + spawnCenter.position.z;
        float newY = transform.position.y;
        Vector3 spawnPosition = new Vector3(newX, newY, newZ);

        // Randomly select an enemy prefab to spawn
        int randomNumber = Random.Range(0, enemyPrefabs.Length);
        GameObject enemyGO = Instantiate(enemyPrefabs[randomNumber], spawnPosition, Quaternion.identity, enemyContainer);

        // Randomly rotate the enemy around the Y-axis for variety
        float randRotationAngle = Random.Range(0, 360);
        enemyGO.transform.localRotation = Quaternion.Euler(0f, randRotationAngle, 0f);

        // Invoke the OnEnemySpawn event with the spawned enemyAI component as the argument
        OnEnemySpawn?.Invoke(this, new OnEnemySpawnEventArgs
        {
            enemyAI = enemyGO.GetComponent<EnemyAI>()
        });
    }

    // Method to visualize the spawn area in the Scene view
    private void OnDrawGizmos()
    {
        if (spawnCenter == null)
        {
            return;
        }

        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(spawnCenter.position, new Vector3(2 * spawnLenghtX, 1f, 2 * spawnLenghtY));
    }
}
