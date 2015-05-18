using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class CameraManager : MonoBehaviour {
	public Camera camNE;
	public Camera camSE;
	public Camera camSW;
	public Camera camNW;

	Vector3 offset;

	float zoomSpeed;
	float moveSpeed;

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
	bool pathing;

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
	public GameObject pathMaster;
	GameObject cube;
	Vector3 p;
	GameObject oldTarget;
	GameObject currTarget;
	Material oldMat;

	public Material silhouette;
	public Material redOpaque;
	public Material greenOpaque;
	public Material blueOpaque;
	public Material yellowOpaque;
	public Material pathA;
	public Material pathAS;
	public Material pathBTL;
	public Material pathBTLS;
	public Material pathBTR;
	public Material pathBTRS;
	public Material pathH;
	public Material pathHS;
	public Material path3B;
	public Material path3BS;
	public Material path3L;
	public Material path3LS;
	public Material path3R;
	public Material path3RS;
	public Material path3T;
	public Material path3TS;
	public Material pathTTL;
	public Material pathTTLS;
	public Material pathTTR;
	public Material pathTTRS;
	public Material pathV;
	public Material pathVS;

	private GameObject[,] grid;

	// Use this for initialization
	void Start () {
		oldTarget = null;
		currTarget = null;
		moveSpeed = 20.0f;
		zoomSpeed = 4.0f;
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

	public void setPathing (bool pathingIn) {
		pathing = pathingIn;
	}

	public void setPlacing (bool placingIn) {
		placing = placingIn;
	}

	public void setDestroying (bool destroyingIn) {
		destroying = destroyingIn;
		Destroy (cube);
		resetHighlight ();
	}

	public void newPath () {
		if (!pathing)
			cube = (GameObject)Instantiate (pathMaster);
	}

	public void newCube (string type) {
		//Destroy (cube);
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

	void path () {
		if (raycast () && !EventSystem.current.IsPointerOverGameObject ()) {
			Cursor.visible = false;
			highlight.transform.position = new Vector3 (p.x, 0.005f, p.z);
			cube.GetComponent<MeshCollider>().enabled = false;
			cube.transform.position = new Vector3 (p.x, 0.005f, p.z);
			if (Input.GetMouseButtonUp (0)) {
				int gridX = (int)(p.x+49.5);
				int gridZ = (int)(p.z+49.5);
				if (grid[gridX,gridZ] == null) {
					int left = gridX - 1;
					int right = gridX + 1;
					int top = gridZ + 1;
					int bottom = gridZ - 1;
					cube.GetComponent<MeshCollider>().enabled = true;
					//TODO determine which path texture to use/if path can be placed down
					//all
					if (grid[left,gridZ] != null && grid[right,gridZ] != null && grid[gridX,top] != null && grid[gridX,bottom] != null) {
						cube.GetComponent<Renderer>().material = pathA;
						cube.tag = "path_a";
						grid[gridX,gridZ] = cube;
						cube = (GameObject) Instantiate (pathMaster);
					} else {
						grid[gridX,gridZ] = cube;
						cube = (GameObject) Instantiate (pathMaster);
					}
				}
			}
		} else {
			Cursor.visible = true;
			resetHighlight();
		}
	}

	// expects cube to be instantiated. places a cube on mouseup and immediately creates another one to use as the silhouette.
	void placeObject () {
		if (raycast () && !EventSystem.current.IsPointerOverGameObject()) {
			if (cube.tag ==  "Red Gro A" || cube.tag == "Red Gro AA" || cube.tag == "Red Gro AAA") {
				oldMat = redOpaque;
			} else if (cube.tag ==  "Green Gro A" || cube.tag == "Green Gro AA" || cube.tag == "Green Gro AAA") {
				oldMat = greenOpaque;
			} else if (cube.tag ==  "Blue Gro A" || cube.tag == "Blue Gro AA" || cube.tag == "Blue Gro AAA") {
				oldMat = blueOpaque;
			} else {
				oldMat = yellowOpaque;
			}
			Color c = oldMat.color;
			silhouette.SetColor ("_Color", new Color(c.r, c.g, c.b, 0.5f));
			cube.GetComponent<BoxCollider>().enabled = false;
			cube.GetComponent<Renderer>().material = silhouette;
			Cursor.visible = false;
			//Debug.DrawLine (new Vector3 (0, 0, 0), new Vector3 (p.x, 0, p.z));
			highlight.transform.position = new Vector3 (p.x, 0.005f, p.z);
			cube.transform.position = new Vector3 (p.x, 0.05f, p.z);
			if (Input.GetMouseButtonUp (0)) {
				int gridX = (int)(p.x+49.5);
				int gridZ = (int)(p.z+49.5);
				if (grid[gridX,gridZ] == null) {
					cube.GetComponent<BoxCollider>().enabled = true;
					cube.GetComponent<Renderer>().material = oldMat;
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
		if (raycast () && !EventSystem.current.IsPointerOverGameObject ()) {
			highlight.transform.position = new Vector3 (p.x, 0.005f, p.z);
			Cursor.visible = false;
			int gridX = (int)(p.x + 49.5);
			int gridZ = (int)(p.z + 49.5);
			currTarget = grid [gridX, gridZ];
			if (currTarget != oldTarget && oldTarget != null) {
				oldTarget.GetComponent<Renderer> ().material = oldMat;
			}
			if (currTarget != null) {
				Material newMat = null;
				Color c = new Color ();
				if (currTarget.tag ==  "Red Gro A" || currTarget.tag == "Red Gro AA" || currTarget.tag == "Red Gro AAA") {
					oldMat = redOpaque;
					c = oldMat.color;
					silhouette.SetColor ("_Color", new Color(c.r, c.g, c.b, 0.5f));
					newMat = silhouette;
				} else if (currTarget.tag ==  "Green Gro A" || currTarget.tag == "Green Gro AA" || currTarget.tag == "Green Gro AAA") {
					oldMat = greenOpaque;
					c = oldMat.color;
					silhouette.SetColor ("_Color", new Color(c.r, c.g, c.b, 0.5f));
					newMat = silhouette;
				} else if (currTarget.tag ==  "Blue Gro A" || currTarget.tag == "Blue Gro AA" || currTarget.tag == "Blue Gro AAA") {
					oldMat = blueOpaque;
					c = oldMat.color;
					silhouette.SetColor ("_Color", new Color(c.r, c.g, c.b, 0.5f));
					newMat = silhouette;
				} else if (currTarget.tag == "Yellow Gro"){
					oldMat = yellowOpaque;
					c = oldMat.color;
					silhouette.SetColor ("_Color", new Color(c.r, c.g, c.b, 0.5f));
					newMat = silhouette;
				} else if (currTarget.tag == "path_a"){
					oldMat = pathA;
					newMat = pathAS;
				} else if (currTarget.tag == "path_btl"){
					oldMat = pathBTL;
					newMat = pathBTLS;
				} else if (currTarget.tag == "path_btr"){
					oldMat = pathBTR;
					newMat = pathBTRS;
				} else if (currTarget.tag == "path_h"){
					oldMat = pathH;
					newMat = pathHS;
				} else if (currTarget.tag == "path_3b"){
					oldMat = path3B;
					newMat = path3BS;
				} else if (currTarget.tag == "path_3l"){
					oldMat = path3L;
					newMat = path3LS;
				} else if (currTarget.tag == "path_3r"){
					oldMat = path3R;
					newMat = path3RS;
				} else if (currTarget.tag == "path_3t"){
					oldMat = path3T;
					newMat = path3TS;
				} else if (currTarget.tag == "path_ttl"){
					oldMat = pathTTL;
					newMat = pathTTLS;
				} else if (currTarget.tag == "path_ttr"){
					oldMat = pathTTR;
					newMat = pathTTRS;
				} else if (currTarget.tag == "path_v"){
					oldMat = pathV;
					newMat = pathVS;
				}
				currTarget.GetComponent<Renderer>().material = newMat;
				if (Input.GetMouseButtonUp (0)) {
					Destroy (grid [gridX, gridZ]);
					grid [gridX, gridZ] = null;
				}
			} else {
				Cursor.visible = true;
			}
			oldTarget = currTarget; 
		} else {
			resetHighlight ();
			Cursor.visible = true;
		}
	}

	// Update is called once per frame
	void Update () {

		if (pathing) {
			path ();
		}
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
				Vector3 newPosition = cams[i].transform.position + cams[i].transform.forward * zoomSpeed;
				if (newPosition.y >= minCamHeight)
					cams [i].transform.position = newPosition;
			}
		} else if (!zoomIn){
			for (int i = 0; i < cams.Length; i++) {
				//cams [i].transform.Translate (-(activeCamera.transform.forward) * zoomSpeed * Time.deltaTime, Space.World);
				Vector3 newPosition = cams[i].transform.position + (-(cams[i].transform.forward)) * zoomSpeed;
				if (newPosition.y <= maxCamHeight)
					cams [i].transform.position = newPosition;
			}
		}
	}

	void OnApplicationQuit () {
		silhouette.SetColor ("_Color", new Color(0.0f, 0.0f, 0.0f, 0.5f));
	}
}
