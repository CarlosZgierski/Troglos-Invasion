using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionZone : MonoBehaviour {

    private GameObject objectInTheZone;
    private bool canInteract = false;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Globals.TAG_INTERACTABLE_OBJECT) || other.CompareTag(Globals.TAG_MAIN_OBJECT) || other.CompareTag(Globals.TAG_SACO_FALSO))
        {
            objectInTheZone = other.gameObject;
            canInteract = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(Globals.TAG_INTERACTABLE_OBJECT) || other.CompareTag(Globals.TAG_MAIN_OBJECT) || other.CompareTag(Globals.TAG_SACO_FALSO))
        {
            objectInTheZone = null;
            canInteract = false;
        }
    }


    //return the game Object that is inside the interaction zone
    public GameObject InteractionCollisionGameObject()
    {
        return objectInTheZone;
    }

    //return the Bool that sees if the player can interact or not
    public bool InteractionCollisionBool()
    {
        return canInteract;
    }

    //Nulls after throw
    public void DeleteReferences()
    {
        objectInTheZone = null;
        canInteract = false;
    }
}
