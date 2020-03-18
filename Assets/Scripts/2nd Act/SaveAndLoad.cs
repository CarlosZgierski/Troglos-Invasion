using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveAndLoad : MonoBehaviour {

    public GameObject aviso;


    [SerializeField]
    private GameObject tela;

    [SerializeField]
    private Transform [] NPC;
    [SerializeField]
    private float[] NPCposX;
    [SerializeField]
    private float[] NPCposY;
    [SerializeField]
    private float[] NPCposZ;

    // Use this for initialization
    void Start()
    {
        for (int i = 0; i < NPC.Length; i++)
        {
            NPCposX[i] = NPC[i].localPosition.x;
            NPCposY[i] = NPC[i].localPosition.y;
            NPCposZ[i] = NPC[i].localPosition.z;
        }
    }

    public void Save()
    {
        this.StopAllCoroutines();
        StartCoroutine(Fala());
        for(int i = 0; i < NPC.Length; i++)
        {
            NPCposX[i] = NPC[i].localPosition.x;
            NPCposY[i] = NPC[i].localPosition.y;
            NPCposZ[i] = NPC[i].localPosition.z;
        }
    }

    IEnumerator Fala()
    {
        aviso.SetActive(true);
        yield return new WaitForSecondsRealtime(2f);
        aviso.SetActive(false);
    }

    public void Load()
    {
        Boss_AI.State = null;
        Boss_AI.morrer = true;
        Boss_AI.soma = 0;
        Boss_AI.bloblo = true;
        Boss_AI.blabla = true;
        for (int i = 0; i < NPC.Length; i++)
        {
                NPC[i].gameObject.transform.localPosition = new Vector3(NPCposX[i], NPCposY[i], NPCposZ[i]);
        }
        tela.SetActive(false);
    }
}
