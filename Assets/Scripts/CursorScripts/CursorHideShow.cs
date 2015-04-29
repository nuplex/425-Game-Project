using UnityEngine;
using System.Collections;

public class CursorHideShow : MonoBehaviour {
	bool isVisible;
	public Texture2D defaultCursor;

	// Use this for initialization
	void Start () {
		Cursor.SetCursor (defaultCursor, new Vector2(65, 65), CursorMode.Auto);
		Cursor.visible = true;
		isVisible = true;
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.V)) {
			isVisible = !isVisible;
			Cursor.visible = isVisible;
		}
	}
}
