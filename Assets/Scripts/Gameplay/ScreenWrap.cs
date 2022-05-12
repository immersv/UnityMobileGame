using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Wraps an object around the screen when it goes out of view.
public class ScreenWrap : MonoBehaviour {

	#region PRIVATE VARIABLES
	private Camera mainCamera;

	private Renderer[] renderers;

	private Plane[] cameraFrustumPlanes;
	private Vector2 cameraBoundsBottomLeft;
	private Vector2 cameraBoundsTopRight;

	private bool wrapped = true;
	#endregion

	#region MONOBEHAVIOUR METHODS
	void Start () {
		mainCamera = Camera.main;

		renderers = gameObject.GetComponentsInChildren<Renderer>();

		cameraFrustumPlanes = GeometryUtility.CalculateFrustumPlanes(mainCamera);
		cameraBoundsBottomLeft = mainCamera.ViewportToWorldPoint(Vector3.zero);
		cameraBoundsTopRight = mainCamera.ViewportToWorldPoint(Vector3.one);
	}
	
	void Update () {
		if (wrapped && transform.position.x >= cameraBoundsBottomLeft.x && transform.position.x <= cameraBoundsTopRight.x 
		    && transform.position.y >= cameraBoundsBottomLeft.y && transform.position.y <= cameraBoundsTopRight.y)
			wrapped = false;

		if (!IsVisible() && !wrapped)
		{
			Vector3 viewportPosition = mainCamera.WorldToViewportPoint(transform.position);
			Vector3 newPosition = transform.position;

			if (viewportPosition.x > 1)
			{
				newPosition.x = cameraBoundsBottomLeft.x + (cameraBoundsTopRight.x - transform.position.x);
				wrapped = true;
			}
			else if (viewportPosition.x < 0)
			{
				newPosition.x = cameraBoundsTopRight.x + (cameraBoundsBottomLeft.x - transform.position.x);
				wrapped = true;
			}
			
			if (viewportPosition.y > 1)
			{
				newPosition.y = cameraBoundsBottomLeft.y + (cameraBoundsTopRight.y - transform.position.y);
				wrapped = true;
			}
			else if (viewportPosition.y < 0)
			{
				newPosition.y = cameraBoundsTopRight.y + (cameraBoundsBottomLeft.y - transform.position.y);
				wrapped = true;
			}
			
			transform.position = newPosition;
		}
	}
	#endregion

	#region PRIVATE METHODS
	// Returns true if any of the object's renderers are visible to the main camera.
	private bool IsVisible() {
		for (int index = 0; index < renderers.Length; index++)
		{
			if (GeometryUtility.TestPlanesAABB(cameraFrustumPlanes, renderers[index].bounds))
				return true;
		}

		return false;
	}
	#endregion
}
