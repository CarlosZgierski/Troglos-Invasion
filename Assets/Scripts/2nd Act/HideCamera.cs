using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class HideCamera : MonoBehaviour {

    //X rotation limits
    [SerializeField] private float maxAngleX;
    [SerializeField] private float minAngleX;

    //Y rotation limits
    [SerializeField] private float maxAngleY;
    [SerializeField] private float minAngleY;

    //Mouse Sensitivity
    [Range(0.1f, 2)][SerializeField] private float mouseSensitivity = 1f;

    //Lock camera rotation
    private float minY;
    private float maxY;

    private Camera m_Camera;

    private float currentY;
    private float currentX;

    void Start ()
    {
        m_Camera = this.GetComponent<Camera>();
	}

    private void Awake()
    {
        maxY = this.transform.rotation.y + maxAngleY;
        minY = this.transform.rotation.y + minAngleY;

        currentX = this.transform.rotation.y;
        currentY = this.transform.rotation.x;
    }

    void Update ()
    {
        CameraMovement();
	}

    private void CameraMovement()
    {
        if (!GameManager.IS_WORLD_PAUSED)
        {
            this.transform.rotation = Quaternion.identity;

            currentX += Input.GetAxis("Mouse X") * mouseSensitivity;
            currentY += Input.GetAxis("Mouse Y") * -1 * mouseSensitivity;

            currentY = Mathf.Clamp(currentY, minAngleX, maxAngleX); //Limits the Y angle of the camera 
            currentX = Mathf.Clamp(currentX, minY, maxY); //Limits the X angle of the camera 

            Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);

            m_Camera.transform.localRotation = rotation;
        }
    }
}
