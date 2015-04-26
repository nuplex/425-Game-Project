using UnityEngine;
using System.Collections;

public class ColorsButtonBehavior : MonoBehaviour {

	public GameObject ColorsPanel;

	// show or hide the colors panel
	public void toggleColorsPanel() {
		ColorsPanel.SetActive (!ColorsPanel.activeSelf);
	}
}
