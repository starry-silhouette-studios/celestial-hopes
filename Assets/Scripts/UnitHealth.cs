using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitHealth
{
    // Fields
    int _currentHealth;
    int _currentMaxHealth;

    // Properties
    public int Health 
    { 
        get
        {
            return _currentHealth;
        }   
        set
        {
            _currentHealth = value;
        }
    }

    public int MaxHealth
    {
        get
        {
            return _currentMaxHealth;
        }
        set
        {
            _currentMaxHealth = value;
        }
    }

    // Constructor
    public UnitHealth(int health, int maxHealth)
    {
        _currentHealth = health;
        _currentMaxHealth = maxHealth;
    }

    // Methods
    public void DmgUnit(int dmgAmount, GameObject gameObject)
    {
        if (_currentHealth > 0)
        {
            _currentHealth -= dmgAmount;

            if (_currentHealth <= 0)
            {
                KillUnit(gameObject);
            }
        }
    }

    public void HealUnit(int healAmount)
    {
        if (_currentHealth < _currentMaxHealth)
        {
            _currentHealth += healAmount;
        }
        
        if (_currentHealth > _currentMaxHealth)
        {
            _currentHealth = _currentMaxHealth;
        }
    }

    public static void KillUnit(GameObject gameObject)
    {
        Animator animator = gameObject.GetComponent<Animator>();
        
        if (animator != null)
        {
            animator.SetTrigger("Die");
        }

        OnDeath(gameObject);
    }

    private static void OnDeath(GameObject gameObject)
    {
        Debug.Log(gameObject.name + " has been killed.");

        GameObject.Destroy(gameObject, 2f);
    }
}
