using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlorgBehaviour : MonoBehaviour
{
    public int damageAmount = 10;
    public int health = 50;
    public float speed;
    public GameObject player;
    
    public void TakeDamage(int dmg)
    {
        GameManager.gameManager._playerHealth.DmgUnit(dmg, gameObject);

        Debug.Log(GameManager.gameManager._playerHealth.Health);
    }
}
