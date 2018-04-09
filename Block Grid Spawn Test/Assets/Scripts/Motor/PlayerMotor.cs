using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PlayerMotor : BaseMotor 
{
    #region Global Variable Declaration

    public Transform wayPointObject;
	public float shootDistance = 25f;
	public static float shootRate = 0.5f;
	public static int whichWeapon = 1;
	public LayerMask layerMask;
	public Transform bulletSpawnPoint;
	public float damage = 30.0f;

	public CameraMotor camMotor;
	private Transform camTransform;
	public Transform targetedEnemy;
	private Ray shootRay;
	private RaycastHit shootHit;
	private bool walking;
	private bool enemyClicked;
	private float nextFire;
	private bool firing = false;
	// for screen point to ray
	public Camera cam;

	float acceleration = 100f;
	float deceleration = 100f;
	float soundLevel = 1f;
	public float closeEnough = 10f;
	public Text targetDistText;

	AudioSource sounds;
	public AudioClip whichClip;
	public List<AudioClip> audioClips;
	public List<Transform> bulletSpawnPoints;
	public AudioClip ARBlast;
	public AudioClip HGBlast;
	public AudioClip SGBlast;
	public AudioClip SwingSword;
	public GameObject placeParticle;
	public List<GameObject> placeParticles;

	public Transform headTransform;

    UnityEngine.AI.NavMeshAgent agent;
    Animator anim;

    #endregion

    protected override void Awake()
	{
		base.Awake();

        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        anim = GetComponentInChildren<Animator> ();
		sounds = GetComponent<AudioSource> ();
		audioClips = new List<AudioClip> ();
		placeParticles = new List<GameObject> ();
	}

	protected override void Start()
	{
		base.Start();

		state = GetComponent<WalkingState> ();

        if (state == null)
		{
			state = this.gameObject.AddComponent<WalkingState> ();	
		}

        state.Construct ();

		camMotor = gameObject.AddComponent<CameraMotor> ();
		camMotor.Init ();
		camTransform = camMotor.CameraContainer;

		audioClips.Add (ARBlast);
		audioClips.Add (HGBlast);
		audioClips.Add (SGBlast);
		audioClips.Add (SwingSword);
		for(int i = 0; i < 14; i++)
		{
			GameObject go = Instantiate (placeParticle, wayPointObject.position, wayPointObject.rotation) as GameObject;
			go.SetActive (false);
			placeParticles.Add (go);
		}

		GameMasterObject.heroes.Add (transform);
	}

	protected override void Update()
	{
		base.Update ();
//		Debug.Log (firing);
		damage = DannyAbilities.classDamage;
		if(!DannyAbilities.Ultimate)
		{
			agent.acceleration = 50.0f;
			agent.speed = 16.0f;
		}
		else if(DannyAbilities.Ultimate)
		{
			agent.acceleration = 100.0f;
			agent.speed = 40.0f;
		}
	
		if(targetedEnemy != null)
		{
			float dist = ((int)Vector3.Distance(targetedEnemy.position, myTransform.position));
			targetDistText.text = "Distance: " + dist;
			if(!DannyAbilities.Ultimate)
			{
				shootDistance = 25f;
			}
			else if(DannyAbilities.Ultimate)
			{
				shootDistance = 9f;
			}
			Vector3 dirirection = targetedEnemy.position + Vector3.up - bulletSpawnPoint.position;
			Quaternion lookDirection = Quaternion.LookRotation (dirirection);
			bulletSpawnPoint.rotation = (lookDirection);

			if(dist > 2000)
			{
				targetedEnemy = null;
			}
		}
		else if(targetedEnemy == null)
		{
			closeEnough = 0f;
		}
		anim.SetBool ("Walking", walking);
//		Debug.Log (walking);

		Ray ray = new Ray(myTransform.position, myTransform.forward);
		if(cam == null)
		{
			cam = camMotor.GetCameraObject ();
//			Debug.Log ("Got Camera Component");
		}
		else if(cam != null)
		{
			ray = cam.ScreenPointToRay (Input.mousePosition);
//			Debug.Log (cam);
		}

		RaycastHit hit;
		if(agent.hasPath)
		{
			agent.acceleration = (agent.remainingDistance <= closeEnough) ? deceleration : acceleration;
		}
		if(Input.GetButtonDown("Aim"))
		{
			
			if(Physics.Raycast(ray, out hit, 200f, layerMask))
			{
				if (hit.collider.CompareTag ("Enemy")) 
				{
					targetedEnemy = hit.transform;
					enemyClicked = true;
				} 
				else 
				{
					walking = true;
					enemyClicked = false;
					targetedEnemy = null;
					wayPointObject.position = hit.point + new Vector3(0f, 1f, 0f);
					for(int p = 0; p < placeParticles.Count; p++)
					{
						if(!placeParticles[p].activeInHierarchy)
						{
							GameObject particle;
							particle = placeParticles[p];
							particle.transform.position = wayPointObject.position;
							particle.transform.rotation = wayPointObject.rotation;
							particle.SetActive (true);
							break;
						}
					}

					FadeWayPoint.placed = true;
					agent.destination = hit.point;
					agent.Resume();
				}
			}
		}

		if (enemyClicked) 
		{
			MoveAndShoot ();
			closeEnough = shootDistance + 1f;
		} 
//		else 
//		{
//			closeEnough = 1f;
//		}

		if (agent.remainingDistance <= closeEnough) 
		{
			walking = false;
		} 
		else 
		{
			walking = true;
		}

		if(firing)
		{
			RaycastHit thisHit;
			Ray thisRay = new Ray (bulletSpawnPoint.position, bulletSpawnPoint.forward);
			if (Physics.Raycast(thisRay, out thisHit))
			{
				if(thisHit.collider.gameObject.tag == "Enemy")
				{
//					Debug.Log ("hit");
					IDamageable enemy = thisHit.collider.gameObject.GetComponent<IDamageable>();
					if(enemy != null)
					{
						enemy.TakeDamage (damage, thisHit.point);
					}
				}
			}
			firing = false;

		}

		switch(whichWeapon)
		{
		case  0:
			whichClip = ARBlast;
			soundLevel = 0.5f;
			for(int i = 0; i < bulletSpawnPoints.Count; i++)
			{
				if (bulletSpawnPoints [i] == bulletSpawnPoints [0]) 
				{
					bulletSpawnPoints [i].gameObject.SetActive (true);
//					bulletSpawnPoint = bulletSpawnPoints [i];
				} 
				else 
				{
					bulletSpawnPoints [i].gameObject.SetActive (false);
				}
			}
			break;
		case 1:
			whichClip = HGBlast;
			soundLevel = 0.4f;
			for(int i = 0; i < bulletSpawnPoints.Count; i++)
			{
				if (bulletSpawnPoints [i] == bulletSpawnPoints [1]) 
				{
					bulletSpawnPoints [i].gameObject.SetActive (true);
//					bulletSpawnPoint = bulletSpawnPoints [i];
				} 
				else 
				{
					bulletSpawnPoints [i].gameObject.SetActive (false);
				}
			}
			break;
		case 2:
			whichClip = SGBlast;
			soundLevel = 0.4f;
			for(int i = 0; i < bulletSpawnPoints.Count; i++)
			{
				if (bulletSpawnPoints [i] == bulletSpawnPoints [2]) 
				{
					bulletSpawnPoints [i].gameObject.SetActive (true);
//					bulletSpawnPoint = bulletSpawnPoints [i];
				} 
				else 
				{
					bulletSpawnPoints [i].gameObject.SetActive (false);
				}
			}

			break;
		case 3:
			whichClip = SwingSword;
			soundLevel = 0.5f;
			break;
		}
	}

	private void MoveAndShoot()
	{
//		Debug.Log (agent.remainingDistance);
		if(targetedEnemy == null)
			return;
		agent.destination = targetedEnemy.position;
		if (agent.remainingDistance > shootDistance) 
		{
			agent.Resume ();
			walking = true;
		} 
		else if(agent.remainingDistance <= shootDistance && !DannyAbilities.Ultimate)
		{
			transform.LookAt(targetedEnemy);

			Vector3 dirToShoot = targetedEnemy.position - myTransform.position;
			if(Time.time > nextFire)
			{
				firing = true;
				anim.SetTrigger ("Fire");
				nextFire = Time.time + shootRate;
				sounds.PlayOneShot (whichClip, soundLevel);
			}
			agent.velocity = Vector3.zero;
			agent.Stop ();
			walking = false;
		}
		else if(agent.remainingDistance <= shootDistance && DannyAbilities.Ultimate)
		{
			transform.LookAt(targetedEnemy);

			Vector3 dirToShoot = targetedEnemy.position - myTransform.position;
			if(Time.time > nextFire)
			{
				firing = true;
				anim.SetTrigger ("Attack");
				nextFire = Time.time + shootRate;
				sounds.PlayOneShot (whichClip, soundLevel);
			}
			agent.velocity = Vector3.zero;
			agent.Stop ();
			walking = false;
		}
	}

	protected override void UpdateMotor()
	{
		// take in user input
		MoveVector = InputeDirection();

		// rotate moveVector with camera forward
//		MoveVector = RotateWithView(MoveVector);

		// send the input to a filter
		MoveVector = state.ProcessMotion(MoveVector);
//		RotationQuaternion = state.ProcessRotation (MoveVector);

		// check if need to change current state
		state.Transition();
		// Move character
		Move();
//		Rotate ();
	}

	private Vector3 InputeDirection()
	{
		Vector3 dir = Vector3.zero;

//		dir.x = Input.GetAxis ("Horizontal");
//		dir.z = Input.GetAxis ("Vertical");

		if(dir.magnitude > 1)
		{
			dir.Normalize ();
		}

		return dir;
	}

	private Vector3 RotateWithView(Vector3 input)
	{
		Vector3 dir = camTransform.TransformDirection (input);
		dir.Set (dir.x, 0f, dir.z);

		return dir.normalized * input.magnitude;
	}
}
