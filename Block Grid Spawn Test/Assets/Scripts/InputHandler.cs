using UnityEngine;
using System.Collections;

public class InputHandler : MonoBehaviour 
{
    #region Global Variable Declaration

    [Header("Input Holders")]
    public float horizontal;
	public float vertical;
	public float fire;
	public float aim;
	public float scrollWheel;
	public float horRot;
	public float verRot;
	public float sprint;

    [Header("Camera Components")]
	//public FreeCameraLook camProperties;
	public Transform camPivot;
	public Transform camTrans;

	[Header("Camera FOV")]
	public float normalFov = 60f;
	public float aimingFov = 40f;
	public float sprintFov = 100f;
    
    [Header("Camera Collision")]   
    public float currentXOffset = 0.5f;
	public float cameraNormalZ = -2f;
    public float cameraAimingZ = 0.96f;
    public float cameraNormalY = 3f;
    public float cameraAimingY = 0f;

    public float shakeRecoil = 0.5f;
	public float shakeMovement = 0.3f;
	public float shakeMin = 0.1f;

    [Header("Shooting parts")]
    //public Transform bulletSpawnPoint;
    public LayerMask layerMask;

    float targetZ;
	float actualZ;
	float curZ;
    float targetFov;
    float curY;
    float curFov;
    float targetShake;
    float curShake;

    [HideInInspector]
	CrosshairManager crosshairManager;
	[HideInInspector]
	ShakeCamera shakeCam;
	[HideInInspector]
	StateManager states;

    [SerializeField]
    AimSound.GunSoundSource gunSoundSource;
    GunFireState fireState;

    public enum GunFireState
    {
        SingleFire = 0,
        BurstFire = 1,
        FullAuto = 2
    }

    public GameObject AssualtRifle;

    #endregion

    void Awake()
    {

    }
    void Start () 
	{
		crosshairManager = CrosshairManager.GetInstance ();
		//camProperties = FreeCameraLook.GetInstance ();
		//camPivot = camProperties.transform.GetChild(0);
		//camTrans = camPivot.parent.GetChild (0);
		//shakeCam = camPivot.GetComponentInChildren<ShakeCamera> ();
	
		states = GetComponent<StateManager> ();

		//layerMask = ~(1 << gameObject.layer);
		states.layerMask = layerMask;
        fireState = GunFireState.FullAuto;
    }
	void Update()
	{
		if (!GameMasterObject.isPlayerActive) 
		{
			return;
		}
	}
	void FixedUpdate () 
	{
		if (!GameMasterObject.isPlayerActive) 
		{
			return;
		}

		HandleInput ();
		UpdateStates ();
		HandleShake();

        // check for obstacles in front of cam
        // CameraCollision (layerMask);

        if (FreeCameraLook.Instance.state == FreeCameraLook.CameraState.ThirdPerson)
        {
		    Ray ray = new Ray(camTrans.position, camTrans.forward);
            states.lookPosition = ray.GetPoint(100);
            RaycastHit hit;

		    if (Physics.Raycast (ray.origin, ray.direction, out hit, 100, layerMask)) 
		    {
			    states.lookHitPosition = hit.point;
		    } 
		    else 
		    {
			    states.lookHitPosition = states.lookPosition;
		    }

            //distance controlled by if we are or are not aiming
            actualZ = targetZ;

            if (!states.sprint && !states.aiming)
            {
                curZ = Mathf.Lerp(curZ, actualZ, Time.deltaTime * 5);
                camTrans.localPosition = new Vector3 (currentXOffset, cameraNormalY, curZ);
            }
            else if (states.sprint && !states.aiming)
            {
                curZ = Mathf.Lerp(curZ, actualZ, Time.deltaTime * 5);
                camTrans.localPosition = new Vector3 (currentXOffset, 0f, curZ);
            }
            else if (states.aiming && !states.sprint)
            {
                curZ = Mathf.Lerp(curZ, actualZ, Time.deltaTime * 5);
                camTrans.localPosition = new Vector3 (currentXOffset, cameraAimingY, curZ);
            }
        }
        else
        {

        }
	}

