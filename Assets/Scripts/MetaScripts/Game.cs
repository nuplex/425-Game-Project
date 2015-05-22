using UnityEngine;
using System.Collections;

[System.Serializable]
public class Game {

	[SerializeField]
	public static Game current;

	[SerializeField]
	public long blopPoints;

	[SerializeField]
	public Grid grid;

	public Game(){
		blopPoints = GetBP ();
		grid = new Grid ();
		current = this;
	}

	public Game GetCurrent(){
		return current;
	}

	private long GetBP(){
		return GameObject.FindGameObjectWithTag ("BP").GetComponent<BlopPoints>().GetPoints();
	}

}
