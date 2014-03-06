using UnityEngine;

public class ComboBox
{
	private static bool forceToUnShow = false; 
	private static int useControlID = -1;
	private bool isClickedComboButton = false;
	//private int selectedItemIndex = 0;
	
	private Rect rect;
	private GUIContent buttonContent;
	private GUIContent[] listContent;
	private string buttonStyle;
	private string boxStyle;
	private GUIStyle listStyle;
	private Rect theSpot;
	private bool theSpotSet = false;
	public ComboBox( Rect rect, GUIContent buttonContent, GUIContent[] listContent, GUIStyle listStyle ){
		this.rect = rect;
		this.buttonContent = buttonContent;
		this.listContent = listContent;
		this.buttonStyle = "button";
		this.boxStyle = "box";
		this.listStyle = listStyle;
	}
	
	public ComboBox(Rect rect, GUIContent buttonContent, GUIContent[] listContent, string buttonStyle, string boxStyle, GUIStyle listStyle){
		this.rect = rect;
		this.buttonContent = buttonContent;
		this.listContent = listContent;
		this.buttonStyle = buttonStyle;
		this.boxStyle = boxStyle;
		this.listStyle = listStyle;
	}
	
	public int Show(int selectedIndex)
	{
		if( forceToUnShow )
		{
			forceToUnShow = false;
			isClickedComboButton = false;
		}
		
		bool done = false;
		int controlID = GUIUtility.GetControlID( FocusType.Passive );       
		
		switch( Event.current.GetTypeForControl(controlID) )
		{
		case EventType.mouseUp:
		{
			if( isClickedComboButton )
			{
				done = true;
			}
		}
			break;
		}       
		
		//if( GUILayout.Button( rect, buttonContent, buttonStyle ) )
		if( GUILayout.Button( listContent [selectedIndex], buttonStyle ) )
		{

			if( useControlID == -1 )
			{
				useControlID = controlID;
				isClickedComboButton = false;
			}
			
			if( useControlID != controlID )
			{
				forceToUnShow = true;
				useControlID = controlID;
			}
			isClickedComboButton = true;
		}


		if (!theSpotSet && Event.current.type == EventType.Repaint){
			theSpot = GUILayoutUtility.GetLastRect();
			theSpotSet = true;
		}

		if( isClickedComboButton )
		{
			//Rect listRect = new Rect( rect.x, rect.y + listStyle.CalcHeight(listContent[0], 1.0f),
			//                         rect.width, listStyle.CalcHeight(listContent[0], 1.0f) * listContent.Length );

			//Rect listRect = new Rect( 0, 0,
			//                         rect.width, listStyle.CalcHeight(listContent[0], 1.0f) * listContent.Length );
			int saved_depth = GUI.depth;
			
			
			
			GUI.depth = 1;

			Rect listRect = new Rect( theSpot.x, theSpot.y+ listStyle.CalcHeight(listContent[0], 1.0f),
			                         rect.width, listStyle.CalcHeight(listContent[0], 1.0f) * listContent.Length );

			GUI.Box( listRect, "", boxStyle );
			//GUILayout.Box(boxStyle );

			//int newSelectedItemIndex = GUI.SelectionGrid( listRect, selectedItemIndex, listContent, 1, listStyle );
			selectedIndex = GUI.SelectionGrid( listRect, selectedIndex, listContent, 1, listStyle );
			//selectedIndex = GUILayout.SelectionGrid(  selectedIndex, listContent, 1, listStyle );
			//if( newSelectedItemIndex != selectedItemIndex )
			//	selectedItemIndex = newSelectedItemIndex;
			GUI.depth = saved_depth;
		}
		
		if( done )
			isClickedComboButton = false;

		//this.buttonContent = ;

		return selectedIndex;
	}
	
	//public int SelectedItemIndex{
	//	get{
	//		return selectedItemIndex;
	//	}
	//	set{
	//		selectedItemIndex = value;
	//	}
	//}
}