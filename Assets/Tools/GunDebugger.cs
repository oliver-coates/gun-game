using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunDebugger : MonoBehaviour
{
    public Gun gun;

    [Header("Prefabs:")]
    public GameObject extendedBarrel;
    public GameObject doubleBarrel;
    public GameObject redDot;
    public GameObject Acog;
    public GameObject opticalScope;

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

    public void AddRedDot()
    {
        AddAttachment(redDot);
    }

    public void AddOpticalScope()
    {
        AddAttachment(opticalScope);
    }

    public void AddAcog()
    {
        AddAttachment(Acog);
    }
}
