using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    private int[] MONSTER_SPAWN_NUM = { 15, 10, 3, 2, 1 };
    public GameObject SpawnPointHead;
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
        spawnPoint = SpawnPointHead.GetComponentsInChildren<Transform>();
        pool = GetComponent<PoolManager>();
    }

    void Update(){
    }
    public void Spawn(){ // 아무 것도 없을 때 : 무한 모드에서 사용함
        //for(int j = 0 ; j < 1 + GameManager.instance.monsterLevel / spawnData.Count ; j++){
            for (int i = 0; i < MONSTER_SPAWN_NUM[GameManager.instance.monsterLevel % spawnData.Count]; i++)
            {
                GameObject enemy = pool.Get(GameManager.instance.monsterLevel % spawnData.Count);
                enemy.transform.position = spawnPoint[i % spawnPoint.Length].position;
                enemy.GetComponent<Damageable>().Level = GameManager.instance.monsterLevel;
            }
        //}
    }

    public void Spawn(int monsterId, int monsterLevel)
    {
        Debug.Log("Spawn Level : "+ GameManager.instance.monsterLevel);
        GameObject enemy = pool.Get(monsterId);
        enemy.transform.position = spawnPoint[Random.Range(0, spawnPoint.Length)].position;
        enemy.GetComponent<Damageable>().Level = monsterLevel;
    }

    public void Spawn(int monsterId)
    {
        Spawn(monsterId, level);
    }

    public void Spawn(string monsterName, int monsterLevel)
    {
        Spawn(spawnData[monsterName], monsterLevel);
    }

    public void Spawn(string monsterName)
    {
        Spawn(spawnData[monsterName], level);
    }

    public void Spawn(int monsterId, int monsterLevel, int spawnPositionNum)
    {
        GameObject enemy = pool.Get(monsterId);
        enemy.transform.position = spawnPoint[spawnPositionNum % spawnPoint.Length].position;
        enemy.GetComponent<Damageable>().Level = monsterLevel;
    }
    public void Spawn(string monsterName, int monsterLevel, int spawnPositionNum)
    {
        Spawn(spawnData[monsterName], level, spawnPositionNum);   
    }
}

