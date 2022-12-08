using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class MyPath : MonoBehaviour
{
    public enum PATHS {NONE, START , FORWARD, TURN, BRIDGE, END};

    public PATHS path;

    public List<GameObject> connected = new List<GameObject>();

    void setPath(string path){
        UnityEditor.EditorApplication.delayCall+=()=>
        {   
            if(transform.Find("Piece"))DestroyImmediate(transform.Find("Piece").gameObject);
            
            GameObject obj = Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>(path));
            obj.name = "Piece";
            obj.transform.position = transform.position;
            obj.transform.rotation = transform.rotation;
            obj.transform.SetParent(transform);
            if(!obj.transform.parent) DestroyImmediate(obj); 
        }; 
    }

    void Update(){
        if(Input.GetKeyDown(KeyCode.R)){
            GetComponent<BoxCollider>().enabled = true;
        }
    }

    void OnValidate(){
        switch(path){
            case PATHS.START:
            case PATHS.END: 
                setPath("Assets/kenney_natureKit_2.1/Models/OBJ format/ground_pathEndClosed.obj");
                break;
            case PATHS.FORWARD:
                setPath("Assets/kenney_natureKit_2.1/Models/OBJ format/ground_pathStraight.obj");
                break;
            case PATHS.TURN:
                setPath("Assets/kenney_natureKit_2.1/Models/OBJ format/ground_pathBendBank.obj");
                break;
            case PATHS.BRIDGE:
                setPath("Assets/kenney_natureKit_2.1/Models/OBJ format/bridge_woodRoundNarrow.prefab");
                break;
        }
    }
}
