using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptSound : MonoBehaviour {

    public AudioSource mesa;
    public AudioClip hide;
    public AudioClip jump;
    public AudioClip sacoPegavel;
    public AudioClip objetoPegavel;
    private AudioSource som; 

    void Start()
    {
        som = GetComponent<AudioSource>();
        som.volume *= Globals.SOUND_FX_SLIDER;
        mesa.volume *= Globals.SOUND_FX_SLIDER;
    }

    public void PlayMesa(bool iniciar)
    {
        if(iniciar)
        {
            mesa.Play();
        } else
        {
            mesa.Stop();
        }
    }

    public void VolumeMesa(int vol)
    {
        mesa.volume = vol * Globals.SOUND_FX_SLIDER;
    }

    public void PlayHide()
    {
        som.clip = hide;
        som.Play();
    }

    public void PlayJump()
    {
        som.clip = jump;
        som.Play();
    }

    public void PlaySaco()
    {
        som.clip = sacoPegavel;
        som.Play();
    }

    public void PlayObjeto()
    {
        som.clip = objetoPegavel;
        som.Play();
    }
}