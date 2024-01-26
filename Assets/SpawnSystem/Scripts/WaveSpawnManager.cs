using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawnManager : MonoBehaviour
{
    //the list of spawn points
    private GameObject[] spawnPoints;
    private int numSpawns;

    public GameObject[] enemies;
    private int numEnemies;

    [Tooltip("How Much the number of enemies increase per wave"), Range(1.0f, 2.0f)]
    public float waveIncrease = 1.5f;
  
    //spawn intterval. How long between wave spawns
    public float spawnInterval = 3f;
    //this could be made public so we can update the UI
    private float timeUntilNextSpawn = 3f;

    int numEnemiesToSpawn = 0;
    private int waveNumber = 0;

    // Start is called before the first frame update
    void Start()
    {
        spawnPoints = GameObject.FindGameObjectsWithTag("EnemySpawnPoint");
        numSpawns = spawnPoints.Length;
        if(numSpawns == 0)
        {
            Debug.LogError("Make sure you have spawnpoints and they are tagged EnemySpawnPoint");
        }

        numEnemies = enemies.Length;
        if(numEnemies == 0)
        {
            Debug.LogError("Add Enemies to the enemy list in the wave spawn manager");
        }
        //the main counter reset
        timeUntilNextSpawn = spawnInterval;
    }

    // Update is called once per frame
    void Update()
    {
        //
        timeUntilNextSpawn -= Time.deltaTime;
        if(timeUntilNextSpawn <= 0f)
        {
            //Spawn enemies
            SpawnEnemies();
            timeUntilNextSpawn = spawnInterval;
        }
    }

    private void SpawnEnemies()
    {
        //increase wave and num enemies
        waveNumber += 1;
        numEnemiesToSpawn = (int)(Mathf.Pow( waveNumber,waveIncrease));

        //now spawn them
        for(int i = 0; i < numEnemiesToSpawn; i++)
        {
            //find a random spawn
            var randSpawn = UnityEngine.Random.Range(0, numSpawns);
            var randEnemy = UnityEngine.Random.Range(0, numEnemies);
            var enemy = Instantiate(enemies[randEnemy], spawnPoints[randSpawn].transform.position, spawnPoints[randSpawn].transform.rotation);
        }
    }
}
