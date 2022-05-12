using UnityEngine;
using System.Collections;

// Singleton to hold references to prefabs.
public class PrefabManager : MonoBehaviour {

	#region PUBLIC VARIABLES
	// An array of large asteroid prefabs. Order doesn't matter.
	public GameObject[] largeAsteroidPrefabs;

	// An array of small asteroid prefabs. Order doesn't matter.
	public GameObject[] smallAsteroidPrefabs;
	#endregion

	#region SINGLETON PATTERN
	public static PrefabManager _instance;
	
	public static PrefabManager Instance
	{
		get {
			if (_instance == null)
			{
				_instance = GameObject.FindObjectOfType<PrefabManager>();
				
				if (_instance == null)
				{
					GameObject container = new GameObject("Prefab Manager");
					_instance = container.AddComponent<PrefabManager>();
				}
			}
			
			return _instance;
		}
	}
	#endregion

	#region PUBLIC METHODS
	// Return a large asteroid prefab.
	public GameObject GetLargeAsteroidPrefab()
	{
		if (largeAsteroidPrefabs.Length > 0)
			return largeAsteroidPrefabs[Random.Range (0, largeAsteroidPrefabs.Length)];

		return null;
	}

	// Return a small asteroid prefab.
	public GameObject GetSmallAsteroidPrefab()
	{
		if (smallAsteroidPrefabs.Length > 0)
			return smallAsteroidPrefabs[Random.Range (0, smallAsteroidPrefabs.Length)];

		return null;
	}
	#endregion
}
