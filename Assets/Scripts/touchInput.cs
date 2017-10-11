using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class touchInput : MonoBehaviour {


	public Text posText;
	private Color posColor = new Color(0f, 1.0f, 0f, 1.0f);
	private Material posColored;
	void Start() {
		posText = GameObject.Find ("Position").GetComponent<Text> ();
	}

	// Update is called once per frame
	void Update () {
		
		// Attach this script to a trackable object
		// Create a plane that matches the target plane
		Plane targetPlane = new Plane(transform.up, transform.position);

		// When user touch the screen
		foreach (Touch touch in Input.touches)
		{
			if (touch.phase == TouchPhase.Began)
			{
				//Creates ray and send to the target plane where the user touch the screen
				Ray ray = Camera.main.ScreenPointToRay(touch.position);
				float dist = 0.0f;
				targetPlane.Raycast(ray, out dist);
				Vector3 planePoint = ray.GetPoint(dist);

				RaycastHit hit;
				// IMPLENTERA HIT
				/*if (Physics.Raycast (ray, out hit, 2)) {
					if (hit.collider.gameObject.GetType == Tile && hit.collider != null) {
						Destroy (init.I.gameObject.GetInstanceID);
					}
				}*/
				/*// Creates and gameobject (cube) and makes it green, 
				// used to mark out the user touch position
				GameObject pos = GameObject.CreatePrimitive(PrimitiveType.Cube);
				posColored = new Material(Shader.Find("Diffuse"));
				posColored.color = posColor;
				pos.GetComponent<Renderer>().material = posColored;
				pos.transform.parent = transform;
				pos.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
				pos.transform.position = planePoint;*/

				// Just to write out the coords of the touch input on the target plane
				float vX = planePoint.x;
				float vZ = planePoint.z;
				posText.text = "vX: " + vX.ToString() + "\n" + 
					"vZ: " + vZ.ToString () + "\n" + "Dist: " + dist.ToString();
				//Destroy (pos, 5.0f);
			}
		}
	}
}
