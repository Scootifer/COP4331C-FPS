using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EntityHealthComponent : MonoBehaviour
{

    public float currentHealth;
    public float maxHealth = 50;

    public void ApplyDamage(float damage)
    {
        currentHealth -= damage;

        if (currentHealth < 0) 
        {
            ApplyDeath();
        }

        if (gameObject.tag.Equals("Player")) GameObject.Find("UICanvas").GetComponent<UIScript>().TakeDamage(currentHealth);

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
