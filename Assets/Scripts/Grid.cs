using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Grid : MonoBehaviour {
	public Tile tile_prefab;
	public int number_of_tiles = 0;
	public float distance_between_tiles = 0.0f;
	public int tiles_per_row = 0;
	public int number_of_mines = 5;

	public static Tile[] tiles_all;
	public static List<Tile> tiles_mined;
	public static List<Tile> tiles_unmined;

	// Use this for initialization
	void Start () {
		CreateTiles ();
	}

	void CreateTiles(){
		tiles_all = new Tile[number_of_tiles];
		float x_offset = 0.0f;
		float z_offset = 0.0f;
		for (int tiles_created = 0; tiles_created < number_of_tiles; tiles_created++) {
			x_offset = x_offset + distance_between_tiles;
			if(tiles_created % tiles_per_row == 0)
			{
				if (tiles_created != 0) {
					z_offset += distance_between_tiles;
				}
				x_offset = 0;
			}
			Tile new_tile = Instantiate (tile_prefab,new Vector3(transform.position.x + (x_offset-0.25f), 0.1f, transform.position.z + (z_offset-0.15f)), transform.rotation);
			new_tile.ID = tiles_created;
			new_tile.tiles_per_row = tiles_per_row;
			tiles_all [tiles_created] = new_tile;
		}
		AssignMines ();
	}

	void AssignMines(){
		tiles_unmined = new List<Tile>(tiles_all);
		tiles_mined = new List<Tile>();
		for(int mines_assigned = 0; mines_assigned < number_of_mines; mines_assigned++){
			Tile current_tile = (Tile)tiles_unmined [Random.Range (0, tiles_unmined.Count)];
			current_tile.GetComponent<Tile> ().is_mined = true;
			//Add it to the tiles mined
			tiles_mined.Add (current_tile);
			//Remove it from the unmined tiles
			tiles_unmined.Remove(current_tile);
		}
	}
}