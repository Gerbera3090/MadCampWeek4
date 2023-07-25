using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// bottom, ceiling, wall에 붙어있는지 감지하는 class
public class TouchingDirections : MonoBehaviour
{
    public ContactFilter2D castFilter;

    public float groundDistance = 0.05f;
    public float wallDistance = 0.05f;
    public float ceilingDistance = 0.2f;
    

    Collider2D touchingCollider;
    Animator animator;

    RaycastHit2D[] groundHits = new RaycastHit2D[5];
    RaycastHit2D[] wallHits = new RaycastHit2D[5];
    RaycastHit2D[] ceilingHits = new RaycastHit2D[5];
    
    [SerializeField] // private이지만 inspector에서 볼 수 있음
    private bool _isGrounded = true;

    public bool IsGrounded {
        get => _isGrounded;
        private set {
            _isGrounded = value;
            animator.SetBool(AnimationStrings.isGrounded, value);
        }
    }

    private Vector2 wallCheckDirection => gameObject.transform.localScale.x > 0 ? Vector2.right : Vector2.left;
    
    [SerializeField] // private이지만 inspector에서 볼 수 있음
    private bool _isOnWall = false;

    public bool IsOnWall {
        get {
            return _isOnWall;
        }
        private set {
            _isOnWall = value;
            animator.SetBool(AnimationStrings.isOnWall, value);
        }
    }
    
    [SerializeField] // private이지만 inspector에서 볼 수 있음
    private bool _isOnCeiling = false;

    public bool IsOnCeiling {
        get {
            return _isOnCeiling;
        }
        private set {
            _isOnCeiling = value;
            animator.SetBool(AnimationStrings.isOnCeiling, value);
        }
    }

    private void Awake() {
        touchingCollider = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // vector.down - 아래 방향 충돌만(바닥), castFilter를 이용해 충돌 감지, groundHits[] 에 충돌 정보 저장
        IsGrounded = touchingCollider.Cast(Vector2.down, castFilter, groundHits, groundDistance) > 0; 
        IsOnWall = touchingCollider.Cast(wallCheckDirection, castFilter, wallHits, wallDistance) > 0; 
        IsOnCeiling = touchingCollider.Cast(Vector2.up, castFilter, ceilingHits, ceilingDistance) > 0; 
    }
}
