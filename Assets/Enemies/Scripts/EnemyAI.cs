using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

/// <summary>
/// Basic Enemy AI that always moves to player
/// </summary>

public class EnemyAI : MonoBehaviour
{

    private Transform playerTransform;
    NavMeshAgent nav;
    public enum EnemyState { chasing, attacking};

    public EnemyState enemyState = EnemyState.chasing;
    [Range(0f,5f)]
    public float attackRange = 2f;

    [Header("Events")]
    public UnityEvent OnEnemyAttacking;



    // Start is called before the first frame update
    void Awake()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        if (playerTransform == null)
        {
            Debug.LogWarning("No Object with tag player on scene");
        }

        nav = GetComponent<NavMeshAgent>();
        if(nav == null)
        {
            Debug.LogWarning("No Nav Mesh Agent");
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var distance = Vector3.Distance(playerTransform.position, transform.position);
        if (enemyState == EnemyState.chasing)
        {
            //move to player
            nav.SetDestination(playerTransform.position);
            if(distance < attackRange)
            {
                enemyState = EnemyState.attacking;
                //invoke an action?
                OnEnemyAttacking.Invoke();
                var health = playerTransform.GetComponent<PlayerHealth>();
                if (health)
                {
                    Debug.Log($"{gameObject.name} is Damaging the Player");
                    health.TakeDamage(10f);
                }
            }
        }
        else
        {
            //simple two states for now
            if(distance >= 1f)
            {
                enemyState = EnemyState.chasing;
            }
        }

    }
}
