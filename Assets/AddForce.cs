using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class AddForce : NetworkBehaviour
{
	public Vector3 force;
	private Vector3 internalForce;

	Rigidbody body;
	bool mouseUp = false;
	Vector3 currenPosition;
	Vector3 savePosition;
	Vector3 tap;
	float mass;


	private float angularDrag;
	private float drag;
	public Collider coll;
	TouchState touchState;
	public GameObject ballPrefab;
	public Transform ballSpawn;
	private static int playerCount = 0; // this is for testing only should be an singleton
	private int players = 0;
	int fingerID = -1;
	bool inverse = true;
	public float forceBoost = 10.0f;
	public 	bool useNetwork = true;
	TouchController controller = null;
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
	void Start () {
		if (!body)
		{
			body = GetComponent<Rigidbody>();
			mass = body.mass;
		}
		if(isLocalPlayer) {
			transform.position = new Vector3(0.0f, 0.0f, 0.0f);
		}
	}





	void Update()
	{
		if (!isLocalPlayer)
		{
			if(controller && controller.run) {
				controller.run = false;
			}
			return;
		}
		if (internalForce != force)
		{
			internalForce = force;
			body.AddForce(force, ForceMode.Impulse);
		}
		if(controller  == null) {
			controller = GetComponent<TouchController>();
			if(!controller) {
				controller = gameObject.AddComponent<TouchController>();
			}
		}



	}





	Vector2 GetViweportPos() {
		Vector2 curScreenPoint = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
		Ray ray =  Camera.main.ScreenPointToRay(curScreenPoint);
		return new Vector2(ray.origin.x, ray.origin.z);
	}
	Vector2 getPosition()
	{

		Vector2 curScreenPoint = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
		Vector2 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint);
		return curPosition;
	}
	[Command]
	void CmdAddplayer()
	{
		playerCount = playerCount + 1; 
		if (playerCount > 1)
			{
			Vector3 place = new Vector3(-1.1f, 0.19f, -0.17f);

			GameObject ball = Instantiate(ballPrefab);
			ball.transform.position = place;

			ball.name = "HomeMadeBaby";
				ball.GetComponent<Rigidbody>().velocity = transform.forward * 6.0f;
				NetworkServer.Spawn(ball);
			//	Destroy(ball, 2.0f);
			}

	}
	public override void OnStartLocalPlayer()
	{
		GetComponent<Renderer>().material.color = Color.blue;
		CmdAddplayer();
	}
	 
}
