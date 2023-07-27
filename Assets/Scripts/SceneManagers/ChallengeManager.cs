using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class ChallengeManager : MonoBehaviour
{
    public GameObject spawnerObj;
    private Spawner spawner;
    private float spawnTimer = 0f; 
    void Awake()
    {
        GameManager.instance.playTime = 0;
        spawner = spawnerObj.GetComponent<Spawner>();
    }

    private void FixedUpdate()
    {
        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0)
        {
            spawnTimer = 10f;
            spawner.Spawn();
            GameManager.instance.monsterLevel += 1;
        }
    }
}
