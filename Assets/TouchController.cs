using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchController : MonoBehaviour
{
	public bool run = true;
	public Vector3 force;
	private Vector3 internalForce;

	Rigidbody body;
	bool mouseUp = false;
	Vector3 currenPosition;
	Vector3 savePosition;
	Vector3 tap;
	float mass;
	Collider collider;
	bool inverse = true;
	public float forceBoost = 10.0f;
	public bool useNetwork = true;
	TouchState touchState;
	private enum KeyState
	{
		IDLE = 1,
		DRAG = 2,
		RELEASE
	}

	private enum TouchState
	{
		IDLE = 1,
		TOUCH_DOWN = 2,
		TOUCH_DRAG,
		TOUCH_RELEASE
	}

	;
	private void Awake()
	{
		//	playerCount = 0;
	}

	// Use this for initialization
	void Start()
	{
		if (!body)
		{
			body = GetComponent<Rigidbody>();
			mass = body.mass;
		}
		collider = GetComponent<Collider>();
		touchState = TouchState.IDLE;

	}
	void OnUp(Vector2 touchPosition)
	{
		Ray ray1 = Camera.main.ScreenPointToRay(Input.mousePosition);
		Ray ray2 = Camera.main.ScreenPointToRay(tap);
		Vector3 end = ray2.origin;
		Vector3 start = ray1.origin;
		Debug.Log("x" + (end.x - start.x) + " y" + (end.z - start.z));
		int multiplier = 1;
		if (inverse)
			multiplier = -1;
		body.AddForce(new Vector3(((end.x - start.x) * multiplier) * forceBoost, 0.0f, ((end.z - start.z) * multiplier) * forceBoost), ForceMode.Impulse);
	}
	void OnDown(Vector2 inputPosition)
	{
		tap = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0.0f);
		body.velocity = new Vector3(0.0f, 0.0f, 0.0f);
	}

	void OnDrag(Vector2 inputPosition)
	{



		//Vector2 pos = getPosition();

		Vector3 curPosition = new Vector3(inputPosition.x, inputPosition.y, 0.0f);



		//	float angleDegrees = GetAngleOfLineBetweenTwoPoints (curPosition, tap);
		//	int i = 0;
		float distance = Vector2.Distance(tap, curPosition) * 100;
		if (distance > 0.05)
		{

			//SetLegsActiveStatus (false);
		}

		//	distance = Vector2.Distance (tap, getPosition ());
		//	Debug.Log ("distance = " + distance);



		//	float ex2 = distance * 0.01f;
		float b = distance % 1000f;
		//Debug.Log ("BBB " + b);
		if (distance > 300.0f)
		{
			float ex = 300.0f / distance;

			curPosition.x = tap.x - ((tap.x - curPosition.x) * ex);
			curPosition.y = tap.y - ((tap.y - curPosition.y) * ex);
			//radius1 = 0.2f;
		}
		savePosition = curPosition;
		// make sure that the user can't drag the pusher outside of the screen
		Vector3 viewPos = Camera.main.WorldToViewportPoint(curPosition);

		if ((viewPos.x < 0.0f || viewPos.x > 0.995f) || (viewPos.y < 0.05f || viewPos.y > 0.995f))
		{
			distance = 0.0f;
		}
		if (distance > 0)
		{
			//transform.position = curPosition;
		}
		//}


		//RaycastHit2D hit = Physics2D.Raycast (new Vector2 (endDebug.x, endDebug.y), new Vector2 (startDebug.x, startDebug.y), 3f);
		//	Debug.Log ("Hit" + hit.collider);
	}

	void Update()
	{
		if (!run)
			return;
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		bool ourEvent = false;

		RaycastHit hit;

		if (collider.Raycast(ray, out hit, Mathf.Infinity))
		{
			if (hit.transform.gameObject == this.gameObject)
			{
				ourEvent = true;

			}
			//Debug.Log("Hit" + hit.transform.name);
		}

		if (Input.GetMouseButtonUp(0) && (touchState == TouchState.TOUCH_DRAG || touchState == TouchState.TOUCH_DOWN))
		{

			{
				touchState = TouchState.IDLE;
				OnUp(ray.origin);
			}
		}
		if (ourEvent)
		{
			//RunTouchStateMachine();

			if (Input.GetMouseButtonDown(0) && touchState == TouchState.TOUCH_DOWN || touchState == TouchState.TOUCH_DRAG)
			{

				touchState = TouchState.TOUCH_DRAG;
				OnDrag(ray.origin);
			}


			if (Input.GetMouseButtonDown(0) && touchState == TouchState.IDLE)
			{

				touchState = TouchState.TOUCH_DOWN;
				OnDown(ray.origin);


			}
		}
	}
}
