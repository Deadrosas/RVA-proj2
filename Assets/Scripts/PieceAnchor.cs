using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceAnchor : MonoBehaviour
{
	public readonly object connectedLock = new object();
	MyPath selfPath;

	List<GameObject> connected = new List<GameObject>();

	void Awake(){
		selfPath = GetComponentInParent<MyPath>();
	}

	void OnTriggerStay(Collider col){
		foreach(GameObject other in connected){
			other.transform.position = transform.position;
		}
	}

    void OnTriggerEnter(Collider other)
    {
        MyPath otherPath = other.GetComponent<MyPath>();
	
		if(otherPath){
			lock(connectedLock){
				Debug.Log(other.gameObject + "	,	"+transform.parent.gameObject);
				if(other.gameObject == transform.parent.gameObject){
					return;
				}

				if(otherPath.connected.Contains(transform.parent.gameObject))
					return;
				
				selfPath.connected.Add(other.gameObject);
				connected.Add(other.gameObject);
			}
		}
    }

	void OnTriggerExit(Collider other){
		if(selfPath.connected.Contains(other.gameObject))
			selfPath.connected.Remove(other.gameObject);

		if(connected.Contains(other.gameObject))
			connected.Remove(other.gameObject);
	}
}
