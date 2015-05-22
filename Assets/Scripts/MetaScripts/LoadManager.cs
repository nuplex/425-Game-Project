using UnityEngine;
using System.Collections;

public class LoadManager : MonoBehaviour {

	// Use this for initialization
	
	void Start(){
		if (SaveLoadGame.pressedContinue) {


			SaveLoadGame.Load ();
			Game loaded = SaveLoadGame.loaded;

			GameObject.FindGameObjectWithTag ("Camera Manager").GetComponent<CameraManager> ().SetGrid (loaded.grid.ToGameObjectGrid ());
			GameObject.FindGameObjectWithTag ("BP").GetComponent<BlopPoints> ().SetPoints (loaded.blopPoints);

			Blop[] blops = loaded.grid.GetBlops ();

			for (int i = 0; i < blops.Length; i++) {
				blops [i].ToGameObject ();
			}

		}

	}

}
