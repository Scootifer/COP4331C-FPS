using System.Collections;
using UnityEngine;

public class GrenadeScript : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField] private GameObject _explosionPrefab;
    [SerializeField] private float _grenadeTimer;

    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        rb.velocity = (transform.forward * 15) + new Vector3(0, 5, 0);

        StartCoroutine(GrenadeTimer());
    }

    private IEnumerator GrenadeTimer() 
    {
        yield return new WaitForSeconds(_grenadeTimer);
        GrenadeExplosion();
    }

    private void OnCollisionEnter(Collision collision)
    {
        rb.velocity = Vector3.zero;
        GrenadeExplosion();
    }

    private void GrenadeExplosion()
    {
        GameObject go = Instantiate(_explosionPrefab, transform.position, Quaternion.identity);

        RaycastHit[] hits = Physics.SphereCastAll(transform.position, 8, transform.up);

        foreach (RaycastHit hit in hits)
        {
            EntityHealthComponent EHC;

            if (hit.transform.TryGetComponent<EntityHealthComponent>(out EHC))
            {

                EHC.ApplyDamage(100);
            }
        }

        StopAllCoroutines();
        Destroy(this.gameObject);
    }
}
