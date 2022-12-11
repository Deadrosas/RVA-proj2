using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class MarkerPath : MonoBehaviour
{

    public GameObject reference = null;

    void Update(){
    
        if(reference != null && reference.activeSelf){
            //transform.Translate(0,-transform.position.y,0);
            // transform.Rotate(0,0,0);
        }
    }

    public void OnDetect(){
        MarkerManager mm = Camera.main.GetComponent<MarkerManager>();
        bool isStart = false;
        foreach(MyPath path in GetComponentsInChildren<MyPath>()){
            if(path.path == MyPath.PATHS.START){
                isStart = true;
                break;  
            }
        }
        if(isStart)
            mm.start = transform.gameObject;
        else{
            if(mm.start != null){
                reference = mm.start;   
            }
        }
    }

    public void OnLose(){
        MarkerManager mm = Camera.main.GetComponent<MarkerManager>();
        bool isStart = false;
        foreach(MyPath path in GetComponentsInChildren<MyPath>()){
            if(path.path == MyPath.PATHS.START){
                isStart = true;
                break;  
            }
        }
        if(isStart)
            mm.start = null;
    }
}