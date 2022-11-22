using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{

    bool started = false;
    float timeAlone = 0;
    
    public float TIME_ALONE_MAX = 5;
    public float SPEED = 0.1f;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            started = true;
        }

        if(started && timeAlone > 0)
        {
            transform.Translate(transform.forward * SPEED * Time.deltaTime);
            timeAlone -= Time.deltaTime;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        timeAlone = TIME_ALONE_MAX;
        Debug.Log("TRIGGER");
        Debug.Log(other);
    }
}
