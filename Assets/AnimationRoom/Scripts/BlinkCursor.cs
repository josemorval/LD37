using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkCursor : MonoBehaviour {

	public float duration;

	IEnumerator Start () {

		while(true){

			GetComponent<SpriteRenderer>().enabled = true;

			yield return new WaitForSeconds(duration);

			GetComponent<SpriteRenderer>().enabled = false;

			yield return new WaitForSeconds(duration);
		}

	}
}
