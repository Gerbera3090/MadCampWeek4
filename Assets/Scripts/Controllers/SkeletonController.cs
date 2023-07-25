using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonController : MonoBehaviour, IController
{
    public float walkspeed = 3f;
    public GameObject spawnPoint;
    Rigidbody2D rb;
    Animator animator;
    TouchingDirections touchingDirections; 
    AttackZone attackZone;
    private SpriteRenderer spriteRenderer;
    Damageable damageable;
    private AttackZone cliffDetect;
    private AttackZone frontPlayerDetect;
    private AttackZone backPlayerDetect;
    private bool isAttacking = false;
    
    public float walkStopRate = 0.6f;

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

    private bool _hasTarget = false;

    public bool HasTarget {
        get => _hasTarget;
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
        damageable = GetComponent<Damageable>();
        IsAlive = true;
        attackZone = GetComponentInChildren<AttackZone>();
        damageable = GetComponent<Damageable>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        cliffDetect = GetComponentsInChildren<AttackZone>()[1];
        frontPlayerDetect = GetComponentsInChildren<AttackZone>()[2];
        backPlayerDetect = GetComponentsInChildren<AttackZone>()[3];
        Debug.Log(frontPlayerDetect.detectTag);
        Debug.Log(backPlayerDetect.detectTag);

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
        if(touchingDirections.IsGrounded && (touchingDirections.IsOnWall || cliffDetect.detectedColliders.Count <= 0 ||
                                             (backPlayerDetect.detectedColliders.Count > 0 && CanMove))  ) {
            //Debug.Log("Flip by wall");
            FlipDirections();
        }
        
        if(CanMove) {
            rb.velocity = new Vector2(CurrentSpeed * walkDirectionVector.x, rb.velocity.y);
        } else {
            //rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, 0, walkStopRate), rb.velocity.y); // rb.velocity.x에서 0으로 감속
        }
        IsMoving = rb.velocity.magnitude > 0;
    }

    public void FlipDirections() {
        if(walkDirection == WalkableDirection.Left) {
            walkDirection = WalkableDirection.Right;
        } else if (walkDirection == WalkableDirection.Right) {
            walkDirection = WalkableDirection.Left;
        } 
    }

    public void Dead()
    {
        //gameObject.SetActive(false);
        IsAlive = false;
        spriteRenderer.enabled = false;
        rb.simulated = false;
        StartCoroutine(RespawnRoutine());
    }
    
    public void CallKnockBack(Vector2 knockBackForceVector, float knockTime)
    {
        Vector2 knockback = new Vector2(knockBackForceVector.x, 0 );
        knockback.y = touchingDirections.IsGrounded ? knockBackForceVector.y : rb.velocity.y;
        rb.velocity = knockback;
        StartCoroutine(KnockTimeRoutine(knockTime));
    }

    public IEnumerator KnockTimeRoutine(float knockTime)
    {
        CanMove = false;
        yield return new WaitForSeconds(knockTime);
        CanMove = true;
    }

    IEnumerator RespawnRoutine(){
        yield return new WaitForSeconds(5f);
        transform.position = spawnPoint.transform.position;
        IsAlive = true;
        spriteRenderer.enabled = true;
        rb.simulated = true;
        CanMove = true;
        damageable.Health = damageable.MaxHealth;
        animator.SetBool(AnimationStrings.isMoving, true);
    }
    
}
