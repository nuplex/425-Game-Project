using UnityEngine;
using System.Collections;

public class MenuManager : MonoBehaviour {

	public UnityEngine.UI.Button newGame;
	public UnityEngine.UI.Button continueGame;

	bool gameExists; 

	// Use this for initialization
	void Start () {
		if (!GameFileExists ()) {
			continueGame.interactable = false;
		}
	}

	bool GameFileExists(){
		return false;
	}

		
	public void NewGame(){
		Application.LoadLevel (1);
	}

	public void LoadGame(){
		Application.LoadLevel (1);
	}
}
