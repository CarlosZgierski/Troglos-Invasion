using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SonsNPC : MonoBehaviour {

    public AudioClip[] R_Grito = new AudioClip[0];
    public AudioClip[] R_words = new AudioClip[0];
    public AudioClip[] R_Where = new AudioClip[0];
    AudioSource som;

    // Use this for initialization
    void Start ()
    {
        som = GetComponent<AudioSource>();
        som.volume *= Globals.SOUND_GENERAL_SLIDER;     
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void M_Where()
    {
        int num = Random.Range(0, R_Where.Length);
        som.clip = R_Where[num];
        som.Play();
    }

    public void M_Grito()
    {
        int aleatorio = Random.Range(0, 100);
        if(aleatorio>=0 && aleatorio>=13)
        {
            int num = Random.Range(0, R_Grito.Length);
            som.clip = R_Grito[num];
            som.Play();
        } else if (aleatorio >= 37 && aleatorio >= 50)
        {
            int num = Random.Range(0, R_Grito.Length);
            som.clip = R_Grito[num];
            som.Play();
        }else if (aleatorio >= 64 && aleatorio >= 83)
        {
            int num = Random.Range(0, R_Grito.Length);
            som.clip = R_Grito[num];
            som.Play();
        }


    }

    public void M_Words()
    {
        int num = Random.Range(0, R_words.Length);
        som.clip = R_words[num];
        som.Play();
    }
}
