using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Transform[] spawnPoint;
    public Dictionary<string, int> spawnData = new Dictionary<string, int>
    {
        { "Goblin", 0 },
        { "HellHound", 1 },
        { "Skeleton" , 2 },
        { "FireSkull" , 3 },
        { "Demon" , 4 }
    };

    private PoolManager pool;
    int level;
    float timer;

    void Awake(){
        spawnPoint = GetComponentsInChildren<Transform>();
        pool = GetComponent<PoolManager>();
    }

    void Update(){
    }
    void Spawn(){ // 아무 것도 없을 때 : 무한 모드에서 사용함
        GameObject enemy = pool.Get(GameManager.instance.monsterLevel % spawnData.Count);
        enemy.transform.position = spawnPoint[Random.Range(0, spawnPoint.Length)].position;
        enemy.GetComponent<Damageable>().Level = GameManager.instance.monsterLevel;
    }

    void Spawn(int monsterId, int monsterLevel)
    {
        GameObject enemy = pool.Get(monsterId);
        enemy.transform.position = spawnPoint[Random.Range(0, spawnPoint.Length)].position;
        enemy.GetComponent<Damageable>().Level = monsterLevel;
    }

    void Spawn(int monsterId)
    {
        Spawn(monsterId, level);
    }

    void Spawn(string monsterName, int monsterLevel)
    {
        Spawn(spawnData[monsterName], monsterLevel);
    }

    void Spawn(string monsterName)
    {
        Spawn(spawnData[monsterName], level);
    }

    void Spawn(int monsterId, int monsterLevel, int spawnPositionNum)
    {
        GameObject enemy = pool.Get(monsterId);
        enemy.transform.position = spawnPoint[spawnPositionNum % spawnPoint.Length].position;
        enemy.GetComponent<Damageable>().Level = monsterLevel;
    }
    void Spawn(string monsterName, int monsterLevel, int spawnPositionNum)
    {
        Spawn(spawnData[monsterName], level, spawnPositionNum);   
    }
}