	void HandleInput()
	{
		horizontal = Input.GetAxisRaw ("horizontal");
		vertical = Input.GetAxisRaw ("vertical");
		horRot = Input.GetAxis ("horRot");
		verRot = Input.GetAxis ("verRot");
		scrollWheel = Input.GetAxis ("Mouse ScrollWheel");
		fire = Input.GetAxis ("Fire");
		aim = Input.GetAxisRaw ("Aim");
		sprint = Input.GetAxisRaw ("Sprint");

        if (Input.GetButtonDown("Primary"))
        {
            FreeCameraLook.Instance.state = FreeCameraLook.CameraState.MultiTarget;
            AssualtRifle.SetActive(false);
        }
        if (Input.GetButtonDown("Secondary"))
        {
            FreeCameraLook.Instance.state = FreeCameraLook.CameraState.ThirdPerson;
            AssualtRifle.SetActive(true);
        }
        if (Input.GetButtonDown("Tertiary"))
        {

        }
    }
    void UpdateStates()
	{
		if (states.onGround) 
		{
			if (sprint > 0 && !states.reloading) 
			{
				states.sprint = true;
			} 
			else if(sprint <= 0)
			{
				states.sprint = false;
			}

			if(sprint <= 0)
			{
				if(aim > 0)
				{
					states.aiming = true;
				}
				else if(aim <= 0)
				{
					states.aiming = false;
				}
			}
			else if(sprint > 0 || states.reloading)
			{
				states.aiming = false;
			}
		} 
		else 
		{
			states.aiming = false;
			states.sprint = false;
		}

		states.canRun = !states.aiming;

		states.horizontal = horizontal;
		states.vertical = vertical;

        if (!states.aiming && !states.sprint && !states.reloading)
		{
            //update target z position of cam
            targetZ = cameraNormalZ; 
			targetFov = normalFov;

			if (fire > 0.5 && !states.reloading) 
			{
				states.shoot = true;
                if (gunSoundSource != null)
                {
                    gunSoundSource.Play();
                }
            }
            else 
			{
				states.shoot = false;
                if (gunSoundSource != null)
                {
                    gunSoundSource.Stop();
                }
            }
        }
		else if (states.aiming && !states.sprint && !states.reloading) 
		{
			targetZ = cameraAimingZ; //update target z position of cam
			targetFov = aimingFov;

			if (fire > 0.5 && !states.reloading) 
			{
				states.shoot = true;
                if (gunSoundSource != null)
                {
                    gunSoundSource.Play();
                }
            }
            else 
			{
				states.shoot = false;
                if (gunSoundSource != null)
                {
                    gunSoundSource.Stop();
                }
            }
        }
		else if(!states.aiming && states.sprint && !states.reloading)
		{			
			if(horizontal > 0 || vertical > 0)
			{
				targetFov = sprintFov;
				targetZ = cameraAimingZ;
			}
			else if(horizontal <= 0 || vertical <= 0)
			{
				targetFov = normalFov;
				targetZ = cameraNormalZ;
			}
		}
		else 
		{
			states.shoot = false;
            if (gunSoundSource != null)
            {
                gunSoundSource.Stop();
            }
            targetZ = cameraNormalZ;
			targetFov = normalFov;
		}
	}
	void HandleShake()
	{
		if(states.shoot && states.handleShooting.curBullets > 0)
		{
			targetShake = shakeRecoil;
			targetFov += 5;
		}
		else
		{
			if(states.vertical != 0)
			{
				targetShake = shakeMovement;
			}
			else 
			{
				if(states.horizontal != 0)
				{
					targetShake = shakeMovement;
				}
				else 
				{

					targetShake = shakeMin;
				}
			}
		}

		curShake = Mathf.Lerp(curShake, targetShake, Time.deltaTime * 10);
        
		curFov = Mathf.Lerp(curFov, targetFov, Time.deltaTime * 5);
		Camera.main.fieldOfView = curFov;
	}

    void CameraCollision(LayerMask layerMask)
	{
		Vector3 origin = camPivot.TransformPoint (Vector3.zero);
		Vector3 direction = camTrans.TransformPoint (Vector3.zero) - camPivot.TransformPoint(Vector3.zero);
		RaycastHit hit;

		// distance controlled by if we are or are not aiming
		// actualZ = targetZ;

		if(Physics.Raycast(origin, direction,out hit, Mathf.Abs(targetZ), layerMask))
		{
            //Debug.Log(hit.collider.gameObject.name);
			float dis = Vector3.Distance (camPivot.position, hit.point);
			actualZ = -dis;		//the opposite of that is where we want to place our camera
		}
	}

}
