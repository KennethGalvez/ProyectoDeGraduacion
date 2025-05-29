using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Animator anim;
    public Rigidbody2D rb;

    [Header("Info Personaje")]
    [SerializeField] private float speed = 5;
    [SerializeField] private float jumpForce = 12;

    private bool dashSkillUnlocked = false;
    private bool doubleJumpSkillUnlocked = false;

    private bool jumpReleased = true;

    private bool canMove = true;
    private bool canDoubleJump;
    private bool canWallSlide;
    private bool isWallSliding;

    private float movingInput;
    private bool facingRight = true;
    private int facingDirection = 1;
    [SerializeField] private Vector2 wallJumpDirection;

    private bool suppressExtraGroundChecks = false;

    [Header("Dash Settings")]
    [SerializeField] private float dashSpeed = 15f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float dashCooldown = 1f;
    private bool canDash = true;
    private bool isDashing = false;
    private float dashTimeCounter;
    private bool dashCooldownInProgress = false;

    [Header("Collision")]
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private float wallCheckDistance;
    private bool isGrounded;
    private bool wasGrounded = false;
    private bool isWallDetected;

    [Header("Coyote Time")]
    [SerializeField] private float coyoteTimeDuration = 0.2f;
    private float coyoteTimeCounter;
    private bool hasJumpedSinceGrounded = false;

    [Header("Ground Check Positions")]
    [SerializeField] private float groundCheckOffset = 0.15f;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        dashSkillUnlocked = QuickTimeManager.dashUnlocked;
        doubleJumpSkillUnlocked = QuickTimeManager.doubleJumpUnlocked;

        if (isDashing)
        {
            DashLogic();
            return;
        }

        CollisionCheck();
        FlipController();
        AnimatorController();
        CheckInput();

        if (isGrounded && jumpReleased)
        {
            canMove = true;
            canDoubleJump = doubleJumpSkillUnlocked;
            isWallSliding = false;
            canDash = dashSkillUnlocked ? true : canDash;
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
            if (isWallSliding)
            {
                WallJump();
            }
            else if ((isGrounded || coyoteTimeCounter > 0f) && !hasJumpedSinceGrounded)
            {
                Jump();
                hasJumpedSinceGrounded = true;
                coyoteTimeCounter = 0f;
            }
            else if (canDoubleJump && doubleJumpSkillUnlocked)
            {
                Jump();
                canDoubleJump = false;
            }

            canWallSlide = false;
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            jumpReleased = true;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash && dashSkillUnlocked)
        {
            StartDash();
        }
    }

    private void Move()
    {
        if (canMove)
        {
            rb.velocity = new Vector2(movingInput * speed, rb.velocity.y);
        }
    }

    private void StartDash()
    {
        isDashing = true;
        canDash = false;
        dashCooldownInProgress = true;
        dashTimeCounter = dashDuration;

        rb.velocity = new Vector2(facingDirection * dashSpeed, 0f);
        anim.SetTrigger("Dash");
    }

    private void DashLogic()
    {
        if (dashTimeCounter > 0)
        {
            rb.velocity = new Vector2(facingDirection * dashSpeed, 0f);
            dashTimeCounter -= Time.deltaTime;
        }
        else
        {
            isDashing = false;
            rb.velocity = new Vector2(0f, rb.velocity.y);
            Invoke(nameof(ResetDash), dashCooldown);
        }
    }

    private void ResetDash()
    {
        canDash = true;
        dashCooldownInProgress = false;
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }

    private void WallJump()
    {
        canMove = false;
        canDoubleJump = true;
        suppressExtraGroundChecks = true;

        rb.velocity = new Vector2(wallJumpDirection.x * -facingDirection, wallJumpDirection.y);

        Invoke(nameof(EnableMovement), 0.2f);
        Invoke(nameof(ReactivateExtraGroundChecks), 0.2f);

        if (dashSkillUnlocked && !canDash && !dashCooldownInProgress)
        {
            dashCooldownInProgress = true;
            Invoke(nameof(ResetDash), dashCooldown);
        }
    }

    private void WallSlide()
    {
        rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
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

    private void Flip()
    {
        facingDirection *= -1;
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
    }

    private void CollisionCheck()
    {
        Vector2 centerPos = transform.position;
        Vector2 leftPos = centerPos + Vector2.left * groundCheckOffset;
        Vector2 rightPos = centerPos + Vector2.right * groundCheckOffset;

        bool centerHit = Physics2D.Raycast(centerPos, Vector2.down, groundCheckDistance, whatIsGround);
        bool leftHit = !suppressExtraGroundChecks && Physics2D.Raycast(leftPos, Vector2.down, groundCheckDistance, whatIsGround);
        bool rightHit = !suppressExtraGroundChecks && Physics2D.Raycast(rightPos, Vector2.down, groundCheckDistance, whatIsGround);

        bool detectedGround = centerHit || leftHit || rightHit;

        isGrounded = detectedGround;

        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTimeDuration;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        // 🆕 Resetear hasJumpedSinceGrounded solo al aterrizar (de NO grounded a grounded)
        if (!wasGrounded && isGrounded)
        {
            hasJumpedSinceGrounded = false;
        }

        wasGrounded = isGrounded;

        isWallDetected = Physics2D.Raycast(centerPos, Vector2.right * facingDirection, wallCheckDistance, whatIsGround);
    }

    private void ReactivateExtraGroundChecks()
    {
        suppressExtraGroundChecks = false;
    }

    private void EnableMovement()
    {
        canMove = true;
    }

    public IEnumerator TakeHitAndRecover()
    {
        anim.SetTrigger("Hit");
        canMove = false;
        rb.velocity = Vector2.zero;
        yield return new WaitForSecondsRealtime(0.5f);
        canMove = true;
    }

    private void AnimatorController()
    {
        bool Run = Mathf.Abs(rb.velocity.x) > 0.1f;
        anim.SetFloat("yVelocity", rb.velocity.y);
        anim.SetBool("Run", Run);
        anim.SetBool("isGrounded", isGrounded);
        anim.SetBool("isWallSliding", isWallSliding);
    }

    private void OnDrawGizmos()
    {
        Vector2 centerPos = transform.position;
        Vector2 leftPos = centerPos + Vector2.left * groundCheckOffset;
        Vector2 rightPos = centerPos + Vector2.right * groundCheckOffset;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(centerPos, centerPos + Vector2.down * groundCheckDistance);
        Gizmos.DrawLine(leftPos, leftPos + Vector2.down * groundCheckDistance);
        Gizmos.DrawLine(rightPos, rightPos + Vector2.down * groundCheckDistance);

        Gizmos.DrawLine(centerPos, centerPos + Vector2.right * wallCheckDistance * facingDirection);
    }
}
