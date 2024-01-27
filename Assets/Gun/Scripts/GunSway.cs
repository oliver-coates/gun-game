using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunSway : MonoBehaviour
{
    [SerializeField] private Transform _cameraTransform;
    [SerializeField] private float _swaySpeed;

    private void Update()
    {
        float lerpAmount = _swaySpeed * Time.deltaTime;

        transform.position = _cameraTransform.position;
        transform.rotation = Quaternion.Slerp(transform.rotation, _cameraTransform.rotation, lerpAmount);
    }
}
