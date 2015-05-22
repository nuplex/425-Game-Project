using UnityEngine;
using System.Collections;

[System.Serializable]
public class Blop : SaveObject{

	public float addToOutputRate;
	public float addToGrowthRate;
	
	public float xOffset;
	public float yOffset;
	public float zOffset;

	public float speed;
	
	public int currDir;

	public float lastTurnX;
	public float lastTurnZ;
	
	public float currentX; //needed for saving/loading games
	public float currentZ; //needed for saving/loading games
	public float y;

	public Blop(GameObject blop, string tag):base(tag){
		this.tag = tag;
		BlopScript bs = blop.GetComponent<BlopScript> ();

		addToOutputRate = bs.addToOutputRate;
		addToGrowthRate = bs.addToGrowthRate;

		xOffset = bs.offset.x;
		yOffset = bs.offset.y;
		zOffset = bs.offset.z;

		currDir = bs.currDir;

		lastTurnX = bs.lastTurnX;
		lastTurnZ = bs.lastTurnZ;

		currentX = bs.currentX;
		currentZ = bs.currentZ;
		y = blop.transform.position.y;

	}
	
	override public GameObject ToGameObject(){
		GameObject blop = (GameObject) GameObject.Instantiate (Resources.Load (tag));
		BlopScript bs = blop.GetComponent<BlopScript> ();

		bs.addToOutputRate = addToOutputRate;
		bs.addToGrowthRate = addToGrowthRate;
		
		bs.offset = new Vector3 (xOffset, yOffset, zOffset);
		
		bs.currDir = currDir;
		
		bs.lastTurnX = lastTurnX;
		bs.lastTurnZ = lastTurnZ;
		
		bs.currentX = currentX;
		bs.currentZ = currentZ;

		blop.transform.position = new Vector3(currentX, y, currentZ);

		return blop;
	}

}
