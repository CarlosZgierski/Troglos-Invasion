using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.Generic;

public class FieldOfView : MonoBehaviour {

    public float fieldOfView;
    [Range(0,360)]
    public float angleView;

    public Transform alvo;

    public LayerMask playerMask;
    public LayerMask obstacleMask;

    [HideInInspector]
    public List<Transform> PLayers = new List<Transform>();

    private bool distancia;

    private NPC_AI Bot;

    private string grito;

    private bool visto;

    private string State;

    void Start()
    {
        Bot = GetComponent<NPC_AI>();
        StartCoroutine("FindVisibleTargetsWithDelay", 0.2f);
    }

    private void FixedUpdate()
    {
        if(visto)
        {
          NPC_AI.player = PLayers[0];
        }
    }

    IEnumerator FindVisibleTargetsWithDelay (float delay)
    {
        while(true)
        {
            yield return new WaitForSeconds(delay);
            FindVisibleTargets();
        }
    }

    void FindVisibleTargets()
    {
        Collider[] PlayerTarget = Physics.OverlapSphere (transform.position, fieldOfView, playerMask);
          for (int i = 0; i< PlayerTarget.Length; i++)
          {
            Transform target = PlayerTarget[i].transform;
            Vector3 dirToPlayer = (target.position - transform.position).normalized;
            if(Vector3.Angle (transform.forward, dirToPlayer) < angleView/2)
            {
                float distToPlayer = Vector3.Distance(transform.position, target.position);

                if(!Physics.Raycast(transform.position, dirToPlayer, distToPlayer, obstacleMask) && distancia)
                {
                 visto = true;
                 PLayers.Add(alvo);
                 grito = Globals.ACT_SCREAMING;
                } else if(State == Globals.ACT_SOUND_HEARD)
                 {
                    visto = false;
                    grito = Bot.lastAction;
                    PLayers.Clear(); 
                 }else
                {
                    visto = false;
                    grito = Bot.lastAction;
                    PLayers.Clear();
                }
            } 
          }
    }

    public string ReGrito()
    {
        string estado;
        estado = grito;
        return estado ;
    }

    public string ReState()
    {
        string estado;
        estado = State;
        return estado;
    }

    public void NewState(string Estadao)
    {
        State = Estadao;
    }

    public void DistCerta(bool dist)
    {
        distancia = dist;
    }

    public Vector3 DirAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if(!angleIsGlobal)
        {
            angleInDegrees += transform.localEulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}
