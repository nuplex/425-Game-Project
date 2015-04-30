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

	private Camera[] cams;
	private Camera activeCamera;
	private int currentCam;
	private const int CAM_NE = 1, CAM_SE = 2, CAM_SW = 3, CAM_NW = 4;

	private const int CAM_CHANGE_DELAY = 8; //fps
	private int camChangeDelay = CAM_CHANGE_DELAY;

	private bool canChangeCam = true;


	// Use this for initialization
	void Start () {
		currentCam = CAM_NE;
		activeCamera = camNE;
		cams = new Camera[4]{camNE, camSE, camSW, camNW};
		offset = new Vector3 (0.0f, 0.0f, 0.0f);
	}
	
	// Update is called once per frame
	void Update () {
		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");
		
		ControlsByCam(currentCam, moveHorizontal, moveVertical);
		
		if (canChangeCam) {
			canChangeCam = false;
			//View Change
			if (Input.GetKey ("q")) {
				ChangeCam(PrevCam ());
			} else if (Input.GetKey ("e")) {
				ChangeCam(NextCam ());
			}
		}
		
		if (!canChangeCam) {
			camChangeDelay--;
			if(camChangeDelay < 0){
				canChangeCam = true;
				camChangeDelay = CAM_CHANGE_DELAY;
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
		activeCamera = cams[cam - 1];
		activeCamera.enabled = true;
		for (int i = 0; i < cams.Length; i++) {
			if(i == cam - 1){
				continue;
			}
			cams[i].enabled = false;
		}
	}
	
	private int NextCam(){
		if (currentCam >= CAM_NW) {
			currentCam = CAM_NE;
		} else {
			currentCam += 1;
		}
		return  currentCam;
	}
	
	
	private int PrevCam(){
		if (currentCam <= CAM_NE) {
			currentCam = CAM_NW;
		} else {
			currentCam -= 1;
		}
		return currentCam;
	}

	void ControlsByCam(int cam, float moveHorizontal, float moveVertical){
		if (cam == CAM_NE) {
			offset = new Vector3 (-moveHorizontal, 0.0f, -moveVertical);
		} else if (cam == CAM_SE) {
			offset = new Vector3 (-moveVertical, 0.0f, moveHorizontal);
		} else if (cam == CAM_SW) {
			offset = new Vector3 (moveVertical, 0.0f, -moveHorizontal);
		} else if (cam == CAM_NE) {
			offset = new Vector3 (moveHorizontal, 0.0f, moveVertical);
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
