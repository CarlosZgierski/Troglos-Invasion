using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MovingCamera : MonoBehaviour {

    public Animator anim;
    public GameObject livro;
    AsyncOperation asyncLoad;

    public GameObject _loading;
    public Slider timePercent;
    public Text progressText;
    float progress;
    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public void Missao()
    {
        anim.SetTrigger("Missao");
    }

    public void Opcao()
    {
        anim.SetTrigger("Opcoes");
        StartCoroutine(HudOpcao(6.5f, true));
    }

    public void SairMissao()
    {
        anim.SetTrigger("Menu1");
    }

    public void SairOpcao()
    {
        anim.SetTrigger("Menu2");
        livro.SetActive(false);
    }

    IEnumerator HudOpcao(float n, bool b)
    {
        yield return new WaitForSecondsRealtime(n);
        livro.SetActive(b);
    }

    public void StartGame()
    {
        StartCoroutine(LoadYourAsyncScene());
    }

    IEnumerator LoadYourAsyncScene()
    {
        anim.SetTrigger("Mesa");
        yield return new WaitForSecondsRealtime(3f);

        // The Application loads the Scene in the background as the current Scene runs.
        // This is particularly good for creating loading screens.
        // You could also load the Scene by using sceneBuildIndex. In this case Scene2 has
        // a sceneBuildIndex of 1 as shown in Build Settings.
        _loading.SetActive(true);
        asyncLoad = SceneManager.LoadSceneAsync("contrucao_Level");
        //  loading.SetActive(true);


        // Wait until the asynchronous scene fully loads
        while (asyncLoad.isDone == false)
        {
            Debug.Log("AAAAAA " + asyncLoad.progress);
            float progress = Mathf.Clamp01(asyncLoad.progress / .9f);
            timePercent.value = progress;
            progressText.text = Mathf.RoundToInt(progress * 100f) + "%";
            yield return null;
        }
    }


    public void ChangeValue(Slider s)
    {
        Globals.SOUND_GENERAL_SLIDER = s.value;
    }

    public void ChangeSubtitle(Text t)
    {
        if (Globals.SUBS_ON)
        {
            t.text = "OFF";
            Globals.SUBS_ON = false;
        }
        else
        {
            t.text = "ON";
            Globals.SUBS_ON = true;
        }
    }
}
