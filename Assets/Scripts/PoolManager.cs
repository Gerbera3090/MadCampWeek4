using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    // 1. 프리펩을 보관할 변수
    public GameObject[] prefabs;

    public Dictionary<string, int> spawnData = new Dictionary<string, int>
    {
        { "Goblin", 0 },
        { "HellHound", 1 },
        { "Skeleton" , 2 },
        { "FireSkull" , 3 },
        { "Demon" , 4 }
    };

    // 2. 풀 담당을 하는 리스트들 (1:1)
    List<GameObject>[] pools;

    void Awake() 
    {
        pools = new List<GameObject>[prefabs.Length];

        for (int index = 0; index < pools.Length; index++){
            pools[index] = new List<GameObject>();
        }
    }

    public GameObject Get(int index)
    {
        GameObject select = null;
        
        // .. 선택한 풀의 놀고 있는 (비활성화 된) 게임오브젝트 접근
        foreach (GameObject item in pools[index]){
            if (!item.activeSelf){
                // .. 발견하면 select 변수에 할당
                select = item;
                select.SetActive(true);
                break;
            }
        }
        
        // .. 못 찾았으면?
        if (!select){
            // .. 새롭게 생성하고 select 변수에 할당
            select = Instantiate(prefabs[index],transform);
            // pool 등록
            pools[index].Add(select);
            
        }

        return select;
    }
}