using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
	private Camera cam;
	Vector3 offset;
	//float targetAngle = 0;
	// Use this for initialization
	public float zoomSpeed = 20.0f;
	public float minZoomFOV = 10.0f;
	public float maxZoomFOV = 120.0f;
	public float standardZoom = 60.0f;
	private int currentView;
	private const int VIEW_1 = 1, VIEW_2 = 2, VIEW_3 = 3, VIEW_4 = 4;

	private const int ROTATE_DELAY = 8; //fps
	private int rotateKeyDelay = ROTATE_DELAY;


	private bool canPressRotate;

	void Start () {
		currentView = VIEW_1;
		cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera> ();
		offset = new Vector3 (0.0f, 0.0f, 0.0f);
		canPressRotate = true;
	}
	
	// Update is called once per frame
	void Update () {
		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");

		ControlsByView (currentView, moveHorizontal, moveVertical);

		if (canPressRotate) {
			canPressRotate = false;
			//View Change
			if (Input.GetKey ("q")) {
				ChangeView (PrevView ());
			} else if (Input.GetKey ("e")) {
				ChangeView (NextView ());
			}
		}

		if (!canPressRotate) {
			rotateKeyDelay--;
			if(rotateKeyDelay < 0){
				canPressRotate = true;
				rotateKeyDelay = ROTATE_DELAY;
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
			cam.fieldOfView = standardZoom;
		}

	}

	void LateUpdate(){

	}

	void ChangeView(int view){
		float x = transform.position.x;
		float y = transform.position.y;
		float z = transform.position.z;

		float ex = transform.rotation.eulerAngles.x;
		//float ey = transform.rotation.eulerAngles.y;
		float ez = transform.rotation.eulerAngles.z;
	

		if (view == VIEW_1) {
			transform.position = new Vector3(x, y, z);
			cam.transform.rotation = Quaternion.Euler(ex, 0f, ez);
		} else if (view == VIEW_2) {
			transform.position = new Vector3(x, y, -z);
			cam.transform.rotation = Quaternion.Euler(ex, 90f, ez);
		} else if (view == VIEW_3) {
			transform.position = new Vector3(-x, y, -z);
			cam.transform.rotation = Quaternion.Euler(ex, 180f, ez);
		} else if (view == VIEW_4) {
			transform.position = new Vector3(-x, y, z);
			cam.transform.rotation = Quaternion.Euler(ex, 270f, ez);
		}
	}

	private int NextView(){
		if (currentView >= VIEW_4) {
			currentView = VIEW_1;
		} else {
			currentView += 1;
		}
		return  currentView;
	}


	private int PrevView(){
		if (currentView <= VIEW_1) {
			currentView = VIEW_4;
		} else {
			currentView -= 1;
		}
		return currentView;
	}

	void Zoom(bool zoomIn){
		if (zoomIn) {
			if(cam.fieldOfView >= minZoomFOV){
				cam.fieldOfView -= zoomSpeed/8;
			}
		} else {
			if(cam.fieldOfView <= maxZoomFOV){
				cam.fieldOfView += zoomSpeed/8;
			}
		}
	}

	void ControlsByView(int view, float moveHorizontal, float moveVertical){
		if (view == VIEW_1) {
			offset = new Vector3 (moveHorizontal, 0.0f, moveVertical);
		} else if (view == VIEW_2) {
			offset = new Vector3 (moveVertical, 0.0f, -moveHorizontal);
		} else if (view == VIEW_3) {
			offset = new Vector3 (-moveHorizontal, 0.0f, -moveVertical);
		} else if (view == VIEW_4) {
			offset = new Vector3 (-moveVertical, 0.0f, moveHorizontal);
		}
		transform.position += offset;
	}

}
