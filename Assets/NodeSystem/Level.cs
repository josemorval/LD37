using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour {

	public GameObject nodes;
	public List<Path> paths;
	// Use this for initialization
	public void StartComponent () {


		//Comenzamos todos los hijos de Level (Path y Nodes)
		for (int i = 0; i < paths.Count; i++) {
			paths [i].StartComponent ();
		}
		for (int i = 0; i < transform.GetChild (0).childCount; i++) {
			getNodeIndex (i).StartComponent ();
		}
	}

	// Update is called once per frame
	public void UpdateComponent () {


		//Actualizamos todos los hijos de Level (Path y Nodes)
		for (int i = 0; i < paths.Count; i++) {
			paths [i].UpdateComponent ();
		}
		for (int i = 0; i < transform.GetChild (0).childCount; i++) {
			getNodeIndex (i).UpdateComponent ();
		}
	}

	public Node getNodeIndex(int index){
		return transform.GetChild (0).GetChild(index).GetComponent<Node>(); // Nodes en cada nivel debe estar como primer hijo.
	}
}
