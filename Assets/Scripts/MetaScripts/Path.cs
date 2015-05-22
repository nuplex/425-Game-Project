using UnityEngine;
using System.Collections;

[System.Serializable]
public class Path : SaveObject {

	public float x;
	public float y;
	public float z;

	public Path(GameObject path, string tag):base(tag){
		x = path.transform.position.x;
		y = path.transform.position.y;
		z = path.transform.position.z;
	}

	override public GameObject ToGameObject(){
		GameObject path = (GameObject) GameObject.Instantiate (Resources.Load ("Path"));

		path.transform.position = new Vector3 (x, y, z);

		return path;
	}

}
