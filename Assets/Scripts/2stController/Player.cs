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
    private bool isWallDetected;

    [Header("Coyote Time")]
    [SerializeField] private float coyoteTimeDuration = 0.2f;
    private float coyoteTimeCounter;

    private bool hasJumpedSinceGrounded = false;


    [Header("Ground Check Positions")]
    [SerializeField] private float groundCheckOffset = 0.15f;  // De 0.15 a 0.2 según tu personaje

    [Header("Jump Buffer")]
    [SerializeField] private float jumpBufferTime = 0.15f;
    private float jumpBufferCounter;


    

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
            DashLogic(); // New method for handling dash
            return; // Skip normal movement while dashing
        }

        CollisionCheck();
        FlipController();
        AnimatorController();
        CheckInput();
        
        if (jumpBufferCounter > 0f)
        {
            if (!isDashing) // 💡 no intentar saltar mientras dash activo
            {
                JumpButton();
                jumpBufferCounter = 0f; // Consumir el buffer
            }
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }


        if (isGrounded && jumpReleased)
        {
            hasJumpedSinceGrounded = false;
            canMove = true;
            canDoubleJump = doubleJumpSkillUnlocked;
            isWallSliding = false;
            canDash = true; // reset dash when grounded
            if (dashSkillUnlocked)
                canDash = true; // Only reset dash if it's unlocked
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
            jumpBufferCounter = jumpBufferTime; // 💡 activa el buffer
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
        dashCooldownInProgress = true; // ✅ Marcar cooldown activo
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
            Invoke(nameof(ResetDash), dashCooldown); // Cooldown before dashing again
        }
    }

    private void ResetDash()
    {
        canDash = true;
        dashCooldownInProgress = false; // ✅ Cooldown terminado
    }



    private void JumpButton()
    {
        if (isWallSliding)
        {
            WallJump();
        }
        else if ((isGrounded || coyoteTimeCounter > 0f) && !hasJumpedSinceGrounded)
        {
            Jump();
            hasJumpedSinceGrounded = true;
            coyoteTimeCounter = 0f; // Consumir coyote time
        }
        else if (canDoubleJump && doubleJumpSkillUnlocked)
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

    private void ReactivateExtraGroundChecks()
    {
        suppressExtraGroundChecks = false;
    }


    private void WallJump()
    {
        canMove = false;
        canDoubleJump = true;
        suppressExtraGroundChecks = true;

        rb.velocity = new Vector2(wallJumpDirection.x * -facingDirection, wallJumpDirection.y);

        Invoke(nameof(EnableMovement), 0.2f);
        Invoke(nameof(ReactivateExtraGroundChecks), 0.2f);

        // ✅ Solo reinicia cooldown si no hay uno activo
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

        // Manejar reacción al daño desde trampas
    public IEnumerator TakeHitAndRecover()
    {
        // Reproducir animación de daño si tienes un trigger o bool
        anim.SetTrigger("Hit"); // Asegúrate de tener esta animación en el Animator

        // Desactivar el movimiento y detener al personaje
        canMove = false;
        rb.velocity = Vector2.zero;

        // Esperar una breve pausa antes de permitir el movimiento de nuevo
        yield return new WaitForSecondsRealtime(0.5f); // Puedes ajustar el tiempo

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

    private void CollisionCheck()
    {
        Vector2 centerPos = transform.position;
        Vector2 leftPos = centerPos + Vector2.left * groundCheckOffset;
        Vector2 rightPos = centerPos + Vector2.right * groundCheckOffset;

        bool centerHit = Physics2D.Raycast(centerPos, Vector2.down, groundCheckDistance, whatIsGround);
        bool leftHit = false;
        bool rightHit = false;

        if (!suppressExtraGroundChecks)
        {
            leftHit = Physics2D.Raycast(leftPos, Vector2.down, groundCheckDistance, whatIsGround);
            rightHit = Physics2D.Raycast(rightPos, Vector2.down, groundCheckDistance, whatIsGround);
        }

        bool detectedGround = centerHit || leftHit || rightHit;

        if (detectedGround)
        {
            isGrounded = true;
            coyoteTimeCounter = coyoteTimeDuration;
        }
        else
        {
            isGrounded = false;
            coyoteTimeCounter -= Time.deltaTime;
        }

        isWallDetected = Physics2D.Raycast(centerPos, Vector2.right * facingDirection, wallCheckDistance, whatIsGround);
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
