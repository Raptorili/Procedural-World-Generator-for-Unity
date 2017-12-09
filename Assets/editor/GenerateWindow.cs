using UnityEngine;
using UnityEditor;

public class GenerateWindow : EditorWindow {

	//Variables to be passed
	enum DrawMode {NoiseMap, ColourMap, Mesh, FalloffMap};
	DrawMode drawMode;

	const int mapChunkSize = 241;
	[Range(0,6)]int levelOfDetail;
	[Range(0,200)]float noiseScale;

	[Range(0,20)]int octaves;
 	[Range(0,1)]float persistance;
 	float lacunarity;

 	int seed;
 	Vector2 offset;

 	bool useFalloff;

 	float meshHeightMultiplier;
 	AnimationCurve meshHeightCurve;

 	bool autoUpdate;

 	TerrainType[] regions;

	//Create Window
	[MenuItem("Window/WorldGen")]
	public static void ShowWindow ()
	{
		GetWindow<GenerateWindow>("WorldGen");
	}

	//Set GUI display
	void OnGUI ()
	{
		GUILayout.Label("Procedurally Generate World", EditorStyles.boldLabel);

		seed = EditorGUILayout.IntField("World Seed: ", seed);

		if (GUILayout.Button("Generate!"))
		{

		}
	}
	void Generate ()
		{
			Random rnd = new Random();
			noiseScale = Random.Range(0.0f, 200.0f);
			octaves = Random.Range(1, 20);
			persistance = Random.Range(0.0f, 6.0f);
			lacunarity = Random.Range(1, 20);
			offset.Set(0,0);
			float[,] noiseMap = Noise.GenerateNoiseMap (mapChunkSize, mapChunkSize, seed, noiseScale, octaves, persistance, lacunarity, offset);

			Color[] colourMap = new Color[mapChunkSize * mapChunkSize];
			for (int y = 0; y < mapChunkSize; y++) {
				for (int x = 0; x < mapChunkSize; x++) {
					float currentHeight = noiseMap [x, y];
					for (int i = 0; i < regions.Length; i++) {
						if (currentHeight <= regions [i].height) {
							colourMap [y * mapChunkSize + x] = regions [i].colour;
							break;
						}
					}
				}
			}

			MapDisplay display = FindObjectOfType<MapDisplay> ();
			display.DrawMesh (MeshGenerator.GenerateTerrainMesh (noiseMap, meshHeightMultiplier, meshHeightCurve, levelOfDetail), TextureGenerator.TextureFromColourMap (colourMap, mapChunkSize, mapChunkSize));

		}


}
