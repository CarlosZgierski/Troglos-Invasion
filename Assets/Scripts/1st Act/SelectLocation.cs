using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SelectLocation : MonoBehaviour {




    public Texture texture1;
    public Texture texture2;
    public Texture texture3;

    public GameObject Mesa;

    public Text dialogo;

    public GameObject andar1;
    public GameObject andar2;
    public GameObject andar3;
    public GameObject[] botoes;
    public GameObject[] milhos;

    public AudioClip[] falas;

    public GameObject Dir;
    public GameObject Esq;
    public GameObject OP;

    public Button[] cameras;

    private bool trava;

    private int andar = 1;

    public string camName;

    private bool intro;

    //Change Cameras
    private int camNumb;
    private bool viewingCam;
    public GameObject[] cams;
    public static int camViews = 0;
    public int maxViews;
    public GameObject canvasIntro;
    public GameObject canvasVisual;


    //Loading screen
    private bool isLoading;
    public GameObject load;
    public float time = 0;
    public float timeSpeed;
    public Slider slider;

    //Player Game Activate
    public GameObject player;
    public GameObject playerCanvas;
    public GameObject mesaMapa;
    public GameObject gameCanvas;


    //texto inicial
    public AudioSource gameplay;
    public AudioSource som;
    public Text tt;
    private int tTime;
    public string[] textos;
    private bool startI;
    private bool startG = false;
    
    // Use this for initialization
    void Awake () {
        trava = true;
        intro = true;
        startI = false;
        tTime = -1;
        StartCoroutine(BeginIntro());
        viewingCam = false;
        Menu.startMenu = true;
        som.volume *= Globals.SOUND_GENERAL_SLIDER;
	}

    // Update is called once per frame
    void Update()
    {
        if (!Menu.startMenu)
        {
            StopCoroutine(BeginIntro());
            som.Stop();
            tt.text = null;
            tt.gameObject.SetActive(false);
        }

        if (isLoading)
        {
            load.SetActive(true);
            time += timeSpeed;
            slider.value = time;
        }
        

        if (Input.GetKeyDown(KeyCode.Space))
        {
            StopAllCoroutines();
            tt.text = null;
            intro = false;
            startI = false;
            tt.text = null;
            trava = false;
            som.Stop();
        }

        //if (SelectLocation.camViews == maxViews)
        //{
        //    intro = true;
        //    OP.SetActive(true);
        //    foreach (Button cam in cameras)
        //    {
        //        cam.interactable = false;
        //    }

        //}
        if(!intro)
        {
            if (trava)
            {
                if (Input.GetKey(KeyCode.Y))
                {
                    Sim();
                }
                if (Input.GetKey(KeyCode.N))
                {
                    Nao();
                }
            }
        }
        else
        {
            if (tTime > 2)
            {
                intro = false;
                startI = false;
                tt.text = null;
                trava = false;
            }
        }

        if (viewingCam && Input.GetMouseButtonDown(1))
        {
            StopAllCoroutines();
            EndCamView();
        }
    }

    public void QuerComecar()
    {
        if (!trava)
        {
            som.Stop();
            this.StopAllCoroutines();
            tt.text = null;
            dialogo.text = "Você tem certeza que achou todos os possíveis sacos?";
            startG = true;

            foreach (GameObject bt in botoes)
            {
                bt.SetActive(true);
            }

            foreach (Button cam in cameras)
            {
                cam.interactable = false;
            }
        }
    }

    public void ComeçarOP()
    {
        StopCoroutine(BeginIntro());
        som.Stop();
        dialogo.text = null;
       // tt.gameObject.SetActive(false);
        StartCoroutine(BeginGame());
    }



    #region Cams related stuff

    IEnumerator BotaoSN(AudioClip clip)
    {
        yield return new WaitForSecondsRealtime(clip.length);

        foreach (GameObject bt in botoes)
        {
            bt.SetActive(true);
        }
    }


    public void Patio()
    {
        if (!trava)
        {
            camNumb = 2;
            som.clip = falas[camNumb - 2];
            som.Play();
            camName = EventSystem.current.currentSelectedGameObject.name;
            Debug.Log(camName);
            trava = true;
            dialogo.text = "Você está prestes a ver o pátio do hotel. Tem certeza?";

            foreach (Button cam in cameras)
            {
                cam.interactable = false;
            }


            StartCoroutine(BotaoSN(som.clip));


        }

    }

    public void Cozinha()
    {
        if (!trava)
        {
            camNumb = 3;
            som.clip = falas[camNumb - 2];
            som.Play();
            camName = EventSystem.current.currentSelectedGameObject.name;
            Debug.Log(camName);
            trava = true;
            dialogo.text = "Esse duto vai dar para a cozinha, é muito perigoso. Quer mesmo dar uma olhada ai?";

            foreach (Button cam in cameras)
            {
                cam.interactable = false;
            }

            StartCoroutine(BotaoSN(som.clip));

        }

    }
    

    public void Recepcao()
    {
        if (!trava)
        {
            camNumb = 4;
            som.clip = falas[camNumb - 2];
            som.Play();
            camName = EventSystem.current.currentSelectedGameObject.name;
            Debug.Log(camName);
            trava = true;
            dialogo.text = "Por esse duto você vai ver a recepção do hotel. Tem certeza?";

            foreach (Button cam in cameras)
            {
                cam.interactable = false;
            }

            StartCoroutine(BotaoSN(som.clip));

        }
    }
    

    public void Q104()
    {
        if (!trava)
        {
            camNumb = 5;
            som.clip = falas[camNumb - 2];
            som.Play();
            camName = EventSystem.current.currentSelectedGameObject.name;
            Debug.Log(camName);
            trava = true;
            dialogo.text = "Este quarto está vazio. Tem certeza que quer observar?";

            foreach (Button cam in cameras)
            {
                cam.interactable = false;
            }

            StartCoroutine(BotaoSN(som.clip));

        }
    }
    

    public void Q108()
    {
        if (!trava)
        {
            camNumb = 6;
            som.clip = falas[camNumb - 2];
            som.Play();
            camName = EventSystem.current.currentSelectedGameObject.name;
            Debug.Log(camName);
            trava = true;
            dialogo.text = "Este quarto está vazio. Tem certeza que quer observar?";

            foreach (Button cam in cameras)
            {
                cam.interactable = false;
            }

            StartCoroutine(BotaoSN(som.clip));

        }
    }
    

    public void Q206()
    {
        if (!trava)
        {
            camNumb = 7;
            som.clip = falas[camNumb - 2];
            som.Play();
            camName = EventSystem.current.currentSelectedGameObject.name;
            Debug.Log(camName);
            trava = true;
            dialogo.text = "Ei, cuidado! Tem um morador nesse quarto. Tem certeza que quer olhar?";

            foreach (Button cam in cameras)
            {
                cam.interactable = false;
            }

            StartCoroutine(BotaoSN(som.clip));

        }
    }
    

    public void Laje()
    {
        if (!trava)
        {
            camNumb = 8;
            som.clip = falas[camNumb - 2];
            som.Play();
            camName = EventSystem.current.currentSelectedGameObject.name;
            Debug.Log(camName);
            trava = true;
            dialogo.text = "Ei! Você vai conseguir ver o telhado da recepção ai. Tem certeza que quer dar uma olhada?";

            foreach (Button cam in cameras)
            {
                cam.interactable = false;
            }

            StartCoroutine(BotaoSN(som.clip));

        }
    }
    

    public void Q301()
    {
        if (!trava)
        {
            camNumb = 9;
            som.clip = falas[camNumb - 2];
            som.Play();
            camName = EventSystem.current.currentSelectedGameObject.name;
            Debug.Log(camName);
            trava = true;
            dialogo.text = "Ei, silêncio! Tem gente morando ai. Acha uma boa ideia dar uma olhada?";

            foreach (Button cam in cameras)
            {
                cam.interactable = false;
            }

            StartCoroutine(BotaoSN(som.clip));

        }
    }


    public void Corredor()
    {
        if (!trava)
        {
            camNumb = 10;
            som.clip = falas[camNumb - 2];
            som.Play();
            camName = EventSystem.current.currentSelectedGameObject.name;
            Debug.Log(camName);
            trava = true;
            dialogo.text = "Eu não tenho certeza se tem alguma coisa para observar ai. Quer mesmo olhar?";

            foreach (Button cam in cameras)
            {
                cam.interactable = false;
            }

            StartCoroutine(BotaoSN(som.clip));

        }
    }


    public void Q304()
    {
        if (!trava)
        {
            camNumb = 11;
            som.clip = falas[camNumb - 2];
            som.Play();
            camName = EventSystem.current.currentSelectedGameObject.name;
            Debug.Log(camName);
            trava = true;
            dialogo.text = "Ei, presta atenção! Tem um cara muito perturbado que mora nesse quarto. Você acha uma boa ideia dar uma olhada ai?";

            foreach (Button cam in cameras)
            {
                cam.interactable = false;
            }

            StartCoroutine(BotaoSN(som.clip));

        }
    }
    #endregion

    #region botoes
    public void Sim()
    {
        if (startG)
        {
            ComeçarOP();
            return;
        }
        StartCoroutine(ChangeCams());
        som.Stop();
        
    }

    
    public void Direita()
    {
        if(andar == 1)
        {
            andar++;
            Esq.SetActive(true);
            andar2.SetActive(true);
            andar1.SetActive(false);
            Mesa.GetComponent<Renderer>().material.mainTexture = texture2;
        }
        else if(andar == 2)
        {
            andar++;
            Dir.SetActive(false);
            andar3.SetActive(true);
            andar2.SetActive(false);
            Mesa.GetComponent<Renderer>().material.mainTexture = texture3;
        }
    }

    public void Esquerda()
    {
        if(andar == 2)
        {
            andar--;
            Esq.SetActive(false);
            andar1.SetActive(true);
            andar2.SetActive(false);
            Mesa.GetComponent<Renderer>().material.mainTexture = texture1;
        }
        else if(andar == 3)
        {
            andar--;
            Dir.SetActive(true);
            andar2.SetActive(true);
            andar3.SetActive(false);
            Mesa.GetComponent<Renderer>().material.mainTexture = texture2;
        }
    }

    public void Nao()
    {
        startG = false;
        som.Stop();
        Debug.Log("GHADJGJKDADJANKDAKDA");
        dialogo.text = null;
        trava = false;
        foreach (GameObject bt in botoes)
        {
            bt.SetActive(false);
            Debug.Log(bt.activeSelf);
        }

        foreach (Button cam in cameras)
        {
            cam.interactable = true;
        }
        
    }
    #endregion


    IEnumerator BeginIntro()
    {
        yield return new WaitForSeconds(0.1f);
        tt.text = textos[0];
        som.Play();
        yield return new WaitForSeconds(5f);
        tt.text = textos[1];
        yield return new WaitForSeconds(7f);
        tt.text = textos[2];
        yield return new WaitForSeconds(7f);
        tt.text = textos[3];
        yield return new WaitForSeconds(6f);
        tt.text = null;
        intro = false;
        startI = false;
        trava = false;
    }


    IEnumerator BeginGame()
    {

        Debug.Log("oiu");
        isLoading = true;
        while(time < 1)
        {
            yield return null;
        }
        mesaMapa.SetActive(false);
        player.SetActive(true);
        playerCanvas.SetActive(true);
        gameCanvas.SetActive(true);
        TargetCam.startGame = true;
        Cursor.lockState = CursorLockMode.Locked;//Mouse lock State
        gameplay.volume = 0.1f * Globals.SOUND_GENERAL_SLIDER;
    }



    IEnumerator ChangeCams()
    {
        Cursor.lockState = CursorLockMode.Locked;
        dialogo.text = null;
        viewingCam = true;
        trava = false;
        foreach (GameObject bt in botoes)
        {
            bt.SetActive(false);
        }

        foreach (Button cam in cameras)
        {
            cam.interactable = true;
        }
        yield return new WaitForSeconds(0.1f);
        cams[0].SetActive(false);
        cams[camNumb].SetActive(true);
        canvasIntro.SetActive(false);
        canvasVisual.SetActive(true);

        cameras[camNumb - 2].gameObject.SetActive(false);


        yield return new WaitForSeconds(10f);
        EndCamView();

    }

    void EndCamView()
    {
        Cursor.lockState = CursorLockMode.None;
        /// camer.depth = -5;
        SelectLocation.camViews++;

        cams[0].SetActive(true);
        cams[camNumb].SetActive(false);
        viewingCam = false;

        canvasIntro.SetActive(true);
        canvasVisual.SetActive(false);
    }
}
