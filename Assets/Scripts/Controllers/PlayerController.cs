using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour, IController
{    
    
    public float walkspeed = 10f;
    public float jumpImpulse = 20f;
    public float rollImpulse = 20f;

    private bool _isDashing = false;
    public bool IsDashing {
        get { return _isDashing; }
        private set {
            _isDashing = value;
            if(value) animator.SetTrigger(AnimationStrings.isDashing);
        }
    }
    public float dashImpulse = 24f;
    public float dashingTime = 0.5f;
    private bool canJump = true;
    
    Vector2 moveInput;
    TouchingDirections  touchingDirections;
    Damageable damageable;
    //public ParticleSystem dust;

    Rigidbody2D rb;
    Animator animator;

    private float BASIC_GRAVITY = 5f;
    string[] attackTypes = { "Physical", "Fire", "Ice" };
    private int presentType = 0;
    
    public Dictionary<string, float> cooldowns = new Dictionary<string, float>
    {
        { "Upper", 0f },
        { "Spin", 0f },
        { "Fire", 0f },
        { "Ice", 0f },
        { "Dash", 0f },
        {"Potion", 0f}
    };

    public Dictionary<string, float> coolTimes = new Dictionary<string, float>
    {
        { "Upper", 2f },
        { "Spin", 5f },
        { "Fire", 20f },
        { "Ice", 20f },
        { "Dash", 2f },
        {"Potion", 5f}
    };

    public Dictionary<string, bool> canUseElemental = new Dictionary<string, bool>
    {
        { "Fire", false },
        { "Ice", false }
    };


    public string AttackType => attackTypes[presentType];

    public float CurrentSpeed {
        get {
            if (CanMove) {
                if(IsMoving) {
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
    public Vector2 faceDirectionVector = Vector2.right;

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

    //private bool _lockVelocity = false;

    public bool LockVelocity {
        //get { return _lockVelocity; }
        get { return animator.GetBool(AnimationStrings.lockVelocity);}
        
        private set {
            //_lockVelocity = value;
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
    }
    
    private void FixedUpdate() {


        if (!LockVelocity)
        {
            if (touchingDirections.IsOnWall && !touchingDirections.IsGrounded)
            {
                rb.velocity = new Vector2(moveInput.x * CurrentSpeed, 0);
                //Debug.Log(rb.velocity);
                rb.gravityScale = 0f;
            }
            else
            {
                if (CanMove)
                {
                    rb.velocity = new Vector2(moveInput.x * CurrentSpeed, rb.velocity.y);
                    SetFacingDirection(moveInput);
                }

                rb.gravityScale = BASIC_GRAVITY;
            }
        }
        else
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.gravityScale = 0f;
        }
        animator.SetBool(AnimationStrings.isHanging, touchingDirections.IsOnWall && !touchingDirections.IsGrounded);
        
        // if(!damageable.IsHit) {
        //     rb.velocity = new Vector2(moveInput.x * CurrentSpeed, rb.velocity.y); // 안맞으면 moveInput 대로 캐릭터가 이동
        // }
        if (!touchingDirections.IsGrounded){
            animator.SetBool(AnimationStrings.isRising, rb.velocity.y > 0);
        }
        // Iterate through the dictionary and decrement each value by 1
        foreach (var key in cooldowns.Keys.ToList()) // Using ToList to avoid modifying the dictionary during iteration
        {
            if (cooldowns[key] > 0)
            {
                //Debug.Log(key + " : "+cooldowns[key]);
                //Debug.Log(key + " Bool Really : " + CheckCooldown(key));
            }
            cooldowns[key] -= Time.deltaTime;
            
        }

        if (touchingDirections.IsOnWall || touchingDirections.IsGrounded) canJump = true;
    }

    public void OnPotion(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (CheckCooldown("Potion"))
            {
                damageable.Health += 50;
                
            }
        }
    }
    
    public void OnFire(InputAction.CallbackContext context)
    {
        if(context.started) {
            if(CheckCooldown("Fire")){
                animator.SetTrigger(AnimationStrings.fireAttack);
                StartCoroutine(ElementalRoutine(1));
            }
        }
    }
    
    public void OnIce(InputAction.CallbackContext context)
    {
        //if(fireCooltime>0)
        if(context.started && CheckCooldown("Ice")) {
            animator.SetTrigger(AnimationStrings.iceAttack);
            StartCoroutine(ElementalRoutine(2));
        }
    }

    private WaitForSeconds elementalTimeWait = new WaitForSeconds(10);
    private IEnumerator ElementalRoutine(int etype)
    {
        presentType = etype;
        yield return elementalTimeWait;
        presentType = 0;
    }

    public void OnMove(InputAction.CallbackContext context) {
        moveInput = context.ReadValue<Vector2>(); // 방향키의 '값'을 읽어서 moveInput에 저장
        if(IsAlive) {
            IsMoving = moveInput != Vector2.zero;
            //Debug.Log("INPUT : "+moveInput);
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
        if(context.started && canJump)
        {
            canJump = false;
            animator.SetTrigger(AnimationStrings.jump);
            rb.velocity = new Vector2(rb.velocity.x, jumpImpulse);
        }
    }

    public void OnAttack(InputAction.CallbackContext context) {
        if(context.started) {
            animator.SetTrigger(AnimationStrings.attack);
        }
    }

    public void OnAttackSpin(InputAction.CallbackContext context)
    {
        if(context.started && CheckCooldown("Spin")) {
            animator.SetTrigger(AnimationStrings.spinAttack);
            CinemachineShake.Instance.ShakeCamera(15f, .6f); // 카메라 쉐이크
            //Debug.Log(AnimationStrings.spinAttack+" ON");
            if (touchingDirections.IsGrounded)
            {
                // Get the current position of the GameObject
                Vector3 currentPosition = transform.position;
                // Increase the y-axis value by 2
                currentPosition.y += 3f;
                // Set the new position to the GameObject's Transform
                transform.position = currentPosition;
            }
            
            rb.gravityScale = 0f;
            rb.velocity = new Vector2(rb.velocity.x * 2, 0);
            
        }
    }

    public void OnAttackUpper(InputAction.CallbackContext context)
    {
        if(context.started && CheckCooldown("Upper")) {
            animator.SetTrigger(AnimationStrings.airborne);
            //Debug.Log(AnimationStrings.airborne+" ON");
        }
    }
    

    public void Dead()
    {
        //게임 오버 화면 넘어가기
        gameObject.SetActive(false);
        SceneManager.LoadScene("DiedScene");
    }

    public void CallKnockBack(Vector2 knockBackForceVector, float knockTime)
    {
        rb.AddForce(knockBackForceVector);
        StartCoroutine(KnockTimeRoutine(knockTime));
    }

    public IEnumerator KnockTimeRoutine(float knockTime)
    {
        //CanMove = false;
        yield return new WaitForSeconds(knockTime);
        //CanMove = true;
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
            if(CheckCooldown("Dash"))
                StartCoroutine(Dash());
        }
    }
    
    private IEnumerator Dash()
    {
 
        IsDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        LockVelocity = true;
        rb.velocity = new Vector2( faceDirectionVector.x * dashImpulse, 0f);
        yield return new WaitForSeconds(dashingTime);

        LockVelocity = false;
        //Debug.Log("Dash End");
        rb.gravityScale = originalGravity;
        IsDashing = false;

    }

    private bool CheckCooldown(string attackName)
    {
        //Debug.Log(attackName + " cooldown : "+ cooldowns[attackName]);
        bool res ;
        if (canUseElemental.ContainsKey(attackName) && !canUseElemental[attackName]) return false;
        res = cooldowns[attackName] <= 0;
        if (res)
        {
            cooldowns[attackName] = coolTimes[attackName];
            //Debug.Log(attackName + " coolTime : "+ cooldowns[attackName] + " added");
        }
        return res;
    }

    public bool CheckCoolTime(string skillName)
    {
        if (cooldowns.ContainsKey(skillName))
        {
            if (canUseElemental.ContainsKey(skillName) && !canUseElemental[skillName]) return false;
            return cooldowns[skillName] <= 0f;
            
        }
        else
        {
            // If 'skillName' is not a key in the 'coolDown' dictionary, return true.
            // You may choose to handle this case differently based on your game's logic.
            return true;
        }
    }
    
}
