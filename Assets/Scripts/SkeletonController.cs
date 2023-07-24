using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonController : MonoBehaviour
{
    public float walkspeed = 3f;

    Rigidbody2D rb;
    Animator animator;
    TouchingDirections touchingDirections; 
    public AttackZone attackZone;

    public float walkStopRate = 0.6f;

    public bool canMove {
        get { return animator.GetBool(AnimationStrings.canMove); }
    }

    public float CurrentSpeed {
        get { return walkspeed; }
    }

    public enum WalkableDirection {Right, Left};

    private WalkableDirection _walkDirection = WalkableDirection.Right;
    private Vector2 walkDirectionVector;

    public WalkableDirection walkDirection {
        get { return _walkDirection; }
        private set {
            if (_walkDirection != value) {
                gameObject.transform.localScale *= new Vector2(-1, 1);
            }
            _walkDirection = value;

            if(value == WalkableDirection.Right) {
                walkDirectionVector = Vector2.right;
            } else if (value == WalkableDirection.Left) {
                walkDirectionVector = Vector2.left;
            }
        } 
    }

    private bool _isMoving = false;

    public bool IsMoving { 
        get { return _isMoving; }
        private set {
            _isMoving = value; // private ismoving 값대로 animator의 변수 설정
            animator.SetBool(AnimationStrings.isMoving, value);
        } 
    }

    private bool _hasTarget = false;

    public bool HasTarget {
        get { return _hasTarget;}
        private set {
            _hasTarget = value;
            animator.SetBool(AnimationStrings.hasTarget, value);
        }
    }

    // 컴포낸트 가져오기
    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        touchingDirections = GetComponent<TouchingDirections>();
    }

    // Start is called before the first frame update
    void Start()
    {
        IsMoving = true;
    }

    void Update() {
        HasTarget = attackZone.detectedColliders.Count > 0;  // 타겟이 있는지 없는지 계속 업데이트
    }

    // Update is called once per frame in physics
    void FixedUpdate()
    {
        if(touchingDirections.IsGrounded && touchingDirections.IsOnWall) {
            FlipDirections();
        }

        if(canMove) {
            rb.velocity = new Vector2(CurrentSpeed * walkDirectionVector.x, rb.velocity.y);
        } else {
            rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, 0, walkStopRate), rb.velocity.y); // rb.velocity.x에서 0으로 감속
        }
        
    }

    private void FlipDirections() {
        if(walkDirection == WalkableDirection.Left) {
            walkDirection = WalkableDirection.Right;
        } else if (walkDirection == WalkableDirection.Right) {
            walkDirection = WalkableDirection.Left;
        } 
    }
}
