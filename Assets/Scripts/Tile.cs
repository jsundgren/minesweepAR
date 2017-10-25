using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {

	public bool isMined = false;
	public int tilesPerRow;
	public int ID;

	public Tile tileUpper;
	public Tile tileLower;
	public Tile tileLeft;
	public Tile tileRight;

	public Tile tileUpperRight;
	public Tile tileUpperLeft;
	public Tile tileLowerRight;
	public Tile tileLowerLeft;
	public List<Tile> adjacentTiles = new List<Tile> ();
	public int adjacentMines = 0;

	public TextMesh displayText;
	public string state = "idle";

	// Use this for initialization
	void Start () {
		
		if(inBounds(Grid.tilesAll, ID + tilesPerRow)) { 
			tileUpper = Grid.tilesAll[ID + tilesPerRow]; 
			adjacentTiles.Add (tileUpper);
		}
		if(inBounds(Grid.tilesAll, ID - tilesPerRow)) { 
			tileLower = Grid.tilesAll[ID - tilesPerRow]; 
			adjacentTiles.Add (tileLower);
		}
		if(inBounds(Grid.tilesAll, ID + 1) && (ID+1) % tilesPerRow != 0) { 
			tileRight = Grid.tilesAll[ID + 1]; 
			adjacentTiles.Add (tileRight);
		}
		if(inBounds(Grid.tilesAll, ID - 1) && ID % tilesPerRow != 0) { 
			tileLeft = Grid.tilesAll[ID - 1]; 
			adjacentTiles.Add (tileLeft);
		}
		if(inBounds(Grid.tilesAll, ID + tilesPerRow + 1) && (ID + tilesPerRow + 1) % tilesPerRow != 0) { 
			tileUpperRight = Grid.tilesAll[ID + tilesPerRow + 1]; 
			adjacentTiles.Add (tileUpperRight);
		}
		if(inBounds(Grid.tilesAll, ID + tilesPerRow - 1) &&     ID % tilesPerRow != 0) { 
			tileUpperLeft  = Grid.tilesAll[ID + tilesPerRow - 1]; 
			adjacentTiles.Add (tileUpperLeft);
		}
		if(inBounds(Grid.tilesAll, ID - tilesPerRow + 1) && (ID+1) % tilesPerRow != 0) { 
			tileLowerRight = Grid.tilesAll[ID - tilesPerRow + 1]; 
			adjacentTiles.Add (tileLowerRight);
		}
		if(inBounds(Grid.tilesAll, ID - tilesPerRow - 1) &&     ID % tilesPerRow != 0) { 
			tileLowerLeft  = Grid.tilesAll[ID - tilesPerRow - 1]; 
			adjacentTiles.Add (tileLowerLeft);
		}

		countMines ();
	}
	
	private bool inBounds(Tile[] inputArray, int targetID){
		if(targetID < 0 || targetID >= inputArray.Length){
			return false;
		} else {
			return true;
		}
	}

	void countMines(){
		foreach (Tile currentTile in adjacentTiles) {
			if (currentTile.isMined) {
				adjacentMines += 1;
			}
		}

		displayText.text = adjacentMines.ToString ();

		if (adjacentMines <= 0) {
			displayText.text = "";
		}
	}
}
