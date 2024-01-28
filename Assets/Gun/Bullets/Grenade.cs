using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    public GameObject _explosionPrefab;


    public float explosionRadius = 1.75f;
    public float minDamage = 80f;
    public float maxDamage = 200f;
    private bool hasExploded = false;

    private void OnCollisionEnter(Collision other)
    {
        if (!hasExploded)
        {
            hasExploded = true;
            StartCoroutine(CExplode());

        }
    }

    private IEnumerator CExplode()
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

        GameObject explosion = Instantiate(_explosionPrefab, transform.position, Random.rotation);

        yield return new WaitForSeconds(2f);

        Destroy(explosion);
        Destroy(gameObject);
    }
}
