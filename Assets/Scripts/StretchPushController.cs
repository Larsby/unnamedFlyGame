using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StretchPushController : MonoBehaviour {

	private bool mouseUp = true;
	private Vector3 savePosition;
	private Vector3 tap;
	float mass;
	private float angularDrag;
	private float drag;
	public Collider coll;
	TouchState touchState;
	int fingerID = -1;
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
		TOUCH_RELEASE}

	;

	// Use this for initialization
	void Start () {
		Rigidbody body = GetComponent<Rigidbody>();
		mass = body.mass;
		coll = GetComponent<Collider>();
		touchState = TouchState.IDLE;
	}
	

	void OnMouseUp()
	{
#if UNITY_IOS
		// allow unity player to work in iphone mode without using a remote but real ios wont do doulbe calls.
		if (Input.touchCount > 0)
			return;
#endif
		Vector3 currentPosition = getPosition();



		OnUp(currentPosition);
	}

	void OnMouseDown()
	{
#if UNITY_IOS
		if (Input.touchCount > 0)
			return;
#endif
		Vector3 currentPosition = getPosition();

		OnDown(currentPosition);
	}

	void OnMouseDrag()
	{
#if UNITY_IOS
		if (Input.touchCount > 0)
			return;
#endif


		Vector3 currentPosition = getPosition();


		OnDrag(currentPosition);
	}
	void OnDown(Vector3 inputPosition)
	{
		Rigidbody body = GetComponent<Rigidbody>();
		body.mass = 0.0f;
		//firstDragMotion = true;

		mouseUp = false;
	


		tap = inputPosition;

		//body.rotation = 0.0f;
		body.velocity = new Vector3(0.0f, 0.0f, 0.0f);


		savePosition = tap;




		//}

	}
	void OnDrag(Vector3 inputPosition)
	{
		//	Debug.Log ("OnMouseDrag ");
		//if (GetComponent<PlacePusherLegs> ().legsDone == true) {
		Vector3 curPosition = new Vector3(inputPosition.x, inputPosition.y, inputPosition.z);




		float distance = Vector2.Distance(tap, curPosition) * 100;



		//	float ex2 = distance * 0.01f;
		float b = distance % 1000f;

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
			transform.position = curPosition;
		}
	

	}
	void Update()
	{
		//RunTouchStateMachine();
		if(Input.GetMouseButtonUp(0)) {
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);


			RaycastHit hit;
			if (coll.Raycast(ray, out hit, 100.0F))
			{
				touchState = TouchState.TOUCH_DRAG;
				OnUp(ray.origin);
			}
		}
		if (Input.GetMouseButtonDown(0) && touchState == TouchState.TOUCH_DOWN || touchState == TouchState.TOUCH_DRAG )
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);


			RaycastHit hit;
			if (coll.Raycast(ray, out hit, 100.0F))
			{
				touchState = TouchState.TOUCH_DRAG;
				OnDrag(ray.origin);
			}

		}
		if (Input.GetMouseButtonDown(0) && touchState == TouchState.IDLE)
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			

			RaycastHit hit;
			if (coll.Raycast(ray, out hit, 100.0F))
			{
				touchState = TouchState.TOUCH_DOWN;
				OnDown(ray.origin);
			}

		}
	}


	void OnUp(Vector3 touchPosition)
	{
		
		Rigidbody body = GetComponent<Rigidbody>();
		//Vector3 position = pusherPosition.GetComponent<Collider2D>().bounds.center;
		Vector3 position = touchPosition;
		float distance = Vector3.Distance(transform.position, position);

		float powerDivider = 600f;

		powerDivider = 600f;
			float forceFactor = distance / powerDivider;
		Vector3 velocity = new Vector3(((position.x * 1.0f) - transform.position.x) * 1,
		                               ((position.y * 1.0f) - transform.position.y) * 1,
		                               ((position.z * 1.0f) - transform.position.z) *1.0f);


		body.velocity = velocity;



		Vector3 forceVec = body.velocity.normalized * forceFactor;

		//	Debug.Log ("Force" + forceVec.x + " " + forceVec.y);
		body.AddForce(forceVec,ForceMode.Impulse);
		//Debug.Log ("vec" + forceVec.x + " " + forceVec.y);
		body.mass = mass;
		if (body.velocity.x == 0.0f && body.velocity.y == 0.0f)
		{
			//pusherPosition.GetComponent<Collider2D>().enabled = true;
		}
		//StartCoroutine(setupGameOverListener());
		mouseUp = true;
		//	GetComponent<PlacePusherLegs> ().radius = 0.4f;
		//GetComponent<PlacePusherLegs> ().Place ();
		touchState = TouchState.IDLE;

	}
	Vector3 getPosition()
	{

		Vector2 curScreenPoint = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
		Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint);
		return curPosition;
	}
}
