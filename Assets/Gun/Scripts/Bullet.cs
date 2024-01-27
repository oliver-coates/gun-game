using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("References:")]
    [SerializeField] private Rigidbody _rb;

    [Header("Settings:")]
    [SerializeField] private float _lifeTime = 3f;
    private float _lifeTimeTimer;
    public float damage;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }


    // Called upon being removed from pool
    public void Init(Vector3 spawnPos, Quaternion spawnRot, Vector3 spawnForce, float damage)
    {
        _lifeTimeTimer = _lifeTime;
        
        this.damage = damage;

        transform.position = spawnPos;
        transform.rotation = spawnRot;
        
        _rb.velocity = Vector3.zero;
        _rb.AddForce(spawnForce);
    }


    private void Update()
    {
        _lifeTimeTimer -= Time.deltaTime;
        if (_lifeTimeTimer < 0)
        {
            Disable();
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        Disable();
    }

    private void Disable()
    {
        BulletObjectPool.ReturnBulletToPool(this);
    }
}

public static class BulletObjectPool
{
    private const string BULLET_BASE_FILE_PATH = "Bullets/Base";
    private static GameObject _bulletBase;

    private static bool _initialised = false;

    private static List<Bullet> _availableBullets;

    private static Transform _bulletsInUseHolder;
    private static Transform _bulletsInPoolHolder;

    private static void Init()
    {
        _initialised = true;

        _availableBullets = new List<Bullet>();

        // Load bullet
        _bulletBase = Resources.Load<GameObject>(BULLET_BASE_FILE_PATH);

        Transform _poolHolder = new GameObject("Bullet Object Pool").transform;

        _bulletsInUseHolder = new GameObject("Bullets in use").transform;
        _bulletsInPoolHolder = new GameObject("Bullets in Pool").transform;

        _bulletsInPoolHolder.SetParent(_poolHolder);
        _bulletsInUseHolder.SetParent(_poolHolder);
    }

    public static Bullet RequestBulletFromPool<T>()
    {
        if (!_initialised)
        {
            Init();
        }

        foreach (Bullet bullet in _availableBullets)
        {
            if (bullet is T)
            {
                bullet.gameObject.SetActive(true);
                _availableBullets.Remove(bullet);
                bullet.transform.SetParent(_bulletsInUseHolder);
                return bullet;
            }
        }

        // No bullet of request type found :(

        // Create bullet
        GameObject newBulletObj = GameObject.Instantiate(_bulletBase);

        // Add component
        Bullet newBullet = (Bullet) newBulletObj.AddComponent(typeof(T));
        newBullet.transform.SetParent(_bulletsInUseHolder);

        return newBullet;
    }

    public static void ReturnBulletToPool(Bullet bullet)
    {
        bullet.gameObject.SetActive(false);
        _availableBullets.Add(bullet);

        bullet.transform.SetParent(_bulletsInPoolHolder);
    }


}