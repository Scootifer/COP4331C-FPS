using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIEyeEffect : MonoBehaviour
{

    private ParticleSystem _partSystem;
    // Start is called before the first frame update
    void Start()
    {
        _partSystem = gameObject.GetComponent<ParticleSystem>();

        StartCoroutine(DelaySpawnAndSelfDestruct());
    }

    private IEnumerator DelaySpawnAndSelfDestruct()
    {
        _partSystem.Play();
        float delay = 2f;
        yield return new WaitForSeconds(delay);

        Destroy(this.gameObject);

    }
}
