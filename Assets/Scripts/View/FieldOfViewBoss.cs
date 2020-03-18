using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.Generic;

public class FieldOfViewBoss : MonoBehaviour {
    public float fieldOfView;
    [Range(0, 360)]
    public float angleView;

    public LayerMask playerMask;
    public LayerMask obstacleMask;

    [HideInInspector]
    public List<Transform> PLayers = new List<Transform>();

    private Transform viwedPlayer;

    public static  bool vision;

    void Start()
    {
        StartCoroutine("FindVisibleTargetsWithDelay", 0.2f);
    }

    private void FixedUpdate()
    {
        if (vision)
        {
            Boss_AI.boss_player = PLayers[0];
        }
        else
        {
            Boss_AI.boss_player = null;
        }
    }


    IEnumerator FindVisibleTargetsWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            FindVisibleTargets();
        }
    }

    void FindVisibleTargets()
    {
        Collider[] PlayerTarget = Physics.OverlapSphere(transform.position, fieldOfView, playerMask);
        for (int i = 0; i < PlayerTarget.Length; i++)
        {
            Transform target = PlayerTarget[i].transform;
            Vector3 dirToPlayer = (target.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, dirToPlayer) < angleView / 2)
            {
                float distToPlayer = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, dirToPlayer, distToPlayer, obstacleMask))
                {
                    vision = true;
                    Boss_AI.State = Globals.ACT_RUN;
                    PLayers.Add(target);
                }
                else
                {
                    vision = false;
                    Boss_AI.State = Globals.ACT_ROTA1;
                    PLayers.Clear();
                }
            }
        }
    }


    public Vector3 DirAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.localEulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}
