using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlorgBehaviour : MonoBehaviour
{
    public int damageAmount = 10;
    public int health = 50;
    
    public void TakeDamage(int damage)
    {
        health -= damage;
        
        if (health <= 0)
        {
            Die();
        }
    }
    
    private void Die()
    {
        Destroy(gameObject);
    }
}
