using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [Header("References:")]
    [SerializeField] private Transform _bulletSpawnLocation;
    [SerializeField] private Transform _reloadAnimator;
    [SerializeField] private Transform _recoilAnimator;


    [Header("Stats:")]
    [SerializeField] private float _firerate = 1;
    [SerializeField] private float _damage;
    [SerializeField] private float _magazineSize = 25;
    [SerializeField] private float _ammo;
    [SerializeField] private float _reloadTime = 3;
    [SerializeField] private float _bulletForce = 100f;
    [SerializeField] private float _inaccuracy = 0.2f;

    [Header("Internal State:")]
    [SerializeField] private float _reloadingTimer;
    private bool _isReloading
    {
        get
        {
            return (_reloadingTimer >= 0);
        } 
    }

    [SerializeField] private float _firerateTimer;
    private bool _isOnFirerateCooldown
    {
        get
        {
            return (_firerateTimer >= 0);
        }
    }

    private bool _hasAmmo
    {
        get
        {
            return (_ammo > 0);
        }
    }

    #region Initialisation

    private void Awake()
    {
        // Start with full ammo
        _ammo = _magazineSize;
        _reloadingTimer = -1f;
        _firerateTimer = -1f;
    }

    #endregion


    #region Input
    private void Update()
    {    
        ListenForInput();
        AnimateTopModel();
    }

    private void ListenForInput()
    {
        if (_isReloading)
        {
            _reloadingTimer -= Time.deltaTime;
            if (_reloadingTimer <= 0)
            {
                FinishReload();
            }
            return;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            StartReload();
        }

        if (_isOnFirerateCooldown)
        {
            _firerateTimer -= Time.deltaTime;
            return;
        }

        if (Input.GetMouseButton(0) && _hasAmmo)
        {
            Shoot();
        }
    }

    public void AnimateTopModel()
    {
        if (_isReloading)
        {
            float lerpRaw = _reloadingTimer / _reloadTime;
            float lerpAmount = Mathf.PingPong(lerpRaw * 2f, 1f); 
            
            // Animate Position
            Vector3 basePosition = Vector3.zero;
            Vector3 reloadPosition = Vector3.down * 0.7f + Vector3.back * 0.5f;

            _reloadAnimator.localPosition = Vector3.Lerp(basePosition, reloadPosition, lerpAmount);

            // Animate Rotation
            Quaternion baseRotation = Quaternion.identity;
            Quaternion reloadRotation = Quaternion.Euler(70, 0, 0);

            _reloadAnimator.localRotation = Quaternion.Slerp(baseRotation, reloadRotation, lerpAmount);
        }

        if (_isOnFirerateCooldown)
        {
            Vector3 basePosition = Vector3.zero;
            Vector3 recoilPosition = Vector3.back * _bulletForce * 0.001f;

            float lerpAmount = _firerateTimer / _firerate;

            _recoilAnimator.localPosition = Vector3.Lerp(basePosition, recoilPosition, lerpAmount);
        }

    }

    #endregion


    private void Shoot()
    {
        _firerateTimer = _firerate;
        _ammo -= 1;

        // Determine spawn pos and random roation
        Vector3 spawnPosition = _bulletSpawnLocation.position;
        Quaternion spawnRotation = Random.rotation;

        // Determine what direction the bullet should fly
        Vector3 forceDirection = _bulletSpawnLocation.forward;

        // Add innacuracy
        forceDirection += _bulletSpawnLocation.up * Random.Range(-_inaccuracy, _inaccuracy);
        forceDirection += _bulletSpawnLocation.right * Random.Range(-_inaccuracy, _inaccuracy);

        Vector3 spawnForce = forceDirection * _bulletForce;
        
        // Spawn bullet 
        Bullet bullet = BulletObjectPool.RequestBulletFromPool<Bullet>();
        bullet.Init(spawnPosition, spawnRotation, spawnForce);
    }

    private void StartReload()
    {
        _reloadingTimer = _reloadTime;

        Debug.Log("Started Reloading");
    }

    private void FinishReload()
    {
        _ammo = _magazineSize;
        Debug.Log("Reloaded");
    }
    
    

}
