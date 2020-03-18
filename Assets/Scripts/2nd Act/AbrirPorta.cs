using UnityEngine;

public class AbrirPorta : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Globals.TAG_PORTA))
        {
            other.GetComponent<Animator>().SetBool("Abrir", true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(Globals.TAG_PORTA))
        {
            other.GetComponent<Animator>().SetBool("Abrir", false);
        }
    }
}
