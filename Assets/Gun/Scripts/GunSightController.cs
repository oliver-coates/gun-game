using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunSightController : MonoBehaviour
{
    public bool isAiming;

    [Header("References:")]
    [SerializeField] private Transform _gunBaseTransform;
    [SerializeField] private Transform _gunSightReflector;
    private Transform selectedSightTransform;
    private Transform selectedSightOldParent;

 

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
        _baseGunPosition = _gunBaseTransform.localPosition;

        if (_gunSights == null)
        {
            _gunSights = new List<Transform>();
        }

        GetRandomSight();
    }

    private void GetRandomSight()
    {
        if (_gunSights.Count == 0)
        {
            _sightPosition = _baseGunPosition;
            _sightRotation = _baseGunRotation;
            return;
        }
        selectedSightTransform = _gunSights[Random.Range(0, _gunSights.Count)]; 

        _gunSightReflector.transform.localPosition = Vector3.zero;
        _gunSightReflector.transform.localPosition = -_gunBaseTransform.InverseTransformPoint(selectedSightTransform.position);

        _gunSightReflector.transform.localRotation = Quaternion.identity;

        Vector3 forward = _gunBaseTransform.InverseTransformDirection(selectedSightTransform.forward);
        Vector3 up = _gunBaseTransform.InverseTransformDirection(selectedSightTransform.up);
        _gunSightReflector.transform.localRotation = Quaternion.LookRotation(forward, up);
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

        _gunBaseTransform.localPosition = _sightPosition;

        //_gunBaseTransform.localPosition = Vector3.Lerp(_baseGunPosition, Vector3.zero, lerpAmount);
        //old: _gunTransform.rotation = Quaternion.Euler(Vector3.Lerp(_baseGunRotation.eulerAngles, _sightRotation.eulerAngles, lerpAmount));
        //_gunBaseTransform.localRotation = Quaternion.Slerp(Quaternion.identity, _sightRotation, lerpAmount);
    }


}
