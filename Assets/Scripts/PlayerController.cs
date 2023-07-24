using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, IController
{
    public float walkspeed = 5f;
    public float jumpImpulse = 10f;

    Vector2 moveInput;
    TouchingDirections  touchingDirections;
    Damageable damageable;

    Rigidbody2D rb;
    Animator animator;

    public float CurrentSpeed {
        get {
            if (CanMove) {
                if(IsMoving && !touchingDirections.IsOnWall) {
                    return walkspeed;
                } else  {
                    return 0;
                }
            } else {
                return 0;
            }      
        }
    }

    private bool _isMoving = false;

    public bool IsMoving { 
        get => _isMoving;
        set {
            _isMoving = value; // private ismoving 값대로 animator의 변수 설정
            animator.SetBool(AnimationStrings.isMoving, value);
        } 
    }

    private bool _isFacingRight = true;

    public bool IsFacingRight {
        get => _isFacingRight;
        set {
            if (_isFacingRight != value) {
                transform.localScale *= new Vector2(-1, 1);
            }
            _isFacingRight = value;
        } 
    }


    public bool CanMove {
        get => animator.GetBool(AnimationStrings.canMove);
        set => animator.SetBool(AnimationStrings.canMove, value);
    }

    public bool IsAlive {
        get => animator.GetBool(AnimationStrings.isAlive);
        set => animator.SetBool(AnimationStrings.isAlive, value);
    }

    // 컴포낸트 가져오기
    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        touchingDirections = GetComponent<TouchingDirections>();
        damageable = GetComponent<Damageable>();
        CanMove = true;
    }
    
    private void FixedUpdate() {
        if(CanMove) rb.velocity = new Vector2(moveInput.x * CurrentSpeed, rb.velocity.y);
    }


    public void OnMove(InputAction.CallbackContext context) {
        moveInput = context.ReadValue<Vector2>(); // 방향키의 '값'을 읽어서 moveInput에 저장
        if(IsAlive) {
            IsMoving = moveInput != Vector2.zero;
            //Debug.Log("INPUT : "+moveInput);
            SetFacingDirection(moveInput);  
        } else {
            IsMoving = false;
        }
    } 

    private void SetFacingDirection(Vector2 moveInput) {
        if(moveInput.x > 0 && !IsFacingRight) {
            IsFacingRight = true;
        } else if (moveInput.x < 0 && IsFacingRight) {
            IsFacingRight = false;      
        }
    }

    public void OnJump(InputAction.CallbackContext context) {
        if(context.started && touchingDirections.IsGrounded) {
            animator.SetTrigger(AnimationStrings.jump);
            rb.velocity = new Vector2(rb.velocity.x, jumpImpulse);
        }
    }

    public void OnAttack(InputAction.CallbackContext context) {
        if(context.started) {
            //Debug.Log("attack input");
            animator.SetTrigger(AnimationStrings.attack);
        }
    }

    public void Dead()
    {
        //게임 오버 화면 넘어가기
        gameObject.SetActive(false);
    }

    public void CallKnockBack(Vector2 knockBackForceVector, float knockTime)
    {
        rb.AddForce(knockBackForceVector);
        StartCoroutine(KnockTimeRoutine(knockTime));
    }

    public IEnumerator KnockTimeRoutine(float knockTime)
    {
        CanMove = false;
        yield return new WaitForSeconds(knockTime);
        CanMove = true;
    }


}
