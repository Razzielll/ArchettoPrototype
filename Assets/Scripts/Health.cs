using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    // Time in seconds after death before the game object is destroyed
    [SerializeField] float timeToDestroy = 2f;

    // The maximum health value for this object
    [SerializeField] float maxHealth = 20;

    // The current health value of this object
    float currentHealth = 0;

    // Flag to determine if the object is dead
    bool isDead = false;

    // Event that is triggered when the object dies
    public event Action<Health> OnDie;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize the current health to the maximum health value
        currentHealth = maxHealth;
    }

    // Method to replenish the health to its maximum value
    public void ReplenishHealth()
    {
        currentHealth = maxHealth;
    }

    // Method to apply damage to the object's health
    public void TakeDamage(GameObject instigator, float damage)
    {
        currentHealth -= damage;

        // If the current health drops to or below zero, the object dies
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // Method called when the object dies
    private void Die()
    {
        // Trigger the OnDie event, passing this Health component as a parameter
        OnDie?.Invoke(this);

        // If the object does not have the "Player" tag, it is destroyed after a delay
        if (!this.gameObject.CompareTag("Player"))
        {
            isDead = true;

            // Disable the CapsuleCollider to prevent further interactions with the object
            GetComponent<CapsuleCollider>().enabled = false;

            // Destroy the game object after the specified timeToDestroy delay
            Destroy(gameObject, timeToDestroy);
        }
        else // If the object has the "Player" tag, it is immediately destroyed without delay
        {
            isDead = true;
            Destroy(gameObject);
        }
    }

    // Method to check if the object is dead
    public bool IsDead()
    {
        return isDead;
    }
}
