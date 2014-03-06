using UnityEngine;
using System.Collections;

public class KDropDown : IKControl {
	private static bool forceToUnShow = false; 
	private static int useControlID = -1;
	private bool isClickedComboButton = false;
	private GUIStyle listStyle;
	private bool done = false;

	private Rect listRect;
	private Rect position;

	GUIContent[] options; 

	Texture2D textureBlack = new Texture2D(2, 2);
	Texture2D textureWhite = new Texture2D(2, 2);

	public void setPosition(Rect pos) {
		this.position = pos;

		// Precalculate the bounding box for the drop down list. 
		listRect = new Rect(position.x, 
		                    position.y+ listStyle.CalcHeight(options[0], 1.0f),
		                    position.width, 
		                    listStyle.CalcHeight(options[0], 1.1f) * options.Length );
	}

	public KDropDown(GUIContent[] options, Rect position) {
		this.options = options;

		this.position = position;

		textureBlack.hideFlags = HideFlags.HideAndDontSave;
		textureBlack.SetPixel(0, 0, Color.black);
		textureBlack.SetPixel(1, 0, Color.black);
		textureBlack.SetPixel(0, 1, Color.black);
		textureBlack.SetPixel(1, 1, Color.black);
		textureBlack.Apply ();

		textureWhite.hideFlags = HideFlags.HideAndDontSave;
		textureWhite.SetPixel(0, 0, Color.white);
		textureWhite.SetPixel(1, 0, Color.white);
		textureWhite.SetPixel(0, 1, Color.white);
		textureWhite.SetPixel(1, 1, Color.white);
		textureWhite.Apply ();

		listStyle = new GUIStyle ();
		listStyle.normal.textColor  = Color.white; 
		listStyle.onHover.textColor = listStyle.hover.textColor = Color.black;

		listStyle.normal.background = textureBlack;
		listStyle.onHover.background = listStyle.hover.background = textureWhite;
		listStyle.padding.left =listStyle.padding.right =listStyle.padding.top =listStyle.padding.bottom = 4;

		// Precalculate the bounding box for the drop down list. 
		listRect = new Rect(position.x, 
		                    position.y+ listStyle.CalcHeight(options[0], 1.0f),
		                    position.width, 
		                    listStyle.CalcHeight(options[0], 1.1f) * options.Length );
	}
	
	public bool Show(int selIndex) {
		if( forceToUnShow ) {
			forceToUnShow = false;
			isClickedComboButton = false;
		}
		
		done = false;
		int controlID = GUIUtility.GetControlID( FocusType.Passive );       
		
		switch( Event.current.GetTypeForControl(controlID) ) {
		case EventType.mouseUp:
			{
				if( isClickedComboButton ) {
					done = true;
				}
			}
			break;
		}       
		
		if( GUI.Button(position, options [selIndex] ) ) {
			if( useControlID == -1 ) {
				useControlID = controlID;
				isClickedComboButton = false;
			}
			
			if( useControlID != controlID ) {
				forceToUnShow = true;
				useControlID = controlID;
			}
			isClickedComboButton = true;
		}

		return isClickedComboButton;
	}

	public int Get(int selIndex) {
		if(isClickedComboButton) {
			GUI.Box(listRect, "", "Box" );
			selIndex = GUI.SelectionGrid(listRect, selIndex, options, 1, listStyle);
		}
		if (done) {
			isClickedComboButton = false;
		}
		return selIndex;
	}

	public void OnDestroy() {
		Resources.UnloadAsset (textureBlack);
		Resources.UnloadAsset (textureWhite);
	}
}
