using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShootingScript : MonoBehaviour
{

    public bool shooting = false;
    public bool grenade = false;
    public bool reloading = false;
    public bool melee = false;
    private Coroutine _shootingCoroutine;
    private Coroutine _grenadeCoroutine;
    private Coroutine _reloadCoroutine;
    private Coroutine _meleeCoroutine;

    [Header("References")]
    [SerializeField] GameObject _playerMeleePoint;
    [SerializeField] LayerMask _attackPointLayer;
    [SerializeField] GameObject _playerCameraGameObject;
    private Camera _playerCamera;
    private UIScript _playerUI;

    [Header("Weapon Related Prefabs")]
    [SerializeField] GameObject _weaponEffectsEmptyGameObject;
    [SerializeField] GameObject _weaponSoundPrefab;
    [SerializeField] GameObject _bulletImpactPrefab;
    [SerializeField] GameObject _grenadePrefab;
    [SerializeField] GameObject _grenadeThrowSound;
    [SerializeField] GameObject _weaponReloadSoundPrefab;

    ParticleSystem _muzzleFlash;

    [Header("Weapon Related Data")]
    [SerializeField] float _weaponShootingRPM;
    [SerializeField] float _rifleDamageAmount = 20;
    [SerializeField] float _meleeDamageAmount = 50;

    [SerializeField] private int _currentMagazine;
    [SerializeField] private int _maxMagazine;
    [SerializeField] private int _reserveAmmo;
    [SerializeField] private int _grenadeCount;


    private void Start()
    {
        _playerCamera = _playerCameraGameObject.GetComponent<Camera>();
        _playerUI = GameObject.Find("UICanvas").GetComponent<UIScript>();

        _muzzleFlash = _weaponEffectsEmptyGameObject.GetComponentInChildren<ParticleSystem>();
        _shootingCoroutine = StartCoroutine(ShootingCoroutine());
        _grenadeCoroutine = StartCoroutine(GrenadeCoroutine());
        _reloadCoroutine = StartCoroutine(ReloadCoroutine());
        _meleeCoroutine = StartCoroutine(MeleeCoroutine());

        _currentMagazine = _maxMagazine;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (!shooting) _muzzleFlash.Stop();

    }

    private IEnumerator ShootingCoroutine()
    {
        //we want this to always be running for ease of not starting and stopping the routine continuously
        while (true)
        {

            if (shooting && !reloading && _currentMagazine > 0)
            {
                ShootWeapon();

                if (!_muzzleFlash.isPlaying) _muzzleFlash.Play();

                yield return new WaitForSeconds(60 / _weaponShootingRPM);
            }

            else
            {
                if (_muzzleFlash.isPlaying) _muzzleFlash.Stop();
                yield return null;
            } 
        }
    }

    void ShootWeapon()
    {
        // Spawn sound
        Instantiate(_weaponSoundPrefab, _weaponEffectsEmptyGameObject.transform.position, Quaternion.identity);


        RaycastHit hit;
        Vector3 midScreenRay = new(Screen.width / 2, Screen.height / 2, 0);

        if (Physics.Raycast(_playerCamera.ScreenPointToRay(midScreenRay), out hit))
        {
            // Debug
            //Vector3 direction = hit.point - _playerCameraGameObject.transform.position;
            //Debug.DrawRay(_playerCameraGameObject.transform.position, direction, Color.blue, 10f);
            
            
            //Spawn particle effect at hit position
            Instantiate(_bulletImpactPrefab, hit.point, Quaternion.identity);

            //Negate ammo
            _currentMagazine--;
            _playerUI.Ammo(_reserveAmmo, $"{_currentMagazine}/{_maxMagazine}");

            //Deal damage if need be
            Transform hitTransform = hit.transform;
            EntityHealthComponent EHC;

            if (hitTransform.TryGetComponent<EntityHealthComponent>(out EHC))
            {
                EHC.ApplyDamage(_rifleDamageAmount);
                
            }
        }
    }

    private IEnumerator GrenadeCoroutine()
    {
        //we want this to always be running for ease of not starting and stopping the routine continuously
        while (true)
        {

            if (grenade && _grenadeCount > 0)
            {
                ThrowGrenade();

                yield return new WaitForSeconds(1.5f);
            }

            else
            {
                yield return null;
            }
        }
    }

    void ThrowGrenade()
    {
        GameObject soundgo = Instantiate(_grenadeThrowSound);
        soundgo.transform.SetParent(transform);
        soundgo.transform.position = Vector3.zero;

        GameObject go = Instantiate(_grenadePrefab, _playerCamera.transform.position + (_playerCamera.transform.forward), _playerCamera.transform.rotation);
        _grenadeCount--;
        _playerUI.Grenade(_grenadeCount);
    }

    public IEnumerator ReloadCoroutine()
    {
        while (true)
        {
            if (reloading)
            {
                Instantiate(_weaponReloadSoundPrefab, transform.position, Quaternion.identity);
                _playerUI.Reload();
                ReloadWeapon();
                yield return new WaitForSeconds(2);
                reloading = false;
            }

            else yield return null;
        }    
    }

    void ReloadWeapon()
    {
        int magDifference = _maxMagazine - _currentMagazine;
        int amount;

        if (_reserveAmmo >= magDifference) amount = magDifference;
        else amount = _reserveAmmo;

        _reserveAmmo -= amount;
        _currentMagazine += amount;
        _playerUI.Ammo(_reserveAmmo, $"{_currentMagazine}/{_maxMagazine}");
    }

    public void AddAmmo(int amount)
    { 
        _reserveAmmo += amount;
        _playerUI.Ammo(_reserveAmmo, $"{_currentMagazine}/{_maxMagazine}");
    }
    
    public void AddGrenades(int amount)
    { 
        _grenadeCount += amount;
        _playerUI.Grenade(_grenadeCount);
    }

    public IEnumerator MeleeCoroutine()
    {
        while (true)
        {
            if (melee)
            {
                MeleeWeapon();
                yield return new WaitForSeconds(0.5f);
                melee = false;
            }

            else
            {
                yield return null;
            }
        }



    }

    void MeleeWeapon()
    {
        ShootPointCollider collider = _playerMeleePoint.GetComponent<ShootPointCollider>();
        List<GameObject> collisionList = collider._collisionList;

        EntityHealthComponent EHC;

        foreach (GameObject obj in collisionList)
        {
            if (obj.TryGetComponent<EntityHealthComponent>(out EHC))
            {
                Debug.Log($"Player melee Damaging {obj.transform.name}");
                EHC.ApplyDamage(_meleeDamageAmount);
            }
        }

    }

}
