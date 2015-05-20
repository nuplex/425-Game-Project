using UnityEngine;
using System.Collections;

public class ButtonPanelBehavior : MonoBehaviour {

	public GameObject ColorsPanel;
	public GameObject cube;
	public Texture2D pathCursor, destroyCursor, placingCursor, defaultCursor;

	// show or hide the colors panel
	public void toggleColorsPanel() {
		ColorsPanel.SetActive (!ColorsPanel.activeSelf);
	}

	public void setCursorToPath() {
		Cursor.SetCursor (pathCursor, new Vector2 (256, 256), CursorMode.Auto);
	}

	public void setCursorToDestroy() {
		Cursor.SetCursor (destroyCursor, new Vector2(256, 256), CursorMode.Auto);
	}

	public void setCursorToDefault() {
		Cursor.SetCursor (defaultCursor, new Vector2(65, 65), CursorMode.Auto);
	}

	public void setCursorToPlacing() {
		Cursor.SetCursor (placingCursor, new Vector2(12, -12), CursorMode.Auto);
	}
}
