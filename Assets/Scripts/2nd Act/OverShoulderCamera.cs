using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverShoulderCamera : MonoBehaviour
{
    [SerializeField] private float yAngleMin = 0f; //Min limit of the Y angle of the camera 
    [SerializeField] private float yAngleMax = 50f; //Max limit of the Y angle of the camera

    private float currentX = 0f;
    private float currentY = 0f;
    private float sensitivityX = 4f;
    private float sensitivityY = 1f;

    private Quaternion rotation;
    private ThirdPersonUserControl m_player;

    private void Start()
    {
        m_player = GetComponentInParent<ThirdPersonUserControl>();
        this.transform.localRotation = Quaternion.identity;
    }

    private void Awake()
    {
        rotation = this.transform.rotation;
        this.transform.localRotation = Quaternion.identity;
    }

    void Update()
    {
        if (!GameManager.IS_WORLD_PAUSED)
        {
            this.transform.localRotation = Quaternion.identity;

            currentX += Input.GetAxis("Mouse X");
            currentY += Input.GetAxis("Mouse Y") * -1;

            currentY = Mathf.Clamp(currentY, yAngleMin, yAngleMax);
        }
    }

    private void LateUpdate()
    {
        if (!GameManager.IS_WORLD_PAUSED)
        {
            rotation = Quaternion.Euler(currentY, 0 , 0); //up & down

            this.transform.localRotation = rotation;

            m_player.RotateAxis(currentX);
        }
    }
}