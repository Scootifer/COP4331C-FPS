using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIHealthComponent : EntityHealthComponent
{
    [SerializeField] private GameObject _bloodSplashPrefab;
    [SerializeField] private GameObject _deathSpawnItem;

    public override void PlayDeathEffects()
    {
        GameObject go = Instantiate(_bloodSplashPrefab, transform.position, Quaternion.identity);

    }

    public override void SpawnDeathItem()
    {
        float randChance = UnityEngine.Random.Range(0, 1.01f);

        if (randChance > 0.75f) Instantiate(_deathSpawnItem, transform.position - new Vector3(0,0.5f,0), Quaternion.identity);
    }

    public override void AwardPoints()
    {
        GameObject managerGO = GameObject.FindGameObjectWithTag("GameManager");
        GameManager managerScript = managerGO.GetComponent<GameManager>();
        managerScript.AddPoints(10);
    }
}

