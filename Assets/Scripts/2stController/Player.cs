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
    private bool canWallJump = true; 
    private bool canWallSlide;
    private bool isWallSliding;

    private float movingInput;
    private bool facingRight = true;
    private int facingDirection = 1;
    [SerializeField] private Vector2 wallJumpDirection;
    
    [Header("Collision")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius;
    [SerializeField] private LayerMask whatIsGround;
    private bool isGrounded;

    [SerializeField] private Transform wallCheck;
    [SerializeField] private float wallCheckDistance;
    private bool isWallDetected;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
    
        CheckInput();
        AnimatorController();
        FlipController(); 
        CollisionCheck();

    }

     private void FixedUpdate() 
    {
        if (isGrounded)
        {
            canMove = true;
            canDoubleJump = true;
        }

        if (Input.GetAxis("Vertical") < 0)
        {
            canWallSlide = false;
        }

        if (isWallDetected && canWallSlide)
        {
            isWallSliding = true;
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.1f);
        }
        else if (!isWallDetected)
        {
            isWallSliding = false;
            Move();
        }
        
    }

    private void CheckInput()
    {   
        if (Input.GetKeyDown(KeyCode.Space))
        {
            JumpButton();
        }

        if (canMove)
        {
            movingInput = Input.GetAxisRaw("Horizontal");
        }
    }

    private void Move()
    {
        if(canMove)
        {
            rb.velocity = new Vector2(movingInput * speed, rb.velocity.y);
        }
    }

    private void JumpButton()
    {
        if (isWallSliding && canWallJump)
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
        if (isGrounded && isWallDetected)
        {
            if (facingRight && movingInput < 0)
            {
                Flip();
            }
            else if (!facingRight && movingInput > 0)
            {
                Flip();
            }
        }

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
        Vector2 direction = new Vector2(wallJumpDirection.x * -facingDirection, wallJumpDirection.y);
        rb.AddForce(direction, ForceMode2D.Impulse);
    }

    private void Flip()
    {
        facingDirection = facingDirection * -1;
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
    }

    private void AnimatorController()
    {
        bool Run = movingInput != 0;

        anim.SetFloat("yVelocity", rb.velocity.y);
        anim.SetBool("isGrounded", isGrounded);
        anim.SetBool("Run", Run);
        anim.SetBool("isWallSliding", isWallSliding);
    }

    private void CollisionCheck()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
        isWallDetected = Physics2D.Raycast(wallCheck.position, Vector2.right, wallCheckDistance, whatIsGround);

        if (!isGrounded && rb.velocity.y < 0)
        {
            canWallSlide = true;
        }
    }

    private void OnDrawGizmos() 
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);

        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y, wallCheck.position.z));    
    }
}
