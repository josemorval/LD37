using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerminalManager : MonoBehaviour {

	public GameManager gameManager;

	public Color activated;
	public Color deactivated;

	public GameObject[] screens;

	public GameObject currentScreen;
	public GameObject currentOption;

	public int currentScreenIndex = 0;
	public int currentOptionIndex = 0;

	void Start () {
		ChangeScreen(0);
	}

	public void ChangeElement(int i){

		if(currentScreenIndex==0){
			
			currentScreen.transform.GetChild(currentOptionIndex).GetComponent<SpriteRenderer>().color = deactivated;
			currentOptionIndex+=i+screens[currentScreenIndex].transform.childCount;
			currentOptionIndex%=screens[currentScreenIndex].transform.childCount;
			currentScreen.transform.GetChild(currentOptionIndex).GetComponent<SpriteRenderer>().color = activated;
			currentOption = currentScreen.transform.GetChild(currentOptionIndex).gameObject;

		}else if(currentScreenIndex==1){
			currentScreen.transform.GetChild(currentOptionIndex).GetComponent<SpriteRenderer>().color = deactivated;
			currentOptionIndex+=i+gameManager.nearCurrentNode.Count;
			currentOptionIndex%=gameManager.nearCurrentNode.Count;
			currentScreen.transform.GetChild(currentOptionIndex).GetComponent<SpriteRenderer>().color = activated;
			currentOption = currentScreen.transform.GetChild(currentOptionIndex).gameObject;
		}


	}

	public void UpdateNodesToVisit(){

		GameObject g;

		for(int i=0;i<screens[1].transform.childCount;i++){
			g = screens[1].transform.GetChild(i).gameObject;
			g.SetActive(i<gameManager.nearCurrentNode.Count);

			if(i<gameManager.nearCurrentNode.Count){
				g.transform.GetChild(0).GetComponent<TextMesh>().text = gameManager.nearCurrentNode[i].name;
			}
		}

	}

	public void ChangeScreen(int k){

		currentScreenIndex = k;
		currentOptionIndex = 0;
		currentScreen = screens[currentScreenIndex];

		for(int i=0;i<screens.Length;i++){
			screens[i].SetActive(i==currentScreenIndex);
		}

		if(k==1){
			for(int i=0;i<gameManager.nearCurrentNode.Count;i++){
				if(i!=currentOptionIndex){
					currentScreen.transform.GetChild(i).GetComponent<SpriteRenderer>().color = deactivated;
				}else{
					currentScreen.transform.GetChild(i).GetComponent<SpriteRenderer>().color = activated;
				}
			}

			currentOption = currentScreen.transform.GetChild(currentOptionIndex).gameObject;

		}else if(k==0){

			for(int i=0;i<screens[currentScreenIndex].transform.childCount;i++){
				if(i!=currentOptionIndex){
					currentScreen.transform.GetChild(i).GetComponent<SpriteRenderer>().color = deactivated;
				}else{
					currentScreen.transform.GetChild(i).GetComponent<SpriteRenderer>().color = activated;
				}
			}

			currentOption = currentScreen.transform.GetChild(currentOptionIndex).gameObject;

		}
	}
  

	public void SetName(string s){
		transform.GetChild(0).transform.GetChild(0).GetComponent<TextMesh>().text = s;
	}

	public void SetLoadingScreen(string s){
		screens[2].transform.GetChild(0).GetComponent<TextMesh>().text = s;
	}

	public void SetPercentVal(float f){
		Vector3 v = screens[2].transform.GetChild(1).GetChild(0).transform.localScale;
		v.x = f;
		screens[2].transform.GetChild(1).GetChild(0).transform.localScale = v;
	}


}
