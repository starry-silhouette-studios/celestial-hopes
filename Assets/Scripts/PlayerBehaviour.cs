using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void PlayerTakeDmg (int dmg)
    {
        GameManager.gameManager._playerHealth.DmgUnit(dmg, gameObject);
        Debug.Log(GameManager.gameManager._playerHealth.Health);
    }

    private void HealPlayer(int healing)
    {
        GameManager.gameManager._playerHealth.HealUnit(healing);
        Debug.Log(GameManager.gameManager._playerHealth.Health);
    }

    /// test
    private void PlayerTakeHeal(int healing)
    {
        GameManager.gameManager._playerHealth.HealUnit(healing);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            FlorgBehaviour enemy = collision.gameObject.GetComponent<FlorgBehaviour>();
            if (enemy != null)
            {
                PlayerTakeDmg(enemy.damageAmount);
            }
        }

        if (collision.gameObject.CompareTag("Healing"))
        {
            HealingBehaviour health= collision.gameObject.GetComponent<HealingBehaviour>();
            if (health != null)
            {
                HealPlayer(health.healing);
            }
            GameObject.Destroy(collision.gameObject);
        }
    }
}
