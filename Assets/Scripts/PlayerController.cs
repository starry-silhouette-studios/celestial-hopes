using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    public float jumpForce;
    private Rigidbody2D rb;
    private float moveHorizontal;
    private bool isJumping = false;
    
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;
    public int attackDamage = 10;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        moveSpeed = 20.0f;
        jumpForce = 15.0f;
    }
    
    void Update()
    {
        moveHorizontal = Input.GetAxis("Horizontal") * moveSpeed;

        if (moveHorizontal > 0.01f)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (moveHorizontal < -0.01f)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }

        if(Input.GetButtonDown("Jump") && Mathf.Abs(rb.velocity.y) < 0.001f)
        {
            isJumping = true;
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

        if (isJumping)
        {
            rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
            isJumping = false;
        }
    }
    
    void Attack() 
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        
        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<FlorgBehaviour>().TakeDamage(attackDamage);
        }
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
