using UnityEngine;
using System.Collections;

// Class for the ship object.
public class Ship : MonoBehaviour {

	#region PUBLIC VARIABLES
	// Whether to control the ship's movement using the accelerometer or touch.
	public bool useAccelerometer = false;

	// The rotation speed of the ship, in degrees per second.
	public float rotationSpeed = 180f;

	// The movement speed of the ship, in force applied per second.
	public float movementSpeed = 500f;

	// A reference to the transform where bullets will be spawned from.
	public Transform launcher;
	#endregion

	#region PRIVATE VARIABLES
	private const string TURN_COROUTINE_NAME = "TurnTowardsPointAndShootCoroutine";

	private GameManager gameManager;

	private bool turning = false;
	#endregion

	#region MONOBEHAVIOUR METHODS
	private void OnEnable()
	{
		if (!useAccelerometer)
		{
			UserInputHandler.OnTap += TurnTowardsTouch;
			UserInputHandler.OnPanBegan += StopTurn;
			UserInputHandler.OnPanHeld += MoveTowardsTouch;
		}
		else
		{
			UserInputHandler.OnAccelerometerChanged += MoveWithAcceleration;
			UserInputHandler.OnTap += TurnTowardsTouch;
		}
	}

	private void OnDisable()
	{
		if (!useAccelerometer)
		{
			UserInputHandler.OnTap -= TurnTowardsTouch;
			UserInputHandler.OnPanBegan -= StopTurn;
			UserInputHandler.OnPanHeld -= MoveTowardsTouch;
		}
		else
		{
			UserInputHandler.OnAccelerometerChanged -= MoveWithAcceleration;
			UserInputHandler.OnTap -= TurnTowardsTouch;
		}
	}

	void Start()
	{
		gameManager = GameManager.Instance;
	}
	#endregion
	
	#region PUBLIC METHODS
	public void OnHit()
	{
		gameManager.LoseLife();
		StartCoroutine(StartInvincibilityTimer(2.5f));
	}
	#endregion

	#region PRIVATE METHODS
	// Turn the ship towards a touch.
	private void TurnTowardsTouch(Touch t)
	{
		Vector3 targetPoint = Camera.main.ScreenToWorldPoint(t.position);
		
		StopCoroutine(TURN_COROUTINE_NAME);
		StartCoroutine(TURN_COROUTINE_NAME, targetPoint);
	}
	
	// Stop turning the ship towards a touch.
	private void StopTurn(Touch t)
	{
		StopCoroutine(TURN_COROUTINE_NAME);
		turning = false;
	}

	// Move the ship towards a touch.
	private void MoveTowardsTouch(Touch t)
	{
		Vector3 targetPoint = Camera.main.ScreenToWorldPoint(t.position);
		
		rigidbody2D.AddForce(transform.forward * movementSpeed * Time.deltaTime);
		TurnTowardsPointUpdate(targetPoint);
	}

	// Move the ship using the accelerometer data.
	private void MoveWithAcceleration(Vector3 acceleration)
	{
		if (!turning)
		{
			acceleration.z = 0;
			
			if (acceleration.sqrMagnitude >= 0.03f)
			{
				Vector3 targetPoint = transform.position + acceleration;

				rigidbody2D.AddForce(transform.forward * movementSpeed * Time.deltaTime);
				TurnTowardsPointUpdate(targetPoint);
			}
		}
	}

	// Shoot a bullet forward.
	private void Shoot()
	{
		Bullet bullet = PoolManager.Instance.Spawn(Constants.BULLET_PREFAB_NAME).GetComponent<Bullet>();
		bullet.SetPosition(launcher.position);
		bullet.SetTrajectory(bullet.transform.position + transform.forward);
	}

	// Updates the ships rotation to turn towards a point in 3D space. Can be called every frame.
	private void TurnTowardsPointUpdate(Vector3 point)
	{
		point = point - transform.position;
		point.z = transform.position.z;
		
		Quaternion startRotation = transform.rotation;
		Quaternion endRotation = Quaternion.LookRotation(point, Vector3.back);
		
		transform.rotation = Quaternion.RotateTowards(startRotation, endRotation, rotationSpeed * Time.deltaTime);
	}

	// Turns the ship towards a point in 3D space using a coroutine.
	private IEnumerator TurnTowardsPointAndShootCoroutine(Vector3 point)
	{
		turning = true;

		point = point - transform.position;
		point.z = transform.position.z;

		Quaternion startRotation = transform.rotation;
		Quaternion endRotation = Quaternion.LookRotation(point, Vector3.back);

		float time = Quaternion.Angle(startRotation, endRotation) / rotationSpeed;

		for (float t = 0; t < 1.0f; t += Time.deltaTime / time)
		{
			transform.rotation = Quaternion.Slerp (startRotation, endRotation, t);
			yield return 0;
		}

		transform.rotation = endRotation;
		Shoot ();

		turning = false;
	}

	// Make the ship invincible for a time.
	private IEnumerator StartInvincibilityTimer(float timeLimit)
	{
		collider2D.enabled = false;

		SpriteRenderer spriteRenderer = GetComponentInChildren<SpriteRenderer>();

		float timer = 0;
		float blinkSpeed = 0.25f;

		while (timer < timeLimit)
		{
			yield return new WaitForSeconds(blinkSpeed);

			spriteRenderer.enabled = !spriteRenderer.enabled;
			timer += blinkSpeed;
		}

		spriteRenderer.enabled = true;
		collider2D.enabled = true;
	}
	#endregion
}
