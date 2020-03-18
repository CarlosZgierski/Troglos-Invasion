using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SelectCam : MonoBehaviour {
    public GameObject sacoSpottado;
    public GameObject pauseSaco;
    public GameObject pauseVisao;
    public string obj;
    public Text[] objetivos;

    public float speed;

    public float minX;
    public float maxX;

    public float minY;
    public float maxY;

    float posX;
    float posY;

    Camera cam;
    Ray ray;
    RaycastHit hit;

    public AudioClip[] sacoSpots;
    public AudioSource som;
    public bool releaseSound;
    public bool millhoRoom;

    public int sacoIndex = 0;
    private bool ViuOSaco = false;

	// Use this for initialization
	void Start () {
        posY = 0;
        posX = 0;
        cam = GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void Update () {
        /////////////////////////////////////////////
        posX += speed * -Input.GetAxis("Mouse Y");
        posY += speed * Input.GetAxis("Mouse X");
        transform.localRotation = Quaternion.Euler(new Vector3(posX, posY, 0));

        if (posX < minX)
        {
            transform.localRotation = Quaternion.Euler(new Vector3(minX, posY, 0));
            posX = minX;
        }

        if (posX > maxX)
        {
            transform.localRotation = Quaternion.Euler(new Vector3(maxX, posY, 0));
            posX = maxX;
        }
        if (posY > maxY)
        {
            transform.localRotation = Quaternion.Euler(new Vector3(posX, maxY, 0));
            posY = maxY;
        }
        if (posY < minY)
        {
            transform.localRotation = Quaternion.Euler(new Vector3(posX, minY, 0));
            posY = minY;
        }
        ray = cam.ViewportPointToRay(new Vector3(0F, 0F, 0));
        if (Physics.Raycast(transform.position, transform.forward, out hit)) 
        {

            Debug.DrawLine(this.transform.position, transform.forward, Color.red);
            float time = 0;
            if (hit.collider.CompareTag(Globals.TAG_MAIN_OBJECT) || hit.collider.CompareTag(Globals.TAG_SACO_FALSO) || ViuOSaco)
            {
                if (releaseSound)
                {
                    int rand = Random.Range(0, sacoSpots.Length);
                    som.clip = sacoSpots[rand];
                    som.Play();
                    releaseSound = false;

                    for (int i = 0; i < objetivos.Length; i++)
                    {
                        if (objetivos[i].text == "")
                        {
                            objetivos[i].text = obj;
                            ViuOSaco = true;
                            break;
                        }
                    }
                }
                sacoSpottado.SetActive(true);
                pauseSaco.SetActive(true);
                pauseVisao.SetActive(false);
                time += Time.deltaTime;
                CheckSaco();
            }
        }

    }

    void CheckSaco()
    {
        switch (sacoIndex)
        {
            case 1:
                Globals.SPOT_SACO_104 = true;
                break;

            case 2:
                Globals.SPOT_SACO_COZINHA = true;
                break;

            case 3:
                Globals.SPOT_SACO_RECEPCAO = true;
                break;

            case 4:
                Globals.SPOT_SACO_108 = true;
                break;

            case 5:
                Globals.SPOT_SACO_LAJE = true;
                break;

            case 6:
                Globals.SPOT_SACO_301 = true;
                break;

            case 7:
                Globals.SPOT_SACO_304 = true;
                break;
        }
    }
    
}
