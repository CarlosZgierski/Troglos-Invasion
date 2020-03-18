using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotSacoPause : MonoBehaviour {

    public GameObject[] sacoIndex;
    public GameObject[] visaoIndex;

	// Use this for initialization
	void Update () {
        SpotMe();
	}

    void SpotMe()
    {
        if (Globals.SPOT_SACO_104)
        {
            visaoIndex[0].SetActive(false);
            sacoIndex[0].SetActive(true);
        }
        if (Globals.SPOT_SACO_COZINHA)
        {
            visaoIndex[1].SetActive(false);
            sacoIndex[1].SetActive(true);
        }
        if (Globals.SPOT_SACO_RECEPCAO)
        {
            visaoIndex[2].SetActive(false);
            sacoIndex[2].SetActive(true);
        }
        if (Globals.SPOT_SACO_108)
        {
            visaoIndex[0].SetActive(false);
            sacoIndex[3].SetActive(true);
        }
        if (Globals.SPOT_SACO_LAJE)
        {
            visaoIndex[4].SetActive(false);
            sacoIndex[4].SetActive(true);

        }
        if (Globals.SPOT_SACO_301)
        {
            visaoIndex[5].SetActive(false);
            sacoIndex[5].SetActive(true);
        }
        if (Globals.SPOT_SACO_304)
        {
            visaoIndex[6].SetActive(false);
            sacoIndex[6].SetActive(true);
        }
    }
}
