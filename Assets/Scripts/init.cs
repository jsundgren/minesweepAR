using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class init : MonoBehaviour {
	public static init I;
	public Tile tilePrefab;
	public int numberOfTiles = 10;
	public float distanceBetweenTiles = 0.01f;
	public int tilesPerRow = 4;
	public bool isTouched = false;
	static Tile[] tilesAll;

	// Use this for initialization
	void Start () {
		CreateTiles ();
	}

	void CreateTiles(){
		tilesAll = new Tile[numberOfTiles];
		float xOffset = 0.0f;
		float zOffset = 0.0f;
		for (int tilesCreated = 0; tilesCreated < numberOfTiles; tilesCreated++) {
			xOffset = xOffset + distanceBetweenTiles;
			if(tilesCreated % tilesPerRow == 0)
			{
				if (tilesCreated != 0) {
					zOffset += distanceBetweenTiles;
				}
				xOffset = 0;
			}
			Tile newTile = Instantiate (tilePrefab,new Vector3(transform.position.x + (xOffset-0.25f), 0.1f, transform.position.z + (zOffset-0.15f)), transform.rotation);
			tilesAll [tilesCreated] = newTile;
		}
	}
	// Update is called once per frame
	void Update () {
		if (isTouched) {
			Destroy (tilePrefab);
			isTouched = false;
		}
	}
}
