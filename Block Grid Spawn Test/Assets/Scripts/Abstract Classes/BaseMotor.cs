using UnityEngine;
using System.Collections;

public abstract class BaseMotor : MonoBehaviour 
{
	protected CharacterController controller;
	protected BaseState state;
	protected Transform myTransform;

	private float baseSpeed = 30.0f;
	private float baseGravity = 25.0f;
	private float baseJumpForce = 7.0f;
	private float terminalVelocity = 30.0f;
	private float groundRayDistance = 0.5f;
	private float groundRayInnerOffset = 0.1f;

	public float Speed { get { return baseSpeed; } set{ baseSpeed = value; }}
	public float Gravity { get {return baseGravity; }}
	public float JumpForce{get{ return baseJumpForce; }}
	public float TerminalVelocity{get{ return terminalVelocity;}}

	public float VerticalVelocity{ get; set;}
	public Vector3 MoveVector { get; set; }
	public Quaternion RotationQuaternion{ get; set;}

	protected abstract void UpdateMotor ();

	protected virtual void Awake()
	{
		myTransform = transform;
	}

	protected virtual void Start()
	{
		controller = GetComponent<CharacterController> ();
		if(controller == null)
		{
			controller = this.gameObject.AddComponent<CharacterController> ();	
		}
	}

	protected virtual void Update()
	{
		UpdateMotor ();
	}

	protected virtual void Move()
	{
		controller.Move (MoveVector * Time.deltaTime);
	}

	protected virtual void Rotate()
	{
		myTransform.rotation = RotationQuaternion;
	}

	public virtual bool Grounded()
	{
		RaycastHit hit;
		Vector3 ray;

		float yRay = (controller.bounds.center.y - controller.bounds.extents.y) + 0.3f,
		centerX = controller.bounds.center.x,
		centerZ = controller.bounds.center.z,
		extentX = controller.bounds.extents.x - groundRayInnerOffset,
		extentZ = controller.bounds.extents.z - groundRayInnerOffset;

		// middle raycast
		ray = new Vector3(centerX, yRay, centerZ);
		Debug.DrawRay (ray, Vector3.down, Color.green);
		if(Physics.Raycast (ray, Vector3.down, out hit, groundRayDistance))
		{
			return true;	
		}

		ray = new Vector3(centerX + extentX, yRay, centerZ + extentZ);
		Debug.DrawRay (ray, Vector3.down, Color.green);
		if(Physics.Raycast (ray, Vector3.down, out hit, groundRayDistance))
		{
			return true;	
		}
		ray = new Vector3(centerX - extentX, yRay, centerZ + extentZ);
		Debug.DrawRay (ray, Vector3.down, Color.green);
		if(Physics.Raycast (ray, Vector3.down, out hit, groundRayDistance))
		{
			return true;	
		}
		ray = new Vector3(centerX - extentX, yRay, centerZ - extentZ);
		Debug.DrawRay (ray, Vector3.down, Color.green);
		if(Physics.Raycast (ray, Vector3.down, out hit, groundRayDistance))
		{
			return true;	
		}
		ray = new Vector3(centerX + extentX, yRay, centerZ - extentZ);
		Debug.DrawRay (ray, Vector3.down, Color.green);
		if(Physics.Raycast (ray, Vector3.down, out hit, groundRayDistance))
		{
			return true;	
		}

		return false;
	}

	public void ChangeState(string stateName)
	{
		System.Type t = System.Type.GetType (stateName);

		state.Destruct ();
		state = gameObject.AddComponent (t) as BaseState;
		state.Construct ();
	}
}
