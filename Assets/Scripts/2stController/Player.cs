using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class Player : MonoBehaviour
{
    public Animator anim;
    public Rigidbody2D rb;

    [Header("Info Personaje")]
    [SerializeField] private float speed = 5;
    [SerializeField] private float jumpForce = 12;

    private bool canMove = true;
    private bool canDoubleJump;
    private bool canWallSlide;
    private bool isWallSliding;

    private float movingInput;
    private bool facingRight = true;
    private int facingDirection = 1;
    [SerializeField] private Vector2 wallJumpDirection;

    [Header("Collision")]
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private float wallCheckDistance;
    private bool isGrounded;
    private bool isWallDetected;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        CollisionCheck();
        FlipController();
        AnimatorController();
        CheckInput();

        if (isGrounded)
        {
            canMove = true;
            canDoubleJump = true;
            isWallSliding = false;
        }

        if (isWallDetected && rb.velocity.y < 0 && movingInput != 0)
        {
            canWallSlide = true;
        }
        else
        {
            canWallSlide = false;
        }

        if (canWallSlide)
        {
            isWallSliding = true;
            WallSlide();
        }
        else
        {
            isWallSliding = false;
        }

        Move();
    }

    private void CheckInput()
    {
        movingInput = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space))
        {
            JumpButton();
        }
    }

    private void Move()
    {
        if (canMove)
        {
            rb.velocity = new Vector2(movingInput * speed, rb.velocity.y);
        }
    }

    private void JumpButton()
    {
        if (isWallSliding)
        {
            WallJump();
        }
        else if (isGrounded)
        {
            Jump();
        }
        else if (canDoubleJump)
        {
            canMove = true;
            canDoubleJump = false;
            Jump();
        }

        canWallSlide = false;
    }

    private void FlipController()
    {
        if (movingInput > 0 && !facingRight)
        {
            Flip();
        }
        else if (movingInput < 0 && facingRight)
        {
            Flip();
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }

    private void WallJump()
    {
        canMove = false;
        canDoubleJump = true;
        rb.velocity = new Vector2(wallJumpDirection.x * -facingDirection, wallJumpDirection.y);
        Invoke("EnableMovement", 0.2f); // Breve retraso para permitir el salto de la pared
    }

    private void WallSlide()
    {
        rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
    }

    private void EnableMovement()
    {
        canMove = true;
    }

    private void Flip()
    {
        facingDirection *= -1;
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
    }

    private void AnimatorController()
    {
        bool Run = Mathf.Abs(rb.velocity.x) > 0.1f;

        anim.SetFloat("yVelocity", rb.velocity.y);
        anim.SetBool("Run", Run);
        anim.SetBool("isGrounded", isGrounded);
        anim.SetBool("isWallSliding", isWallSliding);
    }

    private void CollisionCheck()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround);
        isWallDetected = Physics2D.Raycast(transform.position, Vector2.right * facingDirection, wallCheckDistance, whatIsGround);

        if (!isWallDetected && rb.velocity.y < 0)
        {
            canWallSlide = true;
        }
        else
        {
            canWallSlide = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + wallCheckDistance * facingDirection, transform.position.y));
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y - groundCheckDistance));
    }
}
