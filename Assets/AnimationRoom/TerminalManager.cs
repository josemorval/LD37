using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerminalManager : MonoBehaviour {

	//public GameManager gm;
	public Color activated;
	public Color deactivated;

	public GameObject[] screens;
	GameObject currentScreen;

	int currentScreenIndex = 0;
	int currentOptionIndex = 0;

	void Start () {
		ResetScreen(0);
	}

	void ResetScreen(int k){

		currentScreenIndex = k;
		currentOptionIndex = 0;
		currentScreen = screens[currentScreenIndex];

		for(int i=0;i<screens.Length;i++){
			screens[i].SetActive(i==currentScreenIndex);
		}

		for(int i=0;i<screens[currentScreenIndex].transform.childCount;i++){
			if(i!=currentOptionIndex){
				currentScreen.transform.GetChild(i).GetComponent<SpriteRenderer>().color = deactivated;
			}else{
				currentScreen.transform.GetChild(i).GetComponent<SpriteRenderer>().color = activated;
			}
		}
	}
	
	void Update () {

		if(Input.GetKeyDown(KeyCode.UpArrow)){
			ChangeElement(-1);
		}else if(Input.GetKeyDown(KeyCode.DownArrow)){
			ChangeElement(1);
		}else if(Input.GetKeyDown(KeyCode.Return)){
			currentScreenIndex++;
			currentScreenIndex+=screens.Length;
			currentScreenIndex%=screens.Length;

			ResetScreen(currentScreenIndex);
		}
		
	}

	void ChangeElement(int i){
		currentScreen.transform.GetChild(currentOptionIndex).GetComponent<SpriteRenderer>().color = deactivated;
		currentOptionIndex+=(i+currentScreen.transform.childCount);
		currentOptionIndex%=currentScreen.transform.childCount;
		currentScreen.transform.GetChild(currentOptionIndex).GetComponent<SpriteRenderer>().color = activated;
	}


}
