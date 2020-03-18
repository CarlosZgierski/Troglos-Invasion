using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MilhosRandom : MonoBehaviour
{

    int milhos;

    public GameObject[] pontos = new GameObject[6];

    public GameObject sacoV;
    public GameObject sacoF;

    // Use this for initialization
    void Start()
    {
        milhos = Random.Range(1, 6);
        for (int i = 0; i < pontos.Length; i++)
        {
            if (i == milhos)
                pontos[i].tag = Globals.TAG_MAIN_OBJECT;
            else
                pontos[i].tag = Globals.TAG_SACO_FALSO;
        }

        //if (milhos == 0)
        //{
        //    Instantiate(sacoF, pontos[0].transform.position, Quaternion.identity);
        //    Instantiate(sacoV, pontos[1].transform.position, Quaternion.identity);
        //    Instantiate(sacoF, pontos[2].transform.position, Quaternion.identity);
        //    Instantiate(sacoF, pontos[3].transform.position, Quaternion.identity);
        //    Instantiate(sacoF, pontos[4].transform.position, Quaternion.identity);
        //    Instantiate(sacoF, pontos[5].transform.position, Quaternion.identity);
        //    Instantiate(sacoF, pontos[6].transform.position, Quaternion.identity);
        //} else if (milhos == 1)
        //{
        //    Instantiate(sacoF, pontos[0].transform.position, Quaternion.identity);
        //    Instantiate(sacoF, pontos[1].transform.position, Quaternion.identity);
        //    Instantiate(sacoV, pontos[2].transform.position, Quaternion.identity);
        //    Instantiate(sacoF, pontos[3].transform.position, Quaternion.identity);
        //    Instantiate(sacoF, pontos[4].transform.position, Quaternion.identity);
        //    Instantiate(sacoF, pontos[5].transform.position, Quaternion.identity);
        //    Instantiate(sacoF, pontos[6].transform.position, Quaternion.identity);
        //} else if (milhos == 2)
        //{
        //    Instantiate(sacoF, pontos[0].transform.position, Quaternion.identity);
        //    Instantiate(sacoF, pontos[1].transform.position, Quaternion.identity);
        //    Instantiate(sacoF, pontos[2].transform.position, Quaternion.identity);
        //    Instantiate(sacoV, pontos[3].transform.position, Quaternion.identity);
        //    Instantiate(sacoF, pontos[4].transform.position, Quaternion.identity);
        //    Instantiate(sacoF, pontos[5].transform.position, Quaternion.identity);
        //    Instantiate(sacoF, pontos[6].transform.position, Quaternion.identity);
        //} else if (milhos == 3)
        //{
        //    Instantiate(sacoF, pontos[0].transform.position, Quaternion.identity);
        //    Instantiate(sacoF, pontos[1].transform.position, Quaternion.identity);
        //    Instantiate(sacoF, pontos[2].transform.position, Quaternion.identity);
        //    Instantiate(sacoF, pontos[3].transform.position, Quaternion.identity);
        //    Instantiate(sacoV, pontos[4].transform.position, Quaternion.identity);
        //    Instantiate(sacoF, pontos[5].transform.position, Quaternion.identity);
        //    Instantiate(sacoF, pontos[6].transform.position, Quaternion.identity);
        //}
        //else if (milhos == 4)
        //{
        //    Instantiate(sacoF, pontos[0].transform.position, Quaternion.identity);
        //    Instantiate(sacoF, pontos[1].transform.position, Quaternion.identity);
        //    Instantiate(sacoF, pontos[2].transform.position, Quaternion.identity);
        //    Instantiate(sacoF, pontos[3].transform.position, Quaternion.identity);
        //    Instantiate(sacoF, pontos[4].transform.position, Quaternion.identity);
        //    Instantiate(sacoV, pontos[5].transform.position, Quaternion.identity);
        //    Instantiate(sacoF, pontos[6].transform.position, Quaternion.identity);
        //}else if (milhos == 5)
        //{
        //    Instantiate(sacoF, pontos[0].transform.position, Quaternion.identity);
        //    Instantiate(sacoF, pontos[1].transform.position, Quaternion.identity);
        //    Instantiate(sacoF, pontos[2].transform.position, Quaternion.identity);
        //    Instantiate(sacoF, pontos[3].transform.position, Quaternion.identity);
        //    Instantiate(sacoF, pontos[4].transform.position, Quaternion.identity);
        //    Instantiate(sacoF, pontos[5].transform.position, Quaternion.identity);
        //    Instantiate(sacoV, pontos[6].transform.position, Quaternion.identity);
        //}

    }
}
