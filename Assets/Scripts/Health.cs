
using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] float timeToDestroy = 2f;
    [SerializeField] float maxHealth = 20;
    float currentHealth = 0;
    bool isDead = false;
    public event Action<Health> OnDie;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    public void ReplenishHealth()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(GameObject instigator, float damage)
    {
        currentHealth -= damage;
        if(currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        OnDie?.Invoke(this);
        if (!this.gameObject.CompareTag("Player"))
        {
            isDead = true;
            GetComponent<CapsuleCollider>().enabled = false;
            Destroy(gameObject, timeToDestroy);
        }
        else
        {
            isDead = true;

        }
        
    }

    public bool IsDead()
    {
        return isDead;
    }
}
