using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupplyCrate : MonoBehaviour
{

    [SerializeField] private SupplyCrateType _crateType;
    [SerializeField] private GameObject _pickupSoundPrefab;

    public void OnTriggerEnter(Collider collision)
    {
        if (!collision.transform.tag.Equals("Player")) return;
        PlayerShootingScript PSS = collision.transform.GetComponent<PlayerShootingScript>();

        switch (_crateType)
        {

            case SupplyCrateType.Small:
                int rand = Mathf.RoundToInt(UnityEngine.Random.Range(0, 2));
                if (rand == 0) PSS.AddAmmo(30);
                else PSS.AddGrenades(1);
                break;

            case SupplyCrateType.Large:
                PSS.AddAmmo(30);
                PSS.AddGrenades(1);
                break;

        }

        Instantiate(_pickupSoundPrefab, transform.position, Quaternion.identity);

        Destroy(this.gameObject);

    }

}

public enum SupplyCrateType 
{ 
    Small = 0,
    Large = 1
}
