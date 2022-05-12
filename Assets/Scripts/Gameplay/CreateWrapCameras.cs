using UnityEngine;
using System.Collections;

// Creates extra cameras for the screen wrap effect.
public class CreateWrapCameras : MonoBehaviour {

	#region MONOBEHAVIOUR METHODS
	void Awake () {
		Camera mainCamera = Camera.main;
		mainCamera.orthographic = true;
		mainCamera.cullingMask = ~(1 << Constants.UI_LAYER);

		Vector2 cameraWorldSize = mainCamera.ViewportToWorldPoint(Vector3.one) - mainCamera.ViewportToWorldPoint(Vector3.zero);
		float cameraWidth = cameraWorldSize.x;
		float cameraHeight = cameraWorldSize.y;

		GameManager.Instance.cameraHalfWidth = cameraWidth/2.0f;
		GameManager.Instance.cameraHalfHeight = cameraHeight/2.0f;

		Vector2[] wrapCameraPositions = new Vector2[]{new Vector2(1,0), new Vector2(-1,0), new Vector2(0,1), new Vector2(0,-1),
														new Vector2(1,1), new Vector2(1,-1), new Vector2(-1,1), new Vector2(-1,-1)};

		Transform wrapCamerasParent = new GameObject("Wrap Cameras").transform;

		for (int index = 0; index < wrapCameraPositions.Length; index++)
		{
			Camera wrapCamera = new GameObject("Camera").AddComponent<Camera>();
			Vector3 newPosition = mainCamera.transform.position;
			newPosition.x += cameraWorldSize.x * wrapCameraPositions[index].x;
			newPosition.y += cameraWorldSize.y * wrapCameraPositions[index].y;
			wrapCamera.transform.position = newPosition;
			wrapCamera.transform.parent = wrapCamerasParent;

			wrapCamera.orthographic = true;
			wrapCamera.orthographicSize = mainCamera.orthographicSize;
			wrapCamera.clearFlags = CameraClearFlags.Depth;
			wrapCamera.cullingMask = ~(1 << Constants.ASTEROID_LAYER | 1 << Constants.UI_LAYER);
		}
	}
	#endregion
}
