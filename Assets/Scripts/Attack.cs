using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public int attackDamage = 10;
    public bool isPlayer = true;
    public Vector2 knockBack = new Vector2(2f,5f);
    public float knockTime = 1f;
    
    public string attackType = "Physical";
    public PlayerController pc;

    public string AttackType
    {
        get { return isPlayer ? (attackType == "Physical" ? pc.AttackType : attackType) : attackType; }
    }
    
    private void Awake()
    {
        if (isPlayer) pc = GetComponentInParent<PlayerController>();
    }

    private void FixedUpdate()
    {
        if (!isPlayer) return;
            
        knockBack = new Vector2(knockBack.x * GetComponentInParent<PlayerController>().faceDirectionVector.x, knockBack.y);
        attackType = pc.AttackType;
    }
}
