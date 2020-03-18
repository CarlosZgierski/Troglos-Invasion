using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTrigger : MonoBehaviour {

    [SerializeField] private int roomNumber;
    [SerializeField] private int roomFloor;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(Globals.TAG_PLAYER))
        {
            MiniMapManager.CollidingWithPlayer(roomNumber, roomFloor);
        }
    }
}
