using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, IController
{
    public float walkspeed = 10f;
    public float jumpImpulse = 20f;
    public float rollImpulse = 10f;
    public bool canDash = true;
    
    private bool _isDashing = false;
    public bool IsDashing {
        get { return _isDashing; }
        private set {
            _isDashing = value;
            animator.SetBool(AnimationStrings.isDashing, value);
        }
    }
    public float dashImpulse = 24f;
    public float dashingTime = 0.2f;
    private float dashingCooldown = 1f;

    Vector2 moveInput;
    TouchingDirections  touchingDirections;
    Damageable damageable;
    public ParticleSystem dust;

    Rigidbody2D rb;
    Animator animator;
    TrailRenderer tr;

    private float BASIC_GRAVITY = 5f;
    
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
    private Vector2 faceDirectionVector = Vector2.right;

    public bool IsFacingRight {
        get => _isFacingRight;
        set {
            if (_isFacingRight != value) {
                transform.localScale *= new Vector2(-1, 1);
            }
            _isFacingRight = value;

            faceDirectionVector = IsFacingRight? Vector2.right: Vector2.left;
        } 
    }


    public bool CanMove {
        get => animator.GetBool(AnimationStrings.canMove);
        set => animator.SetBool(AnimationStrings.canMove, value);
    }

    public bool IsAlive {
        get => animator.GetBool(AnimationStrings.isAlive);
        set
        {
            animator.SetBool(AnimationStrings.isAlive, value); 
            Debug.Log("Player Die");
            Debug.Log("HP : " + damageable.Health);
        }
    }

    private bool _lockVelocity = false;

    public bool LockVelocity {
        get { return _lockVelocity; }
        private set {
            _lockVelocity = value;
            animator.SetBool(AnimationStrings.lockVelocity, value);
        }
    }

    // 컴포낸트 가져오기
    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        touchingDirections = GetComponent<TouchingDirections>();
        damageable = GetComponent<Damageable>();
        CanMove = true;
        tr = GetComponent<TrailRenderer>();
>>>>>>> d2ee7c1c964d521b49175bbd7eac05c1e685ff80:Assets/Scripts/Controllers/PlayerController.cs
    }
    
    private void FixedUpdate() {
<<<<<<< HEAD:Assets/Scripts/PlayerController.cs
        if(CanMove) rb.velocity = new Vector2(moveInput.x * CurrentSpeed, rb.velocity.y);
=======
        // if(!damageable.IsHit) {
        //     rb.velocity = new Vector2(moveInput.x * CurrentSpeed, rb.velocity.y); // 안맞으면 moveInput 대로 캐릭터가 이동
        // }
        if (!touchingDirections.IsGrounded){
            animator.SetBool(AnimationStrings.isRising, rb.velocity.y > 0);
        }
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
        if(context.started && (touchingDirections.IsOnWall || touchingDirections.IsGrounded)) {
            animator.SetTrigger(AnimationStrings.jump);
            rb.velocity = new Vector2(rb.velocity.x, jumpImpulse);
        }
    }

    public void OnAttack(InputAction.CallbackContext context) {
        if(context.started) {
<<<<<<< HEAD:Assets/Scripts/PlayerController.cs
            //Debug.Log("attack input");
=======
>>>>>>> d2ee7c1c964d521b49175bbd7eac05c1e685ff80:Assets/Scripts/Controllers/PlayerController.cs
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


    public void OnRoll(InputAction.CallbackContext context) {
        if(context.started && touchingDirections.IsGrounded && rollingCount <= 0) {
            animator.SetTrigger(AnimationStrings.roll);
            rb.velocity = new Vector2(faceDirectionVector.x * rollImpulse, rb.velocity.y);
            StartCoroutine(Rolling());
        }
    }
    private int rollingCount = 0;
    private WaitForSeconds rollWait = new WaitForSeconds(0.5f);
    private IEnumerator Rolling()
    {
        rollingCount++;
        LockVelocity = true;
        yield return rollWait;
        rollingCount--;
        if (rollingCount <= 0)
        {
            LockVelocity = false;
        }
    }

    public void OnDash(InputAction.CallbackContext context) {
        if(context.started) {
            if(canDash)
                StartCoroutine(Dash());
        }
    }
    
    private IEnumerator Dash()
    {
        Collider2D dashattack = GetComponentsInChildren<Collider2D>()[4];
        canDash = false;
        IsDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        dashattack.enabled = true;
        LockVelocity = true;

        Debug.Log("Dash coroutine started");

        rb.velocity = new Vector2(transform.localScale.x * dashImpulse, 0f);
        tr.emitting = true;
        dust.Play();
        yield return new WaitForSeconds(dashingTime);

        LockVelocity = false;


        dashattack.enabled = false;
        tr.emitting = false;
        rb.gravityScale = originalGravity;
        IsDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
        
    }
>>>>>>> d2ee7c1c964d521b49175bbd7eac05c1e685ff80:Assets/Scripts/Controllers/PlayerController.cs
}
