using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthComponent : EntityHealthComponent
{

    public void Awake()
    {
        maxHealth = 100;
        currentHealth = maxHealth;
    }

    public override void PlayDeathEffects() 
    { 
        GameObject managerGO = GameObject.FindGameObjectWithTag("GameManager");
        GameManager managerScript = managerGO.GetComponent<GameManager>();
        managerScript.EndGame(false);
    }

    public override void SpawnDeathItem() { }


    public override void AwardPoints() { }
}
