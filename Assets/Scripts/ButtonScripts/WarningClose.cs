using UnityEngine;
using System.Collections;

public class WarningClose : MonoBehaviour {
	public GameObject warnPanel;

	public void Quitting(){
		warnPanel.SetActive (true);
	}

	public void Yes(){
		Application.LoadLevel (0);
	}

	public void No(){
		warnPanel.SetActive (false);
	}
}
