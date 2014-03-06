using UnityEngine;
using UnityEditor;
using System.Collections;
using System;

// This class is the heart and soul of Kat2D Editor.
public class KEditorManager : EditorWindow {
	// Display variables...
	private int menuHeight = 20;
	private int menuSelection = 0;

	Texture2D whiteTexture = null; // new Texture2D (2, 2);

	// Data definitions
	private static string[] menuItems = new string[] {"Scenes", "Tiles", "Objects", "Settings"};
	private IKForm[] menuForms = new IKForm[menuItems.Length];
	private IKForm currentForm = null;

	public Texture2D getWhiteTexture() {
		if (whiteTexture == null) {
			whiteTexture = new Texture2D(2,2);
			whiteTexture.hideFlags = HideFlags.HideAndDontSave;
		}
		return whiteTexture;
	}

	public int getStartX(){
		return 0;
	}
	public int getStartY(){
		return menuHeight;
	}

	// Add menu named "My Window" to the Window menu
	[MenuItem ("Window/Kat2D Editor")]
	static void Init () {
		//EditorWindow window = EditorWindow.GetWindow<EditorManager2D> ("Kat2D Manager");
		EditorWindow.GetWindow<KEditorManager> ("Kat2D");

	}



	
	void OnGUI () {
		// Draw the top Menu
		GUI.Box(new Rect(0, 0, position.width, menuHeight),"");
		// Draw menu options
		menuSelection = GUI.SelectionGrid(new Rect(0, 0, 250, menuHeight), menuSelection, menuItems, menuItems.Length);

		switch(menuSelection){
		case 0: // Scene form
			if(menuForms[menuSelection]==null){
				menuForms[menuSelection] = new KSceneForm();
			}
			currentForm = menuForms[menuSelection];
			break;
		}

		if (currentForm != null) {
			currentForm.FormGUI (this);
		}
	}

	//void OnInspectorUpdate() {
	void Update() {
		if (currentForm != null) {
			currentForm.FormUpdate (this);
		}
	}

	// This is where we clean up...
	void OnDestry() {
		Resources.UnloadAsset (whiteTexture);
		for (int i=0; i<menuForms.Length; i++) {
			if(menuForms[i] != null) {
				menuForms[i].OnDestroy();
			}
		}
	}

	public void DoRequest() {

	}
}