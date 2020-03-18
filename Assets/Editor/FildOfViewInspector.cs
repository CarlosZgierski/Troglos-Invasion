using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof(FieldOfView))]
public class FildOfViewInspector : Editor {

    private void OnSceneGUI()
    {
        FieldOfView fow = (FieldOfView)target;
        Handles.color = Color.white;
        Handles.DrawWireArc(fow.transform.position, Vector3.up, Vector3.forward, 360, fow.angleView);
        Vector3 viewAngleA = fow.DirAngle(-fow.angleView / 2, false);
        Vector3 viewAngleb = fow.DirAngle(fow.angleView / 2, false);

        Handles.DrawLine(fow.transform.position, fow.transform.position + viewAngleA * fow.fieldOfView);
        Handles.DrawLine(fow.transform.position, fow.transform.position + viewAngleb * fow.fieldOfView);

        Handles.color = Color.red;
        foreach (Transform Players in fow.PLayers)
        {
            Handles.DrawLine(fow.transform.position, Players.position);
        }
    }
}
