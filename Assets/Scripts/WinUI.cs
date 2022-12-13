using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class WinUI : MonoBehaviour
{

    bool hasSet = false;
    public TextMeshProUGUI score;

    public void NextScene(string name){
        SceneManager.LoadScene(name);
        Debug.Log("Next Scene");
    }

    IEnumerator setScoreP(int s){
        for(int i =0; i< s; i++){
            yield return new WaitForSeconds(1f);
            score.text += "K";
        }
    }

    public void setScore(int s){
        if(hasSet) return;
        StartCoroutine(setScoreP(s));
        hasSet = true;
    }
}
