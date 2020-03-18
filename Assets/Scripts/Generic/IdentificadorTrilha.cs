using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdentificadorTrilha : MonoBehaviour {

    public bool entrou;
    public bool adentrou;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(Globals.TAG_BOSS))
        {
            entrou = true;
            if(adentrou)
            {
                other.GetComponent<Boss_AI>().MudarVel(1.6f);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag(Globals.TAG_BOSS))
        {
            entrou = false;
            if (adentrou)
            {
                other.GetComponent<Boss_AI>().MudarVel(2.5f);
            }

        }
    }
}
