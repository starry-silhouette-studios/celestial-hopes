using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlorgBehaviour : MonoBehaviour
{
    public int damageAmount = 10;
    public int health = 50;
    public float speed;
    public float jumpForce = 0.001f;
    public float knockBack = 5f;
    public GameObject player;

    private float distance;
    private Rigidbody2D rb;
    private bool isGrounded = true;
    public void TakeDamage(int dmg)
    {
        GameManager.gameManager._playerHealth.DmgUnit(dmg, gameObject);

        Debug.Log(gameObject.name + " now has " + GameManager.gameManager._playerHealth.Health + " health");

        ApplyKnockBack();
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D component missing from this game object");
        }
    }

    private void Update()
    {
        distance = Vector2.Distance(transform.position, player.transform.position);
        Vector2 direction = player.transform.position - transform.position;

        transform.position = Vector2.MoveTowards(this.transform.position, player.transform.position, speed * Time.deltaTime);

        if (distance <= 1 && isGrounded)
        {
            Jump();
        }
    }

    private void Jump()
    {
        if (rb != null)
        {
            // Add an upward force to the Rigidbody2D component to simulate a jump
            rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
            isGrounded = false; // Ensure the enemy cannot jump again until it is grounded
        }
        else
        {
            Debug.LogError("Rigidbody2D reference is null. Cannot perform jump.");
        }
    }

    private void ApplyKnockBack()
    {
        if (rb != null)
        {
            Vector2 bounceDirection = (transform.position - player.transform.position).normalized;
            rb.AddForce(bounceDirection * knockBack, ForceMode2D.Impulse);
        }
        else
        {
            Debug.LogError("Rigidbody2D reference is null. Cannot apply bounce force.");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the enemy is touching the ground
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // Check if the enemy is leaving the ground
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}
