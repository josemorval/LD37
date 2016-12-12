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
	bool someTimeAction = false;


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

		if(!someTimeAction){
			if(Input.GetKeyDown(KeyCode.Return)){
				CheckEnter();
			}else if(Input.GetKeyDown(KeyCode.UpArrow)){
				CheckUp();
			}else if(Input.GetKeyDown(KeyCode.DownArrow)){
				CheckDown();
			}
		}

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

				if(terminalManager.currentOption.transform.GetChild(1).gameObject.activeInHierarchy){
					terminalManager.currentOption.transform.GetChild(1).gameObject.SetActive(false);
					nearCurrentNode = nodeSelected.neighNodes;
					terminalManager.UpdateNodesToVisit();
					gameState = 2;
					terminalManager.ChangeScreen(1);
				}else{
					someTimeAction = true;
					StartCoroutine(CheckFileCoroutine());
				}

			}

			break;
		case 1:

			nodeSelected = nearCurrentNode[terminalManager.currentOptionIndex];
			terminalManager.ChangeScreen(0);
			terminalManager.SetName("bill@"+nodeSelected.name);
			gameState = 0;

			break;
		case 2:
			
			someTimeAction = true;
			StartCoroutine(SendFileCoroutine());

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

	IEnumerator CheckFileCoroutine(){

		terminalManager.ChangeScreen(2);
		float time = 0f;
		float maxTime = 2f;
		terminalManager.SetLoadingScreen("Checking file system...");
		terminalManager.SetPercentVal(0f);

		while(time<maxTime){

			terminalManager.SetPercentVal(time/maxTime);

			time+=Time.deltaTime;
			yield return null;
		}
			

		if(nodeSelected.hasPackage){
			terminalManager.currentOption.transform.GetChild(1).gameObject.SetActive(true);
		}else{
			terminalManager.SetLoadingScreen("Not file found");
			yield return new WaitForSeconds(1.5f);
		}

		terminalManager.ChangeScreen(0);
		someTimeAction = false;

	}

	IEnumerator SendFileCoroutine(){

		terminalManager.ChangeScreen(2);
		float time = 0f;
		float maxTime = 2f;
		terminalManager.SetLoadingScreen("Sending file to " + nearCurrentNode[terminalManager.currentOptionIndex].name + " machine...");
		terminalManager.SetPercentVal(0f);

		while(time<maxTime){

			terminalManager.SetPercentVal(time/maxTime);

			time+=Time.deltaTime;
			yield return null;
		}

		nearCurrentNode[terminalManager.currentOptionIndex].hasPackage = true;
		nodeSelected.hasPackage = false;
		terminalManager.ChangeScreen(0);
		gameState = 0;

		someTimeAction = false;
	}
}
