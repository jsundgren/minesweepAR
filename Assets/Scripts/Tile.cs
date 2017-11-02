using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Tile : MonoBehaviour {
	public bool is_mined = false;
	public GameObject display_flag;
	public int tiles_per_row;
	public int ID;

	public Tile tile_upper;
	public Tile tile_lower;
	public Tile tile_left;
	public Tile tile_right;

	public Tile tile_upper_right;
	public Tile tile_upper_left;
	public Tile tile_lower_right;
	public Tile tile_lower_left;
	public List<Tile> adjacent_tiles = new List<Tile> ();
	public int adjacent_mines = 0;

	public TextMesh display_text;
	public string state = "idle";

	// Use this for initialization
	void Start () {
		display_flag.GetComponent<Renderer>().enabled = false;
		display_text.GetComponent<Renderer>().enabled = false;

		if(InBounds(Grid.tiles_all, ID + tiles_per_row)) { 
			tile_upper = Grid.tiles_all[ID + tiles_per_row];
		}
		if(InBounds(Grid.tiles_all, ID - tiles_per_row)) { 
			tile_lower = Grid.tiles_all[ID - tiles_per_row];
		}
		if(InBounds(Grid.tiles_all, ID + 1) && (ID+1) % tiles_per_row != 0) { 
			tile_right = Grid.tiles_all[ID + 1];
		}
		if(InBounds(Grid.tiles_all, ID - 1) && ID % tiles_per_row != 0) { 
			tile_left = Grid.tiles_all[ID - 1];
		}
		if(InBounds(Grid.tiles_all, ID + tiles_per_row + 1) && (ID + tiles_per_row + 1) % tiles_per_row != 0) { 
			tile_upper_right = Grid.tiles_all[ID + tiles_per_row + 1];
		}
		if(InBounds(Grid.tiles_all, ID + tiles_per_row - 1) &&     ID % tiles_per_row != 0) { 
			tile_upper_left  = Grid.tiles_all[ID + tiles_per_row - 1];
		}
		if(InBounds(Grid.tiles_all, ID - tiles_per_row + 1) && (ID+1) % tiles_per_row != 0) { 
			tile_lower_right = Grid.tiles_all[ID - tiles_per_row + 1];
		}
		if(InBounds(Grid.tiles_all, ID - tiles_per_row - 1) &&     ID % tiles_per_row != 0) { 
			tile_lower_left  = Grid.tiles_all[ID - tiles_per_row - 1]; 
		}

		if(tile_upper){adjacent_tiles.Add (tile_upper);}
		if(tile_lower){adjacent_tiles.Add (tile_lower);}
		if(tile_left){adjacent_tiles.Add (tile_left);}
		if(tile_right){adjacent_tiles.Add (tile_right);}

		if(tile_upper_left){adjacent_tiles.Add (tile_upper_left);}
		if(tile_upper_right){adjacent_tiles.Add (tile_upper_right);}
		if(tile_lower_left){adjacent_tiles.Add (tile_lower_left);}
		if(tile_lower_right){adjacent_tiles.Add (tile_lower_right);}

		CountMines ();
	}
	
	private bool InBounds(Tile[] input_array, int target_ID){
		if(target_ID < 0 || target_ID >= input_array.Length){
			return false;
		} else {
			return true;
		}
	}

	void CountMines(){
		foreach (Tile current_tile in adjacent_tiles) {
			if (current_tile.is_mined) {
				adjacent_mines += 1;
			}
		}
		display_text.text = adjacent_mines.ToString ();
		if (adjacent_mines <= 0) {
			display_text.text = "";
		}
	}

	public void SetFlag(){
		if (state == "idle") {
			state = "flagged";
			display_flag.GetComponent<Renderer>().enabled = true;
			display_flag.GetComponent<Renderer> ().material.color = Color.red;
		}else if(state == "flagged"){
			state = "idle";
			display_flag.GetComponent<Renderer>().enabled = false;
		}
	}

	public void UncoverTile(){
		if (!is_mined) {
			state = "uncovered";
			display_text.GetComponent<Renderer>().enabled = true;
			GetComponent<Renderer> ().material.color = Color.green;
			if (adjacent_mines == 0) {
				UncoverAdjacentTiles ();
			}
		} else{
			Explode();
		}
	}

	private void UncoverTileExternal(){
		state = "uncovered";
		display_text.GetComponent<Renderer> ().enabled = true;
		GetComponent<Renderer> ().material.color = Color.green;
	}

	void Explode(){
		state = "detonated";
		GetComponent<Renderer> ().material.color = Color.red;
		foreach (Tile current_tile in Grid.tiles_mined) {
			current_tile.ExplodeAll ();
		}
		StartCoroutine(Wait ());
		Restart ();
	}
		
	void ExplodeAll(){
		state = "detonated";
		GetComponent<Renderer> ().material.color = Color.red;
	}

	IEnumerator Wait(){
		yield return new WaitForSeconds(5.0f);
	}

	void Restart(){
		SceneManager.LoadScene ("animation-scene");
	}

	private void UncoverAdjacentTiles(){
		foreach (Tile current_tile in adjacent_tiles) {
			if (!current_tile.is_mined && current_tile.state == "idle" && current_tile.adjacent_mines == 0) {
				current_tile.UncoverTile ();
			} else if (!current_tile.is_mined && current_tile.state == "idle" && current_tile.adjacent_mines > 0) {
				current_tile.UncoverTileExternal ();
			}
		}
	}
}
