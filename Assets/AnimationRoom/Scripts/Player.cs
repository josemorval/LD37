using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player : MonoBehaviour {

	public float vel;
	Animator anim;

	Vector3 initialScale;
	float orientation;

	void Start(){
		anim = GetComponent<Animator>();
		initialScale = transform.localScale;
		orientation = 1f;
	}

	void Update () {

		anim.speed = 0.7f;

		Vector3 v = initialScale;

		if(Input.GetKey(KeyCode.W)){
			transform.position=transform.position+(new Vector3(0f,1f,0f))*vel*Time.deltaTime;
			anim.CrossFade("UpPlayer",0f);
			orientation = 1f;
		}else if(Input.GetKey(KeyCode.S)){
			transform.position=transform.position-(new Vector3(0f,1f,0f))*vel*Time.deltaTime;
			anim.CrossFade("DownPlayer",0f);
			orientation = 1f;
		}else if(Input.GetKey(KeyCode.D)){
			transform.position=transform.position+(new Vector3(1f,0f,0f))*vel*Time.deltaTime;
			anim.CrossFade("RightPlayer",0f);
			orientation = 1f;
		}else if(Input.GetKey(KeyCode.A)){
			transform.position=transform.position-(new Vector3(1f,0f,0f))*vel*Time.deltaTime;
			anim.CrossFade("LeftPlayer",0f);
			orientation = -1f;
		}else{
			anim.speed = 0f;
		}

		v.x *= orientation;
		transform.localScale = v;
	}
}
