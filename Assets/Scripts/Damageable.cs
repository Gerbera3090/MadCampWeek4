using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// 데미지를 받을 수 있도록 하는는 class
public class Damageable : MonoBehaviour
{
    public UnityEvent<int, Vector2> damageableHit;

    Animator animator;

    [SerializeField]
    private int _maxHealth = 100;

    public int MaxHealth {
        get { return _maxHealth;}
        private set { _maxHealth = value; }
    }

    [SerializeField]
    private int _health = 100;

    public int Health {
        get { return _health;}
        private set { 
            _health = value;
            if(_health <= 0) { // 죽은거로 처리
                IsAlive = false;
            }
        }
    }

    [SerializeField]
    private bool _isAlive = true;

    public bool IsAlive {
        get { return _isAlive;}
        private set { 
            _isAlive = value; 
            animator.SetBool(AnimationStrings.isAlive, _isAlive);
        }
    }

    [SerializeField]
    private bool isInvincible;

    private float timeSinceHit = 0;
    public float invincibilityTime = 0.25f; // 한번 맞은 후, 특정 시간동안은 다시 맞을 수 없음


    // component 가져오기
    private void Awake() {
        animator = GetComponent<Animator>();
    }

    // update every frame
    private void Update() {
        if(isInvincible) {
            if(timeSinceHit > invincibilityTime) {
                isInvincible = false;
                timeSinceHit = 0;
            }
            timeSinceHit += Time.deltaTime;
        }
    }
    

    public bool Hit(int damage, Vector2 knockback) {
        if(IsAlive && !isInvincible) {
            Health -= damage;
            animator.SetTrigger(AnimationStrings.hitTrigger);
            Debug.Log("recieved damage of"+ damage);
            damageableHit?.Invoke(damage, knockback); // damageable hit라는 unityevent를 발동
            return true;
        } else {
            return false;
        }   
    }
}
