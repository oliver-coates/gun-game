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

    public DamageText healthText;

    public UnityEvent OnPlayerDied;


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
            TakeDamage(Random.RandomRange(5f,100f)); 
        }
    }


    public void TakeDamage(float damage)
    {
        health -= damage;
        //spawn the damage text
        Vector3 offset = new Vector3(Random.RandomRange(-1f, 1f), Random.RandomRange(1f, 2f), Random.RandomRange(-1f, 1f));
        var newDamageText = Instantiate(healthText, transform.position + offset, transform.rotation);
        newDamageText.SetValue(damage);


        if(health < 0)
        {
            //we are dead
            Debug.Log("We are dead");
            OnPlayerDied.Invoke();
        }
    }


}
