using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour {

	public GameObject nodes;
	// Use this for initialization
	public void StartComponent () {

	}

	// Update is called once per frame
	public void UpdateComponent () {

	}

	public Node getNodeIndex(int index){
		return transform.GetChild (0).GetChild(index).GetComponent<Node>(); // Nodes en cada nivel debe estar como primer hijo.
	}
}
