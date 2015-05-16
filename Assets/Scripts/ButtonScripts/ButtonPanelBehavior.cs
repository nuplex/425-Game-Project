using UnityEngine;
using System.Collections;

public class ButtonPanelBehavior : MonoBehaviour {

	public GameObject ColorsPanel;
	public GameObject cube;
	public Texture2D pathTexture, destroyTexture, defaultTexture;

	// show or hide the colors panel
	public void toggleColorsPanel() {
		ColorsPanel.SetActive (!ColorsPanel.activeSelf);
	}

	public void setCursorToPath() {
		Cursor.SetCursor (pathTexture, new Vector2 (256, 256), CursorMode.Auto);
	}

	public void setCursorToDestroy() {
		Cursor.SetCursor (destroyTexture, new Vector2(256, 256), CursorMode.Auto);
	}

	public void setCursorToDefault() {
		Cursor.SetCursor (defaultTexture, new Vector2(65, 65), CursorMode.Auto);
	}
}
