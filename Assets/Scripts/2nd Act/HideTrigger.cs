using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideTrigger : MonoBehaviour {

    [SerializeField] private GameObject hidingSpot;

    [SerializeField] private GameObject hideCameraPos;

    public GameObject porta1;
    public GameObject porta2;

    public GameObject TargetHidingPosition()
    {
        return hidingSpot;
    }

    public GameObject TargetCameraHiden()
    {
        return hideCameraPos;
    }

    public void Abrir2()
    {
        porta1.GetComponent<PortaArmario>().Abrir();
        porta2.GetComponent<PortaArmario>().Abrir();
    }

}
