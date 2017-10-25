using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class touchInput : MonoBehaviour {

	public Text number;
	public Text posText;
	private Material posColored;
	private string ray_hit = "NO HIT";
//	private float hold_time = 0.5f;
	private float count_time = 0.0f;
	void Start() {
		posText = GameObject.Find ("Position").GetComponent<Text> ();
	}

	// Update is called once per frame
	void Update () {
		// Attach this script to a trackable object
		// Create a plane that matches the target plane
		Plane targetPlane = new Plane(transform.up, transform.position);

		// When user touch the screen
		foreach (Touch touch in Input.touches) {
			if (touch.phase == TouchPhase.Stationary) {
				count_time += touch.deltaTime;
				//Creates ray and send to the target plane where the user touch the screen
				Ray ray = Camera.main.ScreenPointToRay(touch.position);
				float dist = 0.0f;
				targetPlane.Raycast(ray, out dist);
				Vector3 planePoint = ray.GetPoint(dist);
				RaycastHit hit;
				if (Physics.Raycast (ray, out hit, 2)) {
					if (hit.collider != null) {
							ray_hit = "HIT AT: " + hit.collider.gameObject.transform.position.ToString ();
							Destroy (hit.transform.gameObject);
							Instantiate(number,new Vector3(hit.collider.gameObject.transform.position.x, hit.collider.gameObject.transform.position.y, hit.collider.gameObject.transform.position.z));
					}
				}
				// Just to write out the coords of the touch input on the target plane
				float vX = planePoint.x;
				float vZ = planePoint.z;
				posText.text = "vX: " + vX.ToString() + "\n" + 
				"vZ: " + vZ.ToString () + "\n" + "Dist: " + dist.ToString() + "\n" + ray_hit;
			}
		}
	}
}

						/*else if (count_time >= hold_time) {
							GameObject pos = GameObject.CreatePrimitive (PrimitiveType.Cube);
							pos.transform.localScale = new Vector3 (0.01f, 0.01f, 0.01f);
							pos.transform.position = new Vector3 (hit.collider.gameObject.transform.position.x, 
								hit.collider.gameObject.transform.position.y + 0.15f, 
								hit.collider.gameObject.transform.position.z);
						}
						*/
				/*// Creates and gameobject (cube) and makes it green, 
				// used to mark out the user touch position
				GameObject pos = GameObject.CreatePrimitive(PrimitiveType.Cube);
				posColored = new Material(Shader.Find("Diffuse"));
				posColored.color = posColor;
				pos.GetComponent<Renderer>().material = posColored;
				pos.transform.parent = transform;
				pos.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
				pos.transform.position = planePoint;*/