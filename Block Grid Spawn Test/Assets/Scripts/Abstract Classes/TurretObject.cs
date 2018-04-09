using UnityEngine;
using System.Collections;

public abstract class TurretObject : MonoBehaviour 
{
	protected float fireRate = 2f;
	protected float rotationSpeed = 20f;
	protected float radius = 13f;
	protected int pooledBullets = 10;

	protected Transform target;
//	protected Transform partToRotate;
	protected Transform firePoint;

	protected AudioSource sounds;
	protected float fireCountDown = 0f;

	protected virtual void Start () 
	{
		sounds = GetComponent<AudioSource> ();
	}

//	protected virtual void Update () 
//	{
//		
//
//	}

	void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere (transform.position, radius);
	}

	protected abstract void UpdateTarget ();
	protected abstract void Shoot ();

}
