using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using Cinemachine;
using UnityEngine;
using UnityEngine.Events;

// 데미지를 받을 수 있도록 하는는 class
public class Damageable : MonoBehaviour
{
    public float shakeDuration = 0.2f;
    public float shakeIntensity = 0.2f;
    //public UnityEvent<int, Vector2> damageableHit;
    public bool isPlayer;
    Animator animator;
    private IController controller;
    [SerializeField] 
    public float BASIC_HEALTH = 100f; 
    private float _maxHealth = 100f;
    
    private SpriteRenderer sprite;
    public float BURNDAMAGE = 10f;
    private int burnCount = 0;
    private WaitForSeconds burnTimeWait = new WaitForSeconds(5f);
    private float burnTimer = 0;
    public int iceCount = 0;

    private int _level = 0;
    public int Level
    {
        get { return _level; }
        set
        {
            MaxHealth = BASIC_HEALTH * (1 + 0.5f * value);
            _level = value;
            Health = MaxHealth;
        }
    }
    
    public float MaxHealth {
        get { return _maxHealth;}
        private set
        {
            float diff = Mathf.Max(_maxHealth - Health, 0);
            _maxHealth = value;
            Health = _maxHealth - diff;
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
        set { 
            _isAlive = value; 
            if(animator!=null)animator.SetBool(AnimationStrings.isAlive, _isAlive);
        }
    }

    [SerializeField]
    private bool isInvincible;

    private float timeSinceHit = 0;
    public float invincibilityTime = 1f; // 한번 맞은 후, 특정 시간동안은 다시 맞을 수 없음


    // component 가져오기
    private void Awake() {
        animator = GetComponent<Animator>();
        controller = GetComponent<IController>();
        sprite = GetComponent<SpriteRenderer>();
        IsAlive = true;
    }

    private void OnEnable()
    {
        IsAlive = true;
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

        if (!IsAlive) return;
        
        if (burnCount > 0)
        {
            sprite.color = new Color(1f, 0.4f, 0.4f);
            if (burnTimer > 1f)
            {
                Health -= BURNDAMAGE * (1 + 0.5f*Level);
                burnTimer = 0;
                animator.SetTrigger(AnimationStrings.hitTrigger);
            }
            else
            {
                burnTimer += Time.deltaTime;
            }
            //불데미지 소리
        }

        if (iceCount > 0)
        {
            sprite.color = new Color(0.4f, 0.4f, 1f);
        }
        
        if (burnCount + iceCount == 0)
        {
            sprite.color = Color.white;
        }
    }
    
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("Attack")) return;
        if (!IsAlive) return;
        string tp = isPlayer ? "Player" : "Monster";
        if (isPlayer && isInvincible) return;
        var attack = other.gameObject.GetComponent<Attack>();
        //Debug.Log(isPlayer == attack.isPlayer);
        if (isPlayer == attack.isPlayer) return;
        float damage = attack.AttackDamage;
        string attackType = attack.AttackType;
        // attackType에 따라서 데미지 계산
        switch (attackType)
        {
            case "Fire":
                StartCoroutine(BurnRoutine());
                break;
            case "Ice":
                StartCoroutine(IceRoutine());
                break;
            default:
                break;
        }
        
        
        // 체력 감소
        Health -= damage;
        // 피격 모션 및 소리, 넉백 코루틴으로 출력
        animator.SetTrigger(AnimationStrings.hitTrigger);
        controller.CallKnockBack(attack.knockBack, attack.knockTime);
        // 

        if(isPlayer){
            isInvincible = true;  //무적 시간 적용    
            StartCoroutine(ShakeEffect());
        }
        
        // Debug.Log(tp + " received damage of : "+ damage);
        // Debug.Log(tp + " Remained HP : " + Health);
    }
    
    private IEnumerator ShakeEffect()
    {
        //Debug.Log("SHAKE EFFECT!");
        Vector3 originalPosition = Camera.main.transform.position;
        float elapsed = 0f;

        while (elapsed < shakeDuration)
        {
            float x = originalPosition.x + UnityEngine.Random.Range(-shakeIntensity, shakeIntensity);
            float y = originalPosition.y + UnityEngine.Random.Range(-shakeIntensity, shakeIntensity);
            Camera.main.transform.position = new Vector3(x, y, originalPosition.z);
            elapsed += Time.deltaTime;
            yield return null;
        }

        Camera.main.transform.position = originalPosition;
    }

    private IEnumerator BurnRoutine()
    {
        burnCount++;
        yield return burnTimeWait;
        burnCount--;
    }
    private IEnumerator IceRoutine()
    {
        iceCount++;
        yield return burnTimeWait;
        iceCount--;
    }
}
