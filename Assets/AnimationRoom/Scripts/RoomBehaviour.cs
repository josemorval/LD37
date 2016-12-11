using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomBehaviour : MonoBehaviour {

	public Material mat;
	public SpriteRenderer screenWindow;
	IEnumerator Start () {


		float duration = 0.1f;

		mat.SetFloat("_Switch",0f);
		screenWindow.color = Color.black;

		yield return new WaitForSeconds(1f);


		yield return new WaitForSeconds(duration);

		mat.SetFloat("_Switch",1f);
		screenWindow.color = Color.white;

		yield return new WaitForSeconds(duration);
		mat.SetFloat("_Switch",0f);
		screenWindow.color = Color.black;

		yield return new WaitForSeconds(2f);

		duration = 0.03f;

		mat.SetFloat("_Switch",0f);
		screenWindow.color = Color.black;

		yield return new WaitForSeconds(duration);

		mat.SetFloat("_Switch",1f);
		screenWindow.color = Color.white;

		yield return new WaitForSeconds(duration);
		mat.SetFloat("_Switch",0f);
		screenWindow.color = Color.black;

		duration = 0.03f;

		mat.SetFloat("_Switch",0f);
		screenWindow.color = Color.black;

		yield return new WaitForSeconds(duration);

		mat.SetFloat("_Switch",1f);
		screenWindow.color = Color.white;

		yield return new WaitForSeconds(duration);
		mat.SetFloat("_Switch",0f);
		screenWindow.color = Color.black;


		yield return new WaitForSeconds(2f);

		float time = 0f;

		while(time<1.5f){


			mat.SetFloat("_Switch",time/1.5f);
			screenWindow.color = new Vector4(time/1.5f,time/1.5f,time/1.5f,1f);
			time+=Time.deltaTime;

			yield return null;
		}


	}
	

}
