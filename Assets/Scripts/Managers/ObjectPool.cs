using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// A pool of objects that can be reused.
public class ObjectPool {

	#region PRIVATE VARIABLES
	private Queue<GameObject> pool;
	private GameObject prefab;

	private Transform parent;
	#endregion

	#region PUBLIC METHODS
	// Create a new object pool.
	public ObjectPool(GameObject _prefab, int initialCapacity)
	{
		pool = new Queue<GameObject>();
		prefab = _prefab;
		parent = new GameObject(prefab.name + " Pool").transform;

		for (int i = 0; i < initialCapacity; i++)
		{
			GameObject obj = GameObject.Instantiate(prefab) as GameObject;
			obj.transform.parent = parent;
			obj.SetActive(false);
			pool.Enqueue(obj);
		}
	}

	// Spawn an object from the pool.
	public GameObject Spawn()
	{
		GameObject obj;

		if (pool.Count > 0)
			obj = pool.Dequeue();
		else
		{
			obj = GameObject.Instantiate(prefab) as GameObject;
			obj.transform.parent = parent;
		}

		obj.SetActive(true);

		return obj;
	}

	// Recycle an object back into the pool.
	public void Recycle(GameObject obj)
	{
		obj.SetActive(false);
		pool.Enqueue(obj);
	}
	#endregion
}
