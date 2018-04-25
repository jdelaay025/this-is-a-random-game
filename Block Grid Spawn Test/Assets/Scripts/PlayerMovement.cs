using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour 
{
	#region global variable Declaration

	[Header("Movement Speeds")]
	public float runSpeed = 3.7f;
	public float sprintSpeed = 7.5f;
	public float aimSpeed = 0.8f;
	public float speedMultiplier = 22f;
	public float rotateSpeed = 10f;
	public float turnSpeed = 10f;

	[Header("Sprint Duration")]
	public float currentCharge = 0f; 

	InputHandler ih;
	StateManager states;
	Rigidbody rb;

	Vector3 lookPosition;
	Vector3 storeDirection;

	float horizontal;
	float vertical;

	Vector3 lookDirection;

	PhysicMaterial zFriction;
	PhysicMaterial mFriction;
	Collider col;

	#endregion

	void Start ()
	{
		ih = GetComponent<InputHandler>();
		rb = GetComponent<Rigidbody> ();
		states = GetComponent<StateManager> ();
		col = GetComponent<Collider> ();

		zFriction = new PhysicMaterial ("zeroFriction");
		zFriction.dynamicFriction = 0;
		zFriction.staticFriction = 0;

		mFriction = new PhysicMaterial ("MaxFriction");
		mFriction.dynamicFriction = 1;
		mFriction.staticFriction = 1;

		GameMasterObject.heroes.Add (transform);
	}
	void Update ()
	{		
		bool onGroundUpdate = states.onGround;

		if (horizontal != 0 || vertical != 0 || !onGroundUpdate) 
		{
			col.material = zFriction;
		} 
		else 
		{
			col.material = mFriction;
		}

		if(states.sprint)
		{
			turnSpeed = 5f;
			rotateSpeed = 5f;
		}
		else if(!states.sprint)
		{
			turnSpeed = 10f;
			rotateSpeed = 10f;
		}
		// Debug.Log (speed());
	}
    void FixedUpdate ()
	{
		if (!GameMasterObject.isPlayerActive) 
		{
			return;
		}

		lookPosition = states.lookPosition;
		lookDirection = lookPosition - transform.position;

		horizontal = states.horizontal;
		vertical = states.vertical;

		bool onGround = states.onGround;

        Vector3 v = ih.camTrans.forward * vertical;
        Vector3 h = ih.camTrans.right * horizontal;

        if (FreeCameraLook.Instance.state == FreeCameraLook.CameraState.MultiTarget)
        {
            v = transform.forward * vertical;
            h = transform.right * horizontal;
        }
        else
        {
            v = ih.camTrans.forward * vertical;
            h = ih.camTrans.right * horizontal;
        }

		v.y = 0;
		h.y = 0;

		HandleMovement (h, v, onGround);
		HandleRotation (h, v, onGround);

		if (onGround) 
		{
			rb.drag = 4;
		} 
		else 
		{
			rb.drag = 0;
		}
	}
	void OnEnable()
	{
		GameMasterObject.playerUse = this.gameObject;
	}

    void HandleMovement(Vector3 h, Vector3 v, bool onGround)
	{
        if (onGround && states.attacking == false) 
		{
			rb.AddForce ((v + h).normalized * speed());
		}
	}
	void HandleRotation(Vector3 h, Vector3 v, bool onGround)
	{
        if (FreeCameraLook.Instance.state != FreeCameraLook.CameraState.FreeRun)
        {
            lookDirection.y = 0;

            Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
            transform.rotation = Quaternion.Slerp(rb.rotation, targetRotation, Time.deltaTime * rotateSpeed);
        }
        else
        {
            storeDirection = transform.position + h + v;

            Vector3 dir = storeDirection - transform.position;
            dir.y = 0;

            if (horizontal != 0f || vertical != 0)
            {
                float angl = Vector3.Angle(transform.forward, dir);

                if (angl != 0)
                {
                    float angle = Quaternion.Angle(transform.rotation, Quaternion.LookRotation(dir));
                    if (angle != 0)
                    {
                        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), turnSpeed * Time.deltaTime);
                    }
                    else if (angle == 0)
                    {
                        transform.rotation = Quaternion.LookRotation(dir);
                    }
                }
            }
            else if (horizontal == 0f || vertical == 0)
            {
                lookDirection.y = 0;
                Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
                transform.rotation = Quaternion.Slerp(rb.rotation, targetRotation, Time.deltaTime * rotateSpeed);
            }
        }
    }
	float speed()
	{
		float speed = 0f;

		if (states.aiming && !states.reloading && !states.sprint) 
		{
			speed = aimSpeed;
		} 
		else 
		{
			if(states.sprint)
			{
				speed = sprintSpeed;
			}
			else
			{
				speed = runSpeed;
			}
		}
		speed *= speedMultiplier;

		return speed;
	}

}
