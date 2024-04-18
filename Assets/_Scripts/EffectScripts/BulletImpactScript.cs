using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletImpactScript : MonoBehaviour
{

    ParticleSystem bulletParticleSystem;

    // Start is called before the first frame update
    void Start()
    {
        bulletParticleSystem = GetComponent<ParticleSystem>();
        StartCoroutine(DelaySpawnAndSelfDestruct());

    }

    private IEnumerator DelaySpawnAndSelfDestruct()
    {
        yield return new WaitForSeconds(0.05f);
        bulletParticleSystem.Play();
        yield return new WaitForSeconds(0.25f);
        Destroy(this.gameObject);

    }
}
