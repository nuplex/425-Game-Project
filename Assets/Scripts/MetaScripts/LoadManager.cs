using UnityEngine;
using System.Collections;

public class LoadManager : MonoBehaviour {
	// Use this for initialization
	
	void Start(){
		if (SaveLoadGame.pressedContinue) {

			CameraManager cm = GameObject.FindGameObjectWithTag ("Camera Manager").GetComponent<CameraManager> ();

			SaveLoadGame.Load ();
			Game loaded = SaveLoadGame.loaded;

			Gro[] gros = loaded.grid.GetGrosArray();
			for (int i = 0; i < gros.Length; i++) {
				if(gros[i] == null){
					continue;
				}
				cm.manualPlaceGro(gros[i].ToGameObject());
			}
	
			Debug.Log(gros.Length);
			Debug.Log(loaded.grid.GetPathsArray().Length);

			Path[] paths = loaded.grid.GetPathsArray();
			for (int i = 0; i < paths.Length; i++) {
				if(paths[i] == null){
					continue;
				}
				cm.manualPlacePath(paths[i].ToGameObject());
			}

			Blop[] blops = loaded.grid.GetBlops();

			for (int i = 0; i < blops.Length; i++) {
				if(blops[i] == null){
					continue;
				}
				blops [i].ToGameObject ();
			}

			GameObject.FindGameObjectWithTag("BP").GetComponent<BlopPoints>().SetPoints(loaded.blopPoints);
		}

	}

}
