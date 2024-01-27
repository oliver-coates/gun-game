using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunDebugger : MonoBehaviour
{
    public Gun gun;

    [Header("Prefabs:")]
    public GameObject extendedBarrel;
    public GameObject doubleBarrel;

    public void AddAttachment(GameObject attachmentObj)
    {
        Attachment attachment = Instantiate(attachmentObj).GetComponent<Attachment>();
        gun.AddAttachment(attachment);
    }

    public void AddExtendedBarrel()
    {
        AddAttachment(extendedBarrel);
    }

    public void AddDoubleBarrel()
    {
        AddAttachment(doubleBarrel);
    }
}
