using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public float jumpForce = 10f;
    public Transform groundCheck;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private Animator animator;
    private bool isGrounded;

    private bool isJumping;

    // Attack and Roll trigger variables
    private bool attackTrigger;
    private bool rollTrigger;
    private float groundCheckRadius = 0.2f;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        
        // Run State
        float moveInput = Input.GetAxis("Horizontal");

        rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);
        animator.SetFloat("Speed", Mathf.Abs(moveInput));

        animator.SetBool("IsGrounded", isGrounded);
        animator.SetBool("IsJumping", isJumping);

        // Jump State
        if (isGrounded && Input.GetButtonDown("Jump")) {
            rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
            animator.SetBool("isJumping", true);
        }
        else {
            animator.SetBool("isJumping", false);
        }


        //Attacking State
        if (Input.GetButtonDown("Fire1") && !isJumping) {
            attackTrigger = true;
            animator.SetTrigger("Attack Trigger");
        }
        else {
            attackTrigger = false;
        }
        
        // Rolling State
        if (Input.GetButtonDown("Fire2") && !isJumping) {
            isRolling = true;
            rollTrigger = true;
            animator.SetTrigger("RollTrigger");
            rollTime = Time.time + rollDuration;
            rb.velocity = new Vector2(moveInput * moveSpeed * rollSpeedMultiplier, rb.velocity.y);
        }
        else {
            rollTrigger = false;
        }
    }
}

