using UnityEngine;
using System.Collections;

[System.Serializable]
public class BlopPoints : MonoBehaviour {

	public UnityEngine.UI.Text bpText;
	private const string bp = "BP: ";
	long points;

	// Use this for initialization
	void Start () {
		points = 100;
		UpdateText ();
	}
	
	// Update is called once per frame
	void Update () {

	}

	public bool CanBuy(long amt){
		if (points - amt < 0) {
			return false;
		} else {
			return true;
		}
	}

	public void Use(long amt){
		if (points - amt < 0) {
			return;
		}
		points -= amt;
		UpdateText ();
	}

	public void Add(long amt){
		points += amt;
		UpdateText ();
	}

	void UpdateText(){
		bpText.text = bp + points;
	}
}
