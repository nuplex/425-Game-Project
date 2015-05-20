using UnityEngine;
using System.Collections;

public class InfoButton : MonoBehaviour {

	public GameObject pricePanel;
	bool priceActive;

	// Use this for initialization
	void Start () {
		pricePanel.SetActive (false);
		priceActive = false;
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void ShowPrices(){
		if (!priceActive) {
			pricePanel.SetActive (true);
			priceActive = true;
		} else {
			pricePanel.SetActive(false);
			priceActive = false;
		}
	}
}
