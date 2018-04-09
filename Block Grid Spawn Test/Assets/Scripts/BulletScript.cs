using UnityEngine;
using System.Collections;

public class BulletScript : MonoBehaviour 
{
	[Header("Attributes")]
	public float travelSpeed = 75f;
	public float damage = 10f;

	[Header("Components")]
	public GameObject burst;
	Transform myTransform;
	Transform target;

	void Awake()
	{
		myTransform = transform;	
	}

	void Update () 
	{		
		if (target == null) 
		{
			this.gameObject.SetActive(false);
			return;
		} 

		float distanceThisFrame = travelSpeed * Time.deltaTime;
		Vector3 direction = target.position - myTransform.position;

		if (direction.magnitude > distanceThisFrame) 
		{
			myTransform.Translate (direction * distanceThisFrame, Space.World);
		}
		else if(direction.magnitude <= distanceThisFrame)
		{
			
			GameObject impact;
			for(int i = 0; i < GameMasterObject.impacts.Count; i++)
			{
				if(!GameMasterObject.impacts[i].activeInHierarchy)
				{
					GameMasterObject.impacts [i].transform.position = myTransform.position;
					GameMasterObject.impacts [i].transform.rotation = myTransform.rotation;
					impact = GameMasterObject.impacts [i];
					impact.SetActive (true);
					break;
				}
			}
			EnemyHealth enemyHealth = target.GetComponent<EnemyHealth> ();
			if(enemyHealth != null)
			{
				enemyHealth.TakeDamage (damage );
				this.gameObject.SetActive (false);
				return;
			}
		}
	}

	public void SeekTarget(Transform _target)
	{
		target = _target;
	}
}
