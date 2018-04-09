using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MiniGunScript : TurretObject 
{
	//main check
	public bool minigunIsFiring = false;
	//governs the actual individual firing of gun
	public bool isShooting = false;
	// governs the rotation of barrels and sound
	bool rotatingBarrels = false;
	public GameObject muzzleFlash;

	public Transform partToRotate;

	public Transform barrels;
	public LayerMask layerMask;

	public float pauseFiringTime = 2f;

	public AudioClip miniGunFire;
	bool playClip = true;

	public float damage = .5f;
	public GameObject impact;
	List<GameObject> impacts;

	Transform myTransform;

	void Awake()
	{
		myTransform = transform;
		impacts = new List<GameObject> ();
		muzzleFlash = GetComponentInChildren<ParticleSystem> ().gameObject;
	}
	protected override void Start () 
	{
		base.Start();
		InvokeRepeating ("UpdateTarget", 0f, 0.5f);
		fireRate = 2f;
		rotationSpeed = 15f;
		radius = 12f;
		firePoint = GetComponentInChildren<FirePoint> ().GetFirePoint ();
		sounds = GetComponent<AudioSource>();
		pauseFiringTime = 0;
	}

	void Update () 
	{
//		Debug.Log (playClip);
		if (target != null) 
		{
			rotatingBarrels = true;
			muzzleFlash.SetActive (true);
			sounds.volume = 1f;
			if (playClip)
			{
				StartCoroutine (RotateBarrelsSound ());
			}
		}
		else if (target == null) 
		{
			muzzleFlash.SetActive (false);
			sounds.volume = Mathf.Lerp (sounds.volume, 0f, 0.25f);
			return;
		} 

		
		if (rotatingBarrels) 
		{
			barrels.rotation *= Quaternion.Euler (0f, 0f, 90f);

		}

		if (fireRate > 0)
			fireRate -= Time.deltaTime;
		else if (fireRate <= 0) 
		{
			if (target != null) 
			{
				isShooting = true;
				fireRate = .07f;
			}
		}
		if (pauseFiringTime > 0)
			pauseFiringTime -= Time.deltaTime;


		
	}

	void FixedUpdate () 
	{
		if(target != null)
		{
			Vector3 direction = target.position - partToRotate.position;
			Quaternion lookRotation = Quaternion.LookRotation (direction);
			Vector3 rotation = Quaternion.Slerp(partToRotate.rotation, lookRotation, Time.deltaTime * rotationSpeed).eulerAngles;

			partToRotate.rotation = Quaternion.Euler(rotation);

            //rotation of the action fire point object

            Vector3 firePointDir = target.position - firePoint.position;
			Quaternion fpRotation = Quaternion.LookRotation (firePointDir);
			firePoint.rotation = fpRotation;

		}

		RaycastHit hit;
		Ray ray = new Ray (firePoint.position, firePoint.forward);
		if (isShooting) 
		{
			if (Physics.Raycast (ray, out hit/*, radius, layerMask*/)) 
			{
				Shoot ();
				IDamageable enemy = hit.collider.GetComponent<IDamageable> ();
				if(enemy != null)
				{
					enemy.TakeDamage (damage, hit.point);

					GameObject impact;
                    if (GameMasterObject.impacts != null)
                    {
                        for (int i = 0; i < GameMasterObject.impacts.Count; i++)
                        {
                            if (!GameMasterObject.impacts[i].activeInHierarchy)
                            {
                                GameMasterObject.impacts[i].transform.position = hit.point;
                                GameMasterObject.impacts[i].transform.rotation = Quaternion.identity;
                                impact = GameMasterObject.impacts[i];
                                impact.SetActive(true);
                                break;
                            }
                        }
                    }					
				}
			}
			isShooting = false;
		}
	}

	protected override void UpdateTarget ()
	{
		float closestEnemyFromMe = Mathf.Infinity;
		Transform nearestEnemy = null;

		for(int i = 0; i < GameMasterObject.enemies.Count; i++)
		{
			if(GameMasterObject.enemies [i] != null)
			{
				float distanceToEnemy = Vector3.Distance (GameMasterObject.enemies [i].position, myTransform.position);
				if(distanceToEnemy < closestEnemyFromMe)
				{
					closestEnemyFromMe = distanceToEnemy;
					nearestEnemy = GameMasterObject.enemies [i];
				}
			}

			if (nearestEnemy != null && closestEnemyFromMe <= radius) 
			{
				target = nearestEnemy; 
			} 
			else 
			{
				target = null;
			}
		}
	}

	protected override void Shoot ()
	{
		
	}

	public IEnumerator RotateBarrelsSound()
	{
		sounds.PlayOneShot(miniGunFire, 0.15f);
		playClip = false;
		yield return new WaitForSeconds (3);
		rotatingBarrels = false;
		playClip = true;
	}
}
