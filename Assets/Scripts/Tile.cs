using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Tile : MonoBehaviour {
	public bool isMined = false;
	public GameObject displayFlag;
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
		displayFlag.GetComponent<Renderer>().enabled = false;
		displayText.GetComponent<Renderer>().enabled = false;

		if(inBounds(Grid.tilesAll, ID + tilesPerRow)) { 
			tileUpper = Grid.tilesAll[ID + tilesPerRow];
		}
		if(inBounds(Grid.tilesAll, ID - tilesPerRow)) { 
			tileLower = Grid.tilesAll[ID - tilesPerRow];
		}
		if(inBounds(Grid.tilesAll, ID + 1) && (ID+1) % tilesPerRow != 0) { 
			tileRight = Grid.tilesAll[ID + 1];
		}
		if(inBounds(Grid.tilesAll, ID - 1) && ID % tilesPerRow != 0) { 
			tileLeft = Grid.tilesAll[ID - 1];
		}
		if(inBounds(Grid.tilesAll, ID + tilesPerRow + 1) && (ID + tilesPerRow + 1) % tilesPerRow != 0) { 
			tileUpperRight = Grid.tilesAll[ID + tilesPerRow + 1];
		}
		if(inBounds(Grid.tilesAll, ID + tilesPerRow - 1) &&     ID % tilesPerRow != 0) { 
			tileUpperLeft  = Grid.tilesAll[ID + tilesPerRow - 1];
		}
		if(inBounds(Grid.tilesAll, ID - tilesPerRow + 1) && (ID+1) % tilesPerRow != 0) { 
			tileLowerRight = Grid.tilesAll[ID - tilesPerRow + 1];
		}
		if(inBounds(Grid.tilesAll, ID - tilesPerRow - 1) &&     ID % tilesPerRow != 0) { 
			tileLowerLeft  = Grid.tilesAll[ID - tilesPerRow - 1]; 
		}

		if (tileUpper) {adjacentTiles.Add (tileUpper);}
		if (tileLower) {adjacentTiles.Add (tileLower);}
		if (tileLeft) {adjacentTiles.Add (tileLeft);}
		if (tileRight) {adjacentTiles.Add (tileRight);}

		if (tileUpperLeft) {adjacentTiles.Add (tileUpperLeft);}
		if (tileUpperRight) {adjacentTiles.Add (tileUpperRight);}
		if (tileLowerLeft) {adjacentTiles.Add (tileLowerLeft);}
		if (tileLowerRight) {adjacentTiles.Add (tileLowerRight);}

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

	public void setFlag(){
		if (state == "idle") {
			state = "flagged";
			displayFlag.GetComponent<Renderer>().enabled = true;
			displayFlag.GetComponent<Renderer> ().material.color = Color.red;
		}else if(state == "flagged"){
			state = "idle";
			displayFlag.GetComponent<Renderer>().enabled = false;
		}
	}

	public void UncoverTile(){
		if (!isMined) {
			state = "uncovered";
			displayText.GetComponent<Renderer>().enabled = true;
			GetComponent<Renderer> ().material.color = Color.green;
			if (adjacentMines == 0) {
				unCoverAdjacentTiles ();
			}
		} else{
			explode();
		}
	}

	void explode(){
		state = "detonated";
		GetComponent<Renderer> ().material.color = Color.red;
		foreach (Tile currentTile in Grid.tilesMined) {
			currentTile.explodeAll ();
		}
		StartCoroutine(wait ());
		restart ();
	}
		
	void explodeAll(){
		state = "detonated";
		GetComponent<Renderer> ().material.color = Color.red;
	}

	IEnumerator wait(){
		yield return new WaitForSeconds(5.0f);
	}

	void restart(){
		SceneManager.LoadScene ("animation-scene");
	}

	private void uncoverTileExternal(){
		state = "uncovered";
		displayText.GetComponent<Renderer> ().enabled = true;
		GetComponent<Renderer> ().material.color = Color.green;

	}

	private void unCoverAdjacentTiles(){
		foreach (Tile currentTile in adjacentTiles) {
			if (!currentTile.isMined && currentTile.state == "idle" && currentTile.adjacentMines == 0) {
				currentTile.UncoverTile ();
			} else if (!currentTile.isMined && currentTile.state == "idle" && currentTile.adjacentMines > 0) {
				currentTile.uncoverTileExternal ();
			}
		}
	}
}
