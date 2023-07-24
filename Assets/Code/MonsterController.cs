using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MonsterController : MonoBehaviour
{
    public float hp;
    public float maxHp;
    public int type;
    public bool isLive;
    private bool isFacingPositive;
    public float speed;
    public float trackSpeed;

    private WaitForSeconds basicMovingTime = new WaitForSeconds(2f);
    private Rigidbody2D rigid;
    private Collider2D collider2D;
    private Scanner scanner;
    SpriteRenderer spriter;
    
    private bool isBasicMoving;

    private bool printLog = true;
    public void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        collider2D = GetComponent<Collider2D>();
        scanner = GetComponent<Scanner>();
        spriter=GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        isLive = true;
        isBasicMoving = false;
        hp = maxHp;
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("Attack")) return;

        var attack = other.gameObject.GetComponent<PlayerAttack>();
        float damage = attack.GetDamage();
        
        // attackType에 따라서 데미지 계산
        
        // 체력 감소
        HpChange(-damage);
        // 피격 모션 및 소리, 넉백 코루틴으로 출력
        
        // 
        
        Debug.Log(hp);
    }

    public void FixedUpdate()
    {   
        // 바라 보는 방향에 있는 player를 향해 움직임
        // 없으면 그냥 움직임
        GameObject target = scanner.GetTarget(isFacingPositive);
        if ( target == null)
        {
            //어케 움직이징
            //코루틴 줘야겠다.
            if (!isBasicMoving)
            {
                isFacingPositive = !isFacingPositive;
                StartCoroutine(BasicMovingRoutine());
            }
            else
            {
                Vector2 tmpVector   = isFacingPositive ? Vector2.right : Vector2.left ;
                tmpVector *= speed;
                tmpVector.y = rigid.velocity.y;
                rigid.velocity = tmpVector;
                //if(printLog)Debug.Log(rigid.velocity);
            }
        }
        else
        {
            rigid.velocity *= trackSpeed;
        }
        spriter.flipX = rigid.velocity.x > 0;
    }

    IEnumerator BasicMovingRoutine()
    {
        if(printLog)Debug.Log("Move Start");
        isBasicMoving = true;
        //moving animation
        
        // velocity add

        yield return basicMovingTime;
        
        if(printLog)Debug.Log("Move End");
        rigid.velocity = Vector2.zero;
        isBasicMoving = false;
    }
    
    private void HpChange(float delta)
    {
        hp = Mathf.Max(0, Mathf.Min(maxHp, hp + delta));
        CheckLive();
    }

    private void CheckLive()
    {
        if (hp <= 0.0000001f)
        {
            isLive = false;
            Dead();
        }
    }

    void Dead()
    {
        isLive = false;
        gameObject.SetActive(false);
    }
    
}



