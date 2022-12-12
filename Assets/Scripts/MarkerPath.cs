using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class MarkerPath : MonoBehaviour
{

    float positionResolution = 5;
    float rotationResolution = 0.02f;
    float t = 0;

    void Update(){
        t += Time.deltaTime;

        if(t > 0.5){
            t = 0;
            return; 
        }

        if(transform.eulerAngles.x >= 5)
            transform.Rotate(-transform.eulerAngles.x,0,0);
        if(transform.eulerAngles.z >= 5)
            transform.Rotate(0,0,-transform.eulerAngles.z);
        //transform.Translate(0,-transform.position.y,0);

        Vector3 rot = new Vector3(transform.eulerAngles.x, Mathf.Round(transform.eulerAngles.y*rotationResolution)/rotationResolution, transform.eulerAngles.z);

        transform.eulerAngles = rot;

        Vector3 pos = new Vector3(Mathf.Round(transform.position.x*positionResolution)/positionResolution,0,Mathf.Round(transform.position.z*positionResolution)/positionResolution);
        transform.position = pos;

    }
}