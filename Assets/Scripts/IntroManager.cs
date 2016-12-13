using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroManager : MonoBehaviour {

	public SpriteRenderer billText;
	public SpriteRenderer untoldText;
	public SpriteRenderer ordinaryText;
	public SpriteRenderer pressText;
	public TextMesh ourNamesText;

	public SpriteRenderer screenComp;
	public Material mat;

	IEnumerator Start(){


		float time = 0f;
		float maxTime = 6f;
		Color col;

		while(time < maxTime){

			col = billText.color;
			col.a = time/maxTime;

			billText.color = col;
			untoldText.color = col;

			screenComp.color = col;
			float f = time/maxTime;
			mat.SetFloat("_Switch",f*f*f);

			time+=Time.deltaTime;
			yield return null;
		}

		mat.SetFloat("_Switch",1f);

		time = 0f;
		maxTime = 1f;

		while(time < maxTime){

			col = ordinaryText.color;
			col.a = time/maxTime;

			ordinaryText.color = col;

			col = ourNamesText.color;
			col.a = time/maxTime;
			ourNamesText.color = col;

			time+=Time.deltaTime;
			yield return null;
		}

		time = 0f;
		maxTime = 1f;

		while(time < maxTime){

			col = pressText.color;
			col.a = time/maxTime;

			pressText.color = col;

			time+=Time.deltaTime;
			yield return null;
		}

		while(!Input.GetKey(KeyCode.Return)){
			yield return null;
		}


		time = 0f;
		maxTime = 3f;

		while(time < maxTime){

			col = billText.color;
			col.a = 1f-time/maxTime;

			billText.color = col;
			untoldText.color = col;
			ordinaryText.color = col;
			pressText.color = col;

			col = ourNamesText.color;
			col.a = 1f-time/maxTime;
			ourNamesText.color = col;

			time+=Time.deltaTime;
			yield return null;
		}

		SceneManager.LoadScene(2);

	}

	void Update(){

		pressText.transform.localScale = (2f+0.05f*Mathf.Sin(2f*Time.time))*new Vector3(1f,1f,1f);

	}

}
