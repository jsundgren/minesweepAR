using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class touchInput : MonoBehaviour {
	Vector3 planePoint;
	public GameObject moveCube;
	public GameObject field;
	public Text cubeText;

	void Start() {
		cubeText = GameObject.Find ("Cube").GetComponent<Text> ();
		moveCube = Instantiate (moveCube, new Vector3 (-0.5f, 0f, 0f), transform.rotation);
		moveCube.GetComponent<Renderer> ().material.color = Color.red;
		field = Instantiate(field, new Vector3(0f,0.05f,0f), transform.rotation);
		field.GetComponent<Renderer> ().material.color = Color.grey;
	}

	void Update () {
		Plane target_plane = new Plane (transform.up, transform.position);
		foreach (Touch touch in Input.touches) {
			Ray ray = Camera.main.ScreenPointToRay (touch.position);
			float dist = 0.0f;
			target_plane.Raycast (ray, out dist);
			planePoint = ray.GetPoint (dist);
			RaycastHit hit;
			if(touch.phase == TouchPhase.Moved){
				if (Physics.Raycast (ray, out hit, 2)) {
					if (hit.collider != null && hit.collider.gameObject == moveCube) {
						moveCube.transform.position = new Vector3(planePoint.x, 0, planePoint.z);
					}
				}
			}
		}
		float tX = planePoint.x;
		float tZ = planePoint.z;
		cubeText.text = "tX: " + tX.ToString () + "\n" + "tZ: " + tZ.ToString ();
	}
}



			    // Just to write out the coords of the touch input on the target plane
				/*float vX = planePoint.x;
				float vZ = planePoint.z;
				posText.text = "vX: " + vX.ToString () + "\n" + "vZ: " + vZ.ToString () + "\n" + "Dist: " + dist.ToString ();*/

				/*// Creates and gameobject (cube) and makes it green, 
				// used to mark out the user touch position
				GameObject pos = GameObject.CreatePrimitive(PrimitiveType.Cube);
				posColored = new Material(Shader.Find("Diffuse"));
				posColored.color = posColor;
				pos.GetComponent<Renderer>().material = posColored;
				pos.transform.parent = transform;
				pos.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
				pos.transform.position = planePoint;*/