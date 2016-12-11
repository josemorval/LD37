using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour {


	[Header("Is the node visible?")]
	public bool isVisible;

	// Use this for initialization
	public void StartComponent () {

	}

	// Update is called once per frame
	public void UpdateComponent () {
		if (isVisible) {
			GetComponent<MeshRenderer> ().enabled = true;

		} else {
			GetComponent<MeshRenderer> ().enabled = false;
		}
	}
}
