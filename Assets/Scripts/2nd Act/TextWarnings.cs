using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextWarnings : MonoBehaviour {

    public Text tt;
    //-------------------------------
    public string[] movObj;
    public int[] textTime_movObj;
    private bool info_MovObj;
    //-------------------------------
    public string[] lowMov;
    public int[] textTime_lowMov;
    private bool info_LowMov;
    //-------------------------------
    public string[] wrongWay;
    public int[] textTime_wrongWay;
    private bool info_WrongWay;
    //-------------------------------
    public string[] pickupObj;
    private bool pickup;
    public int[] textTime_pickupObj;
    private bool info_PickupObj;
    //-------------------------------
    public string[] goSpeedGo;
    public int[] textTime_goSpeedG;
    private bool info_GoSpeedGo;
    //-------------------------------
    public string[] spotSaco;
    public int[] textTime_spotSaco;
    private bool info_SpotSaco;
    //-------------------------------
    public string[] fakeMission;
    public int[] textTime_fakeMission;
    private bool info_FakeMission;
    //-------------------------------

    public AudioClip[] falas;
    public AudioSource som;

	// Use this for initialization
	void Start () {
        pickup = false;
        info_MovObj = true;
        info_LowMov = true;
        info_WrongWay = true;
        info_PickupObj = true;
        info_GoSpeedGo = true;
        info_SpotSaco = true;
        info_FakeMission = true;


        som.volume *= Globals.SOUND_GENERAL_SLIDER;
    }

    IEnumerator MovObj(float A, float B, float C)
    {
      //  playerRef.AllowPlayerMovement(false);
        Debug.Log("gcvsdbalkba");
        yield return new WaitForSeconds(1f);
        tt.text = movObj[0];
        yield return new WaitForSeconds(A);
        tt.text = movObj[1];
        yield return new WaitForSeconds(B);
        tt.text = movObj[2];
        yield return new WaitForSeconds(C);
        tt.text = null;

      //  playerRef.AllowPlayerMovement(true);
    }

    IEnumerator LowMov(float A, float B)
    {
        yield return new WaitForSeconds(1f);
        tt.text = lowMov[0];
        yield return new WaitForSeconds(A);
        tt.text = lowMov[1];
        yield return new WaitForSeconds(B);
        tt.text = null;
        
    }

    IEnumerator WrongWay(float A, float B)
    {
        yield return new WaitForSeconds(1f);
        tt.text = wrongWay[0];
        yield return new WaitForSeconds(A);
        tt.text = wrongWay[1];
        yield return new WaitForSeconds(B);
        tt.text = null;
        
    }

    IEnumerator PickupObj(float A, float B)
    {
        yield return new WaitForSeconds(1f);
        tt.text = pickupObj[0];
        yield return new WaitForSeconds(A);
        tt.text = pickupObj[1];
        yield return new WaitForSeconds(B);
        tt.text = null;
        pickup = true;

    }

    IEnumerator GoSpeedGo(float A)
    {
        yield return new WaitForSeconds(1f);
        tt.text = goSpeedGo[0];
        yield return new WaitForSeconds(A);
        tt.text = null;
        
    }

    IEnumerator SpotSaco(float A, float B, float C, float D)
    {
        yield return new WaitForSeconds(1f);
        tt.text = spotSaco[0];
        yield return new WaitForSeconds(A);
        tt.text = spotSaco[1];
        info_FakeMission = true;
        yield return new WaitForSeconds(B);
        som.Stop();
        som.clip = falas[6];
        som.Play();
        tt.text = fakeMission[2];
        yield return new WaitForSeconds(C);
        tt.text = fakeMission[3];
        yield return new WaitForSeconds(D);
        tt.text = null;

    }

    IEnumerator FakeMission(float A, float B, float C, float D)
    {
        yield return new WaitForSeconds(1f);
        tt.text = fakeMission[0];
        yield return new WaitForSeconds(A);
        tt.text = fakeMission[1];
        yield return new WaitForSeconds(B);
        
    }

    private void OnTriggerStay(Collider other)
    {

        if (other.CompareTag("movObj"))
        {
            if (info_MovObj)
            {
                info_MovObj = false;
                som.Stop();
                som.clip = falas[0];
                som.Play();
                StartCoroutine(MovObj(textTime_movObj[0], textTime_movObj[1], textTime_movObj[2]));
            }
        }

        else if (other.CompareTag("lowMov"))
        {
            if (info_LowMov)
            {
                info_MovObj = false;
                info_LowMov = false;
                som.Stop();
                som.clip = falas[1];
                som.Play();
                StartCoroutine(LowMov(textTime_lowMov[0], textTime_lowMov[1]));

            }
        }

        else if (other.CompareTag("wrongWay"))
        {
            if (info_WrongWay)
            {
                info_MovObj = false;
                info_LowMov = false;
                info_WrongWay = false;
                som.Stop();
                som.clip = falas[2];
                som.Play();
                StartCoroutine(WrongWay(textTime_wrongWay[0], textTime_wrongWay[1]));
            }
        }

        else if (other.CompareTag("pickupObj") && !pickup)
        {
            if (info_PickupObj)
            {
                info_MovObj = false;
                info_LowMov = false;
                info_WrongWay = false;
                info_PickupObj = false;
                som.Stop();
                som.clip = falas[3];
                som.Play();
                StartCoroutine(PickupObj(textTime_pickupObj[0], textTime_pickupObj[1]));
            }

        }

        else if (other.CompareTag("goSpeedGo") && pickup)
        {
            if (info_GoSpeedGo)
            {
                info_MovObj = false;
                info_LowMov = false;
                info_WrongWay = false;
                info_PickupObj = false;
                info_GoSpeedGo = false;
                som.Stop();
                som.clip = falas[4];
                som.Play();
                StartCoroutine(GoSpeedGo(textTime_goSpeedG[0]));
            }
        }

        else if (other.CompareTag("spotSaco"))
        {
            if (info_SpotSaco)
            {
                info_MovObj = false;
                info_LowMov = false;
                info_WrongWay = false;
                info_PickupObj = false;
                info_GoSpeedGo = false;
                info_SpotSaco = false;
                info_FakeMission = false;
                som.Stop();
                som.clip = falas[5];
                som.Play();
                StartCoroutine(SpotSaco(textTime_spotSaco[0], textTime_spotSaco[1], textTime_spotSaco[2], textTime_spotSaco[3]));
            }
        }

        else if (other.CompareTag("fakeMission"))
        {
            if (info_FakeMission)
            {
                info_MovObj = false;
                info_LowMov = false;
                info_WrongWay = false;
                info_PickupObj = false;
                info_GoSpeedGo = false;
                info_SpotSaco = false;
                info_FakeMission = false;
                som.Stop();
                som.clip = falas[6];
                som.Play();
                StartCoroutine(FakeMission(textTime_fakeMission[0], textTime_fakeMission[1], textTime_fakeMission[2], textTime_fakeMission[3]));
            }
        }
    }
}
