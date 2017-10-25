﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class init : MonoBehaviour {
	public static init I;
	public Tile tilePrefab;
	public int numberOfTiles = 10;
	public float distanceBetweenTiles = 0.01f;
	public int tilesPerRow = 4;
	public int numberOfMines = 5;
	static Tile[] tilesAll;
	static ArrayList tilesMined;
	static ArrayList tilesUnmined;

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
			Tile newTile = (Tile)Instantiate (tilePrefab,new Vector3(transform.position.x + (xOffset-0.25f), 0.1f, transform.position.z + (zOffset-0.15f)), transform.rotation);
			tilesAll [tilesCreated] = newTile;
			AssignMines ();
		}
	}

	void AssignMines(){
		tilesUnmined = new ArrayList(tilesAll);
		tilesMined = new ArrayList ();
		for(int minesAssigned = 0; minesAssigned < numberOfMines; minesAssigned++){
			Tile currentTile = (Tile)tilesUnmined [Random.Range (0, tilesUnmined.Count)];
			currentTile.GetComponent<Tile> ().isMined = true;
			//Add it to the tiles mined
			tilesMined.Add (currentTile);
			//Remove it from the unmined tiles
			tilesUnmined.Remove(currentTile);
		}
	}
}