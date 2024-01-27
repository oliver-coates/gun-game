using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attachment : MonoBehaviour
{
    public enum AttachmentType
    {
        Barrel = 1,
        Underbarrel = 2,
        Sight = 3,
        Magazine = 4,
    }

    public List<Transform> sockets;
    public List<Transform> sights;
    public AttachmentType type;

    [Header("Effects:")]
    public float damageMultiplier;
    public float firerateMultiplier;
    public float soundMultiplier;
    public float accuracyMultiplier;
    public float forceMultiplier;

    public float bulletRandomRotation;

    public float magSize;


}
