using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
	Vector3 offset;
	Vector3 zoomOffset;

	// Use this for initialization
	void Start () {
		offset = new Vector3 (0.0f, 0.0f, 0.0f);
	}
	
	// Update is called once per frame
	void Update () {
		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");
		float zoom = Input.GetAxis ("Mouse ScrollWheel");
		
		offset = new Vector3 (moveHorizontal, 0.0f, moveVertical);
		zoomOffset = new Vector3 (0.0f, zoom, 0.0f);
		
		transform.position += offset;
		transform.position += zoomOffset;
	}

	void LateUpdate(){

	}
}
