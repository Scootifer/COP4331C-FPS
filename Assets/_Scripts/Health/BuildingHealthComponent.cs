using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingHealthComponent : EntityHealthComponent
{
    [SerializeField] private GameObject _explosionPrefab;
    [SerializeField] private GameObject _deathSpawnItem;

    private void Awake()
    {
        currentHealth = 200;
    }

    public override void PlayDeathEffects()
    {
        GameObject go = Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
        
    }

    public override void SpawnDeathItem()
    {
        Instantiate(_deathSpawnItem, transform.position, Quaternion.identity);
    }


    public override void AwardPoints()
    {
        GameObject managerGO = GameObject.FindGameObjectWithTag("GameManager");
        GameManager managerScript = managerGO.GetComponent<GameManager>();
        managerScript.BaseDestroyed();
        managerScript.AddPoints(100);
    }

}
