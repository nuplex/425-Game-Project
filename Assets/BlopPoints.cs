using UnityEngine;
using System.Collections;

public class BlopPoints : MonoBehaviour {

	public UnityEngine.UI.Text bpText;
	private const string bp = "BP: ";
	int points;

	// Use this for initialization
	void Start () {
		points = 100;
		UpdateTex ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public bool CanBuy(int amt){
		if (points - amt < 0) {
			return false;
		} else {
			return true;
		}
	}

	public void Use(int amt){
		if (points - amt < 0) {
			return;
		}
		points -= amt;
		UpdateTex ();
	}

	public void Add(int amt){
		points += amt;
		UpdateTex ();
	}

	void UpdateTex(){
		bpText.text = bp + points;
	}
}
