using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour {

	public string name;
	public bool hasPackage;

	public List<Node> neighNodes;
	public bool hasUsedInspect;

	void Awake(){
		neighNodes = new List<Node>();
	}

	public void AddNode(Node n){

		if(!hasUsedInspect){
			neighNodes.Add(n);
		}
	}

}
