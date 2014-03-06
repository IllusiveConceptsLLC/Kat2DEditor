using System;
using System.Collections.Generic;
using System.Xml.Serialization;

[XmlRoot("Scene")]
public class SceneModel {

	public SceneModel(){
		Name = "";
		SingleArea = AreaHeight = AreaWidth = SceneType = 0;
	}

	// The name of the scene. Redundant, as the name of the XML file
	// will be the same.
	public string Name {get; set;}
	
	
	// This defines whether or not the scene is built of one single area
	// or if it is built from many areas stiched together.
	// 1 = true
	// 0 = false
	public int SingleArea  {get; set;}

	// If this is not a single area, define the width that the areas
	// must conform to. 
	public int AreaWidth   {get; set;}

	// If this is not a single area, define the height that the areas
	// must conform to. 
	public int AreaHeight   {get; set;}
	
	// This defines the type of scene this is. We can use different types 
	// of scenes that represent different things.
	// 0 = Player interactive ( Default ) 
	// 1 = Cut scene
	// 2 = ? 
	public int SceneType  {get; set;}
}