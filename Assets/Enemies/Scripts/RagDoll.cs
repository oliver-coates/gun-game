using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagDoll : MonoBehaviour
{
    bool enabled = false;
    Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        
    }


    public void EnableRagdoll()
    {
        enabled = true;
        animator.enabled = false;
        AddExplosion();
    }


    public void AddExplosion()
    {
        var rbs = gameObject.GetComponentsInChildren<Rigidbody>();
        foreach (var rb in rbs)
        {
            rb.AddExplosionForce(25f, transform.position, 10f, 10f, ForceMode.Impulse);
        }
    }
}
