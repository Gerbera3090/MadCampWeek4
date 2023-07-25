using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// 데미지를 받을 수 있도록 하는는 class
public class Damageable : MonoBehaviour
{
    public UnityEvent<int, Vector2> damageableHit;
    public bool isPlayer;
    Animator animator;
    private IController controller;
    [SerializeField]
    private float _maxHealth = 100f;

    public float MaxHealth {
        get { return _maxHealth;}
        private set
        {
            _maxHealth = value;
        }
    }
    
    [SerializeField]
    private float _health = 100f;

    public float Health {
        get { return _health;}
        set
        {
            _health = Mathf.Max(0, Mathf.Min(_maxHealth, value));
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
        controller = GetComponent<IController>();
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
    
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("Attack")) return;
        string tp = isPlayer ? "Player" : "Monster";
        if (isPlayer && isInvincible) return;
        var attack = other.gameObject.GetComponent<Attack>();
        Debug.Log(isPlayer == attack.isPlayer);
        if (isPlayer == attack.isPlayer) return;
        float damage = attack.attackDamage;
        string attackType = attack.attackType;
        // attackType에 따라서 데미지 계산
        
        // 체력 감소
        Health -= damage;
        // 피격 모션 및 소리, 넉백 코루틴으로 출력
        animator.SetTrigger(AnimationStrings.hitTrigger);
        controller.CallKnockBack(attack.knockBack, attack.knockTime);
        // 
        Debug.Log(tp + " received damage of : "+ damage);
        Debug.Log(tp + " Remained HP : " + Health);
    }
    
    
}
