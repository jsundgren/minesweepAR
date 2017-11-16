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
	public List<Tile> nearby_tiles = new List<Tile> ();
	public int nearby_mines = 0;

	public TextMesh display_text;
	public string state = "idle";

	/*Color one = new Color(0,1.0f,242,1.0f);
	Color two = new Color(5,120,4,1.0f);
	Color three = new Color(233,9,14,1.0f);
	Color four = new Color(0,2,124,1.0f);
	Color five = new Color(123,0,5,1.0f);
	Color six = new Color(11,144,106,1.0f);
	Color seven = Color.black;
	Color eight = new Color(128,128,128,1.0f);*/

	// Use this for initialization
	void Start () {
		display_flag.GetComponent<Renderer>().enabled = false;
		display_text.GetComponent<Renderer>().enabled = false;
		CheckTiles ();
		CountMines ();
	}

	public void CheckTiles() {
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
		if(InBounds(Grid.tiles_all, ID + tiles_per_row - 1) && ID % tiles_per_row != 0) { 
			tile_upper_left  = Grid.tiles_all[ID + tiles_per_row - 1];
		}
		if(InBounds(Grid.tiles_all, ID - tiles_per_row + 1) && (ID+1) % tiles_per_row != 0) { 
			tile_lower_right = Grid.tiles_all[ID - tiles_per_row + 1];
		}
		if(InBounds(Grid.tiles_all, ID - tiles_per_row - 1) && ID % tiles_per_row != 0) { 
			tile_lower_left  = Grid.tiles_all[ID - tiles_per_row - 1]; 
		}

		if(tile_upper){nearby_tiles.Add (tile_upper);}
		if(tile_lower){nearby_tiles.Add (tile_lower);}
		if(tile_left){nearby_tiles.Add (tile_left);}
		if(tile_right){nearby_tiles.Add (tile_right);}

		if(tile_upper_left){nearby_tiles.Add (tile_upper_left);}
		if(tile_upper_right){nearby_tiles.Add (tile_upper_right);}
		if(tile_lower_left){nearby_tiles.Add (tile_lower_left);}
		if(tile_lower_right){nearby_tiles.Add (tile_lower_right);}
	}
	
	private bool InBounds(Tile[] input_array, int target_ID){
		if(target_ID < 0 || target_ID >= input_array.Length){
			return false;
		} else {
			return true;
		}
	}

	void CountMines(){
		foreach (Tile current_tile in nearby_tiles) {
			if (current_tile.is_mined) {
				nearby_mines += 1;
			}
		}
		display_text.text = nearby_mines.ToString ();
		if (nearby_mines <= 0) {
			display_text.text = "";
		}
	}

	public void SetFlag(){
		if (state == "idle") {
			state = "flagged";
			display_flag.GetComponent<Renderer>().enabled = true;
			display_flag.GetComponent<Renderer> ().material.color = Color.red;
			Grid.mines_remaining -= 1;
			if (is_mined) {
				Grid.mines_marked_correct += 1;
			}
		}else if(state == "flagged"){
			state = "idle";
			display_flag.GetComponent<Renderer>().enabled = false;
			Grid.mines_remaining += 1;
			if (is_mined) {
				Grid.mines_marked_correct -= 1;
			}
		}
	}

	public void UncoverTile(){
		if (!is_mined) {
			state = "uncovered";
			display_text.GetComponent<Renderer>().enabled = true;
			GetComponent<Renderer> ().material.color = Color.grey;
			Grid.tiles_uncovered += 1;
			if (nearby_mines == 0) {
				UncoverNearbyTiles ();
			}
		} else{
			Explode();
		}
	}

	private void UncoverTileExternal(){
		state = "uncovered";
		display_text.GetComponent<Renderer> ().enabled = true;
		GetComponent<Renderer> ().material.color = Color.grey;
		Grid.tiles_uncovered += 1;
	}

	public void Explode(){
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

	public IEnumerator Wait(){
		yield return new WaitForSeconds(5.0f);
	}

	public void Restart(){
		SceneManager.LoadScene ("animation-scene");
	}


	private void UncoverNearbyTiles(){
		foreach (Tile current_tile in nearby_tiles) {
			if (!current_tile.is_mined && current_tile.state == "idle" && current_tile.nearby_mines == 0) {
				current_tile.UncoverTile ();
			} else if (!current_tile.is_mined && current_tile.state == "idle" && current_tile.nearby_mines > 0) {
				current_tile.UncoverTileExternal ();
			}
		}
	}
}
