using UnityEngine;
using System.Collections;

public class ButtonPanelBehavior : MonoBehaviour {

	public GameObject ColorsPanel;
	public Texture2D pathTexture, destroyTexture, defaultTexture;

	// show or hide the colors panel
	public void toggleColorsPanel() {
		ColorsPanel.SetActive (!ColorsPanel.activeSelf);
	}

	public void setCursorToPathIcon() {
		Cursor.SetCursor (pathTexture, new Vector2 (256, 256), CursorMode.Auto);
	}

	public void setCursorToDestroyIcon() {
		Cursor.SetCursor (destroyTexture, new Vector2(256, 256), CursorMode.Auto);
	}

	public void setCursorToDefaultIcon() {
		Cursor.SetCursor (defaultTexture, new Vector2(65, 65), CursorMode.Auto);
	}
}
