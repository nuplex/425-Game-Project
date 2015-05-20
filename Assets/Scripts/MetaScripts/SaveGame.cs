using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class SaveGame : MonoBehaviour {
	
	private float saveTimer = 0;
	public int SAVE_EVERY = 30;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		saveTimer += SAVE_EVERY * Time.deltaTime;

		if (saveTimer >= SAVE_EVERY) {
			Save();
			saveTimer = 0;
		}
	}

	void Save(){

	}
}
