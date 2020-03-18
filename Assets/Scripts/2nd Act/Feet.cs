using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Feet : MonoBehaviour {

    private bool onTopOfCover = false;
    private bool onTopOfVault = false;
    private bool onTopOfHide = false;
    private bool onTopOfPush = false;
    private bool onTopOfGlass = false;

    private float coverRotationY = 0;
    private Quaternion pushRotationY;

    private GameObject hidePortaOpen = null;
    private GameObject hideCameraParent = null;
    private GameObject pushedObject;
    private GameObject pushedObjTrigger;
    [SerializeField]
    private GameObject targetVault = null;
    private GameObject hideTarget = null;

    private GameObject hidingCameraPosition = null;

    //luz do aparelho
    //public GameObject spotLight;

    private int floorIndex;

    #region Triggers
    private void OnTriggerEnter(Collider _col)
    {
        if (_col.CompareTag(Globals.TAG_LIGHT)) //Light
        {
            //spotLight.SetActive(true);
        }
        if (_col.CompareTag(Globals.TAG_FLOOR_COVER)) //Cover
        {
            onTopOfCover = true;
            coverRotationY = _col.transform.rotation.y;
        }
        if (_col.CompareTag(Globals.TAG_FLOOR_VAULT)) //Vault
        {
            onTopOfVault = true;
            targetVault = _col.GetComponent<VaultTrigger>().VaultTarget();
        }
        if (_col.CompareTag(Globals.TAG_FLOOR_HIDE)) //Hide
        {
            onTopOfHide = true;
            hideTarget = _col.GetComponent<HideTrigger>().TargetHidingPosition();
            hidingCameraPosition = _col.GetComponent<HideTrigger>().TargetCameraHiden();
            hideCameraParent = _col.GetComponent<HideTrigger>().TargetCameraHiden();
            hidePortaOpen = _col.gameObject;
            pushRotationY = _col.transform.rotation;
        }
        if(_col.CompareTag(Globals.TAG_FLOOR_PUSH)) //Push
        {
            onTopOfPush = true;
            pushRotationY = _col.transform.rotation;
            pushedObject = _col.transform.parent.gameObject;
            pushedObjTrigger = _col.gameObject;
        }
        if(_col == null)
        {
            onTopOfCover = false;
            onTopOfVault = false;
            onTopOfHide = false;
            onTopOfPush = false;
        }
        if (_col.CompareTag(Globals.TAG_GLASS_FLOOR))
        {
            floorIndex = 3;
            onTopOfGlass = true;
        }

    }

    private void OnTriggerExit(Collider _col)
    {
        if (_col.CompareTag(Globals.TAG_LIGHT))
        {
            //spotLight.SetActive(false);
        }
        if (_col.CompareTag(Globals.TAG_FLOOR_COVER))
        {
            onTopOfCover = false;
            coverRotationY = 0;
        }
        if (_col.CompareTag(Globals.TAG_FLOOR_VAULT))
        {
            onTopOfVault = false;
            targetVault = null;
        }
        if (_col.CompareTag(Globals.TAG_FLOOR_HIDE))
        {
            onTopOfHide = false;
        }
        if (_col.CompareTag(Globals.TAG_FLOOR_PUSH))
        {
            onTopOfPush = false;
        }
        if (_col.CompareTag(Globals.TAG_GLASS_FLOOR))
        {
            onTopOfGlass = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other == null)
        {
            onTopOfCover = false;
            onTopOfVault = false;
            onTopOfHide = false;
            onTopOfPush = false;
        }

        //Floor Check

        if (!onTopOfGlass)
        {
            if (other.CompareTag(Globals.TAG_WOOD_FLOOR))
            {
                floorIndex = 1;
            }
            else if (other.CompareTag(Globals.TAG_CONCRETE_FLOOR))
            {
                floorIndex = 2;
            }
            else if (other.CompareTag(Globals.TAG_CARPET_FLOOR))
            {
                floorIndex = 0;
            }
            else
            {
                floorIndex = 1;
            }
        }
    }
    #endregion

    //Functions that return if the feet is coliding with any of the triggers that allow or constrain the player
    public bool FeetCoverBool()
    {
        return onTopOfCover;
    }
    public bool FeetVaultBool()
    {
        return onTopOfVault;
    }
    public bool FeetHideBool()
    {
        return onTopOfHide;
    }
    public bool FeetPushBool()
    {
        return onTopOfPush;
    }

    public float CoverRotation()
    {
        return coverRotationY;
    }
    public Quaternion PushRotation()
    {
        return pushRotationY;
    }


    public GameObject PushedObject()
    {
        return pushedObject;
    }

    public GameObject PushedObjTrigger()
    {
        return pushedObjTrigger;
    }
    
    public Vector3 VaultTarget()
    {
        if (targetVault != null)
        {
            Vector3 _prov = targetVault.transform.position;
            return _prov;
        }
        else return new Vector3(0,0,0);
    }

    //Hiding funcitons for the player controller
    public Vector3 HideTargetReturn()
    {
        if(hideTarget != null)
        {
            Vector3 _prov = hideTarget.transform.position;
            return _prov;
        }
        else return new Vector3(0, 0, 0);
    }

    public Vector3 HideCameraPosition()
    {
        if(hidingCameraPosition != null)
        {
            Vector3 _prov = hidingCameraPosition.transform.position;
            return _prov;
        }
        else return new Vector3(0, 0, 0);
    }

    public GameObject HideCameraParent()
    {
        if (hideCameraParent != null)
        {
            return hideCameraParent;
        }
        else return null;
    }

    public GameObject HidePorta()
    {
        if (hidePortaOpen != null)
        {
            return hidePortaOpen;
        }
        else return null;
    }
    //-----------------------------------------------

    public int CurrentTypeOfGround()
    {
        return floorIndex;
    }

}
