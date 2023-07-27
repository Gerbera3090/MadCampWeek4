using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject player;
    public Damageable playerDamageable;
    public PlayerController playerController;
    public int monsterLevel = 0;
    public int playerKills = 0;
    public float playerPoints = 0;
    public float playTime = 0;
    void Awake()
    {
        // Check if an instance of GameManager already exists
        if (instance == null)
        {
            instance = this; // Set this as the GameManager instance
            DontDestroyOnLoad(gameObject); // Make the GameManager object persist across scene changes
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate GameManager objects in other scenes
        }

        playerDamageable = player.GetComponent<Damageable>();
        playerController = player.GetComponent<PlayerController>();
        
    }

    public int PlayerLevel
    {
        get {return playerDamageable.Level;}
        set { playerDamageable.Level = value; }
    }

    private float BASIC_EXP = 100f;
    public float NeededExp { 
        get { return BASIC_EXP * Mathf.Pow(1.3f, PlayerLevel) ; } // 지수 함수로 증가
    }

    private float _playerExp = 0f;
    public float PlayerExp
    {
        get { return _playerExp; }
        set
        {   
            //Debug.Log(value);
            if (value >= NeededExp)
            {
                _playerExp = 0;
                PlayerLevel += 1;
            }
            else
            {
                _playerExp = value;
            }
        }
    }

    private void FixedUpdate()
    {
        playTime += Time.deltaTime;
    }
}
