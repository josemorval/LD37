using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public List<Level> levels;
	public int activeLevel;
	public int currentNodePlayer;


	public GameObject scanInfo;
	bool scanActive = false;
	List<Node> nodeEnds;

	int option = 0;
	// Use this for initialization
	void Start () {
		nodeEnds = new List<Node> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.S) && !scanActive) {
			GameObject scanInfoObject = GameObject.Instantiate (scanInfo);

			Level level = levels [activeLevel];
			Node current = level.getNodeIndex (currentNodePlayer);
			string info = "";

			nodeEnds.Clear ();
			for (int i = 1; i < level.transform.childCount; i++) {
				if (level.transform.GetChild (i).GetComponent<Path> ().nodeStart == current) {
					info += "\n- ";
					info += level.transform.GetChild (i).GetComponent<Path> ().nodeEnd.name;
					nodeEnds.Add (level.transform.GetChild (i).GetComponent<Path> ().nodeEnd);

				}
			}
			
			scanInfoObject.GetComponent<TextMesh> ().text += info;
			scanActive = true;
		}

		if(Input.GetKeyDown(KeyCode.UpArrow)){
			int active = option % nodeEnds.Count;

			nodeEnds [active].GetComponent<MeshRenderer> ().material.SetColor ("_Color", Color.white);
			option +=1;
		}

		if(Input.GetKeyDown(KeyCode.DownArrow)){
			int active = option % nodeEnds.Count;

			nodeEnds [active].GetComponent<MeshRenderer> ().material.SetColor ("_Color", Color.white);
			option +=1;
		}

		if (scanActive) {
			int active = option % nodeEnds.Count;

			nodeEnds [active].GetComponent<MeshRenderer> ().material.SetColor ("_Color", Color.red);
		}
	}
}
