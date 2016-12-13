using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	public int level;

	public AnimationCurve animInspect;

	public TerminalManager terminalManager;
	public List<Node> nodes;
	public List<Path> paths;

	public Node nodeSelected;
	public List<Node> nearCurrentNode;

	public GameObject selector;

	//Game states
	//0 - inspect and check file
	//1 - node to go
	//2 - node to send file

	int gameState = 0;
	bool someTimeAction = false;


	float TIME_PER_UNIT_TRANSFER_FILE = 0.1f;
	float TIME_PER_UNIT_INSPECT = 0.01f;

	public GameObject timeEnd;
	bool firstOption = false;
	public float levelTime;

	void Awake(){

		nodes = new List<Node>();

		for(int i=0;i<transform.GetChild(0).transform.childCount;i++){
			nodes.Add(transform.GetChild(0).transform.GetChild(i).GetComponent<Node>());
		}

		paths = new List<Path>();

		for(int i=0;i<transform.GetChild(1).transform.childCount;i++){
			paths.Add(transform.GetChild(1).transform.GetChild(i).GetComponent<Path>());
		}

		for(int i=0;i<paths.Count;i++){

			if(paths[i].nodes[0] != nodes[0]){
				paths[i].nodes[0].transform.position=paths[i].transform.GetChild(0).transform.position;
			}
			if(paths[i].nodes[0] != nodes[0]){
				paths[i].nodes[1].transform.position=paths[i].transform.GetChild(paths[i].transform.childCount-1).transform.position;
			}

		}
			
	}

	void Update () {

		if(firstOption){
			timeEnd.gameObject.SetActive(true);
			levelTime-=Time.deltaTime;
			timeEnd.transform.GetChild(0).GetComponent<TextMesh>().text = levelTime.ToString("000");

			if(levelTime<=0f){
				SceneManager.LoadScene("levellost");
			}
		}

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

			if(level<3){
				SceneManager.LoadScene("level"+(level+1).ToString());
			}else{
				SceneManager.LoadScene("levelwin");
			}

		}


		if(gameState==1 || gameState==2){
			selector.SetActive(true);
			selector.transform.position = nearCurrentNode[terminalManager.currentOptionIndex].transform.position+new Vector3(0f,0f,-5f);
		}else{
			selector.SetActive(false);
		}

	
	}

	void CheckEnter(){

		switch(gameState){

		case 0:

			if(terminalManager.currentOption.name=="inspect"){

				firstOption = true;

				//Obtengo nodos a los que poder ir
				//Actualizo terminal manager
				//Paso a 1

				someTimeAction = true;
				StartCoroutine(InspectCoroutine());

			}else if(terminalManager.currentOption.name=="check"){

				firstOption = true;

				//Activo boton send si esta el fichero
				//Me quedo en 0

				if(terminalManager.currentOption.transform.GetChild(0).GetComponent<TextMesh>().text=="send file"){
					terminalManager.currentOption.transform.GetChild(0).GetComponent<TextMesh>().text="check file";
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

			nodeSelected = nodeSelected.neighNodes[terminalManager.currentOptionIndex];
			terminalManager.ChangeScreen(0);
			terminalManager.SetName(nodeSelected.name);
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

	List<Node> GetNearNodes(bool showPath){

		List<Node> ns = new List<Node>();

		for(int i=0;i<paths.Count;i++){
			if(paths[i].nodes[0]==nodeSelected){
				ns.Add(paths[i].nodes[1]);
				if(showPath){
					paths[i].nodes[1].AddNode(nodeSelected);
					paths[i].gameObject.SetActive(true);
				}
			}else if(paths[i].nodes[1]==nodeSelected){
				ns.Add(paths[i].nodes[0]);
				if(showPath){
					paths[i].nodes[0].AddNode(nodeSelected);
					paths[i].gameObject.SetActive(true);
				}
			}
		}

		return ns;

	}

	Path GetPathWithNodes(Node a, Node b){

		bool b1,b2;

		for(int i=0;i<paths.Count;i++){

			b1 = a==paths[i].nodes[0] || a==paths[i].nodes[1];
			b2 = b==paths[i].nodes[0] || b==paths[i].nodes[1];

			if(b1 && b2){
				return paths[i];
			}
		}

		return null;

	}


	IEnumerator InspectCoroutine(){

		terminalManager.currentOption.transform.parent.GetChild(1).transform.GetChild(0).GetComponent<TextMesh>().text="check file";

		nodeSelected.neighNodes = GetNearNodes(false);
		nodeSelected.hasUsedInspect = true;
		nearCurrentNode = nodeSelected.neighNodes;

		terminalManager.ChangeScreen(2);

		float time = 0f;
		float maxTime = 0f;

		for(int i=0;i<nearCurrentNode.Count;i++){
			maxTime+=GetPathWithNodes(nodeSelected,nearCurrentNode[i]).GetLength();
		}

		maxTime*=TIME_PER_UNIT_INSPECT;

		terminalManager.SetLoadingScreen("Discovering servers...");
		terminalManager.SetPercentVal(0f);

		while(time<maxTime){

			terminalManager.SetPercentVal(animInspect.Evaluate(time/maxTime));

			time+=Time.deltaTime;
			yield return null;
		}

		terminalManager.SetPercentVal(1f);

		nodeSelected.neighNodes = GetNearNodes(true);
		terminalManager.UpdateNodesToVisit();
		terminalManager.ChangeScreen(1);

		gameState = 1;
		someTimeAction = false;

	}


	IEnumerator CheckFileCoroutine(){

		terminalManager.ChangeScreen(2);
		float time = 0f;
		float maxTime = 2.5f+2f*Random.value;
		terminalManager.SetLoadingScreen("Checking file system...");
		terminalManager.SetPercentVal(0f);

		while(time<maxTime){

			terminalManager.SetPercentVal(animInspect.Evaluate(time/maxTime));

			time+=Time.deltaTime;
			yield return null;
		}

		terminalManager.SetPercentVal(1f);

		if(nodeSelected.hasPackage){
			//terminalManager.currentOption.transform.GetChild(1).gameObject.SetActive(true);
			terminalManager.currentOption.transform.GetChild(0).GetComponent<TextMesh>().text = "send file";
		}else{
			terminalManager.SetLoadingScreen("Not file found");
			yield return new WaitForSeconds(1f);
		}


		terminalManager.ChangeScreen(0);
		someTimeAction = false;

	}

	IEnumerator SendFileCoroutine(){

		nearCurrentNode[terminalManager.currentOptionIndex].hasPackage = true;
		nodeSelected.hasPackage = false;

		terminalManager.ChangeScreen(2);
		float time = 0f;
		float maxTime = TIME_PER_UNIT_TRANSFER_FILE*GetPathWithNodes(nodeSelected,nearCurrentNode[terminalManager.currentOptionIndex]).GetLength();

		terminalManager.SetLoadingScreen("Sending file to " + nearCurrentNode[terminalManager.currentOptionIndex].name + " machine...");
		terminalManager.SetPercentVal(0f);

		while(time<maxTime){

			terminalManager.SetPercentVal(time/maxTime);

			time+=Time.deltaTime;
			yield return null;
		}

		terminalManager.SetPercentVal(1f);



		terminalManager.ChangeScreen(0);
		gameState = 0;

		someTimeAction = false;
	}
}
