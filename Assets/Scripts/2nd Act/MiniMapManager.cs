using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapManager : MonoBehaviour {

    [SerializeField] private GameObject[] _miniMapRooms1;
    [SerializeField] private GameObject[] _miniMapRooms2;
    [SerializeField] private GameObject[] _miniMapRooms3;

    public static GameObject[] miniMapRooms1;
    public static GameObject[] miniMapRooms2;
    public static GameObject[] miniMapRooms3;

    private void Start()
    {
        miniMapRooms1 = _miniMapRooms1;
        miniMapRooms2 = _miniMapRooms2;
        miniMapRooms3 = _miniMapRooms3;
    }

    public static void CollidingWithPlayer(int _index, int _floor)
    {
        if (_floor == 1)
        {
            for (int i = 0; i < miniMapRooms1.Length; i++)
            {
                if (i == _index)
                {
                    miniMapRooms1[i].SetActive(true);
                }
                else
                {
                    miniMapRooms1[i].SetActive(false);
                }
            }
            for (int i = 0; i < miniMapRooms2.Length; i++)
            {
                miniMapRooms2[i].SetActive(false);
            }
            for (int i = 0; i < miniMapRooms3.Length; i++)
            {
               miniMapRooms3[i].SetActive(false);
            }

        }
        else if (_floor == 2)
        {
            for (int i = 0; i < miniMapRooms2.Length; i++)
            {
                if (i == _index)
                {
                    miniMapRooms2[i].SetActive(true);
                }
                else
                {
                    miniMapRooms2[i].SetActive(false);
                }
            }
            for (int x = 0; x < miniMapRooms1.Length; x++)
            {
                miniMapRooms1[x].SetActive(false);
            }
            for (int y = 0; y < miniMapRooms3.Length; y++)
            {
                miniMapRooms3[y].SetActive(false);
            }
        }
        else 
        {
            for (int i = 0; i < miniMapRooms3.Length; i++)
            {
                if (i == _index)
                {
                    miniMapRooms3[i].SetActive(true);
                }
                else
                {
                    miniMapRooms3[i].SetActive(false);
                }
            }
            for (int x = 0; x < miniMapRooms2.Length; x++)
            {
                miniMapRooms2[x].SetActive(false);
            }
            for (int y = 0; y < miniMapRooms1.Length; y++)
            {
                miniMapRooms1[y].SetActive(false);
            }
        }
    }
}
