using UnityEngine;
using UnityEditor;
using System.Collections;

public class KSceneForm : KForm {
	private enum State {
		None,
		Init,
		CreateForm,
		SaveForm,
		NoScenes,
		PickScene
	}
	private State currentState = State.Init;
	private State nextState = State.Init;
	private string[] sceneNames;
	private SceneModel sceneModel = null;
	private GUIStyle listStyle;
	private Vector2 scrollPosition = Vector2.zero;

	KDropDown singleAreaControl = null;
	KDropDown sceneTypeControl = null;

	string intTemp = "";
	int intReal = 0;

	string errorMsg = null;
	
	public override void FormGUI (KEditorManager manager) {
		float formWidth = 400;
		float formHeight = 200;
		float padding = 10;
		float halfWidth = formWidth/2;
		float y = manager.getStartY();
		SetStartY(manager.getStartY());

		switch (currentState) {
		case State.NoScenes:
			SetStartY(manager.getStartY());
			// On Init, we need to load up the scene names.
			GUI.Label(new Rect(0, NextStartY(50), manager.position.width, 20), "There are no Scenes defined. Click on 'Create Scene' to get started!", getErrorStyle(manager.getWhiteTexture()));
			if(GUI.Button(new Rect(manager.position.width/2 - 50, NextStartY(50), 100, 50 ), "Create Scene")){
				nextState = State.CreateForm;
			}
			break;
		case State.CreateForm:
			GUI.Label(new Rect(0, y, manager.position.width, 20), "Create a new Scene", 
			          getHeaderStyle(manager.getWhiteTexture()));

			// Wrap in scrollview just in case...
			scrollPosition = GUI.BeginScrollView(new Rect(0, 40, manager.position.width, 
			                 manager.position.height-20), scrollPosition, new Rect(0, 0, 500, 350));

			// Create background for form, makes it look pretty
			GUI.Box(new Rect(manager.position.width/2 - halfWidth, y, formWidth, formHeight),"");

			if(errorMsg==null) {
				GUI.Label(new Rect(manager.position.width/2 - halfWidth, y, formWidth, 20), "Scene Form", 
				          getHeaderStyle(manager.getWhiteTexture()));
			}else{
				GUI.Label(new Rect(manager.position.width/2 - halfWidth, y, formWidth, 20), errorMsg, 
				          getErrorStyle(manager.getWhiteTexture()));
			}

			// Need form fields now
			GUI.Label (new Rect(manager.position.width/2 - halfWidth + padding, y+40, 100, 20), 
			           "Scene Name:");
			sceneModel.Name = GUI.TextField(new Rect(manager.position.width/2, y+40, 
			                                         halfWidth - padding, 20), sceneModel.Name);


			/*
			GUI.Label (new Rect(manager.position.width/2 - halfWidth + padding, y+70, halfWidth, 20), 
			           "Is single area Scene?");
			if(singleAreaControl == null){
				singleAreaControl= new KDropDown(new GUIContent[] {new GUIContent("No"), new GUIContent("Yes")}, 
				new Rect(0,0,0,0));
			}
			singleAreaControl.setPosition(new Rect(manager.position.width/2, y+70, 100, 20));
			singleAreaControl.Show(sceneModel.SingleArea);
			*/

			GUI.Label (new Rect(manager.position.width/2 - halfWidth + padding, y+70, halfWidth, 20), "Area Width:");
			if(sceneModel.SingleArea==0){
				intTemp = GUI.TextField(new Rect(manager.position.width/2, y+70, halfWidth - padding, 20), sceneModel.AreaWidth+"");
				if(!System.Int32.TryParse(intTemp, out intReal)) {
					sceneModel.AreaWidth = 0;
				}else{
					sceneModel.AreaWidth = intReal;
				}
			}else{
				GUI.Label (new Rect(manager.position.width/2, y+70, halfWidth, 20), "N/A");
				sceneModel.AreaWidth = 0;
			}

			GUI.Label (new Rect(manager.position.width/2 - halfWidth + padding, y+100, halfWidth, 20), "Area Height:");
			if(sceneModel.SingleArea==0){
				intTemp = GUI.TextField(new Rect(manager.position.width/2, y+100, halfWidth - padding, 20), sceneModel.AreaHeight+"");
				if(!System.Int32.TryParse(intTemp, out intReal)) {
					sceneModel.AreaHeight = 0;
				}else{
					sceneModel.AreaHeight = intReal;
				}
			}else{
				GUI.Label (new Rect(manager.position.width/2, y+100, halfWidth, 20), "N/A");
				sceneModel.AreaHeight = 0;
			}

			GUI.Label (new Rect(manager.position.width/2 - halfWidth + padding, y+130, halfWidth, 20), "Scene Type:");
			if(sceneTypeControl == null){
				sceneTypeControl= new KDropDown(new GUIContent[] {new GUIContent("Playable"), new GUIContent("Cut-Scene")}, 
				new Rect(0,0,0,0));
			}
			sceneTypeControl.setPosition(new Rect(manager.position.width/2, y+130, 100, 20));
			sceneTypeControl.Show(sceneModel.SceneType);


			GUI.Box(new Rect(manager.position.width/2 - halfWidth, 188, formWidth, 25),"");

			if(GUI.Button(new Rect(manager.position.width/2 - 125, 190, 100, 20 ), "Cancel")){
				nextState = State.Init;
			}

			if(GUI.Button(new Rect(manager.position.width/2 + 25, 190, 100, 20 ), "Create Scene")){
				while(true) {
					errorMsg = null;
					if(sceneModel.Name==null || sceneModel.Name.Trim().Length==0) {
						errorMsg = "Name is required.";
						break;
					}
					for(int i=0; i<sceneNames.Length; i++) {
						if(sceneNames[i].Equals(sceneModel.Name)){
							Debug.Log (sceneNames[i] + " | " + sceneModel.Name);
							errorMsg = "Name is already in use.";
							break;
						}
					}

					if(errorMsg != null) {
						break;
					}

					if(sceneModel.SingleArea == 0) {
						if(sceneModel.AreaWidth == 0) {
							errorMsg = "Area width must be greater than zero.";
							break;
						}
						if(sceneModel.AreaHeight == 0) {
							errorMsg = "Area height must be greater than zero.";
							break;
						}
					}
					errorMsg = null;

					//Kat2DEditorUtiity.saveSceneModel(sceneModel);
					/// Go to none, so we can switch states. 
					//nextState = State.None;
					nextState = State.SaveForm;
					break;
				}

			}

			// Do Drop Down last to previent Z index issues.
			//sceneModel.SingleArea = singleAreaControl.Get(sceneModel.SingleArea);
			
			sceneModel.SceneType = sceneTypeControl.Get(sceneModel.SceneType);

			GUI.EndScrollView();
			break;
		case State.PickScene:
			float realHeight = listStyle.CalcHeight(new GUIContent("A"), 1.0f);

			formHeight = realHeight * (sceneNames.Length + 2);

			GUI.Label(new Rect(0, y, manager.position.width, 20), "Choose the Scene you wish to edit.", 
			          getHeaderStyle(manager.getWhiteTexture()));
			
			// Wrap in scrollview just in case...
			scrollPosition = GUI.BeginScrollView(new Rect(0, 40, manager.position.width, 
			                                              manager.position.height-20), scrollPosition, new Rect(0, 0, 500, 350));

			GUI.Box(new Rect(manager.position.width/2 - halfWidth, y, formWidth, formHeight + 29),"");

			GUI.Label(new Rect(manager.position.width/2 - halfWidth, y, formWidth, 20), "Available Scenes", 
			          getHeaderStyle(manager.getWhiteTexture()));

			float top = 0;
			for(int i=0; i<sceneNames.Length; i++) {
				//GUI.Label (new Rect(manager.position.width/2, i*realHeight, halfWidth, realHeight), sceneNames[i]);
				top = 40 + (i * realHeight);
				if(GUI.Button(new Rect(manager.position.width/2 - halfWidth, top, formWidth, realHeight),
				              sceneNames[i])) {

				}
			}
			top = realHeight * sceneNames.Length;
			GUI.Box(new Rect(manager.position.width/2 - halfWidth, formHeight + 18, formWidth, 25),"");
			if(GUI.Button(new Rect(manager.position.width/2 - 50, formHeight + 20, 100, 20 ), "Create Scene")){
				sceneModel = new SceneModel();
				nextState = State.CreateForm;
			}

			GUI.EndScrollView();
			break;
		}
	}


