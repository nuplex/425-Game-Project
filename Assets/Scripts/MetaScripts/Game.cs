using UnityEngine;
using System.Collections;

[System.Serializable]
public class Game {

	public static Game current;
	public GameObject[] all;

	public Game(){
		all = UnityEngine.Object.FindObjectsOfType<GameObject> ();
	}

}
