using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    public float jumpForce;
    private Rigidbody2D rb;
    private float moveHorizontal;
    private bool isJumping = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        moveSpeed = 20.0f;
        jumpForce = 15.0f;
    }
    
    void Update()
    {
        moveHorizontal = Input.GetAxis("Horizontal") * moveSpeed;

        if(Input.GetButtonDown("Jump") && Mathf.Abs(rb.velocity.y) < 0.001f)
        {
            isJumping = true;
        }
    }

    void FixedUpdate()
    {
        Move();

        if (isJumping)
        {
            Jump();
        }
    }

    void Move()
    {
        Vector2 movement = new Vector2(moveHorizontal, rb.velocity.y);
        rb.velocity = movement;
    }

    void Jump()
    {
        rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
        isJumping = false;
    }
}
