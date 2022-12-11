using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.Rotate(0,Random.Range(0,360),0);   
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R)){
            GetComponentInChildren<SkinnedMeshRenderer>().enabled = true;
            GetComponent<BoxCollider>().enabled = true;
        }
        transform.Rotate(0,30*Time.deltaTime,0);
    }

    public void Collect(){
        GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
        GetComponent<BoxCollider>().enabled = false;
    }
}
