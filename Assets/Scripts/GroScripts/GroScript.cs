using UnityEngine;
using System.Collections;

public class GroScript : MonoBehaviour {

	private int color; 
	const int RED = 1, BLUE = 2, GREEN = 3, YELLOW = 4;

	public double growthRate;
	public double outputRate;
	public int[] growthLevels;
	public int currentLevel;

	public int blopsTaken;
	public int blopsOutput;

	public double currentGrowthAmount; 
	public double currentOutputRate;

	public int growthTimer;
	public int outputTimer;
	public bool timeToGrow;
	public bool timeToOutput;


	// Use this for initialization
	void Start () {
		string col = this.gameObject.tag;
		if (col == "Red Gro") {
			color = RED;
		} else if (col == "Blue Gro") {
			color = BLUE;
		} else if (col == "Green Gro") {
			color = GREEN;
		} else if (col == "Yellow Gro") {
			color = YELLOW;
		} else {
			color = RED;
		}



	}

	
	void SetValuesByColor(int color){

	}

	void SetValuesByLevel(int color){

	}

	// Update is called once per frame
	void Update () {
	
	}
}
