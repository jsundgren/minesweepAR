using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CountTime : MonoBehaviour {
	public TextMesh num_time;
	private float game_time = 0;
	public float set_time = 100;
	public Text finished_text;

	void Start(){
		game_time = set_time;
		finished_text = GameObject.Find ("State").GetComponent<Text> ();
	}

	void Update () {
		game_time -= Time.deltaTime;
		num_time.text = game_time.ToString ("f0");
		endTime ();
		if (game_time < 10) {
			num_time.color = Color.red;
		}
	}

	void endTime(){
		if (game_time < 0.0f) {
			finished_text.text = "TIME  IS  UP";
			finished_text.alignment = TextAnchor.MiddleCenter;
			StartCoroutine(Wait ());
			Restart ();
		}
	}

	void Restart(){
		SceneManager.LoadScene ("animation-scene");
	}

	public IEnumerator Wait(){
		yield return new WaitForSeconds(10);
	}
}
