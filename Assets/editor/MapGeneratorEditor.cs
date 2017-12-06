using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof (MapGeneration))]
public class MapGeneratorEditor : Editor {

	public override void OnInspectorGUI(){

		MapGeneration mapGen = (MapGeneration)target;

		if (DrawDefaultInspector ()) {
			if (mapGen.autoUpdate) {
				mapGen.GenerateMap ();
			}
		}

		if(GUILayout.Button("Generate")){
			mapGen.GenerateMap();
		}
	}
}
