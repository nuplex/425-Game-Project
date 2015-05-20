using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GroScript : MonoBehaviour {

	public int colorType;
	private int RA = 1, RAA = 2, RAAA = 3, BA = 4, BAA = 5, BAAA = 6, GA = 7, GAA = 8, GAAA = 9, Y = 10; 

	public float growthRate; //per some in
	public float outputRate; //per second
	public float bpRate;

	public float[] growthLevels;
	public int currentLevel = 0;
	public float currGrowthNeeded;
	public const int MAX_LEVEL = 2000;

	public int blopsTaken = 0;
	public int blopsOutput = 0;
	public float currentOutputWait = 0.0f; //amount of output rate that hasn't been output
	public float currentBPWait = 0.0f;

	public float currentGrowthAmount = 0.0f; 
	public float currentOutputRate = 0.0f;
	public const int OUTPUT_AT = 1;
	public const int BP_OUT_AT = 1;
	public const int BP_NEG_OUT = -1;
	public const int MAX_HEIGHT = 38;
	public bool timeToGrow = false;
	public bool timeToOutput = false;

	public const float MAX_OUTPUT_RATE = 10f;
	public const float MAX_BP_RATE = 100f;

	public int purchaseCost;

	public bool placed;

	// Use this for initialization
	void Start () {
		if (placed != true){
			placed = false;
		}
		string ct = this.gameObject.tag;
		if (ct == "Red Gro A") {
			colorType = RA;
		} else if (ct == "Red Gro AA") {
			colorType = RAA;
		} else if (ct == "Red Gro AAA") {
			colorType = RAAA;
		} else if (ct == "Blue Gro A") {
			colorType = BA;
		} else if (ct == "Blue Gro AA") {
			colorType = BAA;
		} else if (ct == "Blue Gro AAA") {
			colorType = BAAA;
		} else if (ct == "Green Gro A") {
			colorType = GA;
		} else if (ct == "Green Gro AA") {
			colorType = GAA;
		} else if (ct == "Green Gro AAA") {
			colorType = GAAA;
		} else if (ct == "Yellow Gro") {
			colorType = Y;
		} else {
			colorType = RA;
		}

		SetValues(colorType);
		SetGrowthCurve();

		Random.seed = System.DateTime.Now.Millisecond;
	}

	// Update is called once per frame
	void Update () {
		if (!placed) {
			return;
		}

		currentBPWait += bpRate * Time.deltaTime;
		if (currentBPWait >= BP_OUT_AT || currentBPWait <= BP_NEG_OUT) {
			if(currentBPWait < 0){
				ProduceBP(true);
			} else {
				ProduceBP(false);
			}
		}

		currentOutputWait += outputRate * Time.deltaTime;
		if (currentOutputWait >= OUTPUT_AT) {
			Output();
		}

		currentGrowthAmount += growthRate * Time.deltaTime;//growthRate * Time.deltaTime;
		if (currentGrowthAmount >= currGrowthNeeded) {
			Grow ();
		}
	}

	void OnTriggerEnter(Collider other){
		if (!placed) {
			return;
		}

		if (other.gameObject.tag == "Red Blop") {
			if(colorType == RA || colorType == RAA || colorType == RAAA){
				growthRate += 0.0001f;
				outputRate += 0.0001f;
				bpRate += 0.0001f;
			} else if (colorType == BA || colorType == BAA || colorType == BAAA){
				growthRate += 0.0002f;
				outputRate += 0.0002f;
				bpRate += 0.0002f;
			} else if (colorType == GA || colorType == GAA || colorType == GAAA){
				growthRate += 0.0002f;
				outputRate += 0.0002f;
				bpRate += 0.0002f;
			} else if (colorType == Y){
				growthRate += 0.00000f;
				outputRate += 0.00000f;
				bpRate += 0.0000f;
			}
		} else if (other.gameObject.tag == "Blue Blop") {
			if(colorType == RA || colorType == RAA || colorType == RAAA){
				growthRate += 0.0003f;
				outputRate += 0.0003f;
				bpRate += 0.001f;
			} else if (colorType == BA || colorType == BAA || colorType == BAAA){
				growthRate += 0.0002f;
				outputRate += 0.0002f;
				bpRate -= 0.001f;
			} else if (colorType == GA || colorType == GAA || colorType == GAAA){
				growthRate += 0.0001f;
				outputRate += 0.0001f;
				bpRate += 0.0001f;
			} else if (colorType == Y){
				growthRate += 0.0000f;
				outputRate += 0.0000f;
				bpRate += 0.0000f;
			}
		} else if (other.gameObject.tag == "Green Blop") {
			if(colorType == RA || colorType == RAA || colorType == RAAA){
				growthRate += 0.0002f;
				outputRate += 0.0002f;
				bpRate += 0.001f;
			} else if (colorType == BA || colorType == BAA || colorType == BAAA){
				growthRate += 0.0001f;
				outputRate += 0.0001f;
				bpRate += 0.0001f;
			} else if (colorType == GA || colorType == GAA || colorType == GAAA){
				growthRate += 0.00025f;
				outputRate += 0.00025f;
				bpRate += 0.001f;
			} else if (colorType == Y){
				growthRate += 0.00001f;
				outputRate += 0.00001f;
				bpRate += 0.0000f;
			}
		} else if (other.gameObject.tag == "Yellow Blop") {
			if(colorType == RA || colorType == RAA || colorType == RAAA){
				growthRate -= 0.001f;
				outputRate -= 0.001f;
				bpRate += 0.01f;
			} else if (colorType == BA || colorType == BAA || colorType == BAAA){
				growthRate += 0.001f;
				outputRate += 0.001f;
				bpRate += 0.01f;
			} else if (colorType == GA || colorType == GAA || colorType == GAAA){
				growthRate += 0.001f;
				outputRate += 0.001f;
				bpRate += 0.01f;
			} else if (colorType == Y){
				growthRate += 0.000025f;
				outputRate += 0.000025f;
				bpRate += 0.0001f;
			}
		}

		if (bpRate > MAX_BP_RATE) {
			bpRate = MAX_BP_RATE;
		}

		if (outputRate > MAX_OUTPUT_RATE) {
			outputRate = MAX_OUTPUT_RATE;
		}

		Destroy (other.gameObject);

		blopsTaken++;
	}

	void SetValues(int colorType){
		if (colorType == RA) {
			growthRate = 0.1f;
			outputRate = 0.3f;
			bpRate = 0.1f;
			purchaseCost = 15;
		} else if (colorType == RAA) {
			growthRate = 0.12f;
			outputRate = 0.8f;
			bpRate = 0.15f;
			purchaseCost = 1000;
		} else if (colorType == RAAA) {
			growthRate = 0.15f;
			outputRate = 1.2f;
			bpRate = 0.2f;
			purchaseCost = 45000;
		} else if (colorType == BA) {
			growthRate = 0.05f;
			outputRate = 1f;
			bpRate = 0.2f;
			purchaseCost = 25;
		} else if (colorType == BAA) {
			growthRate = 0.08f;
			outputRate = 2f;
			bpRate = 0.5f;
			purchaseCost = 2350;
		} else if (colorType == BAAA) {
			growthRate = 0.1f;
			outputRate = 5f;
			bpRate = 0.8f;
			purchaseCost = 130000;
		} else if (colorType == GA) {
			growthRate = 0.5f;
			outputRate = 0.1f;
			bpRate = 0.15f;
			purchaseCost = 20;
		} else if (colorType == GAA) {
			growthRate = 0.75f;
			outputRate = 0.15f;
			bpRate = 0.25f;
			purchaseCost = 1750;
		} else if (colorType == GAAA) {
			growthRate = 1f;
			outputRate = 0.2f;
			bpRate = 0.5f;
			purchaseCost = 100000;
		} else if (colorType == Y) {
			growthRate = 0.001f;
			outputRate = 0.2f;
			bpRate = 10f;
			purchaseCost = 2500000;
		} else {
			growthRate = 0.1f;
			outputRate = 0.3f;
			bpRate = 0.1f;
			purchaseCost = 15;
		}
	}
	
	void SetGrowthCurve(){
		growthLevels = new float[MAX_LEVEL];
		for (int i = 0; i < growthLevels.Length; i++) {
			if(i < MAX_LEVEL/2){
				growthLevels[i] = 1f;
			} else  if ( i >= MAX_LEVEL/2 && i < (MAX_LEVEL * 0.80f)) {
				growthLevels[i] = 1.5f;
			} else if ( i >= (MAX_LEVEL * 0.80f)){
				growthLevels[i] = 2f;
			}
		}

		currGrowthNeeded = growthLevels [currentLevel];
	}

	void Grow(){
		if (currentLevel == MAX_LEVEL) {
			return;
		}

		while (currentGrowthAmount > currGrowthNeeded) {
			float x = transform.localScale.x;
			float y = transform.localScale.y;
			float z = transform.localScale.z;
			transform.localScale = new Vector3 (x, y + 0.01f, z);

			currentGrowthAmount = currentGrowthAmount - currGrowthNeeded;
			if(currentGrowthAmount < 0){
				currentGrowthAmount = 0;
			}
			currentLevel++;
			if(currentLevel != MAX_LEVEL){
				currGrowthNeeded = growthLevels [currentLevel];
			}
		}

		currentBPWait = (float) BP_OUT_AT;

	}

	void Output(){
		GameObject blop;

		float x = transform.position.x;
		float y = transform.position.y;
		float z = transform.position.z;

		int[] possibilities = GameObject.FindGameObjectWithTag ("Camera Manager").GetComponent<CameraManager> ().CanOutputTo (x, z);
		List<int> pos = new List<int> ();

		for (int i = 0; i <possibilities.Length; i++) {
			if(possibilities[i] != 0){
				pos.Add(i);
			}
		}

		if (pos.Count != 0) {
			//needs to be more complex, put onto path
			int[] chooseFrom = pos.ToArray();
			int ranIndex = (int)Random.Range (0, pos.Count);
			int dir = chooseFrom[ranIndex];
			Vector3 blopPosition;
			//xpos
			if (dir == 0) {
				blopPosition = new Vector3 (x + 1.0f, y, z);
			//xneg
			} else if (dir == 1) {
				blopPosition = new Vector3 (x - 1.0f, y, z);
			//zpos
			} else if (dir == 2) {
				blopPosition = new Vector3 (x, y, z + 1.0f);
			//zneg
			} else if (dir == 3) {
				blopPosition = new Vector3 (x, y, z - 1.0f);
			//default xpos
			} else {
				blopPosition = new Vector3 (x + 1.0f, y, z);
			}

			Vector3 yOffset; //prevent z colliding

			if (colorType == RA || colorType == RAA || colorType == RAA) {
				blop = (GameObject)Instantiate (Resources.Load ("Red Blop"));
				yOffset = new Vector3 (0, 0.001f, 0);
			} else if (colorType == BA || colorType == BAA || colorType == BAAA) {
				blop = (GameObject)Instantiate (Resources.Load ("Blue Blop"));
				yOffset = new Vector3 (0, 0, 0);
			} else if (colorType == GA || colorType == GAA || colorType == GAAA) {
				blop = (GameObject)Instantiate (Resources.Load ("Green Blop"));
				yOffset = new Vector3 (0, 0.002f, 0);
			} else if (colorType == Y) {
				blop = (GameObject)Instantiate (Resources.Load ("Yellow Blop"));
				yOffset = new Vector3 (0, 0.003f, 0);
			} else {
				blop = (GameObject)Instantiate (Resources.Load ("Red Blop"));
				yOffset = new Vector3 (0, 0, 0);
			}

			blop.transform.position = blopPosition;
			blop.transform.position += yOffset;
			blop.GetComponent<BlopScript>().setDirection(dir+1);
			blop.GetComponent<BlopScript>().DetermineDirection();
			
			currentOutputWait = 0f;

			if(colorType == RA || colorType == RAA || colorType == RAAA){
				currentBPWait += 0.01f;
			} else if (colorType == BA || colorType == BAA || colorType == BAAA){
				currentBPWait += 0.001f;
			} else if (colorType == GA || colorType == GAA || colorType == GAAA){
				currentBPWait += 0.01f;
			} else if (colorType == Y){
				currentBPWait = (float) BP_OUT_AT;
			}

			blopsOutput++;
		}
	}

	void ProduceBP(bool negative){
		if (!negative) {
			AddBlopPoints ((int)currentBPWait + (currentLevel / 100));
		} else {
			AddBlopPoints((int) currentBPWait - (currentLevel/100));
		}
		currentBPWait = 0f;
	}

	void AddBlopPoints(int amt){
		GameObject.FindGameObjectWithTag ("BP").GetComponent<BlopPoints> ().Add (amt);
	}

}
