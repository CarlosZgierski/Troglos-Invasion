using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGame : MonoBehaviour {

    public GameObject end;

    //sons
    private AudioSource som;

    public AudioClip[] partes;


    // Use this for initialization
    void Start () {
        som = GetComponent<AudioSource>();
        som.volume *= Globals.SOUND_GENERAL_SLIDER;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && Globals.SACO_NA_MAO)
        {
            Globals.FIM_DE_JOGO = true;
            StartCoroutine(WinWin());
        }
    }

    IEnumerator WinWin()
    {
        int rand = Random.Range(0, partes.Length);
        som.clip = partes[rand];
        som.Play();
        // som.Play();
        Menu.startMenu = false;
        end.SetActive(true);
        yield return new WaitForSecondsRealtime(partes[rand].length + 1);
        SceneManager.LoadScene("menu");
    }
}
