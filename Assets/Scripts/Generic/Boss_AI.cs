using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Boss_AI : MonoBehaviour {

    //estados do boss 
    [HideInInspector]
    public static string State;

    //sons
    bool Gritado;
    private SonsNPC audioPlay;
    int makeSound;

    //posicao
    private NavMeshAgent agente;

    //para onde o boss vai
    [SerializeField]
    private GameObject[] posRota1 = new GameObject[2];
    [SerializeField]
    private GameObject[] posRota2 = new GameObject[4];
    [SerializeField]
    private GameObject[] posRota3 = new GameObject[7];
    [SerializeField]
    private GameObject[] posRota4 = new GameObject[5];

    //posicao do personagem
    [HideInInspector] public GameObject player;

    public static Transform boss_player;
    public Transform Alvo;
    [SerializeField]
    private GameManager gManager;

    [HideInInspector] public Animator anim;

    //teste
    public static bool blabla = true;
    public static bool bloblo = true;
    public static bool morrer = true;
    public static int soma = 0;

    // Use this for initialization
    void Start ()
    {
        player = null;
        makeSound = Random.Range(1000, 2500);
        audioPlay = GetComponent<SonsNPC>();
        anim = GetComponentInChildren<Animator>();
        agente = GetComponent<NavMeshAgent>();
        //State = Globals.ACT_ROTA1;

        if (Alvo ==null)
        {
            Alvo = GameObject.FindGameObjectWithTag(Globals.TAG_PLAYER).transform;
        }
	}

    private void Awake()
    {
        //gManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update ()
    {
        if(!morrer)
        {
            soma+=1;
            if (soma >= 30)
            {
                gManager.GameOver();
            }
        }

        //fazer som
        if (makeSound <= 0)
        {
            audioPlay.M_Words();
            makeSound = Random.Range(1000, 2500);
        }
        else
        {
            makeSound -= 1;
        }

        if (blabla && bloblo)
        {
            blabla = false;
            StartCoroutine(RotaDaVida(1, posRota4, 4));
        }

        if (State == Globals.ACT_RUN)
        {
            this.StopAllCoroutines();
            bloblo = false;
            blabla = true;
            agente.stoppingDistance = 1f;
            anim.SetFloat("Velocidade", 0.6f);

            if (Gritado == false)
            {
                audioPlay.M_Grito();
                Gritado = true;
            }

            if (FieldOfViewBoss.vision)
            {
                agente.SetDestination(boss_player.transform.position);
                transform.LookAt(boss_player.transform.position);
            }
            else
            {
                agente.SetDestination(NPC_AI.player.transform.position);
                //transform.LookAt(NPC_AI.player.transform.position);
            }

            if (Vector3.Distance(transform.position, Alvo.transform.position) <= 1.4f && FieldOfViewBoss.vision && !Globals.FIM_DE_JOGO)
            {
                anim.SetFloat("Velocidade", 0);
                if(morrer)
                {
                    anim.SetTrigger("Ataque");
                    morrer = false;
                }
            }
        }
        else
        {
            agente.stoppingDistance = 0f;
            Gritado = false;
            bloblo = true;
        }

        #region blabla
        //switch (State)
        //{
        //    case "Rota1":
        //        agente.stoppingDistance = 0;
        //        anim.SetFloat("Velocidade", 0.2f);

        //        if (Vector3.Distance(this.gameObject.transform.position, posRota1[0].transform.position) <= 1)
        //        {
        //            anim.SetFloat("Velocidade", 0);
        //            StartCoroutine(RandomRotina());
        //        }

        //        if (Vector3.Distance(this.gameObject.transform.position, posRota1[1].transform.position) <= 1)
        //        {
        //            anim.SetFloat("Velocidade", 0);
        //            StartCoroutine(MudancaRotina(0, posRota1));
        //        }
        //        break;

        //    case "Rota2":
        //        agente.stoppingDistance = 0;
        //        anim.SetFloat("Velocidade", 0.2f);
        //        if (Vector3.Distance(this.gameObject.transform.position, posRota2[0].transform.position) <= 1)
        //        {
        //            anim.SetFloat("Velocidade", 0);
        //            StartCoroutine(RandomRotina());
        //        }

        //        if (Vector3.Distance(this.gameObject.transform.position, posRota2[1].transform.position) <= 1)
        //        {
        //            anim.SetFloat("Velocidade", 0);
        //            StartCoroutine(MudancaRotina(2, posRota2));
        //        }

        //        if (Vector3.Distance(this.gameObject.transform.position, posRota2[2].transform.position) <= 1)
        //        {
        //            anim.SetFloat("Velocidade", 0);
        //            StartCoroutine(MudancaRotina(3, posRota2));
        //        }

        //        if (Vector3.Distance(this.gameObject.transform.position, posRota2[3].transform.position) <= 1)
        //        {
        //            anim.SetFloat("Velocidade", 0);
        //            StartCoroutine(MudancaRotina(0, posRota2));
        //        }
        //        break;

        //    case "Rota3":
        //        agente.stoppingDistance = 0;
        //        anim.SetFloat("Velocidade", 0.2f);
        //        if (Vector3.Distance(this.gameObject.transform.position, posRota3[0].transform.position) <= 1)
        //        {
        //            anim.SetFloat("Velocidade", 0);
        //            StartCoroutine(RandomRotina());
        //        }

        //        if (Vector3.Distance(this.gameObject.transform.position, posRota3[1].transform.position) <= 1)
        //        {
        //            anim.SetFloat("Velocidade", 0);
        //            StartCoroutine(MudancaRotina(2, posRota3));
        //        }

        //        if (Vector3.Distance(this.gameObject.transform.position, posRota3[2].transform.position) <= 1)
        //        {
        //            anim.SetFloat("Velocidade", 0);
        //            StartCoroutine(MudancaRotina(3, posRota3));
        //        }

        //        if (Vector3.Distance(this.gameObject.transform.position, posRota3[3].transform.position) <= 1)
        //        {
        //            anim.SetFloat("Velocidade", 0);
        //            StartCoroutine(MudancaRotina(4, posRota3));
        //        }

        //        if (Vector3.Distance(this.gameObject.transform.position, posRota3[4].transform.position) <= 1)
        //        {
        //            anim.SetFloat("Velocidade", 0);
        //            StartCoroutine(MudancaRotina(5, posRota3));
        //        }

        //        if (Vector3.Distance(this.gameObject.transform.position, posRota3[5].transform.position) <= 1)
        //        {
        //            anim.SetFloat("Velocidade", 0);
        //            StartCoroutine(MudancaRotina(6, posRota3));
        //        }

        //        if (Vector3.Distance(this.gameObject.transform.position, posRota3[6].transform.position) <= 1)
        //        {
        //            anim.SetFloat("Velocidade", 0);
        //            StartCoroutine(MudancaRotina(0, posRota3));
        //        }
        //        break;

        //    case "Rota4":
        //        agente.stoppingDistance = 0;
        //        anim.SetFloat("Velocidade", 0.2f);
        //        if (Vector3.Distance(this.gameObject.transform.position, posRota4[0].transform.position) <= 1)
        //        {
        //            anim.SetFloat("Velocidade", 0);
        //            StartCoroutine(RandomRotina());
        //        }

        //        if (Vector3.Distance(this.gameObject.transform.position, posRota4[1].transform.position) <= 1)
        //        {
        //            anim.SetFloat("Velocidade", 0);
        //            StartCoroutine(MudancaRotina(2, posRota4));
        //        }

        //        if (Vector3.Distance(this.gameObject.transform.position, posRota4[2].transform.position) <= 1)
        //        {
        //            anim.SetFloat("Velocidade", 0);
        //            StartCoroutine(MudancaRotina(3, posRota4));
        //        }

        //        if (Vector3.Distance(this.gameObject.transform.position, posRota4[3].transform.position) <= 1)
        //        {
        //            anim.SetFloat("Velocidade", 0);
        //            StartCoroutine(MudancaRotina(4, posRota4));
        //        }

        //        if (Vector3.Distance(this.gameObject.transform.position, posRota4[4].transform.position) <= 1.5f)
        //        {
        //            anim.SetFloat("Velocidade", 0);
        //            StartCoroutine(MudancaRotina(0, posRota4));
        //        }
        //        break;

        //    case "Run":
        //        agente.stoppingDistance = 2;
        //        anim.SetFloat("Velocidade", 0.6f);

        //        if (Gritado == false)
        //        {
        //            audioPlay.M_Grito();
        //            Gritado = true;
        //        }

        //        if (FieldOfViewBoss.vision)
        //        {
        //            agente.SetDestination(boss_player.transform.position);
        //            transform.LookAt(boss_player.transform.position);
        //        } else
        //        {
        //            agente.SetDestination(NPC_AI.player.transform.position);
        //            //transform.LookAt(NPC_AI.player.transform.position);
        //        }

        //        if (Vector3.Distance(transform.position, Alvo.transform.position) <= 2 && FieldOfViewBoss.vision)
        //        {
        //            anim.SetFloat("Velocidade", 0);
        //            gManager.GameOver();
        //        }

        //        break;
        //}
        #endregion
    }



    IEnumerator RotaDaVida(int posicao, GameObject[] nome, int maxRota)
    {
        agente.SetDestination(nome[posicao].transform.position);
        anim.SetFloat("Velocidade", 0.2f);
        yield return new WaitWhile(() => Vector3.Distance(this.gameObject.transform.position, nome[posicao].transform.position) >= 0.4f);
        anim.SetFloat("Velocidade", 0f);
        yield return new WaitForSeconds(3f);
        if (posicao < maxRota && posicao > 0)
        {
            posicao++;
            StartCoroutine(RotaDaVida(posicao, nome, maxRota));
        }
        else if (posicao == maxRota)
        {
            posicao = 0;
            StartCoroutine(RotaDaVida(posicao, nome, maxRota));
        }
        else if (posicao == 0)
        {
            string rotaName = null;
            posicao = 1;
            int rotina = Random.Range(0, 101);

            if (rotina >= 0 && rotina < 25)
            {
                rotaName = "Rota1";
                maxRota = 1;
                nome = posRota1;
                StartCoroutine(RotaDaVida(posicao, nome, maxRota));
            }
            else if (rotina >= 25 && rotina < 50)
            {
                rotaName = "Rota2";
                maxRota = 3;
                nome = posRota2;
                StartCoroutine(RotaDaVida(posicao, nome, maxRota));
            }
            else if (rotina >= 50 && rotina < 75)
            {
                rotaName = "Rota3";
                maxRota = 6;
                nome = posRota3;
                StartCoroutine(RotaDaVida(posicao, nome, maxRota));
            }
            else if (rotina >= 75 && rotina <= 100)
            {
                rotaName = "Rota4";
                maxRota = 4;
                nome = posRota4;
                StartCoroutine(RotaDaVida(posicao, nome, maxRota));
            }
            Debug.Log("O boss está fazendo a rota " + rotaName);
        }
    }

    public void MudarVel(float num)
    {
        agente.speed = num;
    }
}
