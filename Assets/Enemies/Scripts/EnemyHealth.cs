using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

/// <summary>
/// Basic Health System for Enemy
/// </summary>
public class EnemyHealth : MonoBehaviour
{

    [SerializeField]
    private float maxHealth = 100f;
    private float health;
    private bool canDie = true;

    [Tooltip("The prefab we will spawn when we die")]
    public Transform pickUpPrefab;
    [Range(0,100f)]
    public float percentChangeOfDrop = 100f;
    
    public DamageText healthText;

    public UnityEvent OnEnemyDied;


    // Start is called before the first frame update
    void Start()
    {
        //initialize the health
        health = maxHealth;
        if(pickUpPrefab == null)
        {
            Debug.LogError("You forgot the pickup prefab!!");
        }
    }


    private void Update()
    {

    }

    public void Init(float difficulty)
    {
        Vector3 baseSize = Vector3.one * 0.65f;
        Vector3 size = baseSize + (Vector3.one * difficulty);

        transform.localScale = size;
        health = difficulty * 100f;

        NavMeshAgent agent = GetComponent<NavMeshAgent>(); 

        agent.speed = agent.speed + (agent.speed * difficulty / 2);
    }


    public void TakeDamage(float damage)
    {
        health -= damage;
        //spawn the damage text
        Vector3 offset = new Vector3(Random.Range(-1f, 1f), Random.Range(1f, 2f), Random.Range(-1f, 1f));
        var newDamageText = Instantiate(healthText, transform.position + offset, transform.rotation);
        newDamageText.SetValue(damage);


        if(health < 0 && canDie)
        {
            //we are dead
            Debug.Log("We are dead");
            OnEnemyDied.Invoke();
            canDie = false;
            DropPickUp();
            Destroy(gameObject, 5f);
            
        }
    }

    private void DropPickUp()
    {
        float chance = UnityEngine.Random.Range(0f, 100f);
        if (chance < percentChangeOfDrop)
        {
            //we drop one
            Instantiate(pickUpPrefab, transform.position, transform.rotation);
        }
    }


    //detect the collisions with bullets


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Bullet")
        {
            Debug.Log("Ouch");
            var bullet = other.GetComponent<Bullet>();
            if(bullet)
            {
                TakeDamage((float)bullet.damage);
            }
        }
    }


}
