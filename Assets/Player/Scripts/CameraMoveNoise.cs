using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMoveNoise : MonoBehaviour
{
    private Vector3 startPosition;
    private Quaternion startRotation;
    private float elapsedTime = 0;

    [Range(1f,10f)]
    public float swayAmount = 2f;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.localPosition;
        startRotation = transform.localRotation;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        elapsedTime += Time.deltaTime;
        float n1 = (Mathf.PerlinNoise(elapsedTime,0.2f) -0.5f) * swayAmount;
        float n2 = (Mathf.PerlinNoise(elapsedTime, 0.3f) - 0.5f) * swayAmount;
        float n3 = (Mathf.PerlinNoise(elapsedTime, 0.4f) - 0.5f) * swayAmount;
        transform.localRotation = Quaternion.Euler(n1, n2, n3);
        transform.localPosition = startPosition + new Vector3(n1, n2, n3)/100f;
    }
}
