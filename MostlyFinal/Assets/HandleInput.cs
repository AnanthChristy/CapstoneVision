using UnityEngine;
using System.Collections;

public class HandleInput : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
		if (GvrViewer.Instance.Tilted || GvrViewer.Instance.Triggered) {
			Application.Quit();
		}
	}
}