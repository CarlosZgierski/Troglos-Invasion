using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalInteractiveObject : MonoBehaviour {

    private Rigidbody rb;

    private void Start()
    {
        rb = this.GetComponent<Rigidbody>();
    }

    public void TurnRbOn()
    {
        rb.isKinematic = false;
        this.GetComponent<CapsuleCollider>().enabled = true; //Change this depending on the type of collider
    }

    public bool TurnRbOff()
    {
        rb.isKinematic = true;
        this.GetComponent<CapsuleCollider>().enabled = false; //Change this depending on the type of collider
        return true;
    }

}
