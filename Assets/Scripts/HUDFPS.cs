using UnityEngine;
using System.Collections;

public class HUDFPS : MonoBehaviour
{

	// Attach this to anything in the scene to collect/calculate the fps
	// subscribe to the OnFpsChange event to do something with it...
	//modified from the unitywiki 

	public float updateInterval = 0.5f;

	private float accum = 0;
	private int frames = 0;
	private float timeleft;

	//event
	public delegate void fpsChangedHandler (string timestring);
	public event fpsChangedHandler OnFpsChange;

	void Start ()
	{
		timeleft = updateInterval;
	}

	void Update ()
	{
		timeleft -= Time.deltaTime;
		accum += Time.timeScale / Time.deltaTime;
		++frames;

		// Interval ended - update GUI text and start new interval
		if (timeleft <= 0.0) {
			// display two fractional digits (f2 format)
			float fps = accum / frames;
			string format = System.String.Format ("{0:F2} FPS", fps);
			if (OnFpsChange != null) {
				OnFpsChange (format);
			}
			//guiText.text = format;

			timeleft = updateInterval;
			accum = 0.0f;
			frames = 0;
		}
	}
}