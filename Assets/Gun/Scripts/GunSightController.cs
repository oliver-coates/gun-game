using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunSightController : MonoBehaviour
{
    public bool isAiming;

    [Header("References:")]
    [SerializeField] private Transform _gunTransform;

    [Header("Base Transform:")]
    [SerializeField] private Vector3 _baseGunPosition;
    [SerializeField] private Quaternion _baseGunRotation;

    [Header("Sight transform")]
    [SerializeField] private Vector3 _sightPosition;
    [SerializeField] private Quaternion _sightRotation;

    [Header("Gun Sights:")]
    [SerializeField] private List<Transform> _gunSights;
    
    [SerializeField] private float _aimTime = 0.6f;
    private float _aimingTimer = 0f;

    private void Awake()
    {
        GetRandomSight();
    }

    private void GetRandomSight()
    {
        Transform sightTransform = _gunSights[Random.Range(0, _gunSights.Count)]; 
        
        _sightPosition = transform.position - sightTransform.position;
        _sightRotation = sightTransform.rotation;
    }

    public void AddSight(Transform newSight)
    {
        _gunSights.Add(newSight);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1) && _aimingTimer < 0.05f)
        {
            GetRandomSight();
        }

        if (isAiming)
        {
            _aimingTimer += Time.deltaTime;
        }
        else
        {
            _aimingTimer -= Time.deltaTime;
        }

        _aimingTimer = Mathf.Clamp(_aimingTimer, 0, _aimTime);
        float lerpAmount = _aimingTimer / _aimTime;

        _gunTransform.position = Vector3.Lerp(_baseGunPosition, _sightPosition, lerpAmount);
        //_gunTransform.rotation = Quaternion.Euler(Vector3.Lerp(_baseGunRotation.eulerAngles, _sightRotation.eulerAngles, lerpAmount));
        _gunTransform.rotation = Quaternion.Slerp(Quaternion.identity, _sightRotation, lerpAmount);
    }


}
