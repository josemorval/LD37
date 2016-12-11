using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public List<Level> levels;
	public int activeLevel;
	public int currentNodePlayer;

	public GameObject scanInfo;


	[Header("Keys")]
	public KeyCode scan;
	bool scanActive = false;
	List<Node> nodeEnds;
	GameObject scanInfoObject;

	int option = 0;
	// Use this for initialization
	void Start () {
		//Inicializaciones
		nodeEnds = new List<Node> ();


		//Comenzamos todos los hijos del GameManager
		for (int i = 0; i < levels.Count; i++) {
			levels [i].StartComponent ();
		}
	}
	
	// Update is called once per frame
	void Update () {
		Level level = levels [activeLevel];
		Node current = level.getNodeIndex (currentNodePlayer);

		//We see if any path has been discovered
		for (int i = 1; i < level.transform.childCount; i++) {
			if (level.transform.GetChild (i).GetComponent<Path> ().nodeStart == current) {
				level.transform.GetChild (i).GetComponent<Path> ().isDiscovered = true;
			}
		}



		if (Input.GetKeyDown (scan) && !scanActive) {
			scanInfoObject = GameObject.Instantiate (scanInfo);

			level = levels [activeLevel];
		    current = level.getNodeIndex (currentNodePlayer);
			string info = "";

			nodeEnds.Clear ();
			for (int i = 1; i < level.transform.childCount; i++) {
				if (level.transform.GetChild (i).GetComponent<Path> ().nodeStart == current) {
					info += "\n- ";
					info += level.transform.GetChild (i).GetComponent<Path> ().nodeEnd.name;
					nodeEnds.Add (level.transform.GetChild (i).GetComponent<Path> ().nodeEnd);

				}
				if (level.transform.GetChild (i).GetComponent<Path> ().nodeEnd == current) {
					info += "\n- ";
					info += level.transform.GetChild (i).GetComponent<Path> ().nodeStart.name;
					nodeEnds.Add (level.transform.GetChild (i).GetComponent<Path> ().nodeStart);

				}
			}
			
			scanInfoObject.GetComponent<TextMesh> ().text += info;
			scanActive = true;
		}

		if(Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow)){
			int active = option % nodeEnds.Count;

			nodeEnds [active].GetComponent<MeshRenderer> ().material.SetColor ("_Color", Color.white);
			option +=1;
		}

		if (Input.GetKeyDown (KeyCode.Return) && scanActive) {
			int active = option % nodeEnds.Count;
			level = levels [activeLevel];

			//Update new current node player index
			for (int i = 0; i < level.transform.GetChild(0).childCount; i++) {
				if (level.getNodeIndex(i) == nodeEnds[active].GetComponent<Node>()) {
					currentNodePlayer = i;
					break;
				}
			}

			//We delete scan window and get back to normal color the nodes that could be visited by player
			Destroy(scanInfoObject);
			nodeEnds [active].GetComponent<MeshRenderer> ().material.SetColor ("_Color", Color.white);
			option = 0;
			nodeEnds.Clear ();
			scanActive = false;

		}
		
		if (scanActive) {
			int active = option % nodeEnds.Count;

			nodeEnds [active].GetComponent<MeshRenderer> ().material.SetColor ("_Color", Color.red);
		}

		//Actualizamos todos los hijos del GameManager
		for (int i = 0; i < levels.Count; i++) {
			levels [i].UpdateComponent ();
		}
	}
}
