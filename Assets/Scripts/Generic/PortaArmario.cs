using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortaArmario : MonoBehaviour
{
    Animator anim;

	// Use this for initialization
	void Start ()
    {
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void Abrir()
    {
        StartCoroutine("AbreFecha");
    }

    IEnumerator AbreFecha()
    {
        anim.SetBool("Abrir", true);
        yield return new WaitForSeconds(1f);
        anim.SetBool("Abrir", false);
        StopCoroutine("AbreFecha");
    }
}
