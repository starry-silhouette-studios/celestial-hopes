using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    public float jumpForce;
    private float moveHorizontal;
    
    private bool isJumping = false;
    private bool doubleJump = false;

    private bool isWallSliding;
    private float wallSlidingSpeed = 2f;

    private bool isWallJumping;
    private float wallJumpingDirection;
    private float wallJumpingTime = 0.2f;
    private float wallJumpingCounter;
    private float wallJumpingDuration = 0.4f;
    private Vector2 wallJumpingPower = new Vector2(10f, 20f);

    public Transform attackPoint;
    public LayerMask enemyLayers;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    public float attackRange = 0.5f;
    public int attackDamage = 10;

    public float groundCheckRadius;
    private bool isTouchingGround;

    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        moveSpeed = 10.0f;
        jumpForce = 15.0f;
        
        animator.SetBool("isAttacking", false);
    }
    
    void Update()
    {
        isTouchingGround = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        moveHorizontal = Input.GetAxis("Horizontal") * moveSpeed;

        if (moveHorizontal > 0.01f)
        {
            transform.localScale = new Vector3(1, 1, 1);
        } 
        else if (moveHorizontal < -0.01f)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }

        if (isTouchingGround)
        {
            isJumping = false;
        }

        if (Input.GetButtonDown("Jump") && isTouchingGround)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);

            doubleJump = true;

            isJumping = true;
        }

        if (Input.GetButtonDown("Jump") && !isTouchingGround && doubleJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);

            doubleJump = false;
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            Attack();
        }

        WallSlide();
        WallJump();
    }

    void FixedUpdate()
    {
        if (!isWallJumping)
        {
            Vector2 movement = new Vector2(moveHorizontal, rb.velocity.y);
            rb.velocity = movement;
        }
        

        //if (isJumping)
        //{
        //    rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
        //    isJumping = false;
        //    doubleJump = true;
        //}
    }

    private bool IsWalled()
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);
    }

    private void WallJump()
    {
        if (isWallSliding)
        {
            isWallJumping = false;
            wallJumpingDirection = -Mathf.Sign(transform.localScale.x);
            wallJumpingCounter = wallJumpingTime;

            CancelInvoke(nameof(StopWallJumping));
        }
        else
        {
            wallJumpingCounter -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump") && wallJumpingCounter > 0f)
        {
            isWallJumping = true;

            float horizontalVelocity = moveHorizontal * 0.5f;
            float wallJumpVelocityX = wallJumpingDirection * wallJumpingPower.x + horizontalVelocity;

            Debug.Log(horizontalVelocity);
            
            rb.velocity = new Vector2(wallJumpVelocityX, wallJumpingPower.y);

            Invoke(nameof(DisableWallSlide), 0.1f);

            wallJumpingCounter = 0f;

            if (transform.localScale.x != wallJumpingDirection)
            {
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            }

            Invoke(nameof(StopWallJumping), wallJumpingDuration);
        }
    }

    private void DisableWallSlide()
    {
        isWallSliding = false;
    }

    private void WallSlide()
    {
        if (IsWalled() && !isTouchingGround && moveHorizontal != 0f && !isWallJumping)
        {
            isWallSliding = true;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
        }
        else
        {
            isWallSliding = false;
        }
    }

    private void StopWallJumping()
    {
        isWallJumping = false;
    }

    void Attack() 
    {
        Debug.Log(animator.GetBool("isAttacking"));
        
        animator.SetBool("isAttacking", true);
        
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        
        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<FlorgBehaviour>().TakeDamage(attackDamage);
        }
        
        StartCoroutine(StopAttackAnimation());
    }
    
    IEnumerator StopAttackAnimation()
    {
        yield return new WaitForSeconds(0.5f);
        
        animator.SetBool("isAttacking", false);
    }
    
    void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
        {
            return;
        }
        
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
