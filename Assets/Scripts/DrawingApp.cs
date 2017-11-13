using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawingApp : MonoBehaviour {

	private LineRenderer line; //Reference to Line Renderer component
	private bool isMousePressed; //stores whether mouse is pushed down or not
	public List<Vector3> pointsList; //stores the points which the user draws on
	private Vector3 mousePos; //holds mouse position

	/// <summary>
	/// Sets up the line renderer components (temporary colors)
	/// </summary>
	void Awake ()
	{
		line = gameObject.AddComponent<LineRenderer> ();
		line.positionCount = 0;
		line.startWidth = 0.1f;
		line.endWidth = 0.1f;
		line.startColor = Color.green;
		line.endColor = Color.green;
		line.useWorldSpace = true;
		isMousePressed = false;
		pointsList = new List<Vector3> ();
	}

	/// <summary>
	/// Draws the lines as the mouse moves
	/// </summary>
	void Update ()
	{
		// If mouse button down, remove old line and set the new line color to green
		if (Input.GetMouseButtonDown (0)) {
			isMousePressed = true;
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
}
