using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb2D;

    private float moveSpeed;
    private float jumpForce;
    private bool isJumping;
    private float moveHorizontal;
    private float moveVertical;

    void Start()
    {
        // test
        rb2D = gameObject.GetComponent<Rigidbody2D>();

        moveSpeed = 3.0f;
        jumpForce = 3.0f;
        isJumping = false;
    }
    
    void Update()
    {
        moveHorizontal = Input.GetAxisRaw("Horizontal");
        moveVertical = Input.GetAxisRaw("Vertical");
    }

    void FixedUpdate()
    {
        if(moveHorizontal != 0.0)
        {
            rb2D.AddForce(
                new Vector2(moveHorizontal * moveSpeed, 0.0f),
                ForceMode2D.Impulse
            );
        }

        if (moveVertical != 0.0)
        {
            if (isJumping != true) { 
                rb2D.AddForce(
                    new Vector2(0.0f, moveVertical * jumpForce),
                    ForceMode2D.Impulse
                );

                isJumping = true;

                Debug.Log("Sander is een sukkel");
            }
        }
        else
        {
            isJumping = false;
        }
    }
}
