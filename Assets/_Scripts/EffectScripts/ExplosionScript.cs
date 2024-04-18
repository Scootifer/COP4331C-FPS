using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionScript : MonoBehaviour
{
    ParticleSystem partSystem;

    [SerializeField] float delay;
    [SerializeField] GameObject _explosionSoundPrefab;

    // Start is called before the first frame update
    void Start()
    {
        partSystem = GetComponent<ParticleSystem>();
        GameObject go = Instantiate(_explosionSoundPrefab);
        go.transform.SetParent(transform);
        go.transform.position = Vector3.zero;
        StartCoroutine(DelaySpawnAndSelfDestruct());

    }

    private IEnumerator DelaySpawnAndSelfDestruct()
    {
        partSystem.Play();
        yield return new WaitForSeconds(delay);

        Destroy(this.gameObject);

    }
}
