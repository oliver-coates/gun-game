using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupScript : MonoBehaviour
{
    //list of potential attachements it could be
    public GameObject[] attachments;

    private GameObject attachmentPrefab;
    public Transform pivot;

    private void Start()
    {
        var i = UnityEngine.Random.Range(0, attachments.Length);
        Debug.Log($"Adding attachemtn {attachments[i].name}");
        attachmentPrefab = Instantiate(attachments[i],pivot);
        attachmentPrefab.transform.localPosition = Vector3.zero;
    }




    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("Got the player");
            //tell the player they have picked up the pickup
            Gun gun = other.transform.parent.GetComponentInChildren<Gun>();
            if(gun)
            {
                //var i = UnityEngine.Random.Range(0, attachments.Length);
                //Debug.Log($"Adding attachemtn {attachments[i].name}");
                //var attachmentPrefab = Instantiate(attachments[i]);
                gun.AddAttachment(attachmentPrefab.GetComponent<Attachment>());
            }
            else
            {
                Debug.Log("Couldnt find the Gun ");
            }
            //then destroy the gameobject
            Destroy(gameObject);
        }
    }
}
