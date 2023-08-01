using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    public event EventHandler<OnEnemySpawnEventArgs> OnEnemySpawn;

    public class OnEnemySpawnEventArgs: EventArgs
    {
        public EnemyAI enemyAI;
    }

    [SerializeField] GameObject[] enemyPrefabs;
    [SerializeField] Transform spawnCenter;
    [SerializeField] float spawnLenghtX;
    [SerializeField] float spawnLenghtY;
    [SerializeField] float spawnPerSecond = 1f;
    [SerializeField] float spawnQuantity = 10f;
    [SerializeField] Transform enemyContainer;


    float timer = 0;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < spawnQuantity; i++)
        {
            SpawnEnemy();
        }
    }

    

    public void SpawnEnemy()
    {
        float newRandX = Random.Range(-spawnLenghtX, spawnLenghtX);
        float newRandY = Random.Range(-spawnLenghtY, spawnLenghtY);


        float newX = newRandX + spawnCenter.position.x;
        float newZ = newRandY + spawnCenter.position.z;
        float newY = transform.position.y;
        Vector3 spawnPosition = new Vector3(newX,newY,newZ);
        int randomNumber = Random.Range(0, 2);
        GameObject enemyGO= Instantiate(enemyPrefabs[randomNumber], spawnPosition,Quaternion.identity, enemyContainer);
        float randRotationAngle = Random.Range(0, 360);
        enemyGO.transform.localRotation = Quaternion.Euler(0f,randRotationAngle,0f);
        OnEnemySpawn?.Invoke(this, new OnEnemySpawnEventArgs
        {
            enemyAI = enemyGO.GetComponent<EnemyAI>()
        }) ;
    }

    private void OnDrawGizmos()
    {
        if(spawnCenter == null)
        {
            return;
        }
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(spawnCenter.position, new Vector3(2*spawnLenghtX, 1f, 2*spawnLenghtY));
    }
}
