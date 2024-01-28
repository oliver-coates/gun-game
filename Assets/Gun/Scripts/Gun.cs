using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gun : MonoBehaviour
{
    public event Action onShoot;
    
    [Header("References:")]
    [SerializeField] private GunSightController _gunSightController;
    [SerializeField] private Transform _reloadAnimator;
    [SerializeField] private Transform _recoilAnimator;
    [SerializeField] private Transform _sprintAnimator;
    [SerializeField] private PlayerMovement _playerMovement;
    public AudioSource _audioSource;
    public Image _ammoImage;

    #region Stats
    [Header("Base Stats:")]
    private readonly float _baseFireRate = 0.2f;
    private readonly float _baseDamage = 10f;
    private readonly float _baseReloadTime = 1.5f;
    private readonly float _baseGunSound = 1;
    private readonly float _baseBulletForce = 100;
    private readonly float _baseAccuracy = 0.5f;
    private readonly float _baseBulletSpawnRandomRot = 0f;

    [Header("Actual Stats (read only):")]
    [SerializeField] private float _firerate;
    [SerializeField] private float _damage;
    [SerializeField] private float _gunSound;
    [SerializeField] private float _reloadTime;
    [SerializeField] private float _bulletForce;
    [SerializeField] private float _accuracy;
    [SerializeField] private float _bulletSpawnRandomRot;

    [Header("Magazine:")]
    [SerializeField] private float _magazineSize = 25;
    [SerializeField] private float _ammo;

    [Header("Tuners:")]
    [SerializeField] private float _accuracyTuner;
    #endregion
    
    #region Internal State:
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
    
    private float _sprintTimer;
    private float _sprintRotationTimer;
    #endregion

    #region Slots
    [SerializeField] private List<Attachment> _allAttachments;

    [SerializeField] private List<Transform> _barrelSockets;
    [SerializeField] private List<Transform> _underbarrelSockets;
    [SerializeField] private List<Transform> _sightSockets;
    [SerializeField] private List<Transform> _magazineSockets;

    #endregion

    #region Audio

    [SerializeField] private List<AudioClip> _shootSounds;
    [SerializeField] private AudioClip _reloadSound;

    #endregion

    #region Initialisation

    private void Awake()
    {
        // Start with full ammo
        _ammo = _magazineSize;
        _reloadingTimer = -1f;
        _firerateTimer = -1f;

        UpdateStats();
    }

    #endregion


    #region Input
    private void Update()
    {    
        ListenForInput();
        AnimateTopModel();

        _ammoImage.fillAmount = (_ammo / _magazineSize);
    }

    private void ListenForInput()
    {
        if (_isReloading)
        {
            _reloadingTimer -= Time.deltaTime;
            _gunSightController.isAiming = false;

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

        _gunSightController.isAiming = Input.GetMouseButton(1);
    }

    #endregion


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

        UpdateSprinting();
    }

    private void UpdateSprinting()
    {
        _sprintTimer = Mathf.Clamp(_sprintTimer, 0f, 4f);
        _sprintRotationTimer = Mathf.Clamp(_sprintRotationTimer, 0f, 1f);

        if (_sprintTimer == 4f)
        {
            _sprintTimer = 2f;
        }

        Vector3 basePosition = Vector3.zero;
        Vector3 sprintPosition = new Vector3(-0.145f, -0.02f, -0.07f);
        Quaternion sprintRotation = Quaternion.Euler(20f, -41f, 3.15f);

        float sprintLerpAmount = 0f;

        if (_playerMovement.isSprinting)
        {
            _sprintTimer += Time.deltaTime;

            sprintLerpAmount = Mathf.PingPong(_sprintTimer, 0.2f);
            _sprintRotationTimer += Time.deltaTime * 4f;
        }
        else
        {
            _sprintTimer -= Time.deltaTime;

            sprintLerpAmount -= Time.deltaTime * 0.5f;
            _sprintRotationTimer -= Time.deltaTime * 4f;
        }

        

    
        _sprintAnimator.transform.localPosition = Vector3.Lerp(basePosition, sprintPosition, sprintLerpAmount);
        _sprintAnimator.transform.localRotation = Quaternion.Slerp(Quaternion.identity, sprintRotation, _sprintRotationTimer);
    }


    private void UpdateStats()
    {
        _firerate = _baseFireRate;
        _damage = _baseDamage;
        _gunSound = _baseGunSound;
        _accuracy = _baseAccuracy;
        _magazineSize = 0;
        _reloadTime = _baseReloadTime;
        _bulletForce = _baseBulletForce;
        _bulletSpawnRandomRot = _baseBulletSpawnRandomRot;

        foreach(Attachment attachment in _allAttachments)
        {
            _firerate = _firerate * attachment.firerateMultiplier;
            _damage = _damage * attachment.damageMultiplier;
            _gunSound = _gunSound * attachment.soundMultiplier;
            _accuracy = _accuracy * attachment.accuracyMultiplier;
            _magazineSize += attachment.magSize;

            _bulletSpawnRandomRot += attachment.bulletRandomRotation;
            _bulletForce = _bulletForce * attachment.forceMultiplier;
            
        }
    }

    private void Shoot()
    {
        onShoot?.Invoke();
        
        _firerateTimer = _firerate;
        foreach (Transform bulletSpawnLocation in _barrelSockets)
        {
            _ammo -= 1;

            // Determine spawn pos and random roation
            Vector3 spawnPosition = bulletSpawnLocation.position;
            
            Quaternion forwardRotation = Quaternion.LookRotation(bulletSpawnLocation.forward);
            Quaternion spawnRotation = Quaternion.Lerp(forwardRotation, UnityEngine.Random.rotation, _bulletSpawnRandomRot);

            // Determine what direction the bullet should fly
            Vector3 forceDirection = bulletSpawnLocation.forward;

            // Add innacuracy
            float inaccuracy = (1 - _accuracy) * _accuracyTuner;
            forceDirection += bulletSpawnLocation.up * UnityEngine.Random.Range(-inaccuracy, inaccuracy);
            forceDirection += bulletSpawnLocation.right * UnityEngine.Random.Range(-inaccuracy, inaccuracy);

            Vector3 spawnForce = forceDirection * _bulletForce;
            
            // Spawn bullet 
            Bullet bullet = BulletObjectPool.RequestBulletFromPool<Bullet>();
            bullet.Init(spawnPosition, spawnRotation, spawnForce, _damage);

            // Play random sound
            AudioClip randSound = _shootSounds[UnityEngine.Random.Range(0, _shootSounds.Count)];
            _audioSource.PlayOneShot(randSound, _gunSound);
        }
        
    }

    private void StartReload()
    {
        _reloadingTimer = _reloadTime;

        _audioSource.PlayOneShot(_reloadSound);
    }

    private void FinishReload()
    {
        _ammo = _magazineSize;
    }
    
    public void AddNewSight(Transform newSight)
    {
        _gunSightController.AddSight(newSight);
    }

    private List<Transform> GetSocketListFromAttachmentType(Attachment.AttachmentType type)
    {        
        switch(type)
        {
            case Attachment.AttachmentType.Barrel:
                return _barrelSockets;
            
            case Attachment.AttachmentType.Underbarrel:
                return _underbarrelSockets;
            
            case Attachment.AttachmentType.Sight:
                return _sightSockets;
            
            case Attachment.AttachmentType.Magazine:
                return _magazineSockets;
        }
        return null;
    }

    public Transform RetrieveRandomSocket(Attachment.AttachmentType type)
    {
        List<Transform> socketList = GetSocketListFromAttachmentType(type);

        Transform randomSocket = socketList[UnityEngine.Random.Range(0, socketList.Count)];
        socketList.Remove(randomSocket);

        return randomSocket;
    }

    public void AddSocketsOfType(Attachment.AttachmentType type, List<Transform> additionalSockets)
    {
        List<Transform> socketList = GetSocketListFromAttachmentType(type);

        socketList.AddRange(additionalSockets);
    }

    public void AddAttachment(Attachment attachment)
    {
        // Get a random socket:
        Transform chosenSocket = RetrieveRandomSocket(attachment.type);

        // Set position and rotation
        attachment.transform.SetParent(chosenSocket);
        attachment.transform.localPosition = Vector3.zero;
        attachment.transform.localRotation = Quaternion.identity;
        attachment.transform.localScale = Vector3.one;

        // Add the new sockets on
        AddSocketsOfType(attachment.type, attachment.sockets);
        
        _allAttachments.Add(attachment);

        foreach(Transform newSight in attachment.sights)
        {
            _gunSightController.AddSight(newSight);
        }

        UpdateStats();
    }
}