	public override void FormUpdate (KEditorManager manager) {
		if (nextState != State.None) {
			currentState = nextState;
			nextState = State.None;
		}
		switch (currentState) {
		case State.NoScenes:
			if(sceneModel==null) {
				sceneModel = new SceneModel();
			}
			break;
		case State.Init:
			// On Init, we need to load up the scene names.
			loadSceneNames();


			listStyle = new GUIStyle ();
			listStyle.normal.textColor  = Color.white; 
			listStyle.onHover.textColor = listStyle.hover.textColor = Color.black;
			
			listStyle.normal.background = listStyle.onHover.background 
				= listStyle.hover.background = manager.getWhiteTexture();
			listStyle.padding.left =listStyle.padding.right =listStyle.padding.top =listStyle.padding.bottom = 4;


			if(sceneNames.Length==0){
				currentState = State.NoScenes;
			}else{
				if(sceneModel==null) {
					sceneModel = new SceneModel();
				}
				//currentState = State.CreateForm;
				currentState = State.PickScene;
			}
			break;
		case State.SaveForm:
			Kat2DEditorUtiity.saveSceneModel(sceneModel);
			loadSceneNames();
			nextState = State.PickScene;
			break;
		case State.CreateForm:
			break;
		}
	}

	private void loadSceneNames() {
		sceneNames = Kat2DEditorUtiity.getSceneNames();
	}

 	public override void OnDestroy() {
		base.OnDestroy ();

		if (singleAreaControl != null) {
			singleAreaControl.OnDestroy();
		}
	}
}