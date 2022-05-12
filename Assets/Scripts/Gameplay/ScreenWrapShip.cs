using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/* Wraps an object around the screen when it's pivot goes out of view. Used for the ship 
 * so there are no "dead zones" at the edges.
 */
public class ScreenWrapShip : MonoBehaviour {

	#region PRIVATE VARIABLES
	private Camera mainCamera;

	private Vector2 cameraBoundsBottomLeft;
	private Vector2 cameraBoundsTopRight;
	#endregion

	#region MONOBEHAVIOUR METHODS
	void Start () {
		mainCamera = Camera.main;

		cameraBoundsBottomLeft = mainCamera.ViewportToWorldPoint(Vector3.zero);
		cameraBoundsTopRight = mainCamera.ViewportToWorldPoint(Vector3.one);
	}
	
	void Update () {
		Vector3 viewportPosition = mainCamera.WorldToViewportPoint(transform.position);
		Vector3 newPosition = transform.position;

		if (viewportPosition.x > 1)
		{
			newPosition.x = cameraBoundsBottomLeft.x + (transform.position.x - cameraBoundsTopRight.x);
		}
		else if (viewportPosition.x < 0)
		{
			newPosition.x = cameraBoundsTopRight.x - (cameraBoundsBottomLeft.x - transform.position.x);
		}
		
		if (viewportPosition.y > 1)
		{
			newPosition.y = cameraBoundsBottomLeft.y + (transform.position.y - cameraBoundsTopRight.y);
		}
		else if (viewportPosition.y < 0)
		{
			newPosition.y = cameraBoundsTopRight.y - (cameraBoundsBottomLeft.y - transform.position.y);
		}
		
		transform.position = newPosition;
	}
	#endregion
}
