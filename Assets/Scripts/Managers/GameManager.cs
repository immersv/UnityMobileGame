using UnityEngine;
using System.Collections;

// Singleton to manage the game state.
public class GameManager : MonoBehaviour {

	#region PRIVATE VARIABLES
	private int maxNumLives = 3;
	private int lives;

	private int score;

	public float cameraHalfWidth;
	public float cameraHalfHeight;

	private Camera mainCamera;
	#endregion

	#region SINGLETON PATTERN
	public static GameManager _instance;
	
	public static GameManager Instance
	{
		get {
			if (_instance == null)
			{
				_instance = GameObject.FindObjectOfType<GameManager>();
				
				if (_instance == null)
				{
					GameObject container = new GameObject("Game Manager");
					_instance = container.AddComponent<GameManager>();
				}
			}
			
			return _instance;
		}
	}
	#endregion

	#region MONOBEHAVIOUR METHODS
	void Start()
	{
		lives = maxNumLives;
		mainCamera = Camera.main;

		StartCoroutine(SpawnAsteroids());
	}
	#endregion
	
	#region PUBLIC METHODS
	// Lose a life.
	public void LoseLife() {
		lives--;

		if (lives == 0)
			Restart();
	}

	// Gain points.
	public void GainPoints(int points)
	{
		score += points;
	}

	// Restart the game.
	public void Restart() {
		Application.LoadLevel(Application.loadedLevel);
	}
	#endregion

	#region PRIVATE METHODS
	// Spawn asteroids every few seconds.
	private IEnumerator SpawnAsteroids()
	{
		while (true)
		{
			SpawnAsteroid();
			
			yield return new WaitForSeconds(Random.Range (2f, 8f));
		}
	}

	// Spawn an asteroid off the screen.
	private void SpawnAsteroid()
	{
		Asteroid newAsteroid = PoolManager.Instance.Spawn(Constants.ASTEROID_PREFAB_NAME).GetComponent<Asteroid>();

		Vector2 direction = newAsteroid.GetForceApplied();

		SpriteRenderer spriteRenderer = newAsteroid.GetComponentInChildren<SpriteRenderer>();
		float halfWidth = spriteRenderer.bounds.size.x / 2.0f;
		float halfHeight = spriteRenderer.bounds.size.y / 2.0f;

		// Asteroid moving up and right
		if (direction.x >= 0 && direction.y >= 0)
		{
			// Enter from bottom of screen
			if (Random.Range (0,2) == 0)
				newAsteroid.transform.position = new Vector3(Random.Range (mainCamera.transform.position.x - cameraHalfWidth, mainCamera.transform.position.x), mainCamera.transform.position.y - cameraHalfHeight - halfHeight, newAsteroid.transform.position.z);
			// Enter from left of screen
			else
				newAsteroid.transform.position = new Vector3(mainCamera.transform.position.x - cameraHalfWidth - halfWidth, Random.Range (mainCamera.transform.position.y - cameraHalfHeight, mainCamera.transform.position.y), newAsteroid.transform.position.z);
		}
		// Asteroid moving down and right
		else if (direction.x >= 0 && direction.y < 0)
		{
			// Enter from top of screen
			if (Random.Range (0,2) == 0)
				newAsteroid.transform.position = new Vector3(Random.Range (mainCamera.transform.position.x - cameraHalfWidth, mainCamera.transform.position.x), mainCamera.transform.position.y + cameraHalfHeight + halfHeight, newAsteroid.transform.position.z);
			// Enter from left of screen
			else
				newAsteroid.transform.position = new Vector3(mainCamera.transform.position.x - cameraHalfWidth - halfWidth, Random.Range (mainCamera.transform.position.y, mainCamera.transform.position.y + cameraHalfHeight), newAsteroid.transform.position.z);
		}
		// Asteroid moving up and left
		else if (direction.x < 0 && direction.y >= 0)
		{
			// Enter from bottom of screen
			if (Random.Range (0,2) == 0)
				newAsteroid.transform.position = new Vector3(Random.Range (mainCamera.transform.position.x, mainCamera.transform.position.x + cameraHalfWidth), mainCamera.transform.position.y - cameraHalfHeight - halfHeight, newAsteroid.transform.position.z);
			// Enter from right of screen
			else
				newAsteroid.transform.position = new Vector3(mainCamera.transform.position.x + cameraHalfWidth + halfWidth, Random.Range (mainCamera.transform.position.y - cameraHalfHeight, mainCamera.transform.position.y), newAsteroid.transform.position.z);
		}
		//Asteroid moving down and left
		else
		{
			// Enter from top of screen
			if (Random.Range (0,2) == 0)
				newAsteroid.transform.position = new Vector3(Random.Range (mainCamera.transform.position.x, mainCamera.transform.position.x + cameraHalfWidth), mainCamera.transform.position.y + cameraHalfHeight + halfHeight, newAsteroid.transform.position.z);
			// Enter from right of screen
			else
				newAsteroid.transform.position = new Vector3(mainCamera.transform.position.x + cameraHalfWidth + halfWidth, Random.Range (mainCamera.transform.position.y, mainCamera.transform.position.y + cameraHalfHeight), newAsteroid.transform.position.z);
		}
	}
	#endregion
}
