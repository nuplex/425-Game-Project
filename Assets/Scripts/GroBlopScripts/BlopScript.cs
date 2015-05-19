using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BlopScript : MonoBehaviour {

	public float addToOutputRate;
	public float addToGrowthRate;

	private Vector3 offset;
	public float speed;

	private int currDir;
	private const int X_POS = 1, X_NEG = 2, Z_POS = 3, Z_NEG = 4;

	private float lastTurnX;
	private float lastTurnZ;
	
	// Use this for initialization
	void Start () {
		//should be set by the gro producing it
		addToGrowthRate = 1;
		addToOutputRate = 1;

		speed = 2.0f;

		currDir = X_POS;
		lastTurnX = transform.position.x;
		lastTurnZ = transform.position.z;

		Random.seed = System.DateTime.Now.Millisecond;
	}
	
	// Update is called once per frame
	void Update () {
		//should be doing path finding and moving on the path here
	}

	void FixedUpdate(){

		float x = transform.position.x;
		float z = transform.position.z;

		if (NeedToTurn()) {
			DetermineDirection();
		}

			//49.75 will make the blop dissapear once its top hits the edge
		if (x >= 49.75f || x <= -49.75f || z >= 49.75f || z <= -49.75f) {
			Destroy(gameObject);
		}

		if (currDir == X_POS) {
			offset = new Vector3 (speed * Time.deltaTime, 0, 0);
		} else if (currDir == X_NEG) {
			offset = new Vector3 (-speed * Time.deltaTime, 0, 0);
		} else if (currDir == Z_POS) {
			offset = new Vector3 (0, 0, speed * Time.deltaTime);
		} else if (currDir == Z_NEG) {
			offset = new Vector3 (0, 0, -speed * Time.deltaTime);
		}

		transform.position += offset;

	}

	void SetEnergy(float outAdd, float growAdd){
		addToGrowthRate = growAdd;
		addToOutputRate = outAdd;
	}

	bool NeedToTurn(){
		if (StillMoving ()) {
			return false;
		}

		float x = transform.position.x;
		float z = transform.position.z;

		/* Determining if about to hit edge
		 * Less than 0.5 means graphically the blop
		 * is about to hit an edge.
		 * 
		 * In reality you only need if next grid over is non-path
		 */
		float space;

		if (currDir == X_POS || currDir == X_NEG) {
			space = Mathf.Abs(Mathf.Ceil (x) - x);
		} else {
			space = Mathf.Abs(Mathf.Ceil (z) - z);
		}
		if (Mathf.Round(space * 10) / 10 == 0.5f) {
			return true;
		} else {
			return false;
		}

	}

	bool StillMoving(){
		float x = transform.position.x;
		float z = transform.position.z;

		float moved;
		if (currDir == X_POS || currDir == X_NEG) {
			moved = Mathf.Abs (lastTurnX - x);
		} else if (currDir == Z_POS || currDir == Z_NEG) {
			moved = Mathf.Abs (lastTurnZ - z);
		} else {
			moved = 1.1f;
		}	

		if (moved <= 1f) {
			return true;
		}

		return false;
	}

	void DetermineDirection(){
		//should determine next direction to go based off of path
		//if about to go off edge of path, change direction to continue on path

		float x = transform.position.x;
		float z = transform.position.z;





		//since we are right now randomly determining, lets tend to stay straight
		float change = Random.Range (1, 5);
		if (change < 3.8) {
			return;
		}

		int[] possibilities = GameObject.FindGameObjectWithTag ("Camera Manager").GetComponent<CameraManager> ().CanMoveTo (x, z);
		List<int> pos = new List<int> ();

		for (int i = 0; i <possibilities.Length; i++) {
			if(possibilities[i] != 0){
				pos.Add(i+1); //plus one to get match constants
			}
		}

		if (pos.Count == 0) {
			//lol wut
			Destroy(gameObject);
		}

		int[] chooseFrom = pos.ToArray ();

		int ranIndex = (int) Random.Range (1, pos.Count + 1);
		int dir = chooseFrom [ranIndex];

		if (dir == OppositeDir(currDir) && pos.Count != 1) {
			int newIndex = (ranIndex + 1) % chooseFrom.Length;
			dir = chooseFrom[newIndex];
		}

		if (dir == X_POS) {
			currDir = X_POS;
		} else if (dir == X_NEG) {
			currDir = X_NEG;
		} else if (dir == Z_POS) {
			currDir = Z_POS;
		} else if (dir == Z_NEG) {
			currDir = Z_NEG;
		} else {
			currDir = X_POS;
		}

		lastTurnX = x;
		lastTurnZ = z;
	}

	int OppositeDir(int dir){
		if (dir == X_POS) {
			return X_NEG;
		} else if (dir == X_NEG) {
			return X_POS;
		} else if (dir == Z_POS) {
			return Z_NEG;
		} else if (dir == Z_NEG) {
			return Z_POS;
		}
		return X_POS;
	}
}
