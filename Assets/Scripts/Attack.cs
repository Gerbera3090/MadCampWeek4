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
}
