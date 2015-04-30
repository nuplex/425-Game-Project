using UnityEngine;
using System.Collections;

public class CameraManager : MonoBehaviour {
	public Camera camNE;
	public Camera camSE;
	public Camera camSW;
	public Camera camNW;

	Vector3 offset;

	public float zoomSpeed = 20.0f;
	public float minZoomFOV = 10.0f;
	public float maxZoomFOV = 120.0f;
	public float standardZoom = 60.0f;
	public float moveSpeed = 20.0f;

	Vector3 forward, backward, left, right;

	private Camera[] cams;
	private Camera activeCamera;
	private int currentCam;
	private const int CAM_NE = 1, CAM_SE = 2, CAM_SW = 3, CAM_NW = 4;

	// Use this for initialization
	void Start () {
		currentCam = 0;
		activeCamera = camSW;
		camNE.enabled = false;
		camSE.enabled = false;
		camSW.enabled = true;
		camNW.enabled = false;
		cams = new Camera[4]{camSW, camSE, camNE, camNW};
		offset = new Vector3 (0.0f, 0.0f, 0.0f);
		setDirections ();
	}

	// sets movement directions for all four cameras based on which one is active
	void setDirections() {
		forward = activeCamera.transform.forward;
		forward = Quaternion.AngleAxis (-20, activeCamera.transform.right) * forward;
		Debug.Log (forward.ToString ("F4"));
		right = activeCamera.transform.right;
		left = -activeCamera.transform.right;
		backward = -forward;
	}

	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown ("q")) {
			ChangeCam(PrevCam ());
			setDirections ();
		} else if (Input.GetKeyDown ("e")) {
			ChangeCam(NextCam ());
			setDirections ();
		}

		if (Input.GetKey("w")) {
			for (int i = 0; i < cams.Length; i++) {
				cams[i].transform.Translate(forward * moveSpeed * Time.deltaTime, Space.World);
			}
		}
		if (Input.GetKey("s")) {
			for (int i = 0; i < cams.Length; i++) {
				cams[i].transform.Translate(backward * moveSpeed * Time.deltaTime, Space.World);
			}
		}
		if (Input.GetKey("a")) {
			for (int i = 0; i < cams.Length; i++) {
				cams[i].transform.Translate(left * moveSpeed * Time.deltaTime, Space.World);
			}
		}
		if (Input.GetKey("d")) {
			for (int i = 0; i < cams.Length; i++) {
				cams[i].transform.Translate(right * moveSpeed * Time.deltaTime, Space.World);
			}
		}


		//Zooming
		float zoomDir = Input.GetAxis ("Mouse ScrollWheel");
		print (zoomDir);
		if (zoomDir < 0) {
			Zoom (false);
		} else  if (zoomDir > 0) {
			Zoom (true);
		}
		
		//reset zoom
		if (Input.GetKey ("z")) {
			for(int i = 0; i < cams.Length; i++){
				cams[i].fieldOfView = standardZoom;
			}
		}
	}

	void ChangeCam(int cam){
		activeCamera = cams[cam];
		activeCamera.enabled = true;
		for (int i = 0; i < cams.Length; i++) {
			if(i == cam){
				continue;
			}
			cams[i].enabled = false;
		}
	}
	
	private int NextCam(){
		currentCam = (currentCam + 1) % 4;
		return currentCam;
		/*if (currentCam >= CAM_NW) {
			currentCam = CAM_NE;
		} else {
			currentCam += 1;
		}
		return currentCam;*/
	}
	
	
	private int PrevCam(){
		currentCam = (currentCam + 3) % 4;
		return currentCam;
		/*if (currentCam <= CAM_NE) {
			currentCam = CAM_NW;
		} else {
			currentCam -= 1;
		}
		return currentCam;*/
	}

	void ControlsByCam(int cam, float moveHorizontal, float moveVertical){
		if (cam == CAM_NE) {
			offset = new Vector3 (-moveHorizontal, 0.0f, -moveVertical);
		} else if (cam == CAM_SE) {
			offset = new Vector3 (-moveVertical, 0.0f, moveHorizontal);
		} else if (cam == CAM_SW) {
			offset = new Vector3 (moveHorizontal, 0.0f, moveVertical);
		} else if (cam == CAM_NW) {
			offset = new Vector3 (moveVertical, 0.0f, -moveHorizontal);
		}

		for (int i = 0; i < cams.Length; i++) {
			cams[i].transform.position += offset;
		}
	}

	void Zoom(bool zoomIn){
		for (int i = 0; i < cams.Length; i++) {
			if (zoomIn) {
				if (cams[i].fieldOfView >= minZoomFOV) {
					cams[i].fieldOfView -= zoomSpeed / 8;
				}
			} else {
				if (cams[i].fieldOfView <= maxZoomFOV) {
					cams[i].fieldOfView += zoomSpeed / 8;
				}
			}
		}
	}
}
