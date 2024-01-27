using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Spawn alet values and animate a damage text TextMeshPro component
/// </summary>


public class DamageText : MonoBehaviour
{

    public TMP_Text damageText;

    // Start is called before the first frame update
    void Start()
    {
        //delete after a certain length of time
        Destroy(gameObject, 1f);
    }


    public void SetValue(float damageValue)
    {
        damageText.text = (-(int)damageValue).ToString();
    }


}
