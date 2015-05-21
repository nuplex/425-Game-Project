using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

[System.Serializable]
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

	bool pressedQuit = false;

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

	public GameObject warnPanel;

	public Material silhouette;
	public Material redOpaque;
	public Material greenOpaque;
	public Material blueOpaque;
	public Material yellowOpaque;
	public Material pathDefault;
	public Material pathDefaultS;
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
		zoomSpeed = 3.0f;
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

	string pathType(int gridX, int gridZ) {
		GameObject curr = grid [gridX, gridZ];
		if (curr == null ||
			curr.tag != "Red Gro A" && curr.tag != "Red Gro AA" && curr.tag != "Red Gro AAA" &&
			curr.tag != "Blue Gro A" && curr.tag != "Blue Gro AA" && curr.tag != "Blue Gro AAA" &&
			curr.tag != "Green Gro A" && curr.tag != "Green Gro AA" && curr.tag != "Green Gro AAA" &&
			curr.tag != "Yellow Gro") {
			int left = gridX - 1;
			int right = gridX + 1;
			int top = gridZ + 1;
			int bottom = gridZ - 1;
			//left edge
			if (left < 0) {
				//left top
				if (top > 99) {
					if (grid [right, gridZ] != null && grid [gridX,bottom] != null) {
						return "path_btr";
					} else if (grid [right,gridZ] != null) {
						return "path_h";
					} else if (grid [gridX,bottom] != null) {
						return "path_v";
					} else
						return "path_default";
				//left bottom
				}else if (bottom < 0){
					if (grid [right,gridZ] != null && grid [gridX,top] != null) {
						return "path_ttr";
					} else if (grid [right,gridZ] != null) {
						return "path_h";
					} else if (grid [gridX,top] != null) {
						return "path_v";
					} else {
						return "path_default";
					}
				} else if (grid [right, gridZ] != null && grid [gridX, top] != null && grid [gridX, bottom] != null) {
					return "path_3r";
				} else if (grid [right, gridZ] != null && grid [gridX, top] != null) {
					return "path_ttr";
				} else if (grid [right, gridZ] != null && grid [gridX, bottom] != null) {
					return "path_btr";
				} else if (grid [right, gridZ] != null) {
					return "path_h";
				} else if (grid [gridX, bottom] != null || grid [gridX, top] != null) {
					return "path_v";
				} else {
					return "path_default";
				}
			}
			//right edge
			else if (right > 99) {
				//right top
				if (top > 99) {
					if (grid [left,gridZ] != null && grid [gridX,bottom] != null) {
						return "path_btl";
					} else if (grid [left,gridZ] != null) {
						return "path_h";
					} else if (grid [gridX,bottom] != null) {
						return "path_v";
					} else
						return "path_default";
				//right bottom
				} else if (bottom < 0){
					if (grid [left,gridZ] != null && grid [gridX,top] != null) {
						return "path_ttl";
					} else if (grid [left,gridZ] != null) {
						return "path_h";
					} else if (grid [gridX,top] != null) {
						return "path_v";
					} else {
						return "path_default";
					}
				} else if (grid [left, gridZ] != null && grid [gridX, top] != null && grid [gridX, bottom] != null) {
					return "path_3l";
				} else if (grid [left, gridZ] != null && grid [gridX, top] != null) {
					return "path_ttl";
				} else if (grid [left, gridZ] != null && grid [gridX, bottom] != null) {
					return "path_btl";
				} else if (grid [left, gridZ] != null) {
					return "path_h";
				} else if (grid [gridX, bottom] != null || grid [gridX, top] != null) {
					return "path_v";
				} else {
					return "path_default";
				}
			}
			//top edge (and not left or right edge)
			else if (top > 99) {
				if(grid [left, gridZ] != null && grid [right, gridZ] != null && grid [gridX, bottom] != null){
					return "path_3b";
				} else if(grid [left, gridZ] != null && grid [gridX, bottom] != null){
					return "path_btl";
				} else if(grid [right, gridZ] != null && grid [gridX, bottom] != null){
					return "path_btr";
				} else if(grid [left, gridZ] != null || grid [right, gridZ] != null){
					return "path_h";
				} else if(grid [gridX, bottom] != null){
					return "path_v";
				} else {
					return "path_default";
				}
			}
			//bottom edge (and not left or right edge)
			else if (bottom < 0) {
				if(grid [left, gridZ] != null && grid [right, gridZ] != null && grid [gridX, top] != null){
					return "path_3t";
				} else if(grid [left, gridZ] != null && grid [gridX, top] != null){
					return "path_ttl";
				} else if(grid [right, gridZ] != null && grid [gridX, top] != null){
					return "path_ttr";
				} else if(grid [left, gridZ] != null || grid [right, gridZ] != null){
					return "path_h";
				} else if(grid [gridX, top] != null){
					return "path_v";
				} else {
					return "path_default";
				}
			}
			//all
			else if (grid [left, gridZ] != null && grid [right, gridZ] != null && grid [gridX, top] != null && grid [gridX, bottom] != null)
				return "path_a";
			//3t
			else if (grid [left, gridZ] != null && grid [right, gridZ] != null && grid [gridX, top] != null)
				return "path_3t";
			//3b
			else if (grid [left, gridZ] != null && grid [right, gridZ] != null && grid [gridX, bottom] != null)
				return "path_3b";
			//3l
			else if (grid [left, gridZ] != null && grid [gridX, top] != null && grid [gridX, bottom] != null)
				return "path_3l";
			//3r
			else if (grid [right, gridZ] != null && grid [gridX, top] != null && grid [gridX, bottom] != null)
				return "path_3r";
			//ttl
			else if (grid [left, gridZ] != null && grid [gridX, top] != null)
				return "path_ttl";
			//ttr
			else if (grid [right, gridZ] != null && grid [gridX, top] != null)
				return "path_ttr";
			//btl
			else if (grid [left, gridZ] != null && grid [gridX, bottom] != null)
				return "path_btl";
			//btr
			else if (grid [right, gridZ] != null && grid [gridX, bottom] != null)
				return "path_btr";
			//h
			else if (grid [right, gridZ] != null || grid [left, gridZ] != null)
				return "path_h";
			//v
			else if (grid [gridX, top] != null || grid [gridX, bottom] != null)
				return "path_v";
			//default
			else
				return "path_default";
		} else {
			return "cube";
		}
	}

	//TODO check to see if this is actually a path!!!
	void refreshPaths (int gridX, int gridZ) {
		if (gridX >= 0 && gridX <= 99 && gridZ >= 0 && gridZ <= 99) {
			GameObject curr = grid [gridX, gridZ];
			if (curr != null) {
				switch (pathType (gridX, gridZ)) {
				case "path_a":
					curr.GetComponent<Renderer> ().material = pathA;
					curr.tag = "path_a";
					break;
				case "path_3t":
					curr.GetComponent<Renderer> ().material = path3T;
					curr.tag = "path_3t";
					break;
				case "path_3b":
					curr.GetComponent<Renderer> ().material = path3B;
					curr.tag = "path_3b";
					break;
				case "path_3l":
					curr.GetComponent<Renderer> ().material = path3L;
					curr.tag = "path_3l";
					break;
				case "path_3r":
					curr.GetComponent<Renderer> ().material = path3R;
					curr.tag = "path_3r";
					break;
				case "path_ttl":
					curr.GetComponent<Renderer> ().material = pathTTL;
					curr.tag = "path_ttl";
					break;
				case "path_ttr":
					curr.GetComponent<Renderer> ().material = pathTTR;
					curr.tag = "path_ttr";
					break;
				case "path_btl":
					curr.GetComponent<Renderer> ().material = pathBTL;
					curr.tag = "path_btl";
					break;
				case "path_btr":
					curr.GetComponent<Renderer> ().material = pathBTR;
					curr.tag = "path_btr";
					break;
				case "path_h":
					curr.GetComponent<Renderer> ().material = pathH;
					curr.tag = "path_h";
					break;
				case "path_v":
					curr.GetComponent<Renderer> ().material = pathV;
					curr.tag = "path_v";
					break;
				case "path_default":
					curr.GetComponent<Renderer> ().material = pathDefault;
					curr.tag = "path_default";
					break;
				default:
					break;
				}
			}
		}
	}

	void path () {
		if (raycast () && !EventSystem.current.IsPointerOverGameObject ()) {
			//Cursor.visible = false;
			GameObject.Find("Manager").GetComponent<ButtonPanelBehavior>().setCursorToPlacing();
			highlight.transform.position = new Vector3 (p.x, 0.005f, p.z);
			cube.GetComponent<MeshCollider>().enabled = false;
			cube.transform.position = new Vector3 (p.x, 0.005f, p.z);
			if (Input.GetMouseButtonUp (0)) {

				GameObject bP = GameObject.FindGameObjectWithTag("BP");

				if (bP.GetComponent<BlopPoints>().CanBuy (10)) {
					bP.GetComponent<BlopPoints>().Use (10);
					int gridX = (int)(p.x+49.5);
					int gridZ = (int)(p.z+49.5);
					if (grid[gridX,gridZ] == null) {
						cube.GetComponent<MeshCollider>().enabled = true;
							
						switch (pathType (gridX, gridZ)) {
						case "path_a":
							cube.GetComponent<Renderer>().material = pathA;
							cube.tag = "path_a";
							break;
						case "path_3t":
							cube.GetComponent<Renderer>().material = path3T;
							cube.tag = "path_3t";
							break;
						case "path_3b":
							cube.GetComponent<Renderer>().material = path3B;
							cube.tag = "path_3b";
							break;
						case "path_3l":
							cube.GetComponent<Renderer>().material = path3L;
							cube.tag = "path_3l";
							break;
						case "path_3r":
							cube.GetComponent<Renderer>().material = path3R;
							cube.tag = "path_3r";
							break;
						case "path_ttl":
							cube.GetComponent<Renderer>().material = pathTTL;
							cube.tag = "path_ttl";
							break;
						case "path_ttr":
							cube.GetComponent<Renderer>().material = pathTTR;
							cube.tag = "path_ttr";
							break;
						case "path_btl":
							cube.GetComponent<Renderer>().material = pathBTL;
							cube.tag = "path_btl";
							break;
						case "path_btr":
							cube.GetComponent<Renderer>().material = pathBTR;
							cube.tag = "path_btr";
							break;
						case "path_h":
							cube.GetComponent<Renderer>().material = pathH;
							cube.tag = "path_h";
							break;
						case "path_v":
							cube.GetComponent<Renderer>().material = pathV;
							cube.tag = "path_v";
							break;
						case "path_default":
							cube.GetComponent<Renderer>().material = pathDefault;
							cube.tag = "path_default";
							break;
						}
						grid[gridX,gridZ] = cube;
						refreshPaths (gridX-1, gridZ);
						refreshPaths (gridX+1, gridZ);
						refreshPaths (gridX, gridZ+1);
						refreshPaths (gridX, gridZ-1);
						cube = (GameObject) Instantiate (pathMaster);
						//TODO check surrounding cells to see 
					}
				}
			}
		} else {
			//Cursor.visible = true;
			GameObject.Find("Manager").GetComponent<ButtonPanelBehavior>().setCursorToPath();
			resetHighlight();
			cube.transform.position = new Vector3 (0.0f,1000,0.0f);
		}
	}

	// expects cube to be instantiated. places a cube on mouseup and immediately creates another one to use as the silhouette.
	void placeObject () {
		Color c = new Color ();
		if (raycast () && !EventSystem.current.IsPointerOverGameObject()) {
			if (cube.tag ==  "Red Gro A" || cube.tag == "Red Gro AA" || cube.tag == "Red Gro AAA") {
				oldMat = redOpaque;
				c = oldMat.color;
				silhouette.SetColor ("_Color", new Color(c.r, c.g+(100f/255f), c.b+(100f/255f), 1.0f));
			} else if (cube.tag ==  "Green Gro A" || cube.tag == "Green Gro AA" || cube.tag == "Green Gro AAA") {
				oldMat = greenOpaque;
				c = oldMat.color;
				silhouette.SetColor ("_Color", new Color(115f/255f, 225f/255f, 110f/255f, 1.0f));
			} else if (cube.tag ==  "Blue Gro A" || cube.tag == "Blue Gro AA" || cube.tag == "Blue Gro AAA") {
				oldMat = blueOpaque;
				c = oldMat.color;
				silhouette.SetColor ("_Color", new Color(c.r+(100f/255f), c.g+(100f/255f), c.b, 1.0f));
			} else {
				oldMat = yellowOpaque;
				c = oldMat.color;
				silhouette.SetColor ("_Color", new Color(c.r, c.g, c.b+(100f/255f), 1.0f));
			}
			cube.GetComponent<BoxCollider>().enabled = false;
			cube.GetComponent<Renderer>().material = silhouette;
			//Cursor.visible = false;
			GameObject.Find("Manager").GetComponent<ButtonPanelBehavior>().setCursorToPlacing();
			//Debug.DrawLine (new Vector3 (0, 0, 0), new Vector3 (p.x, 0, p.z));
			highlight.transform.position = new Vector3 (p.x, 0.005f, p.z);
			cube.transform.position = new Vector3 (p.x, 0.05f, p.z);
			if (Input.GetMouseButtonUp (0)) {

				GameObject bP = GameObject.FindGameObjectWithTag("BP");
				int cost = cube.GetComponent<GroScript>().purchaseCost;

				if(bP.GetComponent<BlopPoints>().CanBuy(cost)) {
					bP.GetComponent<BlopPoints>().Use(cost);
					int gridX = (int)(p.x+49.5);
					int gridZ = (int)(p.z+49.5);
					if (grid[gridX,gridZ] == null) {
						cube.GetComponent<BoxCollider>().enabled = true;
						cube.GetComponent<Renderer>().material = oldMat;
						cube.GetComponent<GroScript>().placed = true;
						//cube.transform.position = new Vector3 (p.x, 0.05f, p.z);
						grid[gridX,gridZ] = cube;
						refreshPaths(gridX-1,gridZ);
						refreshPaths(gridX+1,gridZ);
						refreshPaths(gridX,gridZ+1);
						refreshPaths(gridX,gridZ-1);
						cube = (GameObject) Instantiate (cubeMaster);
					}
				}
			}
		} else {
			//Cursor.visible = true;
			GameObject.Find("Manager").GetComponent<ButtonPanelBehavior>().setCursorToDefault();
			resetHighlight ();
			cube.transform.position = new Vector3 (0.0f,1000,0.0f);
			/*p = activeCamera.ScreenPointToRay(Input.mousePosition);
			cube.transform.position = activeCamera.transform.position + p.direction * 20;*/
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
				if (currTarget.tag ==  "Red Gro A" || currTarget.tag == "Red Gro AA" || currTarget.tag == "Red Gro AAA") {
					oldMat = redOpaque;
					silhouette.SetColor ("_Color", new Color(0f,0f,0f,1f));
					newMat = silhouette;
				} else if (currTarget.tag ==  "Green Gro A" || currTarget.tag == "Green Gro AA" || currTarget.tag == "Green Gro AAA") {
					oldMat = greenOpaque;
					silhouette.SetColor ("_Color", new Color(0f,0f,0f,1f));
					newMat = silhouette;
				} else if (currTarget.tag ==  "Blue Gro A" || currTarget.tag == "Blue Gro AA" || currTarget.tag == "Blue Gro AAA") {
					oldMat = blueOpaque;
					silhouette.SetColor ("_Color", new Color(0f,0f,0f,1f));
					newMat = silhouette;
				} else if (currTarget.tag == "Yellow Gro"){
					oldMat = yellowOpaque;
					silhouette.SetColor ("_Color", new Color(0f,0f,0f,1f));
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
				} else if (currTarget.tag == "path_default") {
					oldMat = pathDefault;
					newMat = pathDefaultS;
				}
				currTarget.GetComponent<Renderer>().material = newMat;
				if (Input.GetMouseButtonUp (0)) {

					GameObject bP = GameObject.FindGameObjectWithTag("BP");

					if (bP.GetComponent<BlopPoints>().CanBuy (50)) {
						bP.GetComponent<BlopPoints>().Use (50);
						Destroy (grid [gridX, gridZ]);
						grid [gridX, gridZ] = null;
						refreshPaths(gridX-1,gridZ);
						refreshPaths(gridX+1,gridZ);
						refreshPaths(gridX,gridZ+1);
						refreshPaths(gridX,gridZ-1);
					}
				}
			} else {
				Cursor.visible = true;
			}
			oldTarget = currTarget; 
		} else {
			if (EventSystem.current.IsPointerOverGameObject() && oldTarget != null && oldMat != null)
				oldTarget.GetComponent<Renderer> ().material = oldMat;
			resetHighlight ();
			Cursor.visible = true;
		}
	}

	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown (KeyCode.Escape)) {
			if(!pressedQuit){
				pressedQuit = true;
				warnPanel.SetActive(true);
			} else {
				pressedQuit = false;
				warnPanel.SetActive(false);
			}
		}

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

	public bool isPath(GameObject o) {
		return (o.tag == "path_a" || o.tag == "path_btl" || o.tag == "path_btr" ||
				o.tag == "path_3b" || o.tag == "path_3t" || o.tag == "path_3l" ||
				o.tag == "path_3r" || o.tag == "path_ttl" || o.tag == "path_ttl" ||
				o.tag == "path_h" || o.tag == "path_v" || o.tag == "path_default");
	}

	public int[] CanOutputTo(float x, float z) {
		int gridX = (int) Mathf.Floor (x) + 50;
		int gridZ = (int) Mathf.Floor (z) + 50;
		int left = gridX - 1;
		int right = gridX + 1;
		int top = gridZ + 1;
		int bottom = gridZ - 1;
		int[] surrounding = new int[4];
		if (right <= 99 && grid[right,gridZ] != null && isPath (grid[right,gridZ])) {
			surrounding [0] = 1;
		} else {
			surrounding [0] = 0;
		}
		if (left >= 0 && grid[left,gridZ] != null && isPath (grid[left,gridZ])) {
			surrounding [1] = 1;
		} else {
			surrounding [1] = 0;
		}
		if (top <= 99 && grid[gridX,top] != null && isPath (grid[gridX,top])) {
			surrounding [2] = 1;
		} else {
			surrounding [2] = 0;
		}
		if (bottom >= 0 && grid[gridX,bottom] != null && isPath (grid[gridX,bottom])) {
			surrounding [3] = 1;
		} else {
			surrounding [3] = 0;
		}
		return surrounding;
	}

	//x+=1 x-=2 z+=3 z-=4; 0 unavailable, 1 available
	public int[] CanMoveTo(float x, float z){
		int gridX = (int) Mathf.Floor (x) + 50;
		int gridZ = (int) Mathf.Floor (z) + 50;
		int left = gridX - 1;
		int right = gridX + 1;
		int top = gridZ + 1;
		int bottom = gridZ - 1;
		int[] surrounding = new int[4];
		if (right <= 99 && grid[right,gridZ] != null) {
			surrounding [0] = 1;
		} else {
			surrounding [0] = 0;
		}
		if (left >= 0 && grid[left,gridZ] != null) {
			surrounding [1] = 1;
		} else {
			surrounding [1] = 0;
		}
		if (top <= 99 && grid[gridX,top] != null) {
			surrounding [2] = 1;
		} else {
			surrounding [2] = 0;
		}
		if (bottom >= 0 && grid[gridX,bottom] != null) {
			surrounding [3] = 1;
		} else {
			surrounding [3] = 0;
		}
		return surrounding;
	}

	void OnApplicationQuit () {
		silhouette.SetColor ("_Color", new Color(0.0f, 0.0f, 0.0f, 0.5f));
	}
}
