using UnityEngine;
using System.Collections;

public class LoadManager : MonoBehaviour {

	// Use this for initialization
	void Start () {

		if (SaveLoadGame.pressedContinue) {
			Game gm = SaveLoadGame.loaded;
			GameObject[] all = gm.all;
			GameObject[] currAll = UnityEngine.Object.FindObjectsOfType<GameObject> ();
			
			for(int i = 0; i < all.Length; i++){
				if(all[i].gameObject.tag != "LoadManager"){
					Destroy(currAll[i]);
				}
			}
			
			for(int i = 0; i < all.Length; i++){
				Instantiate(all[i]);
			}
			
			return;
		}

	}

}
