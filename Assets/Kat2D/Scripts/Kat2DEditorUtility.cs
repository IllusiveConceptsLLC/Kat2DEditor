using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

public class Kat2DEditorUtiity {
	//Application.dataPath = C:\Unity4.3\Kat2DEditor\Assets

	// Define path to stored EditorData
	public static string EditorResourcePath = "/Kat2D/Resources";

	//public static string GameResourcePath = "/Game/Resources";

	public static string GameSceneDataPath = "/Kat2D/Data/Scenes";

	public static string[] getSceneNames() {
		string[] sceneNames = Directory.GetDirectories(Application.dataPath+GameSceneDataPath);
		int ix = 0;
		while(ix < sceneNames.Length){
			string ff = Path.GetFileName(sceneNames[ix]);
			sceneNames[ix] = ff;
			ix++;
		}
		return sceneNames;
	}

	public static bool saveSceneModel(SceneModel sm) {
		if (sm.Name != null && sm.Name.Trim ().Length > 0) {
			saveObjectToXML (Application.dataPath + GameSceneDataPath + "/" + sm.Name + "/", sm.Name + ".xml", sm);
			return true;
		} else {
			Debug.Log("saveSceneModel failed:");
			return false;
		}
	}

	public static void saveObjectToXML(string path, string name, System.Object obj){
		//Debug.Log ("Saving");
		var serializer = new XmlSerializer(obj.GetType());
		
		// Scene data is one thing, but we need to do the rooms seperately...
		Directory.CreateDirectory(path);
		
		//Debug.Log ("Using path: " + path + name);
		var stream = new FileStream(path + name, FileMode.Create);
		serializer.Serialize(stream, obj);
		stream.Close();
	}
}

// The editor will only know about preferences and such.

// Scene data will be stored in the game directory.