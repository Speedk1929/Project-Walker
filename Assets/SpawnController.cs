using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;

public class SpawnController : MonoBehaviour
{
    [Header("Wave Parameters")]
    public double lengthOfWaves = 180;
    public double waveSpawnPoints = 0;
    public double waveAllowedEntitiesMoidifier = 0.5;
    public double waveSpawnInterval = 0.5;

    [Header("Intermission Parameters")]
    public double intermissionSpawnPoints = 0;
    public double allowedEntitiesMoidifier = 0.5;
    public double intermissionSpawnInterval = 1;

    [Header("Spawn Parameters")]
    public bool waveMode;
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

    }
    private void Start()
    {
        
        GameObject.FindGameObjectWithTag("Player").TryGetComponent(out playerStats);
        spawnLocations = GameObject.FindGameObjectsWithTag("Spawner").ToList();
        IntermissionStart();

       
    }

    public void WaveStart()
    {

        waveSpawnPoints = 100;

        StartCoroutine(Wave(waveSpawnPoints, waveSpawnInterval));


    }

    public void IntermissionStart()
    {
        intermissionSpawnPoints = 100;

        StartCoroutine(Wave(intermissionSpawnPoints, intermissionSpawnInterval));


    }



    public IEnumerator Wave(double spawnPoints, double spawnInterval)
    {

        int randomUnit = UnityEngine.Random.Range(0, enemyList.Count);



        if (0 <= spawnPoints - enemyList[randomUnit].GetComponent<Enemy>().spawnCost)
        {

            GameObject spawnedEnemy = Instantiate(enemyList[randomUnit]);
            spawnPoints -= enemyList[randomUnit].GetComponent<Enemy>().spawnCost;


            RandomSpawnLocation(spawnedEnemy, spawnLocations);

        }

        yield return new WaitForSeconds(Convert.ToSingle(spawnInterval));


        if(spawnPoints <= 0)
        {

            yield return null;
            waveMode = !waveMode;

        }



        yield return Wave(spawnPoints, spawnInterval);
    }

    
    public void RandomSpawnLocation(GameObject enemy, List<GameObject> spawnLocations)
    {

       int randomLocation = UnityEngine.Random.Range(0, spawnLocations.Count);


       enemy.transform.position = spawnLocations[randomLocation].transform.position;



    }





















}
