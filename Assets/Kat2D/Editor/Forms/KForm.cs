using System;
using UnityEngine;

public abstract class KForm : IKForm {
	private int startX = 0;
	private int startY = 0;

	public void SetStartY(int startY) {
		this.startY = startY;
	}
	public int NextStartY(int increment) {
		int retval = this.startY;
		this.startY += increment;
		return retval;
	}

	public int NextStartX(int increment) {
		int retval = this.startX;
		this.startX += increment;
		return retval;
	}

	public abstract void FormGUI (KEditorManager manager);

	public abstract void FormUpdate (KEditorManager manager);

	private GUIStyle errorStyle = null;
	private GUIStyle headerStyle = null;

	public GUIStyle getErrorStyle(Texture2D bg) {
		if (errorStyle == null) {
			errorStyle = new GUIStyle ();
			errorStyle.normal.textColor = Color.red; 
			errorStyle.alignment = TextAnchor.MiddleCenter;
			errorStyle.normal.background = errorStyle.onHover.background =errorStyle.hover.background = bg;
			errorStyle.padding.left = errorStyle.padding.right = errorStyle.padding.top = errorStyle.padding.bottom = 4;
		}
		return errorStyle;
	}

	public GUIStyle getHeaderStyle(Texture2D bg) {
		if (headerStyle == null) {
			headerStyle = new GUIStyle ();
			headerStyle.normal.textColor = Color.black; 
			headerStyle.alignment = TextAnchor.MiddleCenter;
			headerStyle.normal.background = headerStyle.onHover.background =headerStyle.hover.background = bg;
			headerStyle.padding.left = headerStyle.padding.right = headerStyle.padding.top = headerStyle.padding.bottom = 4;
		}
		return headerStyle;
	}

	public virtual void OnDestroy () {
		//
	}
}