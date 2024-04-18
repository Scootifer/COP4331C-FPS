using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSoundPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SelfDestruct());

    }

    private IEnumerator SelfDestruct()
    {
        yield return new WaitForSeconds(1.5f);

        Destroy(this.gameObject);
    
    }
}
