using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	public TerminalManager terminalManager;
	public List<Node> nodes;
	public List<Path> paths;

	public Node nodeSelected;
	public List<Node> nearCurrentNode;

	//Game states
	//0 - inspect and check file
	//1 - node to go
	//2 - node to send file

	int gameState = 0;

	void Awake(){

		nodes = new List<Node>();

		for(int i=0;i<transform.GetChild(0).transform.childCount;i++){
			nodes.Add(transform.GetChild(0).transform.GetChild(i).GetComponent<Node>());
		}

		paths = new List<Path>();

		for(int i=0;i<transform.GetChild(1).transform.childCount;i++){
			paths.Add(transform.GetChild(1).transform.GetChild(i).GetComponent<Path>());
		}
	}

	void Update () {

		//if(!someTimeAction){
			if(Input.GetKeyDown(KeyCode.Return)){
				CheckEnter();
			}else if(Input.GetKeyDown(KeyCode.UpArrow)){
				CheckUp();
			}else if(Input.GetKeyDown(KeyCode.DownArrow)){
				CheckDown();
			}
		//}

		if(nodes[0].hasPackage){

			int indexNextScene = SceneManager.GetActiveScene().buildIndex+1+SceneManager.sceneCountInBuildSettings;
			indexNextScene%=SceneManager.sceneCountInBuildSettings;
			SceneManager.LoadScene(indexNextScene);

		}


	
	}

	void CheckEnter(){

		switch(gameState){

		case 0:

			if(terminalManager.currentOption.name=="inspect"){

				//Obtengo nodos a los que poder ir
				//Actualizo terminal manager
				//Paso a 1

				nodeSelected.neighNodes = GetNearNodes();
				nodeSelected.hasUsedInspect = true;
				nearCurrentNode = nodeSelected.neighNodes;
				terminalManager.UpdateNodesToVisit();
				terminalManager.ChangeScreen(1);

				gameState = 1;

			}else if(terminalManager.currentOption.name=="check"){

				//Activo boton send si esta el fichero
				//Me quedo en 0

			}else if(terminalManager.currentOption.name=="send"){

				//Voy a 2

				nearCurrentNode = nodeSelected.neighNodes;
				terminalManager.UpdateNodesToVisit();
				terminalManager.ChangeScreen(1);
				gameState = 2;

			}

			break;
		case 1:

			nodeSelected = nearCurrentNode[terminalManager.currentOptionIndex];
			terminalManager.ChangeScreen(0);
			terminalManager.SetName("bill@"+nodeSelected.name);
			gameState = 0;

			break;
		case 2:


			nearCurrentNode[terminalManager.currentOptionIndex].hasPackage = true;
			nodeSelected.hasPackage = false;
			terminalManager.ChangeScreen(0);
			gameState = 0;

			break;
		default:
			break;

		}

	}

	void CheckUp(){
		terminalManager.ChangeElement(-1);
	}

	void CheckDown(){
		terminalManager.ChangeElement(1);
	}

	List<Node> GetNearNodes(){

		List<Node> ns = new List<Node>();

		for(int i=0;i<paths.Count;i++){
			if(paths[i].nodes[0]==nodeSelected){
				ns.Add(paths[i].nodes[1]);
				paths[i].nodes[1].AddNode(nodeSelected);
				paths[i].gameObject.SetActive(true);
			}else if(paths[i].nodes[1]==nodeSelected){
				ns.Add(paths[i].nodes[0]);
				paths[i].nodes[0].AddNode(nodeSelected);
				paths[i].gameObject.SetActive(true);
			}
		}

		return ns;

	}
}
