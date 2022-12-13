using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunCicle : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
          
    }

    // Update is called once per frame
    void Update()
    {
        float offsetAngle = 0f; // Think of this like a time zone
        float hoursAngle = 360f / 24f;
        float minuteAngle = 360f / (24f * 60f);
        float secondAngle = 360f / (24f * 60f * 60f);
        DateTime currentTime = System.DateTime.Now;
        float currentSunAngle = offsetAngle +
            currentTime.Hour * hoursAngle +
            currentTime.Minute * minuteAngle +
            currentTime.Second * secondAngle; 
        transform.rotation = Quaternion.Euler(currentSunAngle,0,0);
    }
}
