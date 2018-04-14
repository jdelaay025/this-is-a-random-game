using UnityEngine;
//using UnityEditor;

public class FreeCameraLook : Pivot 
{
    #region Global Variable Declaration

    [SerializeField] private float moveSpeed = 5f;
	[SerializeField] public float turnSpeed = 3.5f;
	[SerializeField] public float turnSmoothing = .1f;
	[SerializeField] private float tiltMax = 75f;
	[SerializeField] private float tiltMin = 45f;
	//[SerializeField] private bool lockCursor = false;

	private float lookAngle;
	private float tiltAngle;

	private const float LookDistance = 100f;
	public ParticleSystem burst;
	public ParticleSystem burst2;

	private float smoothX = 0;
	private float smoothY = 0;
	private float smoothXVelocity = 0;
	private float smoothYVelocity = 0;
	public GameObject player;
	public float setTurnSpeed;
	public bool controllerInverted = false;

    #endregion

    protected override void Awake () 
	{
		base.Awake ();

		Cursor.lockState = CursorLockMode.Confined;

		cam = GetComponentInChildren<Camera> ().transform;

		pivot = cam.parent;
		setTurnSpeed = turnSpeed;
	}
	protected override void Start()
	{
        base.Start();

		player = GameMasterObject.playerUse;
	}

	protected override void Update () 
	{
		base.Update ();

        //if (GameMasterObject.Instance != null)
        //{
        //    if (GameMasterObject.Instance.lockCursor == true)
        //    {
                HandleRotationMovement();
        //    }
        //}

        // controllerInverted = InvertedControlsScript.isInverted;

        /*if (lockCursor && Input.GetMouseButtonUp (0)) 
		{
			Cursor.lockState = CursorLockMode.Locked;
		}*/
    }

    void OnDisable()
	{
		//Cursor.lockState = CursorLockMode.None;
	}

	protected override void Follow(float deltaTime)
	{
		transform.position = Vector3.Lerp (transform.position, target.position, deltaTime * moveSpeed);
	}

	void HandleRotationMovement()
	{
		//if (HUDJoystick_Keyboard.joystickOrKeyboard) 
		//{
			float x = Input.GetAxis ("horRot");
			float y = Input.GetAxis ("verRot");

			if (turnSmoothing > 0) 
			{
				smoothX = Mathf.SmoothDamp (smoothX, x, ref smoothXVelocity, turnSmoothing);
				smoothY = Mathf.SmoothDamp (smoothY, y, ref smoothYVelocity, turnSmoothing);
			} 
			else 
			{
				smoothX = x;
				smoothY = y;
			}
			
			lookAngle += smoothX * turnSpeed;
			
			transform.rotation = Quaternion.Euler (0f, lookAngle, 0f);
			
			tiltAngle -= smoothY * turnSpeed;
			tiltAngle = Mathf.Clamp (tiltAngle, - tiltMin, tiltMax);

			if (controllerInverted == true) 
			{
				pivot.localRotation = Quaternion.Euler (-tiltAngle, 0, 0);
			} 
			else if(controllerInverted == false)
			{
				pivot.localRotation = Quaternion.Euler(tiltAngle, 0, 0);
			}

		//} 
		//else if (!HUDJoystick_Keyboard.joystickOrKeyboard)
		//{
		//	float x = Input.GetAxis("horRot2");
		//	float y = Input.GetAxis("verRot2");

		//	if (turnSmoothing > 0) 
		//	{
		//		smoothX = Mathf.SmoothDamp (smoothX, x, ref smoothXVelocity, turnSmoothing);
		//		smoothY = Mathf.SmoothDamp (smoothY, y, ref smoothYVelocity, turnSmoothing);
		//	} else 
		//	{
		//		smoothX = x;
		//		smoothY = y;
		//	}
			
		//	lookAngle += smoothX * turnSpeed;
			
		//	transform.rotation = Quaternion.Euler (0f, lookAngle, 0f);
			
		//	tiltAngle -= smoothY * turnSpeed;
		//	tiltAngle = Mathf.Clamp (tiltAngle, - tiltMin, tiltMax);
			
		//	if (controllerInverted == true) 
		//	{
		//		pivot.localRotation = Quaternion.Euler (-tiltAngle, 0, 0);
		//	} 
		//	else if(controllerInverted == false)
		//	{
		//		pivot.localRotation = Quaternion.Euler(tiltAngle, 0, 0);
		//	}

		//}
	}

}
