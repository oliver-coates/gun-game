using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SpinAndBob : MonoBehaviour
{

    float elapsedTime = 0;
    Vector3 startPosition;


    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position; 
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;
        transform.Rotate(0, 90f * Time.deltaTime, 0);
        var yOffset = 0.5f * Mathf.Sin(5f * elapsedTime);
        transform.position = startPosition + new Vector3(0, yOffset, 0);
    }
}
