using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour {

    public GameObject MovCam;

    public static bool startMenu = true;

    public Text tt;
    public string[] conversa;
    public float[] tTime;

    public AudioClip[] falas;

    public AudioSource som;

    public GameObject loading;

    public Transform[] camPoints;
    public float[] camTimes;
    public float speed;

    [SerializeField] private Vector3 target;

    Vector3 pos0;
    Vector3 pos1;
    Vector3 pos2;
    Vector3 pos3;
    Vector3 pos4;
    Vector3 pos5;
    Vector3 pos6;
    Vector3 pos7;
    Vector3 pos8;

    private float time;

    public GameObject botoes;

    Menu menu;

    // Use this for initialization
    void Awake () {
        menu = GetComponent<Menu>();
        menu.enabled = true;

        pos0 = new Vector3(camPoints[0].position.x, camPoints[0].position.y, camPoints[0].position.z);
        pos1 = new Vector3(camPoints[1].position.x, camPoints[1].position.y, camPoints[1].position.z);
        pos2 = new Vector3(camPoints[2].position.x, camPoints[2].position.y, camPoints[2].position.z);
        pos3 = new Vector3(camPoints[3].position.x, camPoints[3].position.y, camPoints[3].position.z);
        pos4 = new Vector3(camPoints[4].position.x, camPoints[4].position.y, camPoints[4].position.z);
        pos5 = new Vector3(camPoints[5].position.x, camPoints[5].position.y, camPoints[5].position.z);
        pos6 = new Vector3(camPoints[6].position.x, camPoints[6].position.y, camPoints[6].position.z);
        pos7 = new Vector3(camPoints[7].position.x, camPoints[7].position.y, camPoints[7].position.z);
        pos8 = new Vector3(camPoints[8].position.x, camPoints[8].position.y, camPoints[8].position.z);
        target = pos1;
        transform.position = pos0;

        StartCoroutine(Chat(tTime[11], tTime[0], tTime[1], tTime[2], tTime[3], tTime[4], tTime[5], tTime[6], tTime[7], tTime[8], tTime[9], tTime[10]));

    }
	
	// Update is called once per frame
	void Update ()
    {
        if (!Menu.startMenu)
        {
            StopAllCoroutines();
            time = 0;
            target = pos8;
            botoes.SetActive(true);
            tt.gameObject.SetActive(false);
            tt.text = null;
            som.Stop();
            som.clip = null;
            MovCam.SetActive(true);
            gameObject.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StopAllCoroutines();
            time = 0;
            target = pos8;
            botoes.SetActive(true);
            tt.gameObject.SetActive(false);
            tt.text = null;
            som.Stop();
            som.clip = null;
            Menu.startMenu = false;
        }

        Debug.Log(menu.enabled);
        time += Time.deltaTime;


        transform.position = Vector3.MoveTowards(transform.position, target, time / speed);


        if (transform.position == pos1)
        {
            if (time >= falas[0].length)
            {
                target = pos2;
                time = 0;
            }
        }
        else if (transform.position == pos2)
        {
            if (time >= falas[1].length)
            {
                time = 0;
                target = pos3;
            }
        }
        else if (transform.position == pos3)
        {
            if (time >= falas[2].length)
            {
                time = 0;
                target = pos4;
            }
        }
        else if (transform.position == pos4)
        {
            if (time >= falas[3].length)
            {
                time = 0;
                target = pos5;
            }
        }
        else if (transform.position == pos5)
        {
            if (time >= falas[4].length)
            {
                time = 0;
                target = pos6;
            }
        }
        else if (transform.position == pos6)
        {
            if (time >= falas[5].length)
            {
                time = 0;
                target = pos7;
            }
        }
        else if (transform.position == pos7)
        {
            if (time >= falas[6].length)
            {
                time = 0;
                target = pos8;
            }
        }
        else if (transform.position == pos8)
        {
            if (time >= falas[7].length)
            {
                Menu.startMenu = false;
            }
        }
        
    }

    IEnumerator Chat(float S, float A, float A1, float A2, float A3, float B, float C, float D, float E, float F, float G, float H)
    {
        yield return new WaitForSeconds(S);
        som.clip = falas[0];
        som.Play();
        tt.text = conversa[0];
        yield return new WaitForSeconds(A);
        tt.text = conversa[1];
        yield return new WaitForSeconds(A1);
        tt.text = conversa[2];
        yield return new WaitForSeconds(A2);
        tt.text = conversa[3];
        yield return new WaitForSeconds(A3);
        tt.text = conversa[4];
       som.clip = falas[1];
        som.Play();
        yield return new WaitForSeconds(B);
        tt.text = conversa[5];
        som.clip = falas[2];
        som.Play();
        yield return new WaitForSeconds(C);
        tt.text = conversa[6];
        som.clip = falas[3];
        som.Play();
        yield return new WaitForSeconds(D);
        tt.text = conversa[7];
        som.clip = falas[4];
        som.Play();
        yield return new WaitForSeconds(E);
        tt.text = conversa[8];
        som.clip = falas[5];
        som.Play();
        yield return new WaitForSeconds(F);
        tt.text = conversa[9];
        som.clip = falas[6];
        som.Play();
        yield return new WaitForSeconds(G);
        tt.text = conversa[10];
        som.clip = falas[7];
        som.Play();
        yield return new WaitForSeconds(H);
        botoes.SetActive(true);
        tt.gameObject.SetActive(false);
        tt.text = null;
    }

    
}
