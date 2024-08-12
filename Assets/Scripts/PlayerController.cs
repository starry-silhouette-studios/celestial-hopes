using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    public float jumpForce;
    private Rigidbody2D rb;
    private float moveHorizontal;
    private bool isJumping = false;
    private bool doubleJump = false;
    
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;
    public int attackDamage = 10;
    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask groundLayer;
    private bool isTouchingGround;

    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        moveSpeed = 20.0f;
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
    }

    void FixedUpdate()
    {
        Vector2 movement = new Vector2(moveHorizontal, rb.velocity.y);
        rb.velocity = movement;

        //if (isJumping)
        //{
        //    rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
        //    isJumping = false;
        //    doubleJump = true;
        //}
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
