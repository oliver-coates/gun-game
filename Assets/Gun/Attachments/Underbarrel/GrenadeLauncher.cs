using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeLauncher : MonoBehaviour
{

    public Transform shootLocation;
    public GameObject grenadePrefab;

    public AudioSource audioSource;
    public AudioClip thumpSound;

    private Gun gun;

    [Range(0.01f, 1f)]
    public float shootChance;

    public float shootCooldown = 3f;
    public float shootForce = 5f;

    private float cooldownTimer;

    void Start()
    {
        gun = FindObjectOfType<Gun>();

        gun.onShoot += AttemptShoot;
    }

    private void Update()
    {
        cooldownTimer -= Time.deltaTime;
    }

    public void AttemptShoot()
    {
        if (cooldownTimer < 0)
        {
            float dice = Random.Range(0f, 1f);

            if (dice < shootChance)
            {
                Shoot();
            }
        }
    }

    private void Shoot()
    {
        cooldownTimer = shootCooldown;

        Rigidbody rb = Instantiate(grenadePrefab, shootLocation.position, shootLocation.rotation).GetComponent<Rigidbody>();
        rb.AddForce(shootLocation.forward * shootForce);

        audioSource.PlayOneShot(thumpSound);
    }

   
}
