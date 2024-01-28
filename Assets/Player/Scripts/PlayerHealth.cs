using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// Basic Player Health
/// Has TakeDamage function
/// </summary>
public class PlayerHealth : MonoBehaviour
{
    [SerializeField]
    float maxHealth = 100f;
    private float health = 100f;
    //set this to represent the health
    public Image healthBarImage;


    //events
    public UnityEvent OnPlayerDied;
    public UnityEvent OnPlayerHurt;


    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            TakeDamage(10f);
        }
    }

    public void TakeDamage(float damage)
    {
        OnPlayerHurt.Invoke();
        health -= damage;
        if (health <= 0)
        {
            Application.Quit();
            OnPlayerDied.Invoke();
        }
        //sdjust the helath bar
        healthBarImage.fillAmount = health / maxHealth;

    }    

}
