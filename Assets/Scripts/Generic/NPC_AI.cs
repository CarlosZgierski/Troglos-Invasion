using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPC_AI : MonoBehaviour {

    //estado que se encontra
    private string State;

    //sound
    private bool Gritado;
    private bool visto;
    private SonsNPC audioPlay;

    //posicao
    private NavMeshAgent agente;

    //identificar o NPC 
    [SerializeField] private int NPC_ID;
    //maromba = 1
    //surda = 2
    //pratos = 3
    //paranoico = 4
    //homem tv = 5
    //velhinha e espiao = 6
    //zelador = 7
    //faxineira = 8
    //faxineira = 9
    //nemo = 10
    //cega = 11


    //tempo de duracao
    [SerializeField] private int[] T_acao = new int[3];

    //salvar ultima acao
    [HideInInspector] public string lastAction;

    //para onde o jogador vai
    [SerializeField] private GameObject[] pos_Acoes = new GameObject[3];

    //para onde o jogador vai olhar
    [SerializeField] private Transform[] pos_olhar = new Transform[3];

    //Current sound heard transform
    private Transform soundHeardTransform;
    private Vector3 soundHeardVector;

    //posicao do player
    public static Transform player;
    public Transform alvo;

    // fazer som
    int makeSound;


    //delimitacao de area
    [SerializeField] private GameObject distance;
    [SerializeField] private int dist;
    private bool foraderaio = false;

    FieldOfView FOV;
    [HideInInspector] public Animator anim;

    // Use this for initialization
    void Start ()
    {
        player = null;
        makeSound = Random.Range(1000, 2500);
        anim = GetComponentInChildren<Animator>();
        audioPlay = GetComponent<SonsNPC>();
        State = Globals.ACT_AÇAO1;
        lastAction = Globals.ACT_AÇAO1;
        agente = GetComponent<NavMeshAgent>();
        FOV = GetComponent<FieldOfView>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(this.transform.position,distance.transform.position)<dist)
        {
            FOV.DistCerta(!foraderaio);
        }
        else
        {
            FOV.DistCerta(foraderaio);
        }
        //FieldOfView.State = State;
        //print(State);
        
        if (FOV.ReGrito() == Globals.ACT_SCREAMING)
        {
            anim.SetBool("Vendo", true);
            State = Globals.ACT_SCREAMING;
        } else if (FOV.ReState() == Globals.ACT_SOUND_HEARD)
        {
            anim.SetBool("Vendo", false);
            State = Globals.ACT_SOUND_HEARD;
        } else
        {
            anim.SetBool("Vendo", false);
            State = lastAction;
        }

        //fazer som
        if (makeSound <= 0)
        {
            audioPlay.M_Words();
            makeSound = Random.Range(1000,2500);
        }
        else
        {
            if(!GameManager.IS_WORLD_PAUSED)
            {
                makeSound -= 1;
            }
        }

        SwitchNpcActions();        
	}

    #region Npc Actions
    
        IEnumerator Action1(int tempo)
    {
        yield return new WaitForSeconds(tempo);
        lastAction = Globals.ACT_AÇAO2;
        State = Globals.ACT_AÇAO2;
    }

    IEnumerator Action2(int tempo)
    {
        yield return new WaitForSeconds(tempo);

        if (NPC_ID == 3 || NPC_ID == 5 || NPC_ID == 7)
        {
            lastAction = Globals.ACT_AÇAO1;
            State = Globals.ACT_AÇAO1;
        } else
        {
            lastAction = Globals.ACT_AÇAO3;
            State = Globals.ACT_AÇAO3;
        }
    }

    IEnumerator Action3(int tempo)
    {
        yield return new WaitForSeconds(tempo);

        if (NPC_ID == 6 || NPC_ID == 9)
        {
            lastAction = Globals.ACT_AÇAO1;
            State = Globals.ACT_AÇAO1;
        } else
        {
            lastAction = Globals.ACT_AÇAO4;
            State = Globals.ACT_AÇAO4;
        }
    }

    IEnumerator Action4(int tempo)
    {
        yield return new WaitForSeconds(tempo);
        if(NPC_ID == 10)
        {
            lastAction = Globals.ACT_AÇAO5;
            State = Globals.ACT_AÇAO5;
        } else
        {
            lastAction = Globals.ACT_AÇAO1;
            State = Globals.ACT_AÇAO1;
        }
    }

    IEnumerator Action5(int tempo)
    {
        yield return new WaitForSeconds(tempo);
        lastAction = Globals.ACT_AÇAO1;
        State = Globals.ACT_AÇAO1;
    }
    #endregion

    private void SwitchNpcActions()
    {

        if(NPC_ID == 1)
        {
            switch (State)
            {
                case "Screaming":
                    this.StopCoroutine(Action1(T_acao[0]));
                    this.StopCoroutine(Action2(T_acao[1]));
                    this.StopCoroutine(Action3(T_acao[2]));
                    this.StopCoroutine(Action4(T_acao[3]));
                    this.StopCoroutine(Action5(T_acao[4]));

                    visto = false;

                    agente.stoppingDistance = 50;
                    anim.SetFloat("Velocidade", 0);
                    transform.LookAt(alvo.transform.position);
                    agente.SetDestination(player.transform.position);
                    Boss_AI.State = Globals.ACT_RUN;
                    if (Gritado == false)
                    {
                        audioPlay.M_Grito();
                        Gritado = true;
                    }
                    break;

                case "SoundHeard":
                    Gritado = false;
                    if (!visto)
                    {
                        audioPlay.M_Where();
                        visto = true;
                    }
                    //parar as coroutines
                    this.StopCoroutine(Action1(T_acao[0]));
                    this.StopCoroutine(Action2(T_acao[1]));
                    this.StopCoroutine(Action3(T_acao[2]));
                    this.StopCoroutine(Action4(T_acao[3]));
                    this.StopCoroutine(Action5(T_acao[4]));
                    agente.stoppingDistance = 0;

                    //fazer acao
                    agente.SetDestination(soundHeardVector);
                    anim.SetFloat("Velocidade", 0.2f);
                    if (Vector3.Distance(this.transform.position, soundHeardVector) <= 0.5f)
                    {
                        anim.SetFloat("Velocidade", 0);
                        StartCoroutine("SoundHeard");
                    }
                    break;

                case "Açao1":
                    Gritado = false;
                    visto = false;
                    //parar as coroutines
                    this.StopCoroutine(Action1(T_acao[0]));
                    this.StopCoroutine(Action2(T_acao[1]));
                    this.StopCoroutine(Action3(T_acao[2]));
                    this.StopCoroutine(Action4(T_acao[3]));
                    this.StopCoroutine(Action5(T_acao[4]));
                    

                    //olhar pro lugar
                    var rotation = Quaternion.LookRotation(pos_olhar[0].position - transform.position);
                    transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime);

                    //fazer acao
                    agente.SetDestination(pos_Acoes[0].transform.position);
                        agente.stoppingDistance = 0;
                        
                        if (Vector3.Distance(transform.position, pos_Acoes[0].transform.position) < 0.01f)
                        {
                            anim.SetFloat("Velocidade", 0);
                            StartCoroutine(Action1(T_acao[0]));
                        } else
                            {
                                anim.SetFloat("Velocidade", 0.2f);
                            }


                    break;

                case "Açao2":
                    Gritado = false;
                    visto = false;
                    //parar as coroutines
                    StopCoroutine(Action1(T_acao[0]));
                    agente.stoppingDistance = 0;

                    //fazer acao
                    agente.SetDestination(pos_Acoes[1].transform.position);

                    if (Vector3.Distance(transform.position, pos_Acoes[1].transform.position) < 0.03f)
                    {
                        anim.SetBool("Peito", true);
                        anim.SetFloat("Velocidade", 0);
                        StartCoroutine(Action2(T_acao[1]));
                    }
                    else
                    {
                        anim.SetFloat("Velocidade", 0.2f);
                    }

                    //olhar pro lugar
                        rotation = Quaternion.LookRotation(pos_olhar[1].position - transform.position);
                        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime);
                    break;

                case "Açao3":
                    Gritado = false;
                    visto = false;
                    //parar as coroutines
                    StopCoroutine(Action2(T_acao[1]));
                    agente.speed = 2.3f;
                    //fazer acao
                    agente.stoppingDistance = 0;
                    agente.SetDestination(pos_Acoes[2].transform.position);
                   
                    if (Vector3.Distance(transform.position, pos_Acoes[2].transform.position) < 0.01f)
                    {
                        anim.SetBool("Peito", true);
                        anim.SetFloat("Velocidade", 0);
                        StartCoroutine(Action3(T_acao[2]));
                    }else
                    {
                        anim.SetBool("Peito", false);
                        anim.SetFloat("Velocidade", 0.2f);
                    }
                    break;

                case "Açao4":
                    Gritado = false;
                    visto = false;
                    //parar as coroutines
                    StopCoroutine(Action2(T_acao[2]));
                    agente.speed = 1.3f;
                    //fazer acao
                    agente.stoppingDistance = 0;
                    agente.SetDestination(pos_Acoes[3].transform.position);
                    if (Vector3.Distance(transform.position, pos_Acoes[3].transform.position) < 0.01f)
                    {
                        anim.SetFloat("Velocidade", 0);
                        StartCoroutine(Action4(T_acao[3]));
                    }else
                    {
                        anim.SetFloat("Velocidade", 0.2f);
                        anim.SetBool("Peito", false);
                    }
                    break;
            }
        }

        if(NPC_ID==2)
        {
            switch (State)
            {
                case "Screaming":
                    this.StopCoroutine(Action1(T_acao[0]));
                    this.StopCoroutine(Action2(T_acao[1]));
                    this.StopCoroutine(Action3(T_acao[2]));
                    this.StopCoroutine(Action4(T_acao[3]));
                    this.StopCoroutine(Action5(T_acao[4]));

                    visto = false;

                    agente.stoppingDistance = 50;
                    anim.SetFloat("Velocidade", 0);
                    anim.SetBool("Vendo", true);
                    transform.LookAt(alvo.transform.position);
                    agente.SetDestination(player.transform.position);
                    Boss_AI.State = Globals.ACT_RUN;

                    if (Gritado == false)
                    {
                        audioPlay.M_Grito();
                        Gritado = true;
                    }
                    //print("AAAAAAAAAAAAAAAHHHHHHHHHHHHHH");
                    break;

                case "SoundHeard":
                    Gritado = false;
                    if (!visto)
                    {
                        audioPlay.M_Where();
                        visto = true;
                    }
                    //parar as coroutines
                    this.StopCoroutine(Action1(T_acao[0]));
                    this.StopCoroutine(Action2(T_acao[1]));
                    this.StopCoroutine(Action3(T_acao[2]));
                    this.StopCoroutine(Action4(T_acao[3]));
                    this.StopCoroutine(Action5(T_acao[4]));
                    agente.stoppingDistance = 0;

                    //fazer acao
                    agente.SetDestination(soundHeardVector);
                    anim.SetFloat("Velocidade", 0.2f);
                    if (Vector3.Distance(this.transform.position, soundHeardVector) <= 1)
                    {
                        anim.SetFloat("Velocidade", 0);
                        StartCoroutine("SoundHeard");
                    }
                    break;

                case "Açao1":
                    Gritado = false;
                    visto = false;
                    //parar as coroutines
                    this.StopCoroutine(Action1(T_acao[0]));
                    this.StopCoroutine(Action2(T_acao[1]));
                    this.StopCoroutine(Action3(T_acao[2]));
                    this.StopCoroutine(Action4(T_acao[3]));
                    this.StopCoroutine(Action5(T_acao[4]));

                    //fazer acao
                        agente.SetDestination(pos_Acoes[0].transform.position);
                        agente.stoppingDistance = 0;
                        anim.SetFloat("Velocidade", 0.2f);
                        if (Vector3.Distance(transform.position, pos_Acoes[0].transform.position) < 0.10f)
                        {
                            anim.SetFloat("Velocidade", 0);
                            StartCoroutine(Action1(T_acao[0]));
                        }

                    //olhar pro lugar
                        var rotation = Quaternion.LookRotation(pos_olhar[0].position - transform.position);
                        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime);
                        //transform.LookAt(pos_olhar[0].position);
                    break;

                case "Açao2":
                    Gritado = false;
                    visto = false;
                    //parar as coroutines
                    StopCoroutine(Action1(T_acao[0]));
                    agente.stoppingDistance = 0;

                    //fazer acao
                    agente.SetDestination(pos_Acoes[1].transform.position);
                    anim.SetFloat("Velocidade", 0.2f);
                    if (Vector3.Distance(transform.position, pos_Acoes[1].transform.position) < 0.10f)
                    {
                        anim.SetFloat("Velocidade", 0);
                        StartCoroutine(Action2(T_acao[1]));
                    }
                    break;

                case "Açao3":
                    Gritado = false;
                    visto = false;
                    //parar as coroutines
                    StopCoroutine(Action2(T_acao[1]));

                    //fazer acao
                    agente.stoppingDistance = 0;
                    agente.SetDestination(pos_Acoes[2].transform.position);
                    anim.SetFloat("Velocidade", 0.2f);
                    if (Vector3.Distance(transform.position, pos_Acoes[2].transform.position) < 0.10f)
                    {
                        anim.SetFloat("Velocidade", 0);
                        StartCoroutine(Action3(T_acao[2]));
                    }
                    break;

                case "Açao4":
                    Gritado = false;
                    visto = false;
                    //parar as coroutines
                    StopCoroutine(Action2(T_acao[2]));

                    //fazer acao
                    agente.stoppingDistance = 0;
                    agente.SetDestination(pos_Acoes[3].transform.position);
                    anim.SetFloat("Velocidade", 0.2f);
                    if (Vector3.Distance(transform.position, pos_Acoes[3].transform.position) < 0.10f)
                    {
                        anim.SetFloat("Velocidade", 0);
                        StartCoroutine(Action4(T_acao[3]));
                    }

                    //olhar pro lugar
                    if (NPC_ID == 2)
                    {
                        //transform.LookAt(pos_olhar[1].position);
                        rotation = Quaternion.LookRotation(pos_olhar[1].position - transform.position);
                        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime);
                    }
                    break;
            }
        }

        if(NPC_ID==3)
        {
            switch (State)
            {
                case "Screaming":
                    this.StopCoroutine(Action1(T_acao[0]));
                    this.StopCoroutine(Action2(T_acao[1]));
                    this.StopCoroutine(Action3(T_acao[2]));
                    this.StopCoroutine(Action4(T_acao[3]));
                    this.StopCoroutine(Action5(T_acao[4]));

                    visto = false;

                    agente.stoppingDistance = 50;
                    anim.SetFloat("Velocidade", 0);
                    transform.LookAt(alvo.transform.position);
                    agente.SetDestination(player.transform.position);
                    Boss_AI.State = Globals.ACT_RUN;

                    if (Gritado == false)
                    {
                        audioPlay.M_Grito();
                        Gritado = true;
                    }
                    //print("AAAAAAAAAAAAAAAHHHHHHHHHHHHHH");
                    break;

                case "SoundHeard":
                    Gritado = false;
                    if (!visto)
                    {
                        audioPlay.M_Where();
                        visto = true;
                    }
                    //parar as coroutines
                    this.StopCoroutine(Action1(T_acao[0]));
                    this.StopCoroutine(Action2(T_acao[1]));
                    this.StopCoroutine(Action3(T_acao[2]));
                    this.StopCoroutine(Action4(T_acao[3]));
                    this.StopCoroutine(Action5(T_acao[4]));
                    agente.stoppingDistance = 0;

                    //fazer acao
                    agente.SetDestination(soundHeardVector);
                    anim.SetFloat("Velocidade", 0.2f);
                    if (Vector3.Distance(this.transform.position, soundHeardVector) <= 1)
                    {
                        anim.SetFloat("Velocidade", 0);
                        StartCoroutine("SoundHeard");
                    }
                    break;

                case "Açao1":
                    Gritado = false;
                    visto = false;
                    //parar as coroutines
                    this.StopCoroutine(Action1(T_acao[0]));
                    this.StopCoroutine(Action2(T_acao[1]));
                    this.StopCoroutine(Action3(T_acao[2]));
                    this.StopCoroutine(Action4(T_acao[3]));
                    this.StopCoroutine(Action5(T_acao[4]));

                    //fazer acao
                        agente.SetDestination(pos_Acoes[0].transform.position);
                        agente.stoppingDistance = 0;
                        anim.SetFloat("Velocidade", 0.2f);
                        if (Vector3.Distance(transform.position, pos_Acoes[0].transform.position) < 0.3f)
                        {
                            anim.SetFloat("Velocidade", 0);
                            StartCoroutine(Action1(T_acao[0]));
                        }
                    break;

                case "Açao2":
                    Gritado = false;
                    visto = false;
                    //parar as coroutines
                    StopCoroutine(Action1(T_acao[0]));
                    agente.stoppingDistance = 0;

                    //fazer acao
                    agente.SetDestination(pos_Acoes[1].transform.position);
                    anim.SetFloat("Velocidade", 0.2f);
                    if (Vector3.Distance(transform.position, pos_Acoes[1].transform.position) < 0.3f)
                    {
                        anim.SetFloat("Velocidade", 0);
                        StartCoroutine(Action2(T_acao[1]));
                    }
                    break;
            }
        }

        if(NPC_ID==4)
        {
            switch (State)
            {
                case "Screaming":
                    this.StopCoroutine(Action1(T_acao[0]));
                    this.StopCoroutine(Action2(T_acao[1]));
                    this.StopCoroutine(Action3(T_acao[2]));
                    this.StopCoroutine(Action4(T_acao[3]));
                    this.StopCoroutine(Action5(T_acao[4]));

                    visto = false;

                    agente.stoppingDistance = 50;
                    anim.SetFloat("Velocidade", 0);
                    transform.LookAt(alvo.transform.position);
                    agente.SetDestination(player.transform.position);
                    Boss_AI.State = Globals.ACT_RUN;

                    if (Gritado == false)
                    {
                        audioPlay.M_Grito();
                        Gritado = true;
                    }
                    //print("AAAAAAAAAAAAAAAHHHHHHHHHHHHHH");
                    break;

                case "SoundHeard":
                    Gritado = false;
                    if (!visto)
                    {
                        audioPlay.M_Where();
                        visto = true;
                    }
                    //parar as coroutines
                    this.StopCoroutine(Action1(T_acao[0]));
                    this.StopCoroutine(Action2(T_acao[1]));
                    this.StopCoroutine(Action3(T_acao[2]));
                    this.StopCoroutine(Action4(T_acao[3]));
                    this.StopCoroutine(Action5(T_acao[4]));
                    agente.stoppingDistance = 0;

                    //fazer acao
                    agente.SetDestination(soundHeardVector);
                    anim.SetFloat("Velocidade", 0.2f);
                    if (Vector3.Distance(this.transform.position, soundHeardVector) <= 0.5f)
                    {
                        anim.SetFloat("Velocidade", 0);
                        StartCoroutine("SoundHeard");
                    }
                    break;

                case "Açao1":
                    Gritado = false;
                    visto = false;
                    //parar as coroutines
                    this.StopCoroutine(Action1(T_acao[0]));
                    this.StopCoroutine(Action2(T_acao[1]));
                    this.StopCoroutine(Action3(T_acao[2]));
                    this.StopCoroutine(Action4(T_acao[3]));
                    this.StopCoroutine(Action5(T_acao[4]));

                        //fazer acao
                        anim.SetBool("Falar", true);
                        agente.SetDestination(pos_Acoes[0].transform.position);
                        agente.stoppingDistance = 0;
                        anim.SetFloat("Velocidade", 0.2f);
                        if (Vector3.Distance(transform.position, pos_Acoes[0].transform.position) < 0.25f)
                        {
                            anim.SetFloat("Velocidade", 0);
                            StartCoroutine(Action1(T_acao[0]));
                        }
                        //olhar pro lugar
                        var rotation = Quaternion.LookRotation(pos_olhar[0].position - transform.position);
                        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime);
                        //transform.LookAt(pos_olhar[0].position);
                    break;

                case "Açao2":
                    Gritado = false;
                    visto = false;
                    //parar as coroutines
                    StopCoroutine(Action1(T_acao[0]));
                    agente.stoppingDistance = 0;

                    //fazer acao
                    anim.SetBool("Falar", false);
                    agente.SetDestination(pos_Acoes[1].transform.position);
                    anim.SetFloat("Velocidade", 0.2f);
                    if (Vector3.Distance(transform.position, pos_Acoes[1].transform.position) < 0.25f)
                    {
                        anim.SetFloat("Velocidade", 0);
                        StartCoroutine(Action2(T_acao[1]));
                    }

                    //olhar pro lugar
                        rotation = Quaternion.LookRotation(pos_olhar[1].position - transform.position);
                        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime);
                        //transform.LookAt(pos_olhar[1].position);
                    break;

                case "Açao3":
                    Gritado = false;
                    visto = false;
                    //parar as coroutines
                    StopCoroutine(Action2(T_acao[1]));

                    //fazer acao
                    anim.SetBool("Falar", true);
                    agente.stoppingDistance = 0;
                    agente.SetDestination(pos_Acoes[2].transform.position);
                    anim.SetFloat("Velocidade", 0.2f);
                    if (Vector3.Distance(transform.position, pos_Acoes[2].transform.position) < 0.25f)
                    {
                        anim.SetFloat("Velocidade", 0);
                        StartCoroutine(Action3(T_acao[2]));
                    }

                    //olhar pro lugar
                        rotation = Quaternion.LookRotation(pos_olhar[2].position - transform.position);
                        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime);
                        //transform.LookAt(pos_olhar[2].position);
                    break;

                case "Açao4":
                    Gritado = false;
                    visto = false;
                    //parar as coroutines
                    StopCoroutine(Action2(T_acao[2]));

                    //fazer acao
                    anim.SetBool("Falar", false);
                    agente.stoppingDistance = 0;
                    agente.SetDestination(pos_Acoes[3].transform.position);
                    anim.SetFloat("Velocidade", 0.2f);
                    if (Vector3.Distance(transform.position, pos_Acoes[3].transform.position) < 0.25f)
                    {
                        anim.SetFloat("Velocidade", 0);
                        StartCoroutine(Action4(T_acao[3]));
                    }
                        rotation = Quaternion.LookRotation(pos_olhar[3].position - transform.position);
                        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime);
                        //transform.LookAt(pos_olhar[3].position);
                    break;
            }
        }

        if(NPC_ID==5)
        {
            switch (State)
            {
                case "Screaming":
                    this.StopCoroutine(Action1(T_acao[0]));
                    this.StopCoroutine(Action2(T_acao[1]));

                    visto = false;

                    agente.stoppingDistance = 50;
                    anim.SetFloat("Velocidade", 0);
                    transform.LookAt(alvo.transform.position);
                    agente.SetDestination(player.transform.position);

                    if (Gritado == false)
                    {
                        audioPlay.M_Grito();
                        Gritado = true;
                    }
                    //print("AAAAAAAAAAAAAAAHHHHHHHHHHHHHH");
                    break;

                case "SoundHeard":
                    Gritado = false;
                    if (!visto)
                    {
                        audioPlay.M_Where();
                        visto = true;
                    }
                    //parar as coroutines
                    this.StopCoroutine(Action1(T_acao[0]));
                    this.StopCoroutine(Action2(T_acao[1]));
                    agente.stoppingDistance = 0;
                    anim.SetBool("Sentar", false);
                    //fazer acao
                    agente.SetDestination(soundHeardVector);
                    anim.SetFloat("Velocidade", 0.2f);
                    if (Vector3.Distance(this.transform.position, soundHeardVector) <= 1)
                    {
                        anim.SetFloat("Velocidade", 0);
                        StartCoroutine("SoundHeard");
                    }
                    break;

                case "Açao1":
                    Gritado = false;
                    visto = false;
                    //parar as coroutines
                    this.StopCoroutine(Action1(T_acao[0]));
                    this.StopCoroutine(Action2(T_acao[1]));
                    anim.SetBool("Sentar", true);
                    //fazer acao
                        agente.SetDestination(pos_Acoes[0].transform.position);
                        agente.stoppingDistance = 0;
                        anim.SetFloat("Velocidade", 0.2f);
                        if (Vector3.Distance(transform.position, pos_Acoes[0].transform.position) < 0.05f)
                        {
                            anim.SetFloat("Velocidade", 0);
                            StartCoroutine(Action1(T_acao[0]));
                        }

                    //olhar pro lugar
                        var rotation = Quaternion.LookRotation(pos_olhar[0].position - transform.position);
                        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime);
                        //transform.LookAt(pos_olhar[0].position);
                    break;

                case "Açao2":
                    Gritado = false;
                    visto = false;
                    //parar as coroutines
                    StopCoroutine(Action1(T_acao[0]));
                    agente.stoppingDistance = 0;
                    anim.SetBool("Sentar", false);
                    //fazer acao
                    agente.SetDestination(pos_Acoes[1].transform.position);
                    anim.SetFloat("Velocidade", 0.2f);
                    if (Vector3.Distance(transform.position, pos_Acoes[1].transform.position) < 0.4f)
                    {
                        anim.SetFloat("Velocidade", 0);
                        StartCoroutine(Action2(T_acao[1]));
                    }

                    //olhar pro lugar
                        rotation = Quaternion.LookRotation(pos_olhar[1].position - transform.position);
                        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime);
                        //transform.LookAt(pos_olhar[1].position);
                    break;
            }
        }

        if(NPC_ID==6)
        {
            switch (State)
            {
                case "Screaming":
                    this.StopCoroutine(Action1(T_acao[0]));
                    this.StopCoroutine(Action2(T_acao[1]));
                    this.StopCoroutine(Action3(T_acao[2]));
                    this.StopCoroutine(Action4(T_acao[3]));
                    this.StopCoroutine(Action5(T_acao[4]));

                    visto = false;

                    agente.stoppingDistance = 50;
                    anim.SetFloat("Velocidade", 0);
                    transform.LookAt(alvo.transform.position);
                    agente.SetDestination(player.transform.position);

                    if (Gritado == false)
                    {
                        audioPlay.M_Grito();
                        Gritado = true;
                    }
                    //print("AAAAAAAAAAAAAAAHHHHHHHHHHHHHH");
                    break;

                case "SoundHeard":
                    Gritado = false;
                    if (!visto)
                    {
                        audioPlay.M_Where();
                        visto = true;
                    }
                    //parar as coroutines
                    this.StopCoroutine(Action1(T_acao[0]));
                    this.StopCoroutine(Action2(T_acao[1]));
                    this.StopCoroutine(Action3(T_acao[2]));
                    this.StopCoroutine(Action4(T_acao[3]));
                    this.StopCoroutine(Action5(T_acao[4]));
                    agente.stoppingDistance = 0;

                    //fazer acao
                    agente.SetDestination(soundHeardVector);
                    anim.SetFloat("Velocidade", 0.2f);
                    if (Vector3.Distance(this.transform.position, soundHeardVector) <= 1)
                    {
                        anim.SetFloat("Velocidade", 0);
                        StartCoroutine("SoundHeard");
                    }
                    break;

                case "Açao1":
                    Gritado = false;
                    visto = false;
                    //parar as coroutines
                    this.StopCoroutine(Action1(T_acao[0]));
                    this.StopCoroutine(Action2(T_acao[1]));
                    this.StopCoroutine(Action3(T_acao[2]));
                    this.StopCoroutine(Action4(T_acao[3]));
                    this.StopCoroutine(Action5(T_acao[4]));

                    //fazer acao
                        agente.SetDestination(pos_Acoes[0].transform.position);
                        agente.stoppingDistance = 0;
                        anim.SetFloat("Velocidade", 0.2f);
                        if (Vector3.Distance(transform.position, pos_Acoes[0].transform.position) < 0.3f)
                        {
                            anim.SetFloat("Velocidade", 0);
                            StartCoroutine(Action1(T_acao[0]));
                        }

                    //olhar pro lugar
                        var rotation = Quaternion.LookRotation(pos_olhar[0].position - transform.position);
                        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime);
                        //transform.LookAt(pos_olhar[0].position);
                    break;

                case "Açao2":
                    Gritado = false;
                    visto = false;
                    //parar as coroutines
                    StopCoroutine(Action1(T_acao[0]));
                    agente.stoppingDistance = 0;

                    //fazer acao
                    agente.SetDestination(pos_Acoes[1].transform.position);
                    anim.SetFloat("Velocidade", 0.2f);
                    if (Vector3.Distance(transform.position, pos_Acoes[1].transform.position) < 0.3f)
                    {
                        anim.SetFloat("Velocidade", 0);
                        StartCoroutine(Action2(T_acao[1]));
                    }

                    //olhar pro lugar
                        rotation = Quaternion.LookRotation(pos_olhar[1].position - transform.position);
                        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime);
                        //transform.LookAt(pos_olhar[1].position);
                    break;

                case "Açao3":
                    Gritado = false;
                    visto = false;
                    //parar as coroutines
                    StopCoroutine(Action2(T_acao[1]));

                    //fazer acao
                    agente.stoppingDistance = 0;
                    agente.SetDestination(pos_Acoes[2].transform.position);
                    anim.SetFloat("Velocidade", 0.2f);
                    if (Vector3.Distance(transform.position, pos_Acoes[2].transform.position) < 0.3f)
                    {
                        anim.SetFloat("Velocidade", 0);
                        StartCoroutine(Action3(T_acao[2]));
                    }

                    //olhar pro lugar
                        rotation = Quaternion.LookRotation(pos_olhar[2].position - transform.position);
                        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime);
                        //transform.LookAt(pos_olhar[2].position);
                    break;
            }
        }

        if(NPC_ID==7)
        {
            switch (State)
            {
                case "Screaming":
                    this.StopCoroutine(Action1(T_acao[0]));
                    this.StopCoroutine(Action2(T_acao[1]));
                    this.StopCoroutine(Action3(T_acao[2]));
                    this.StopCoroutine(Action4(T_acao[3]));
                    this.StopCoroutine(Action5(T_acao[4]));

                    visto = false;

                    agente.stoppingDistance = 50;
                    anim.SetFloat("Velocidade", 0);
                    transform.LookAt(alvo.transform.position);
                    agente.SetDestination(player.transform.position);
                    Boss_AI.State = Globals.ACT_RUN;

                    if (Gritado == false)
                    {
                        audioPlay.M_Grito();
                        Gritado = true;
                    }
                    //print("AAAAAAAAAAAAAAAHHHHHHHHHHHHHH");
                    break;

                case "SoundHeard":
                    Gritado = false;
                    if (!visto)
                    {
                        audioPlay.M_Where();
                        visto = true;
                    }
                    //parar as coroutines
                    this.StopCoroutine(Action1(T_acao[0]));
                    this.StopCoroutine(Action2(T_acao[1]));
                    this.StopCoroutine(Action3(T_acao[2]));
                    this.StopCoroutine(Action4(T_acao[3]));
                    this.StopCoroutine(Action5(T_acao[4]));
                    agente.stoppingDistance = 0;

                    //fazer acao
                    agente.SetDestination(soundHeardVector);
                    anim.SetFloat("Velocidade", 0.2f);
                    if (Vector3.Distance(this.transform.position, soundHeardVector) <= 1)
                    {
                        anim.SetFloat("Velocidade", 0);
                        StartCoroutine("SoundHeard");
                    }
                    break;

                case "Açao1":
                    Gritado = false;
                    visto = false;
                    //parar as coroutines
                    this.StopCoroutine(Action1(T_acao[0]));
                    this.StopCoroutine(Action2(T_acao[1]));
                    this.StopCoroutine(Action3(T_acao[2]));
                    this.StopCoroutine(Action4(T_acao[3]));
                    this.StopCoroutine(Action5(T_acao[4]));

                    //fazer acao
                    if (NPC_ID != 0)
                    {

                        agente.SetDestination(pos_Acoes[0].transform.position);
                        agente.stoppingDistance = 0;
                        anim.SetFloat("Velocidade", 0.2f);
                        if (Vector3.Distance(transform.position, pos_Acoes[0].transform.position) < 0.3f)
                        {
                            anim.SetFloat("Velocidade", 0);
                            StartCoroutine(Action1(T_acao[0]));
                        }
                    }

                    //olhar pro lugar
                        var rotation = Quaternion.LookRotation(pos_olhar[0].position - transform.position);
                        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime);
                        //transform.LookAt(pos_olhar[0].position);
                    break;

                case "Açao2":
                    Gritado = false;
                    visto = false;
                    //parar as coroutines
                    StopCoroutine(Action1(T_acao[0]));
                    agente.stoppingDistance = 0;

                    //fazer acao
                    agente.SetDestination(pos_Acoes[1].transform.position);
                    anim.SetFloat("Velocidade", 0.2f);
                    if (Vector3.Distance(transform.position, pos_Acoes[1].transform.position) < 0.3f)
                    {
                        anim.SetFloat("Velocidade", 0);
                        StartCoroutine(Action2(T_acao[1]));
                    }

                    //olhar pro lugar
                        rotation = Quaternion.LookRotation(pos_olhar[1].position - transform.position);
                        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime);
                        //transform.LookAt(pos_olhar[1].position);
                    break;
            }
        }

        if(NPC_ID==8)
        {
            switch (State)
            {
                case "Screaming":
                    this.StopCoroutine(Action1(T_acao[0]));
                    this.StopCoroutine(Action2(T_acao[1]));
                    this.StopCoroutine(Action3(T_acao[2]));
                    this.StopCoroutine(Action4(T_acao[3]));
                    this.StopCoroutine(Action5(T_acao[4]));

                    visto = false;

                    agente.stoppingDistance = 50;
                    anim.SetFloat("Velocidade", 0);
                    transform.LookAt(alvo.transform.position);
                    agente.SetDestination(player.transform.position);
                    Boss_AI.State = Globals.ACT_RUN;

                    if (Gritado == false)
                    {
                        audioPlay.M_Grito();
                        Gritado = true;
                    }
                    //print("AAAAAAAAAAAAAAAHHHHHHHHHHHHHH");
                    break;

                case "SoundHeard":
                    Gritado = false;
                    if (!visto)
                    {
                        audioPlay.M_Where();
                        visto = true;
                    }
                    //parar as coroutines
                    this.StopCoroutine(Action1(T_acao[0]));
                    this.StopCoroutine(Action2(T_acao[1]));
                    this.StopCoroutine(Action3(T_acao[2]));
                    this.StopCoroutine(Action4(T_acao[3]));
                    this.StopCoroutine(Action5(T_acao[4]));
                    agente.stoppingDistance = 0;

                    //fazer acao
                    agente.SetDestination(soundHeardVector);
                    anim.SetFloat("Velocidade", 0.2f);
                    if (Vector3.Distance(this.transform.position, soundHeardVector) <= 1)
                    {
                        anim.SetFloat("Velocidade", 0);
                        StartCoroutine("SoundHeard");
                    }
                    break;

                case "Açao1":
                    Gritado = false;
                    visto = false;
                    //parar as coroutines
                    this.StopCoroutine(Action1(T_acao[0]));
                    this.StopCoroutine(Action2(T_acao[1]));
                    this.StopCoroutine(Action3(T_acao[2]));
                    this.StopCoroutine(Action4(T_acao[3]));
                    this.StopCoroutine(Action5(T_acao[4]));

                        //fazer acao
                        agente.SetDestination(pos_Acoes[0].transform.position);
                        agente.stoppingDistance = 0;
                        anim.SetFloat("Velocidade", 0.2f);
                        if (Vector3.Distance(transform.position, pos_Acoes[0].transform.position) < 0.3f)
                        {
                            anim.SetFloat("Velocidade", 0);
                            StartCoroutine(Action1(T_acao[0]));
                        }
                    break;

                case "Açao2":
                    Gritado = false;
                    visto = false;
                    //parar as coroutines
                    StopCoroutine(Action1(T_acao[0]));
                    agente.stoppingDistance = 0;

                    //fazer acao
                    agente.SetDestination(pos_Acoes[1].transform.position);
                    anim.SetFloat("Velocidade", 0.2f);
                    if (Vector3.Distance(transform.position, pos_Acoes[1].transform.position) < 0.3f)
                    {
                        anim.SetFloat("Velocidade", 0);
                        StartCoroutine(Action2(T_acao[1]));
                    }
                    break;

                case "Açao3":
                    Gritado = false;
                    visto = false;
                    //parar as coroutines
                    StopCoroutine(Action2(T_acao[1]));

                    //fazer acao
                    agente.stoppingDistance = 0;
                    agente.SetDestination(pos_Acoes[2].transform.position);
                    anim.SetFloat("Velocidade", 0.2f);
                    if (Vector3.Distance(transform.position, pos_Acoes[2].transform.position) < 0.3f)
                    {
                        anim.SetFloat("Velocidade", 0);
                        StartCoroutine(Action3(T_acao[2]));
                    }
                    break;

                case "Açao4":
                    Gritado = false;
                    visto = false;
                    //parar as coroutines
                    StopCoroutine(Action2(T_acao[2]));

                    //fazer acao
                    agente.stoppingDistance = 0;
                    agente.SetDestination(pos_Acoes[3].transform.position);
                    anim.SetFloat("Velocidade", 0.2f);
                    if (Vector3.Distance(transform.position, pos_Acoes[3].transform.position) < 0.3f)
                    {
                        anim.SetFloat("Velocidade", 0);
                        StartCoroutine(Action4(T_acao[3]));
                    }
                    break;
            }
        }

        if(NPC_ID==9)
        {
            switch (State)
            {
                case "Screaming":
                    this.StopCoroutine(Action1(T_acao[0]));
                    this.StopCoroutine(Action2(T_acao[1]));
                    this.StopCoroutine(Action3(T_acao[2]));
                    this.StopCoroutine(Action4(T_acao[3]));
                    this.StopCoroutine(Action5(T_acao[4]));

                    visto = false;

                    agente.stoppingDistance = 50;
                    anim.SetFloat("Velocidade", 0);
                    transform.LookAt(alvo.transform.position);
                    agente.SetDestination(player.transform.position);
                    Boss_AI.State = Globals.ACT_RUN;

                    if (Gritado == false)
                    {
                        audioPlay.M_Grito();
                        Gritado = true;
                    }
                    //print("AAAAAAAAAAAAAAAHHHHHHHHHHHHHH");
                    break;

                case "SoundHeard":
                    Gritado = false;
                    if (!visto)
                    {
                        audioPlay.M_Where();
                        visto = true;
                    }
                    //parar as coroutines
                    this.StopCoroutine(Action1(T_acao[0]));
                    this.StopCoroutine(Action2(T_acao[1]));
                    this.StopCoroutine(Action3(T_acao[2]));
                    this.StopCoroutine(Action4(T_acao[3]));
                    this.StopCoroutine(Action5(T_acao[4]));
                    agente.stoppingDistance = 0;

                    //fazer acao
                    agente.SetDestination(soundHeardVector);
                    anim.SetFloat("Velocidade", 0.2f);
                    if (Vector3.Distance(this.transform.position, soundHeardVector) <= 1)
                    {
                        anim.SetFloat("Velocidade", 0);
                        StartCoroutine("SoundHeard");
                    }
                    break;

                case "Açao1":
                    Gritado = false;
                    visto = false;
                    //parar as coroutines
                    this.StopCoroutine(Action1(T_acao[0]));
                    this.StopCoroutine(Action2(T_acao[1]));
                    this.StopCoroutine(Action3(T_acao[2]));
                    this.StopCoroutine(Action4(T_acao[3]));
                    this.StopCoroutine(Action5(T_acao[4]));

                    //fazer acao
                    if (NPC_ID != 0)
                    {

                        agente.SetDestination(pos_Acoes[0].transform.position);
                        agente.stoppingDistance = 0;
                        anim.SetFloat("Velocidade", 0.2f);
                        if (Vector3.Distance(transform.position, pos_Acoes[0].transform.position) < 0.3f)
                        {
                            anim.SetFloat("Velocidade", 0);
                            StartCoroutine(Action1(T_acao[0]));
                        }
                    }
                    break;

                case "Açao2":
                    Gritado = false;
                    visto = false;
                    //parar as coroutines
                    StopCoroutine(Action1(T_acao[0]));
                    agente.stoppingDistance = 0;

                    //fazer acao
                    agente.SetDestination(pos_Acoes[1].transform.position);
                    anim.SetFloat("Velocidade", 0.2f);
                    if (Vector3.Distance(transform.position, pos_Acoes[1].transform.position) < 0.3f)
                    {
                        anim.SetFloat("Velocidade", 0);
                        StartCoroutine(Action2(T_acao[1]));
                    }
                    break;

                case "Açao3":
                    Gritado = false;
                    visto = false;
                    //parar as coroutines
                    StopCoroutine(Action2(T_acao[1]));

                    //fazer acao
                    agente.stoppingDistance = 0;
                    agente.SetDestination(pos_Acoes[2].transform.position);
                    anim.SetFloat("Velocidade", 0.2f);
                    if (Vector3.Distance(transform.position, pos_Acoes[2].transform.position) < 0.3f)
                    {
                        anim.SetFloat("Velocidade", 0);
                        StartCoroutine(Action3(T_acao[2]));
                    }
                    break;
            }
        }

        if(NPC_ID==10)
        {
            switch (State)
            {
                case "Screaming":
                    this.StopCoroutine(Action1(T_acao[0]));
                    this.StopCoroutine(Action2(T_acao[1]));
                    this.StopCoroutine(Action3(T_acao[2]));
                    this.StopCoroutine(Action4(T_acao[3]));
                    this.StopCoroutine(Action5(T_acao[4]));

                    visto = false;

                    agente.stoppingDistance = 50;
                    anim.SetFloat("Velocidade", 0);
                    transform.LookAt(alvo.transform.position);
                    agente.SetDestination(player.transform.position);
                    Boss_AI.State = Globals.ACT_RUN;

                    if (Gritado == false)
                    {
                        audioPlay.M_Grito();
                        Gritado = true;
                    }
                    //print("AAAAAAAAAAAAAAAHHHHHHHHHHHHHH");
                    break;

                case "SoundHeard":
                    Gritado = false;
                    if (!visto)
                    {
                        audioPlay.M_Where();
                        visto = true;
                    }
                    //parar as coroutines
                    this.StopCoroutine(Action1(T_acao[0]));
                    this.StopCoroutine(Action2(T_acao[1]));
                    this.StopCoroutine(Action3(T_acao[2]));
                    this.StopCoroutine(Action4(T_acao[3]));
                    this.StopCoroutine(Action5(T_acao[4]));
                    agente.stoppingDistance = 0;

                    //fazer acao
                    agente.SetDestination(soundHeardVector);
                    anim.SetFloat("Velocidade", 0.2f);
                    if (Vector3.Distance(this.transform.position, soundHeardVector) <= 1)
                    {
                        anim.SetFloat("Velocidade", 0);
                        StartCoroutine("SoundHeard");
                    }
                    break;

                case "Açao1":
                    Gritado = false;
                    visto = false;
                    //parar as coroutines
                    this.StopCoroutine(Action1(T_acao[0]));
                    this.StopCoroutine(Action2(T_acao[1]));
                    this.StopCoroutine(Action3(T_acao[2]));
                    this.StopCoroutine(Action4(T_acao[3]));
                    this.StopCoroutine(Action5(T_acao[4]));

                    //fazer acao
                        agente.SetDestination(pos_Acoes[0].transform.position);
                        agente.stoppingDistance = 0;
                        anim.SetFloat("Velocidade", 0.2f);
                        if (Vector3.Distance(transform.position, pos_Acoes[0].transform.position) < 0.3f)
                        {
                        anim.SetBool("Fumando", true);
                        anim.SetFloat("Velocidade", 0);
                            StartCoroutine(Action1(T_acao[0]));
                        }
                     //olhar pro lugar
                        var rotation = Quaternion.LookRotation(pos_olhar[0].position - transform.position);
                        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime);
                        //transform.LookAt(pos_olhar[0].position);
                    break;

                case "Açao2":
                    Gritado = false;
                    visto = false;
                    //parar as coroutines
                    StopCoroutine(Action1(T_acao[0]));
                    agente.stoppingDistance = 0;
                    anim.SetBool("Fumando", false);
                    //fazer acao
                    agente.SetDestination(pos_Acoes[1].transform.position);
                    anim.SetFloat("Velocidade", 0.2f);
                    if (Vector3.Distance(transform.position, pos_Acoes[1].transform.position) < 0.3f)
                    {
                        anim.SetFloat("Velocidade", 0);
                        StartCoroutine(Action2(T_acao[1]));
                    }

                    //olhar pro lugar
                        rotation = Quaternion.LookRotation(pos_olhar[1].position - transform.position);
                        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime);
                        //transform.LookAt(pos_olhar[1].position);
                    break;

                case "Açao3":
                    Gritado = false;
                    visto = false;
                    //parar as coroutines
                    StopCoroutine(Action2(T_acao[1]));

                    //fazer acao
                    agente.stoppingDistance = 0;
                    agente.SetDestination(pos_Acoes[2].transform.position);
                    anim.SetFloat("Velocidade", 0.2f);
                    if (Vector3.Distance(transform.position, pos_Acoes[2].transform.position) < 0.3f)
                    {
                        anim.SetFloat("Velocidade", 0);
                        StartCoroutine(Action3(T_acao[2]));
                    }

                    //olhar pro lugar
                        rotation = Quaternion.LookRotation(pos_olhar[2].position - transform.position);
                        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime);
                        //transform.LookAt(pos_olhar[2].position);
                    break;

                case "Açao4":
                    Gritado = false;
                    visto = false;
                    //parar as coroutines
                    StopCoroutine(Action2(T_acao[2]));

                    //fazer acao
                    agente.stoppingDistance = 0;
                    agente.SetDestination(pos_Acoes[3].transform.position);
                    anim.SetFloat("Velocidade", 0.2f);
                    if (Vector3.Distance(transform.position, pos_Acoes[3].transform.position) < 0.3f)
                    {
                        anim.SetFloat("Velocidade", 0);
                        StartCoroutine(Action4(T_acao[3]));
                    }

                    //olhar pro lugar
                        rotation = Quaternion.LookRotation(pos_olhar[3].position - transform.position);
                        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime);
                        //transform.LookAt(pos_olhar[3].position);
                    break;

                case "Açao5":
                    Gritado = false;
                    visto = false;
                    //parar as coroutines
                    StopCoroutine(Action2(T_acao[3]));

                    //fazer acao
                    agente.stoppingDistance = 0;
                    agente.SetDestination(pos_Acoes[4].transform.position);
                    anim.SetFloat("Velocidade", 0.2f);
                    if (Vector3.Distance(transform.position, pos_Acoes[4].transform.position) < 0.3f)
                    {
                        anim.SetFloat("Velocidade", 0);
                        StartCoroutine(Action5(T_acao[4]));
                    }

                    //olhar pro lugar
                        rotation = Quaternion.LookRotation(pos_olhar[4].position - transform.position);
                        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime);
                        //transform.LookAt(pos_olhar[4].position);
                    break;



            }
        }

        if (NPC_ID == 11)
        {
            switch (State)
            {
                case "Screaming":
                    this.StopCoroutine(Action1(T_acao[0]));
                    this.StopCoroutine(Action2(T_acao[1]));
                    this.StopCoroutine(Action3(T_acao[2]));
                    this.StopCoroutine(Action4(T_acao[3]));
                    this.StopCoroutine(Action5(T_acao[4]));

                    visto = false;

                    agente.stoppingDistance = 50;
                    anim.SetFloat("Velocidade", 0);
                    transform.LookAt(alvo.transform.position);
                    agente.SetDestination(player.transform.position);
                    Boss_AI.State = Globals.ACT_RUN;

                    if (Gritado == false)
                    {
                        audioPlay.M_Grito();
                        Gritado = true;
                    }
                    //print("AAAAAAAAAAAAAAAHHHHHHHHHHHHHH");
                    break;

                case "SoundHeard":
                    Gritado = false;
                    if (!visto)
                    {
                        audioPlay.M_Where();
                        visto = true;
                    }
                    //parar as coroutines
                    this.StopCoroutine(Action1(T_acao[0]));
                    this.StopCoroutine(Action2(T_acao[1]));
                    this.StopCoroutine(Action3(T_acao[2]));
                    this.StopCoroutine(Action4(T_acao[3]));
                    this.StopCoroutine(Action5(T_acao[4]));
                    agente.stoppingDistance = 0;

                    //fazer acao
                    agente.SetDestination(soundHeardVector);
                    anim.SetFloat("Velocidade", 0.2f);
                    if (Vector3.Distance(this.transform.position, soundHeardVector) <= 1)
                    {
                        anim.SetFloat("Velocidade", 0);
                        StartCoroutine("SoundHeard");
                    }
                    break;

                case "Açao1":
                    Gritado = false;
                    visto = false;
                    //parar as coroutines
                    this.StopCoroutine(Action1(T_acao[0]));
                    this.StopCoroutine(Action2(T_acao[1]));
                    this.StopCoroutine(Action3(T_acao[2]));
                    this.StopCoroutine(Action4(T_acao[3]));
                    this.StopCoroutine(Action5(T_acao[4]));

                    //fazer acao
                        agente.SetDestination(pos_Acoes[0].transform.position);
                        agente.stoppingDistance = 0;
                        anim.SetFloat("Velocidade", 0.2f);
                        if (Vector3.Distance(transform.position, pos_Acoes[0].transform.position) < 0.3f)
                        {
                            anim.SetFloat("Velocidade", 0);
                            StartCoroutine(Action1(T_acao[0]));
                        }
                    break;

                case "Açao2":
                    Gritado = false;
                    visto = false;
                    //parar as coroutines
                    StopCoroutine(Action1(T_acao[0]));
                    agente.stoppingDistance = 0;

                    //fazer acao
                    agente.SetDestination(pos_Acoes[1].transform.position);
                    anim.SetFloat("Velocidade", 0.2f);
                    if (Vector3.Distance(transform.position, pos_Acoes[1].transform.position) < 0.3f)
                    {
                        anim.SetFloat("Velocidade", 0);
                        StartCoroutine(Action2(T_acao[1]));
                    }
                    break;

                case "Açao3":
                    Gritado = false;
                    visto = false;
                    //parar as coroutines
                    StopCoroutine(Action2(T_acao[1]));

                    //fazer acao
                    agente.stoppingDistance = 0;
                    agente.SetDestination(pos_Acoes[2].transform.position);
                    anim.SetFloat("Velocidade", 0.2f);
                    if (Vector3.Distance(transform.position, pos_Acoes[2].transform.position) < 0.3f)
                    {
                        anim.SetFloat("Velocidade", 0);
                        StartCoroutine(Action3(T_acao[2]));
                    }
                    break;

                case "Açao4":
                    Gritado = false;
                    visto = false;
                    //parar as coroutines
                    StopCoroutine(Action2(T_acao[2]));

                    //fazer acao
                    agente.stoppingDistance = 0;
                    agente.SetDestination(pos_Acoes[3].transform.position);
                    anim.SetFloat("Velocidade", 0.2f);
                    if (Vector3.Distance(transform.position, pos_Acoes[3].transform.position) < 0.3f)
                    {
                        anim.SetFloat("Velocidade", 0);
                        StartCoroutine(Action4(T_acao[3]));
                    }
                    break;
            }
        }

        /*switch (State)
        {
            case "Screaming":
                this.StopCoroutine(Action1(T_acao[0]));
                this.StopCoroutine(Action2(T_acao[1]));
                this.StopCoroutine(Action3(T_acao[2]));
                this.StopCoroutine(Action4(T_acao[3]));
                this.StopCoroutine(Action5(T_acao[4]));

                visto = false;

                agente.stoppingDistance = 50;
                anim.SetFloat("Velocidade", 0);
                transform.LookAt(alvo.transform.position);
                agente.SetDestination(player.transform.position);
                Boss_AI.State = Globals.ACT_RUN;

                if (Gritado == false)
                {
                    audioPlay.M_Grito();
                    Gritado = true;
                }
                //print("AAAAAAAAAAAAAAAHHHHHHHHHHHHHH");
                break;

            case "SoundHeard":
                Gritado = false;
                if (!visto)
                {
                    audioPlay.M_Where();
                    visto = true;
                }
                //parar as coroutines
                this.StopCoroutine(Action1(T_acao[0]));
                this.StopCoroutine(Action2(T_acao[1]));
                this.StopCoroutine(Action3(T_acao[2]));
                this.StopCoroutine(Action4(T_acao[3]));
                this.StopCoroutine(Action5(T_acao[4]));
                agente.stoppingDistance = 0;

                //fazer acao
                agente.SetDestination(soundHeardVector);
                anim.SetFloat("Velocidade", 0.2f);
                if (Vector3.Distance(this.transform.position, soundHeardVector) <= 1)
                {
                    anim.SetFloat("Velocidade", 0);
                    StartCoroutine("SoundHeard");
                }
                break;

            case "Açao1":
                Gritado = false;
                visto = false;
                //parar as coroutines
                this.StopCoroutine(Action1(T_acao[0]));
                this.StopCoroutine(Action2(T_acao[1]));
                this.StopCoroutine(Action3(T_acao[2]));
                this.StopCoroutine(Action4(T_acao[3]));
                this.StopCoroutine(Action5(T_acao[4]));

                //fazer acao
                if (NPC_ID != 0)
                {

                    agente.SetDestination(pos_Acoes[0].transform.position);
                    agente.stoppingDistance = 0;
                    anim.SetFloat("Velocidade", 0.2f);
                    if (Vector3.Distance(transform.position, pos_Acoes[0].transform.position) < 1)
                    {
                        anim.SetFloat("Velocidade", 0);
                        StartCoroutine(Action1(T_acao[0]));
                    }
                }

                //olhar pro lugar
                if (NPC_ID == 1 || NPC_ID == 2 || NPC_ID == 4 || NPC_ID == 5 || NPC_ID == 6 || NPC_ID == 7 || NPC_ID == 10)
                {
                    var rotation = Quaternion.LookRotation(pos_olhar[0].position - transform.position);
                    transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime);
                    //transform.LookAt(pos_olhar[0].position);
                }
                break;

            case "Açao2":
                Gritado = false;
                visto = false;
                //parar as coroutines
                StopCoroutine(Action1(T_acao[0]));
                agente.stoppingDistance = 0;

                //fazer acao
                agente.SetDestination(pos_Acoes[1].transform.position);
                anim.SetFloat("Velocidade", 0.2f);
                if (Vector3.Distance(transform.position, pos_Acoes[1].transform.position) < 1)
                {
                    anim.SetFloat("Velocidade", 0);
                    StartCoroutine(Action2(T_acao[1]));
                }

                //olhar pro lugar
                if (NPC_ID == 1 || NPC_ID == 4 || NPC_ID == 5 || NPC_ID == 6 || NPC_ID == 7 || NPC_ID == 10)
                {
                    var rotation = Quaternion.LookRotation(pos_olhar[1].position - transform.position);
                    transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime);
                    //transform.LookAt(pos_olhar[1].position);
                }
                break;

            case "Açao3":
                Gritado = false;
                visto = false;
                //parar as coroutines
                StopCoroutine(Action2(T_acao[1]));

                //fazer acao
                agente.stoppingDistance = 0;
                agente.SetDestination(pos_Acoes[2].transform.position);
                anim.SetFloat("Velocidade", 0.2f);
                if (Vector3.Distance(transform.position, pos_Acoes[2].transform.position) < 1)
                {
                    anim.SetFloat("Velocidade", 0);
                    StartCoroutine(Action3(T_acao[2]));
                }

                //olhar pro lugar
                if (NPC_ID == 4 || NPC_ID == 6 || NPC_ID == 10)
                {
                    var rotation = Quaternion.LookRotation(pos_olhar[2].position - transform.position);
                    transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime);
                    //transform.LookAt(pos_olhar[2].position);
                }
                break;

            case "Açao4":
                Gritado = false;
                visto = false;
                //parar as coroutines
                StopCoroutine(Action2(T_acao[2]));

                //fazer acao
                agente.stoppingDistance = 0;
                agente.SetDestination(pos_Acoes[3].transform.position);
                anim.SetFloat("Velocidade", 0.2f);
                if (Vector3.Distance(transform.position, pos_Acoes[3].transform.position) < 1)
                {
                    anim.SetFloat("Velocidade", 0);
                    StartCoroutine(Action4(T_acao[3]));
                }

                //olhar pro lugar
                if (NPC_ID == 2)
                {
                    transform.LookAt(pos_olhar[1].position);
                }
                else if (NPC_ID == 4 || NPC_ID == 10)
                {
                    var rotation = Quaternion.LookRotation(pos_olhar[3].position - transform.position);
                    transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime);
                    //transform.LookAt(pos_olhar[3].position);
                }
                break;

            case "Açao5":
                Gritado = false;
                visto = false;
                //parar as coroutines
                StopCoroutine(Action2(T_acao[3]));

                //fazer acao
                agente.stoppingDistance = 0;
                agente.SetDestination(pos_Acoes[4].transform.position);
                anim.SetFloat("Velocidade", 0.2f);
                if (Vector3.Distance(transform.position, pos_Acoes[4].transform.position) < 1)
                {
                    anim.SetFloat("Velocidade", 0);
                    StartCoroutine(Action5(T_acao[4]));
                }

                //olhar pro lugar
                if (NPC_ID == 10)
                {
                    var rotation = Quaternion.LookRotation(pos_olhar[4].position - transform.position);
                    transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime);
                    //transform.LookAt(pos_olhar[4].position);
                }
                break;



        }*/
    }

    IEnumerator SoundHeard()
    {

        yield return new WaitForSecondsRealtime(4);
        FOV.NewState(lastAction);
        State = Globals.ACT_AÇAO1;
    }

    private void OnTriggerEnter(Collider _col)
    {
        if (_col.gameObject.CompareTag(Globals.TAG_SOUND_TRIGGER))
        {
            if(NPC_ID != 2)
            {
                soundHeardTransform = _col.gameObject.transform;
                soundHeardVector = soundHeardTransform.position;
                State = Globals.ACT_SOUND_HEARD;
                FOV.NewState(State);
            }
        }
    }
}
