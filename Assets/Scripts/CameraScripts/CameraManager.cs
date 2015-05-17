using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class CameraManager : MonoBehaviour {
	public Camera camNE;
	public Camera camSE;
	public Camera camSW;
	public Camera camNW;

	Vector3 offset;

	public float zoomSpeed = 1.0f;
	public float minZoomFOV = 10.0f;
	public float maxZoomFOV = 120.0f;
	public float standardZoom = 30.0f;
	public float moveSpeed = 20.0f;

	Vector3 forward, backward, left, right;

	private Camera[] cams;
	private Camera[] naturalSet;
	private Camera[] unnaturalSet;

	private Camera activeCamera;
	private int currentCam;
	private const int CAM_NE = 1, CAM_SE = 2, CAM_SW = 3, CAM_NW = 4;

	private bool naturalMode;
	bool placing;
	bool destroying;

	float maxCamHeight;
	float minCamHeight;

	public GameObject rA;
	public GameObject rAA;
	public GameObject rAAA;
	public GameObject gA;
	public GameObject gAA;
	public GameObject gAAA;
	public GameObject bA;
	public GameObject bAA;
	public GameObject bAAA;
	public GameObject yS;

	public GameObject highlight;
	public GameObject plane;
	public GameObject cubeMaster;
	GameObject cube;
	Vector3 p;

	private GameObject[,] grid;

	// Use this for initialization
	void Start () {
		maxCamHeight = 50.0f;
		minCamHeight = 1.0f;
		grid = new GameObject[100, 100];
		placing = false;
		destroying = false;
		naturalMode = true;
		currentCam = 0;
		activeCamera = camSW;
		camNE.enabled = false;
		camSE.enabled = false;
		camSW.enabled = true;
		camNW.enabled = false;
		cams = new Camera[4]{camSW, camSE, camNE, camNW};
		naturalSet = new Camera[4]{camSW, camSE, camNE, camNW};
		unnaturalSet = new Camera[4]{camNE, camSE, camSW, camNW};
		offset = new Vector3 (0.0f, 0.0f, 0.0f);
		p = new Vector3 (0,0,0);
		setDirections ();
	}

	void resetHighlight () {
		highlight.transform.position = new Vector3 (0,-1,0);
	}

	public void setPlacing (bool placingIn) {
		placing = placingIn;
	}

	public void setDestroying (bool destroyingIn) {
		destroying = destroyingIn;
		//Destroy (cube);
		resetHighlight ();
	}

	public void newCube (string type) {
		Destroy (cube);
		if (!placing) {
			switch (type) {
			case "rA":
				cubeMaster = rA;
				cube = (GameObject) Instantiate (cubeMaster);
				break;
			case "rAA":
				cubeMaster = rAA;
				cube = (GameObject) Instantiate (cubeMaster);
				break;
			case "rAAA":
				cubeMaster = rAAA;
				cube = (GameObject) Instantiate (cubeMaster);
				break;
			case "gA":
				cubeMaster = gA;
				cube = (GameObject) Instantiate (cubeMaster);
				break;
			case "gAA":
				cubeMaster = gAA;
				cube = (GameObject) Instantiate (cubeMaster);
				break;
			case "gAAA":
				cubeMaster = gAAA;
				cube = (GameObject) Instantiate (cubeMaster);
				break;
			case "bA":
				cubeMaster = bA;
				cube = (GameObject) Instantiate (cubeMaster);
				break;
			case "bAA":
				cubeMaster = bAA;
				cube = (GameObject) Instantiate (cubeMaster);
				break;
			case "bAAA":
				cubeMaster = bAAA;
				cube = (GameObject) Instantiate (cubeMaster);
				break;
			case "yS":
				cubeMaster = yS;
				cube = (GameObject) Instantiate (cubeMaster);
				break;
			}
		}
	}

	// sets movement directions for all four cameras based on which one is active
	void setDirections() {
		forward = activeCamera.transform.forward;
		forward = Quaternion.AngleAxis (-20, activeCamera.transform.right) * forward;
		right = activeCamera.transform.right;
		left = -activeCamera.transform.right;
		backward = -forward;
	}

	// if the mouse is on a grid cell, returns true and sets p to the world coordinates cell
	// returns false if the mouse is not on a grid cell
	bool raycast () {
		RaycastHit hitInfo;
		float cellWidth = 1.0f;
		if (Physics.Raycast (activeCamera.transform.position, (activeCamera.ScreenPointToRay (Input.mousePosition).direction), out hitInfo)) {
			Vector3 pivotToPoint = hitInfo.point - plane.transform.position;
			float tileX = pivotToPoint.x / cellWidth;
			float tileZ = pivotToPoint.z / cellWidth;
			tileX = Mathf.Floor (tileX);
			tileZ = Mathf.Floor (tileZ);
			float worldX = plane.transform.position.x + tileX * cellWidth + cellWidth / 2.0f;
			float worldZ = plane.transform.position.z + tileZ * cellWidth + cellWidth / 2.0f;
			p = new Vector3(worldX, 0, worldZ);
			return true;
		} else {
			return false;
		}
	}

	// expects cube to be instantiated. places a cube on mouseup and immediately creates another one to use as the silhouette.
	void placeObject () {
		if (raycast () && !EventSystem.current.IsPointerOverGameObject()) {
			Cursor.visible = false;
			Debug.DrawLine (new Vector3 (0, 0, 0), new Vector3 (p.x, 0, p.z));
			highlight.transform.position = new Vector3 (p.x, 0.005f, p.z);
			cube.transform.position = new Vector3 (p.x, 0.05f, p.z);
			if (Input.GetMouseButtonUp (0)) {
				int gridX = (int)(p.x+49.5);
				int gridZ = (int)(p.z+49.5);
				if (grid[gridX,gridZ] == null) {
					cube.GetComponent<GroScript>().placed = true;
					//cube.transform.position = new Vector3 (p.x, 0.05f, p.z);
					grid[gridX,gridZ] = cube;
					cube = (GameObject) Instantiate (cubeMaster);
				}
			}
		} else {
			Cursor.visible = true;
			resetHighlight ();
			cube.transform.position = new Vector3 (0.0f,1000,0.0f);
			/*p = activeCamera.ScreenPointToRay(Input.mousePosition);
			cube.transform.position = activeCamera.transform.position + p.direction * 20;*/
		}
		if (Input.GetKeyDown (KeyCode.Escape)) {
			placing = false;
			Cursor.visible = true;
			resetHighlight ();
			Destroy (cube);
		}
		
	}

	void destroyObject () {
		if (raycast () && !EventSystem.current.IsPointerOverGameObject()) {
			Cursor.visible = false;
			highlight.transform.position = new Vector3 (p.x, 0.005f, p.z);
			if (Input.GetMouseButtonUp (0)) {
				int gridX = (int)(p.x + 49.5);
				int gridZ = (int)(p.z + 49.5);
				if (grid [gridX, gridZ] != null) {
					Destroy (grid [gridX, gridZ]);
					grid [gridX, gridZ] = null;
				}
			}
		} else {
			resetHighlight ();
			Cursor.visible = true;
		}
	}

	// Update is called once per frame
	void Update () {

		if (placing) {
			placeObject ();
		}
		if (destroying) {
			destroyObject ();
		}
		if (Input.GetKeyDown ("q")) {
			ChangeCam(PrevCam ());
			if(naturalMode){
				setDirections ();
			}
		} else if (Input.GetKeyDown ("e")) {
			ChangeCam(NextCam ());
			if(naturalMode){
				setDirections ();
			}
		}

		//switch between camera modes
		if (Input.GetKeyDown ("n")) {
			if(naturalMode == false){
				currentCam = 0;
				naturalMode = true;
				cams = naturalSet;
				activeCamera = camSW;
				camNE.enabled = false;
				camSE.enabled = false;
				camSW.enabled = true;
				camNW.enabled = false;
			} else {
				currentCam = CAM_NE;
				activeCamera = camNE;
				camNE.enabled = true;
				camSE.enabled = false;
				camSW.enabled = false;
				camNW.enabled = false;
				naturalMode = false;
				cams = unnaturalSet;
			}
		}

		// TODO add x and z limit to camera movement
		if (naturalMode) {
			if (Input.GetKey ("w")) {
				for (int i = 0; i < cams.Length; i++) {
					cams [i].transform.Translate (forward * moveSpeed * Time.deltaTime, Space.World);
				}
			}
			if (Input.GetKey ("s")) {
				for (int i = 0; i < cams.Length; i++) {
					cams [i].transform.Translate (backward * moveSpeed * Time.deltaTime, Space.World);
				}
			}
			if (Input.GetKey ("a")) {
				for (int i = 0; i < cams.Length; i++) {
					cams [i].transform.Translate (left * moveSpeed * Time.deltaTime, Space.World);
				}
			}
			if (Input.GetKey ("d")) {
				for (int i = 0; i < cams.Length; i++) {
					cams [i].transform.Translate (right * moveSpeed * Time.deltaTime, Space.World);
				}
			}
		} else {
			float moveHorizontal = Input.GetAxis ("Horizontal");
			float moveVertical = Input.GetAxis ("Vertical");
			
			ControlsByCam(currentCam, moveHorizontal, moveVertical);
		}


		//Zooming
		float zoomDir = Input.GetAxis ("Mouse ScrollWheel");
		if (zoomDir < 0) {
			Zoom (false);
		} else  if (zoomDir > 0) {
			Zoom (true);
		}
		
		//reset zoom
		/*if (Input.GetKey ("z")) {
			for(int i = 0; i < cams.Length; i++){
				cams[i].fieldOfView = standardZoom;
			}
		}*/
	}

	void ChangeCam(int cam){
		if (naturalMode) {
			activeCamera = cams [cam];
		} else {
			activeCamera = cams [cam - 1];
		}
		activeCamera.enabled = true;
		for (int i = 0; i < cams.Length; i++) {
			if(naturalMode){
				if(i == cam){
					continue;
				}
			} else {
				if(i == cam - 1){
					continue;
				}
			}
			cams[i].enabled = false;
		}
	}
	
	private int NextCam(){
		if (naturalMode) {
			currentCam = (currentCam + 1) % 4;
			return currentCam;
		} else {
			if (currentCam >= CAM_NW) {
				currentCam = CAM_NE;
			} else {
				currentCam += 1;
			}
			return currentCam;
		}
	}
	
	
	private int PrevCam(){
		if (naturalMode) {
			currentCam = (currentCam + 3) % 4;
			return currentCam;
		} else {
			if (currentCam <= CAM_NE) {
				currentCam = CAM_NW;
			} else {
				currentCam -= 1;
			}
			return currentCam;
		}
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
		if (zoomIn) {
			for (int i = 0; i < cams.Length; i++) {
				//cams [i].transform.Translate (activeCamera.transform.forward * zoomSpeed * Time.deltaTime, Space.World);
				Vector3 newPosition = cams[i].transform.position + activeCamera.transform.forward;
				if (newPosition.y >= minCamHeight)
					cams [i].transform.position = newPosition;
			}
		} else if (!zoomIn){
			for (int i = 0; i < cams.Length; i++) {
				//cams [i].transform.Translate (-(activeCamera.transform.forward) * zoomSpeed * Time.deltaTime, Space.World);
				Vector3 newPosition = cams[i].transform.position + (-(activeCamera.transform.forward));
				if (newPosition.y <= maxCamHeight)
					cams [i].transform.position = newPosition;
			}
		}
	}
}
