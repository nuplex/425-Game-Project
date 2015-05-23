using UnityEngine;
using System.Collections;

[System.Serializable]
public class Gro : SaveObject{
	//[SerializeField]
	public int colorType;

	//[SerializeField]
	public float growthRate; //per some in
	//[SerializeField]
	public float outputRate; //per second

	//[SerializeField]
	public float bpRate;

	//[SerializeField]
	public float[] growthLevels;

	//[SerializeField]
	public int currentLevel;

	//[SerializeField]
	public float currGrowthNeeded;

	//[SerializeField]
	public int blopsTaken;

	//[SerializeField]
	public int blopsOutput;

	//[SerializeField]
	public float currentOutputWait; //amount of output rate that hasn't been output

	//[SerializeField]
	public float currentBPWait;

	//[SerializeField]
	public float currentGrowthAmount; 

	//[SerializeField]
	public float currentOutputRate;

	//[SerializeField]
	public int purchaseCost;

	//[SerializeField]
	public bool placed;

	public float x;
	public float y;
	public float z;

	public Gro(GameObject gro, string tag):base(tag){

		GroScript gs = gro.GetComponent<GroScript> ();
		colorType = gs.colorType;
		growthRate = gs.growthRate;
		bpRate = gs.bpRate;
	
		currentLevel = gs.currentLevel;
		currGrowthNeeded = gs.currGrowthNeeded;

		blopsTaken = gs.blopsTaken;
		blopsOutput = gs.blopsOutput;

		currentOutputWait = gs.currentOutputWait;
		currentBPWait = gs.currentBPWait;

		currentGrowthAmount = gs.currentGrowthAmount;
		currentOutputRate = gs.currentOutputRate;

		purchaseCost = gs.purchaseCost;

		placed = gs.placed;

		x = gro.transform.position.x;
		y = gro.transform.position.y;
		z = gro.transform.position.z;
	}

	override public GameObject ToGameObject(){
		GameObject gro = (GameObject) GameObject.Instantiate(Resources.Load(tag));
		GroScript gs = gro.GetComponent<GroScript> ();

		gs.colorType = colorType;
		gs.growthRate = growthRate;
		gs.bpRate = bpRate;
		
		gs.currentLevel = currentLevel;

		gro.transform.localScale = new Vector3 (gro.transform.localScale.x, gro.transform.localScale.y + (0.01f * currentLevel), gro.transform.localScale.z);

		gs.currGrowthNeeded = currGrowthNeeded;
		
		gs.blopsTaken = blopsTaken;
		gs.blopsOutput = blopsOutput;
		
		gs.currentOutputWait = currentOutputWait;
		gs.currentBPWait = currentBPWait;
		
		gs.currentGrowthAmount = currentGrowthAmount;
		gs.currentOutputRate = currentOutputRate;
		
		gs.purchaseCost = purchaseCost;
		
		gs.placed = placed;

		gro.transform.position = new Vector3(x, y, z);

		return gro;
	}

}
