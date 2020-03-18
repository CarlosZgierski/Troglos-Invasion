using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {

    

    [SerializeField] private bool canZoom = false;
    [SerializeField] private float yAngleMin = 0f; //Min limit of the Y angle of the camera 
    [SerializeField] private float yAngleMax = 50f; //Max limit of the Y angle of the camera
    [SerializeField] private float desiredCameraDistance;
    [SerializeField] private GameObject centralHeadReference;
    [SerializeField] private GameObject backCheckReference;
    [Range(0,10)][SerializeField] private float speedOfZoomMulti = 1;
    [Range(0, 10)] [SerializeField] private float speedOfBackZoomMulti = 0.5f;
    [SerializeField] private float maxZoomAlowed = 0.75f;

    private Camera mainCamera;

    [SerializeField] private Transform player;
    private Transform camTransform;

    private float playerDistance; //Distance betwen the camera and the player ,can be altered in the menus in the future
    private float currentX = 0f;
    private float currentY = 0f;
    [SerializeField]private float sensitivityX = 4f;
    [SerializeField]private float sensitivityY = 1f;
    private bool cameraGoingFoward;

    //Variables for the auto correct positioning of the camera
    private Ray cameraCenterRay;
    private RaycastHit cameraCenterRayhit;
    private Ray cameraBackRay;
    private RaycastHit cameraBackRayhit;

    [SerializeField] public LayerMask raycastLayer = 1 << 1 | 1 <<9| 1<<10 | 1<<13 | 1<< 15; //this fucking shit is the layer mask of the raycast

    private void Start()
    {
        camTransform = this.transform;

        mainCamera = Camera.main;

        playerDistance = desiredCameraDistance;
    }

    private void Update()
    {
        if(!GameManager.IS_WORLD_PAUSED)
        {
            currentX += Input.GetAxis("Mouse X");
            currentY += Input.GetAxis("Mouse Y") * -1;

            currentY = Mathf.Clamp(currentY, yAngleMin, yAngleMax); //Limits the Y angle of the camera 

            if (canZoom)
            {
                MouseZoom();
            }
        }
    }

    private void LateUpdate()
    {
        if (!GameManager.IS_WORLD_PAUSED)
        {
            Vector3 direction = new Vector3(0, 0, -playerDistance);

            Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);

            camTransform.position = player.position + rotation * direction;

            camTransform.LookAt(player.position);

            playerDistance = Mathf.Clamp(playerDistance, maxZoomAlowed, desiredCameraDistance);
        }
    }

    private void FixedUpdate()
    {
        if (!GameManager.IS_WORLD_PAUSED)
        {
            KeepGoodDistance();
        }
    }

    //Doing a Zoom in and out with the scroll wheel
    void MouseZoom()
    {
        playerDistance += Input.GetAxis("Mouse ScrollWheel") * -1 * 3; // Makes the zoom in/out based on the scroll of the mouse
        
        playerDistance = Mathf.Clamp(playerDistance, 2f, 5f);
    }

    void KeepGoodDistance()
    {
        cameraCenterRay = new Ray(centralHeadReference.transform.position, centralHeadReference.transform.forward);
        cameraBackRay = new Ray(backCheckReference.transform.position, backCheckReference.transform.forward * -1);

        if (Physics.Raycast(cameraCenterRay, out cameraCenterRayhit, desiredCameraDistance + 0.5f ,raycastLayer))
        {
            if (!cameraCenterRayhit.transform.CompareTag(Globals.TAG_PLAYER))
            {
                if(playerDistance > maxZoomAlowed)
                    MakeCameraGoFoward();
                print("foward");
            }
            else if(cameraCenterRayhit.transform.CompareTag(Globals.TAG_PLAYER))
            {
                if (playerDistance != desiredCameraDistance)
                {
                    if (!Physics.Raycast(cameraBackRay, out cameraBackRayhit, 0.85f, raycastLayer)) //check behind the camera so the camera can go back
                    {
                        MakeCameraGoBack();
                        print("back1");
                    }
                }
            }
        }
        else
        {
            MakeCameraGoFoward();
            print("back2");
        }
    }

    void MakeCameraGoFoward()
    {
        playerDistance -= 0.05f * speedOfZoomMulti; //
    }

    void MakeCameraGoBack()
    {
        playerDistance += 0.05f * speedOfBackZoomMulti; //
    }
}
