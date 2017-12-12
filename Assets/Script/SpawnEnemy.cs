using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour {

    public Transform spawnPoint;
    public int spawnLimit;
    public GameObject enemy;
    private int spawnCounter;
    private float spawntime;
    //Instantiate(ninjaStar, firepoint.position, firepoint.rotation);

    // Use this for initialization
    void Start()
    {
        resetSpawnDelay();
        spawnCounter = 0;
    }

    // Update is called once per frame
    void Update()
    {
        spawntime -= Time.deltaTime;
        if (spawntime < 0 && spawnCounter < spawnLimit)
        {
            Instantiate(enemy, spawnPoint.position, spawnPoint.rotation);
            spawnCounter++;
            resetSpawnDelay();
        }
    }

    private void resetSpawnDelay()
    {
        spawntime = 5f;
    }
}
