using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunDebugger : MonoBehaviour
{
    public Gun gun;

    [Header("Prefabs:")]
    public GameObject extendedBarrel;
    public GameObject doubleBarrel;
    public GameObject loudener;
    public GameObject silencer;

    public GameObject redDot;
    public GameObject Acog;
    public GameObject opticalScope;

    public GameObject laser;
    public GameObject flashlight;
    public GameObject grenadeLauncher;
    public GameObject mirror;

    public GameObject standardMag;
    public GameObject largeMag;
    public GameObject drumMag;

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

    public void AddLoudener()
    {
        AddAttachment(loudener);
    }

    public void AddSilencer()
    {
        AddAttachment(silencer);
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

    public void AddLaser()
    {
        AddAttachment(laser);
    }

    public void AddGrenadeLauncher()
    {
        AddAttachment(grenadeLauncher);
    }

    public void AddWingMirror()
    {
        AddAttachment(mirror);
    }

    public void AddFlashlight()
    {
        AddAttachment(flashlight);
    }


    public void AddMag()
    {
        AddAttachment(standardMag);
    }

    public void AddLargeMag()
    {
        AddAttachment(largeMag);
    }

    public void AddDrumMag()
    {
        AddAttachment(drumMag);
    }
}
