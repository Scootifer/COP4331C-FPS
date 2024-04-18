using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EntityHealthComponent : MonoBehaviour
{

    public float currentHealth { get; private set; }
    [SerializeField] private float maxHealth;

    

    private void Awake()
    {
        maxHealth *= transform.localScale.x;
        Debug.Log($"AI Health {maxHealth}");

        currentHealth = maxHealth;
    }

    public void ApplyDamage(float damage)
    {
        currentHealth -= damage;

        if (currentHealth < 0) 
        {
            ApplyDeath();
        }
    }

    public abstract void PlayDeathEffects();
    public abstract void SpawnDeathItem();
    public abstract void AwardPoints();


    private void ApplyDeath() 
    {
        PlayDeathEffects();
        SpawnDeathItem();
        AwardPoints();
        Destroy(this.gameObject);

    }

    public float GetMaxHealth() 
    {
        return maxHealth;
    } 
}
