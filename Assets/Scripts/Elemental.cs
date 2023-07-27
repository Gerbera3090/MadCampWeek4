using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Elemental : MonoBehaviour
{
    public enum ElementalType{Fire, Ice}

    [FormerlySerializedAs("_type")] public ElementalType type;
    public void OnCollisionEnter2D(Collision2D other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        switch (type)
        {  
            case ElementalType.Fire:
                GameManager.instance.playerController.canUseElemental["Fire"] = true; 
                break;
            case ElementalType.Ice:
                GameManager.instance.playerController.canUseElemental["Ice"] = true; 
                break;
            default:
                break;
        }
        Destroy(gameObject);
        
    }
}
