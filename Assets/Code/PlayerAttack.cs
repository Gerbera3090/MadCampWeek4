using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private string _attackType = "physical";
    private int _damage = 10;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public string GetAttackType()
    {
        return _attackType;
    }

    public int GetDamage()
    {
        return _damage;
    }
}
