using UnityEngine;
using System.Collections;

[System.Serializable]
public class Grid{

	//[SerializeField]
	//public SaveObject[,] grid;

	[SerializeField]
	public Blop[] blops;

	[SerializeField]
	public Path[] paths;

	[SerializeField]
	public Gro[] gros;

	public Grid(){
		//int lengthX = CameraManager.GRID_LENGTH_X;
		//int lengthZ = CameraManager.GRID_LENGTH_Z;

		gros = GetGros ();

		paths = GetPaths ();
	
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

	/*
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
	}*/

	public Path[] GetPaths(){
		Path[] paths;

		CameraManager cm = GameObject.FindGameObjectWithTag ("Camera Manager").GetComponent<CameraManager> ();
		GameObject[,] grid = cm.GetGrid ();
		int k = 0;
		for (int i = 0; i < CameraManager.GRID_LENGTH_X; i++) {
			for (int j = 0; j < CameraManager.GRID_LENGTH_Z; j++){
				GameObject o = grid[i,j];
				if( o == null){
					continue;
				}
				if(o.tag == "path_a" || o.tag == "path_btl" || o.tag == "path_btr" ||
				              o.tag == "path_3b" || o.tag == "path_3t" || o.tag == "path_3l" ||
				              o.tag == "path_3r" || o.tag == "path_ttl" || o.tag == "path_ttr" ||
				              o.tag == "path_h" || o.tag == "path_v" || o.tag == "path_default"){
					k++;
				}
			}
		}

		paths = new Path[k];

		k = 0;
		for (int i = 0; i < CameraManager.GRID_LENGTH_X; i++) {
			for (int j = 0; j < CameraManager.GRID_LENGTH_Z; j++){
				GameObject o = grid[i,j];
				if( o == null){
					continue;
				}
				if(o.tag == "path_a" || o.tag == "path_btl" || o.tag == "path_btr" ||
				   o.tag == "path_3b" || o.tag == "path_3t" || o.tag == "path_3l" ||
				   o.tag == "path_3r" || o.tag == "path_ttl" || o.tag == "path_ttr" ||
				   o.tag == "path_h" || o.tag == "path_v" || o.tag == "path_default"){
					paths[k] = new Path(o, o.tag);
					k++;
				}
			}
		}

		return paths;
	}

	public Gro[] GetGros(){
		Gro[] gros;
		GameObject[] ra = GameObject.FindGameObjectsWithTag ("Red Gro A");
		GameObject[] raa = GameObject.FindGameObjectsWithTag ("Red Gro AA");
		GameObject[] raaa = GameObject.FindGameObjectsWithTag ("Red Gro AAA");
		GameObject[] ba = GameObject.FindGameObjectsWithTag ("Blue Gro A");
		GameObject[] baa = GameObject.FindGameObjectsWithTag ("Blue Gro AA");
		GameObject[] baaa = GameObject.FindGameObjectsWithTag ("Blue Gro AAA");
		GameObject[] ga = GameObject.FindGameObjectsWithTag ("Green Gro A");
		GameObject[] gaa = GameObject.FindGameObjectsWithTag ("Green Gro AA");
		GameObject[] gaaa = GameObject.FindGameObjectsWithTag ("Green Gro AAA");
		GameObject[] y = GameObject.FindGameObjectsWithTag ("Yellow Gro");

		
		int grosLength = ra.Length + raa.Length + raaa.Length 
				+ ba.Length + baa.Length + baaa.Length 
				+ ga.Length + gaa.Length + gaaa.Length + y.Length;
		gros = new Gro[grosLength];
		
		int copyStart = 0;
		for (int i = copyStart, j = 0; j < ra.Length; i++, j++) {
			gros[i] = new Gro(ra[j], ra[j].tag);
		}

		copyStart += ra.Length;
		for (int i = copyStart, j = 0; j < raa.Length; i++, j++) {
			gros[i] = new Gro(raa[j], raa[j].tag);
		}

		copyStart += raa.Length;
		for (int i = copyStart, j = 0; j < raaa.Length; i++, j++) {
			gros[i] = new Gro(raaa[j], raaa[j].tag);
		}

		copyStart += raaa.Length;
		for (int i = copyStart, j = 0; j < ba.Length; i++, j++) {
			gros[i] = new Gro(ba[j], ba[j].tag);
		}

		copyStart += ba.Length;
		for (int i = copyStart, j = 0; j < baa.Length; i++, j++) {
			gros[i] = new Gro(baa[j], baa[j].tag);
		}

		copyStart += baa.Length;
		for (int i = copyStart, j = 0; j < baaa.Length; i++, j++) {
			gros[i] = new Gro(baaa[j], baaa[j].tag);
		}

		copyStart += baaa.Length;
		for (int i = copyStart, j = 0; j < ga.Length; i++, j++) {
			gros[i] = new Gro(ga[j], ga[j].tag);
		}

		copyStart += ga.Length;
		for (int i = copyStart, j = 0; j < gaa.Length; i++, j++) {
			gros[i] = new Gro(gaa[j], gaa[j].tag);
		}

		copyStart += gaa.Length;
		for (int i = copyStart, j = 0; j < gaaa.Length; i++, j++) {
			gros[i] = new Gro(gaaa[j], gaaa[j].tag);
		}

		copyStart += gaaa.Length;
		for (int i = copyStart, j = 0; j < y.Length; i++, j++) {
			gros[i] = new Gro(y[j], y[j].tag);
		}
		
		return gros;

	}

	public Blop[] GetBlops(){
		return blops;
	}

	public Path[] GetPathsArray(){
		return paths;
	}

	public Gro[] GetGrosArray(){
		return gros;
	}

}
