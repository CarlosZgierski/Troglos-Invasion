using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundTrap : MonoBehaviour {

    [SerializeField] private GameObject soundTrigger;

    [SerializeField] private float colliderRadious;
    [SerializeField] private float soundDuration;

    private void Start()
    {
        soundTrigger.GetComponent<SphereCollider>().radius = 0.0f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(Globals.TAG_PLAYER) && other.gameObject.name != "HeadTrigger")
        {
            //print(other.gameObject.name);
            if(!other.GetComponent<ThirdPersonUserControl>().PlayerIsCrouched())
                StartCoroutine(MakeNoise());
        }
    }

    private IEnumerator MakeNoise()
    {
        soundTrigger.GetComponent<SphereCollider>().radius = colliderRadious;

        yield return new WaitForSeconds(soundDuration);

        soundTrigger.GetComponent<SphereCollider>().radius = 0.0f;
    }
}
