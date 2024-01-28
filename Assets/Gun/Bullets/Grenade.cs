using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    public float explosionRadius = 1.75f;
    public float minDamage = 80f;
    public float maxDamage = 200f;

    private void OnCollisionEnter(Collision other)
    {
        
        Collider[] hits = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider hit in hits)
        {
            EnemyHealth enemyHealth = GetComponent<EnemyHealth>();

            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(Random.Range(minDamage, maxDamage));
            }
        }

        Destroy(gameObject);

    }
}
