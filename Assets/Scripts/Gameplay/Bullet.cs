using UnityEngine;
using System.Collections;

// Class for bullet objects.
public class Bullet : MonoBehaviour {

	#region PUBLIC VARIABLES
	// The bullet's speed in Unity units.
	public float speed = 7f;
	#endregion

	#region PRIVATE VARIABLES
	private Camera mainCamera;
	#endregion

	#region MONOBEHAVIOUR METHODS
	void Start()
	{
		mainCamera = Camera.main;
	}
	
	void Update()
	{
		Vector3 newPosition = transform.position + transform.forward * speed * Time.deltaTime;
		newPosition.z = transform.position.z;
		transform.position = newPosition;
		
		Vector3 viewportPosition = mainCamera.WorldToViewportPoint(transform.position);
		
		if (viewportPosition.x > 1 || viewportPosition.x < 0 || viewportPosition.y > 1 || viewportPosition.y < 0)
		{
			PoolManager.Instance.Recycle(Constants.BULLET_PREFAB_NAME, gameObject);
		}
	}
	#endregion

	#region PUBLIC METHODS
	// Set the position of the bullet.
	public void SetPosition(Vector3 position)
	{
		transform.position = position;
	}

	// Set the tragectory of the bullet.
	public void SetTrajectory(Vector3 target)
	{
		transform.LookAt(target, Vector3.back);
	}
	#endregion



}
