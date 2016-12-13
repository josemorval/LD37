using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path : MonoBehaviour {
	public List<Node> nodes;

	public float GetLength(){
		float f = 0f;
		for(int i=0;i<transform.childCount-1;i++){
			f += (transform.GetChild(i+1).localPosition-transform.GetChild(i).localPosition).magnitude;
		}
		return f;
	}

	
}
