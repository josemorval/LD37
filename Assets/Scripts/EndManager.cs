using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndManager : MonoBehaviour {

	public TextMesh text;
	public SpriteRenderer screenComp;
	public Material mat;

	IEnumerator Start(){


		float time = 0f;
		float maxTime = 6f;
		Color col;

		while(time<maxTime){

			col = text.color;
			col.a = time/maxTime;
			text.color = col;

			time+=Time.deltaTime;
			yield return null;
		}

		yield return new WaitForSeconds(4f);

		time = 0f;
		maxTime = 3f;

		while(time < maxTime){

			col = text.color;
			col.a = 1f-time/maxTime;
			text.color = col;
			screenComp.color = col;

			float f = 1f-time/maxTime;
			mat.SetFloat("_Switch",f*f*f);

			time+=Time.deltaTime;
			yield return null;
		}

		mat.SetFloat("_Switch",0f);

		SceneManager.LoadScene(1);

	}


}
