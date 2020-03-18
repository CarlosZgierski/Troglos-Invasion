using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GlowItens : MonoBehaviour {

    public Material shaderNGlow;
    public Material shaderGlow;
    Material[] mats;
    private Renderer rend;

    private bool perto;

    public int ID_Texto;
    string[] T_Texto = new string[10];
    public Text Texto;
    //1 = puxar
    //2 = pegar
    [SerializeField]
    private GameObject player;
    Vector3 dist;
	// Use this for initialization
	void Start ()
    {
        rend = GetComponent<Renderer>();
        mats = rend.materials;
        if (ID_Texto == 0)
        T_Texto[0] = "";
        if (ID_Texto==1)
        T_Texto[1] = "Aperte 'E' para puxar & soltar / 'Espaço' para subir";
        if (ID_Texto == 2)
        T_Texto[2] = "Aperte 'E' para pega r/ MouseEsq. mirar / MouseDir. jogar ";
        if (ID_Texto == 3)
        T_Texto[3] = "Aperte 'E' para pega o saco";
        if (ID_Texto == 4)
         T_Texto[4] = "Aperte 'E' para esconder";
        if (ID_Texto == 5)
         T_Texto[0] = "";
    }
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        if (ID_Texto == 5 && !Globals.SACO_NA_MAO)
        {
            rend.material = shaderNGlow;
        }else if (ID_Texto == 5 && Globals.SACO_NA_MAO)
        {
            rend.material = shaderGlow;
        }

        if(ID_Texto != 5)
        {
            if (Vector3.Distance(this.gameObject.transform.position, player.transform.position) >= 3f)
            {
                if (ID_Texto == 4)
                {
                    mats[1] = shaderNGlow;
                    rend.materials = mats;
                }
                else
                {
                    rend.material = shaderNGlow;
                }

            }
            else
            {
                if (ID_Texto == 4)
                {
                    mats[1] = shaderGlow;
                    rend.materials = mats;
                }
                else
                {
                    rend.material = shaderGlow;
                }
            }


            if (Vector3.Distance(this.gameObject.transform.position, player.transform.position) <= 1f)
            {
                if (!Globals.SACO_NA_MAO)
                {
                    perto = true;
                    Texto.text = T_Texto[ID_Texto];
                    Globals.Texto_Visivel = true;
                }
                else
                {
                    perto = false;
                    Texto.text = T_Texto[0];
                    Globals.Texto_Visivel = false;
                }
            }
            else
            {
                if (Globals.Texto_Visivel && perto)
                {
                    Texto.text = T_Texto[0];
                    Globals.Texto_Visivel = false;
                }
                perto = false;
            }


        }
        
    }
}
