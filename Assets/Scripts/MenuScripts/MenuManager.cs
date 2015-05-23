using UnityEngine;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class MenuManager : MonoBehaviour {

	public UnityEngine.UI.Button newGame;
	public UnityEngine.UI.Button continueGame;
	public GameObject loading;

	bool gameExists; 

	// Use this for initialization
	void Start () {
		if (!GameFileExists ()) {
			continueGame.interactable = false;
		}
	}

	bool GameFileExists(){
		return File.Exists (Application.persistentDataPath + "/svs.gd");
	}

		
	public void NewGame(){
		Application.LoadLevel (1);
		if(Application.isLoadingLevel) {
			loading.SetActive (true);
		}
	}

	public void LoadGame(){
		SaveLoadGame.Load ();
		SaveLoadGame.pressedContinue = true;
		Application.LoadLevel (1);
		if(Application.isLoadingLevel) {
			loading.SetActive (true);
		}
	}

	public void QuitGame(){
		Application.Quit ();
	}
}
