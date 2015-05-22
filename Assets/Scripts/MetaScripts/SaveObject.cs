using UnityEngine;
using System.Collections;

[System.Serializable]
public abstract class SaveObject {

	//[SerializeField]
	public string tag;

	public SaveObject(string tag){
		this.tag = tag;
	}

	public abstract GameObject ToGameObject();

}
