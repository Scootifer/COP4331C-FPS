using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthComponent : EntityHealthComponent
{
    public override void PlayDeathEffects() 
    { 
        GameObject managerGO = GameObject.FindGameObjectWithTag("GameManager");
        GameManager managerScript = managerGO.GetComponent<GameManager>();
        managerScript.EndGame();
    }

    public override void SpawnDeathItem() { }


    public override void AwardPoints() { }
}
