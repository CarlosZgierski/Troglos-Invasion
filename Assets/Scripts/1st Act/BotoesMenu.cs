using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotoesMenu : MonoBehaviour {
    public MovingCamera cam;
    public int rand;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnMouseUp()
    {
        switch (rand)
        {
            case 1:
                Debug.Log("Vai pro quadro");
                cam.Missao();
                break;

            case 2:
                Debug.Log("Vai pra opções");
                cam.Opcao();
                break;

            case 3:
                Debug.Log("Start Game");
                cam.StartGame();
                break;

            case 4:
                Debug.Log("Sair das opcoes");
                cam.Opcao();
                break;
        }
        
    }
}
