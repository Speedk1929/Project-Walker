using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class SpawnController : MonoBehaviour
{
    [Header("Wave Parameters")]
    public int numberOfWaves = 1;
    public int waveSpawnPoints = 0;
    public double waveAllowedEntitiesMoidifier = 0.5;
    public double waveSpawnInterval = 0.5;
    public int numberOfEnemiesLeft = 0;
   

    [Header("Intermission Parameters")]
    public double allowedEntitiesMoidifier = 0.5;
    public double intermissionSpawnInterval = 1;
    public double intermissionLength = 45;
    public double intermissionCurrentLength = 0;
    

    [Header("Spawn Parameters")]
    public bool isWave = false;
    public double playerSpawnBuffer = 10;
    public List<GameObject> spawnLocations = new List<GameObject>();
    PlayerStats playerStats;
    public List<GameObject> enemyList = new List<GameObject>();


    public static SpawnController current;
    public event Action StartWave;
    public event Action EndWave;
    public event Action WaveIntermission;


    private void Awake()
    {

        current = this;
        GameObject.FindGameObjectWithTag("Player").TryGetComponent(out playerStats);
        spawnLocations = GameObject.FindGameObjectsWithTag("Spawner").ToList();
        IntermissionStart();
    }

    public void Update()
    {
        if (!isWave)
        {
            intermissionCurrentLength -= Time.deltaTime;

        }
        else if (isWave)
        {

            List<Enemy> aliveEnemies = FindObjectsOfType<Enemy>().ToList();
            numberOfEnemiesLeft = aliveEnemies.Count(Enemy => Enemy.apartOfWave == true);

        }
    }

    public void WaveStart()
    {
        numberOfWaves++;
        isWave = true;
        (int, double) spawnParams = FindSpawnParameters(isWave);
        waveSpawnPoints = spawnParams.Item1;
        waveSpawnInterval = spawnParams.Item2;
        StartCoroutine(Wave(waveSpawnPoints, waveSpawnInterval));
        Debug.Log("Wavespawn Interval: " + waveSpawnInterval);

    }

    public void IntermissionStart()
    {

        intermissionCurrentLength = intermissionLength;
        isWave = false;
        (int, double) spawnParams = FindSpawnParameters(isWave);
        intermissionSpawnInterval = spawnParams.Item2;
        StartCoroutine(Wave(0, intermissionSpawnInterval));


    }



    public IEnumerator Wave(int spawnPoints, double spawnInterval)
    {

        int randomUnit = UnityEngine.Random.Range(0, enemyList.Count);

        if (isWave)
        {
            if (0 <= spawnPoints - enemyList[randomUnit].GetComponent<Enemy>().spawnCost)
            {

                GameObject spawnedEnemy = Instantiate(enemyList[randomUnit]);
                spawnPoints -= enemyList[randomUnit].GetComponent<Enemy>().spawnCost;
                spawnedEnemy.GetComponent<Enemy>().apartOfWave = isWave;

                RandomSpawnLocation(spawnedEnemy, spawnLocations);

            }

            if (spawnPoints <= 0 && numberOfEnemiesLeft <= 0)
            {

                IntermissionStart();
                yield break;
            }
            waveSpawnPoints = spawnPoints;
        }
        

        else
        {

            GameObject spawnedEnemy = Instantiate(enemyList[randomUnit]);
            RandomSpawnLocation(spawnedEnemy, spawnLocations);

            if (intermissionCurrentLength <= 0)
            {

                WaveStart();
                yield break;

            }

        }

        yield return new WaitForSeconds(Convert.ToSingle(spawnInterval));

        yield return Wave(spawnPoints, spawnInterval);
    }

    
    public void RandomSpawnLocation(GameObject enemy, List<GameObject> spawnLocations)
    {
        double distanceToPlayer = 0;
        while (distanceToPlayer < playerSpawnBuffer)
        {

            int randomLocation = UnityEngine.Random.Range(0, spawnLocations.Count);
            distanceToPlayer = Vector3.Distance(spawnLocations[randomLocation].transform.position, playerStats.transform.position);

            if (distanceToPlayer >= playerSpawnBuffer)
            {

                enemy.transform.position = spawnLocations[randomLocation].transform.position;


            }

        }

    }


    public (int, double) FindSpawnParameters(bool isWave)
    {

        int spawnPoints = 0;
        double spawnInterval = 0;


        if (isWave)
        {
            spawnPoints = Convert.ToInt32(Math.Pow(Convert.ToDouble(numberOfWaves), 3) + 40);
            spawnInterval = -math.sqrt(Convert.ToDouble(numberOfWaves) / (Convert.ToDouble(numberOfWaves) + 100)) + 1;
            
        }

        else
        {
            spawnPoints = 0;
            spawnInterval = -math.sqrt(numberOfWaves / (numberOfWaves + 100)) + 2;
        }



        (int, double) spawnParameters = (spawnPoints, spawnInterval);


        return spawnParameters;
    }



}
