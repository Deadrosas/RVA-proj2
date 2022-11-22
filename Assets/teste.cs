using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class teste : MonoBehaviour
{

    bool isFound = false;

    public void setFound(bool found)
    {
        isFound = found;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        if(isFound)
        {
            transform.Translate(0, 0, 0.001f);
            Debug.Log("Found");
        }
    }
}
