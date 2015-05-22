using UnityEngine;
using System.Collections;

[System.Serializable]
public class Grid{

	[SerializeField]
	public SaveObject[,] grid;

	[SerializeField]
	public Blop[] blops;

	public int lengthX = 100;

	public int lengthZ = 100;

	public Grid(){
		//int lengthX = CameraManager.GRID_LENGTH_X;
		//int lengthZ = CameraManager.GRID_LENGTH_Z;
		Debug.Log (lengthX);
		grid = new SaveObject[lengthX, lengthZ];
		GameObject[,] currGrid = GameObject.FindGameObjectWithTag ("Camera Manager").GetComponent<CameraManager> ().GetGrid ();
	
		for(int i = 0; i < lengthX; i++){
			for(int j = 0; i <lengthZ; i++){
				if(currGrid[i,j] == null){
					grid[i,j] = null;
					continue;
				}
				grid[i,j] = TranslateToGrid(currGrid[i,j]);
			}
		}
	
		blops = GetBlopGameObjects ();

	}

	public SaveObject TranslateToGrid(GameObject go){
		if (go.tag == "Red Gro A" || go.tag == "Red Gro AA" || go.tag == "Red Gro AAA" 
			|| go.tag == "Green Gro A" || go.tag == "Green Gro AA" || go.tag == "Green Gro AAA"
			|| go.tag == "Blue Gro A" || go.tag == "Blue Gro AA" || go.tag == "Blue Gro AAA"
			|| go.tag == "Yellow Gro") {
			return new Gro(go, go.tag);
		} else if (go.tag == "path_a" || go.tag == "path_btl" || go.tag == "path_btr" || go.tag == "path_h" ||
			go.tag == "path_3b" || go.tag == "path_3l" || go.tag == "path_3r" || go.tag == "path_3t" || 
			go.tag == "path_ttl" || go.tag == "path_ttr" || go.tag == "path_v" || go.tag == "path_default") {
			return new Path(go, go.tag);
		} else {
			return null;
		}
	}

	private Blop[] GetBlopGameObjects(){
		Blop[] blops;

		GameObject[] rb = GameObject.FindGameObjectsWithTag ("Red Blop");
		GameObject[] bb = GameObject.FindGameObjectsWithTag ("Blue Blop");
		GameObject[] gb = GameObject.FindGameObjectsWithTag ("Green Blop");
		GameObject[] yb = GameObject.FindGameObjectsWithTag ("Yellow Blop");

		int blopsLength = rb.Length + bb.Length + gb.Length + yb.Length;
		blops = new Blop[blopsLength];

		int copyStart = 0;
		for (int i = copyStart, j = 0; i < rb.Length; i++, j++) {
			blops[i] = new Blop(rb[j], rb[j].tag);
		}
		copyStart += rb.Length;
		for (int i = copyStart, j = 0; i < bb.Length; i++, j++) {
			blops[i] = new Blop(bb[j], bb[j].tag);
		}
		copyStart += bb.Length;
		for (int i = copyStart, j = 0; i < gb.Length; i++, j++) {
			blops[i] = new Blop(gb[j], gb[j].tag);
		}
		copyStart += gb.Length;
		for (int i = copyStart, j = 0; i < yb.Length; i++, j++) {
			blops[i] = new Blop(yb[j], yb[j].tag);
		}

		return blops;
	}

	public GameObject[,] ToGameObjectGrid(){
		Debug.Log("HERE");
		Debug.Log(lengthX);
		GameObject[,] goGrid = new GameObject[lengthX, lengthZ];
		
		for(int i = 0; i < lengthX; i++){
			for(int j = 0; i < lengthZ; i++){
				Debug.Log("HERE");
				if(grid[i,j] != null){
					Debug.Log("IN HERE");
					goGrid[i,j] = grid[i,j].ToGameObject();
				}
			}
		}

		return goGrid;
	}

	public Blop[] GetBlops(){
		return blops;
	}

}
