using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TargetCam : MonoBehaviour {

    public GameObject[] cams;

    public AudioSource audio_intro;

    public Text tt;
    public string[] textos;
    public float[] texto_time;
    public static bool startGame;

    // Use this for initialization
    void Start () {
        // TargetCam.startGame = true;
        audio_intro.volume *= Globals.SOUND_GENERAL_SLIDER;
    }
    private void Update()
    {
        if (TargetCam.startGame)
        {
            TargetCam.startGame = false;
            StartCoroutine(BeginIntro(texto_time[0], texto_time[1]));
        }

        if (Input.GetKeyDown(KeyCode.Alpha0))
            SceneManager.LoadScene("contrucao_level");
    }

    IEnumerator BeginIntro(float A, float B)
    {
        yield return new WaitForSeconds(0.1f);
        audio_intro.Play();
        tt.text = textos[0];
        yield return new WaitForSeconds(A);
        tt.text = textos[1];
        yield return new WaitForSeconds(B);
        tt.text = null;

    }

}
