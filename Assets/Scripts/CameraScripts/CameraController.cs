using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
	Vector3 offset;

	// Use this for initialization
	void Start () {
		offset = new Vector3 (0.0f, 0.0f, 0.0f);
	}
	
	// Update is called once per frame
	void Update () {

	}

	void LateUpdate(){
		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");

		offset = new Vector3 (moveHorizontal, 0.0f, moveVertical);

		transform.position += offset;

		/*if (Input.GetKey ("w")) {
			offset += Inp
		} else if (Input.GetKey ("a")) {
			
		} else if (Input.GetKey ("s")) {
			
		} else if (Input.GetKey ("d")) {
			
		}*/
	}
}
