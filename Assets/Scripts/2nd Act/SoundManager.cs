using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

    //the best way to balance noise level is by using it as units (Meters), and it goes between a range(ie. 1-5)

    //call this method everytime something makes anykind of sound
    public IEnumerator MakeNoise(GameObject _trigger, int _noiseLevel, float _soundDuration)
    {
        _trigger.GetComponent<SphereCollider>().radius = _noiseLevel;

        yield return new WaitForSeconds(_soundDuration);

        _trigger.GetComponent<SphereCollider>().radius = 0.25f;
    }

    public IEnumerator MakeNoise(GameObject _trigger, int _noiseLevel, float _soundDuration, float _originalNoiseLevel)
    {
        _trigger.GetComponent<SphereCollider>().radius = _noiseLevel;

        yield return new WaitForSeconds(_soundDuration);

        _trigger.GetComponent<SphereCollider>().radius = _originalNoiseLevel;
    }

}
