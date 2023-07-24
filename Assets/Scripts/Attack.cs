using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    Collider2D attackCollider;

    public int attackDamage = 10;

    public Vector2 knockback = new Vector2(0,0);

    private void Awake() {
        attackCollider = GetComponent<Collider2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collision) {
        Damageable damageable = collision.GetComponent<Damageable>(); // 부딧힌 collision 중 damageable component 가져옴

        if(damageable != null) {
            damageable.Hit(attackDamage, knockback);
            Debug.Log(collision.name + "hit for" + attackDamage);
        }
    }
}
