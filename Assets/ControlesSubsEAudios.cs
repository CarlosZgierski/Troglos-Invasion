using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlesSubsEAudios : MonoBehaviour {

    public GameObject[] Subs;

    public float[] _soundFXS;
    public AudioSource[] SoundFX;

    public float[] _soundGeneralS;
    public AudioSource[] SoundGeneral;

	// Use this for initialization
	void Start () {
        GetInicialVolume();
    }
	
    void GetInicialVolume()
    {
        for (int x = 0; x < SoundFX.Length; x++)
        {
            _soundFXS[x] = SoundFX[x].volume;
        }

        for (int y = 0; y < SoundGeneral.Length; y++)
        {
            _soundGeneralS[y] = SoundGeneral[y].volume;
        }
    }

    void ChangeSubs()
    {
        if (Globals.SUBS_ON)
            return;
        foreach(GameObject t in Subs)
        {
            t.SetActive(false);
        }
    }

    void ChangeVolumes()
    {
        for (int x = 0; x < SoundFX.Length; x++)
        {
            SoundFX[x].volume = Globals.SOUND_FX_SLIDER * _soundFXS[x];
        }

        for (int y = 0; y < SoundGeneral.Length; y++)
        {
            SoundGeneral[y].volume = Globals.SOUND_GENERAL_SLIDER * _soundGeneralS[y];
        }
    }

    private void Update()
    {
        ChangeSubs();
        ChangeVolumes();
    }
}
