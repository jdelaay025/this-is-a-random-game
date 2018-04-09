using UnityEngine;
using System.Collections;

public class enemyLionAi : MonoBehaviour 
{
	public float speed = 10f;
	public float damage = 20f;
	public float closeEnoughToWP = 10f;
    public float timeTillAttack = 2.5f;

	Transform myTransform;
	Transform target;
	Transform capturePoint;

    Animator anim;
    float timer = 0f;

	public bool goToCapturePoint = false;
    public bool goToPlayer = false;

	void Awake () 
	{
		myTransform = transform;
        anim = GetComponent<Animator>();
	}

	void Start () 
	{
		GameMasterObject.dragonsMonsters.Add (myTransform);
		capturePoint = GameMasterObject.CapturePoint;
	}

	void Update () 
	{
		Vector3 direction = Vector3.zero;

		if(!goToCapturePoint)
		{
			if(target == null)
			{
                if (goToPlayer)
                {
                    GetTargetPosition();
                }
            }
			else if(target != null)
			{
				direction = target.position + new Vector3(0f, 5f, 0f) - myTransform.position;

				Quaternion lookRotation = Quaternion.LookRotation(direction);
				Quaternion turn = Quaternion.Slerp (myTransform.rotation, lookRotation, 0.25f);

                myTransform.rotation = turn;

                float dist = Vector3.Distance (target.position, myTransform.position);
                //Debug.Log(dist);
                if (dist > closeEnoughToWP)
                {
                    myTransform.Translate(direction.normalized * speed * Time.deltaTime, Space.World);
                }
                else if (dist <= closeEnoughToWP)
                {
                    if (timer > 0)
                    {
                        timer -= Time.deltaTime;
                    }
                    else if (timer <= 0)
                    {
                        anim.SetTrigger("Bite");
                        timer = timeTillAttack;
                    }
                }
			}
		}
		else if(goToCapturePoint)
		{
			if(capturePoint == null)
			{
				capturePoint = GameMasterObject.CapturePoint;
			}
			else if(capturePoint != null)
			{
				direction = capturePoint.position + new Vector3(0f, 1f, 0f) - myTransform.position;

				Quaternion lookRotation = Quaternion.LookRotation(direction);
				Quaternion turn = Quaternion.Slerp (myTransform.rotation, lookRotation, 0.25f);
				myTransform.rotation = turn;

				float dist = Vector3.Distance (capturePoint.position, myTransform.position);
				if(dist > closeEnoughToWP)
				{
					myTransform.Translate (direction.normalized * speed * Time.deltaTime, Space.World);
				}
			}
		}
	} 

	public void GetTargetPosition()
	{
		if(target != null)
		{
			return;
		}
		else if(target == null)
		{
			float closestEnemy = Mathf.Infinity;
			for (int i = 0; i < GameMasterObject.heroes.Count; i++) 
			{
				float dist = Vector3.Distance (GameMasterObject.heroes[i].position, myTransform.position);
				if(dist < closestEnemy)
				{
					closestEnemy = dist;
					target = GameMasterObject.heroes [i];
				}
			}
		}
	}

    public void Bite()
    {
        if (target != null)
        {
            IDamageable enemy = target.GetComponent<IDamageable>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage, Vector3.zero);
            }
        }
    }
}
