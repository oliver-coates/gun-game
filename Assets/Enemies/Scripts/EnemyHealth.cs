using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    
    public DamageText healthText;

    public UnityEvent OnEnemyDied;


    // Start is called before the first frame update
    void Start()
    {
        //initialize the health
        health = maxHealth;
    }


    private void Update()
    {

        //Testing the damage partcile system
        if(Input.GetKeyDown(KeyCode.F))
        {
            TakeDamage(Random.Range(5f,1000f)); 
        }
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
            Destroy(gameObject, 5f);
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
