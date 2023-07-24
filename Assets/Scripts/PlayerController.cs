using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float walkspeed = 5f;
    public float jumpImpulse = 10f;

    Vector2 moveInput;
    TouchingDirections  touchingDirections;

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
        get {
            return _isMoving;
        }
        private set {
            _isMoving = value; // private ismoving 값대로 animator의 변수 설정
            animator.SetBool(AnimationStrings.isMoving, value);
        } 
    }

    private bool _isFacingRight = true;

    public bool IsFacingRight {
        get {
            return _isFacingRight;
        }
        private set {
            if (_isFacingRight != value) {
                transform.localScale *= new Vector2(-1, 1);
            }
            _isFacingRight = value;
        } 
    }

    public bool CanMove {
        get {
            return animator.GetBool(AnimationStrings.canMove);
        }
    }

    public bool IsAlive {
        get {
            return animator.GetBool(AnimationStrings.isAlive);
        }
    }

    // 컴포낸트 가져오기
    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        touchingDirections = GetComponent<TouchingDirections>();
    }

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        
    }

    private void FixedUpdate() {
        rb.velocity = new Vector2(moveInput.x * CurrentSpeed, rb.velocity.y); // 항상 moveInput 대로 캐릭터가 이동
    }


    public void OnMove(InputAction.CallbackContext context) {
        moveInput = context.ReadValue<Vector2>(); // 방향키의 '값'을 읽어서 moveInput에 저장

        if(IsAlive) {
            IsMoving = moveInput != Vector2.zero;

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
            Debug.Log("attack input");
            animator.SetTrigger(AnimationStrings.attack);
        }
    }

    public void OnHit(int damage, Vector2 knockback) {
        
    }
}
