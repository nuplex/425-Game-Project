using UnityEngine;
using System.Collections;

public class SaveButton : MonoBehaviour {

	public GameObject saveConfirmation;
	private float countdown = 0;
	private float SHOW_FOR = 1f;

	public void Save(){
		SaveLoadGame.Save ();
		countdown = SHOW_FOR;
		saveConfirmation.SetActive (true);
	}

	public void Update(){
		if (countdown > 0) {
			countdown -= Time.deltaTime;
			if (countdown < 0) {
				saveConfirmation.SetActive (false);
			}
		}
	}
}
