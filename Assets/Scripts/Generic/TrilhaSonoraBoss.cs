using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrilhaSonoraBoss : MonoBehaviour {

    public AudioSource trilha1;
    public AudioSource trilha2;
    public AudioSource trilha3;

    public IdentificadorTrilha col_trilha1;
    public IdentificadorTrilha col_trilha2;
    public IdentificadorTrilha col_trilha3;

    // Use this for initialization
    void Start ()
    {

    }
	
	// Update is called once per frame
	void Update ()
    {
        if (col_trilha1.entrou)
        {
            if(trilha1.volume<=0.2f)
            {
                trilha1.volume += 0.2f*Time.deltaTime * Globals.SOUND_GENERAL_SLIDER;
            }            
        }else
        {
            if (trilha1.volume > 0f)
            {
                trilha1.volume -= 0.4f * Time.deltaTime * Globals.SOUND_GENERAL_SLIDER;
            }
        }

        if (col_trilha2.entrou && Boss_AI.State == Globals.ACT_RUN)
        {
            if (trilha2.volume <= 0.2f)
            {
                trilha2.volume += 0.3f * Time.deltaTime * Globals.SOUND_GENERAL_SLIDER;
            }
        }
        else
        {
            if (trilha2.volume > 0f)
            {
                trilha2.volume -= 0.4f * Time.deltaTime * Globals.SOUND_GENERAL_SLIDER;
            }
        }

        if (col_trilha3.entrou && Boss_AI.State == Globals.ACT_RUN)
        {
            if (trilha3.volume <= 0.2f)
            {
                trilha3.volume += 0.3f * Time.deltaTime * Globals.SOUND_GENERAL_SLIDER;
            }
        }
        else
        {
            if (trilha3.volume > 0f)
            {
                trilha3.volume -= 0.4f * Time.deltaTime * Globals.SOUND_GENERAL_SLIDER;
            }
        }
    }
}
