using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// collider 안에 있는 object들을 모두 기록하는 class
public class AttackZone : MonoBehaviour
{
    public List<Collider2D> detectedColliders = new List<Collider2D>();
    Collider2D collider;
    public string detectTag;

    private void Awake() {
        collider = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag(detectTag)) return;
        detectedColliders.Add(collision);
        if(collision.CompareTag("Player"))Debug.Log(name + "Player detected");
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (!collision.CompareTag(detectTag)) return;
        detectedColliders.Remove(collision);
    }
    
    
}
