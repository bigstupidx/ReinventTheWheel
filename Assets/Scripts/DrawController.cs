using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawController : MonoBehaviour {

	public GameObject LinePrefab; //empty object with a line renderer
	public List<LineRenderer> pathList; //List of past lines
	public LineRenderer line; // reference to line renderer
	public bool isMousePressed;
	private Vector3 mousePos; 
	private List<Vector3> pointsList; //list of points on current line

	// Use this for initialization
	void Start () {
		isMousePressed = false;
		pathList = new List<LineRenderer> ();
		pointsList = new List<Vector3> ();
	}
	
	/// <summary>
	/// tracks the mouse movements of the player
	/// </summary>
	void Update () {
		
		// If mouse button down, spawn a new line and start a path
		if (Input.GetMouseButtonDown (0)) {
			isMousePressed = true;
			SpawnNewLine ();
			line.positionCount = 0;
			pointsList.RemoveRange (0, pointsList.Count);
			line.startColor = Color.green;
			line.endColor = Color.green;
		}
		if (Input.GetMouseButtonUp (0)) {
			isMousePressed = false;
		}
		// Drawing line when mouse is moving(presses)
		if (isMousePressed) {
			mousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			mousePos.z = 0;
			if (!pointsList.Contains (mousePos)) {
				pointsList.Add (mousePos);
				line.positionCount = pointsList.Count;
				line.SetPosition (pointsList.Count - 1, (Vector3)pointsList [pointsList.Count - 1]);
			}
		}
	}

	/// <summary>
	/// spawns a new line whenever the user starts drawing again
	/// </summary>
	void SpawnNewLine() {
		GameObject go = Instantiate (this.LinePrefab, this.transform);
		this.line = go.GetComponent<LineRenderer> ();
		pathList.Add (this.line);
	}
}
