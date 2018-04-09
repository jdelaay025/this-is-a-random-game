using UnityEngine;
using System.Collections;

public class enemyAi : MonoBehaviour 
{

    #region Global Variable Declaration

    public float speed = 10f;
	public float damage = 2f;
	public float closeEnoughToWP = 0.3f;

	Transform myTransform;
	Transform target;
	int wayPointIndex = 0;

    #endregion

    void Awake () 
	{
		myTransform = transform;
	}
	void Start () 
	{
		target = WayPointManager.points [0];
		GameMasterObject.enemies.Add (myTransform);
	}
	void Update () 
	{
		Vector3 direction = target.position - myTransform.position;
		Quaternion lookRotation = Quaternion.LookRotation(direction);
		Quaternion turn = Quaternion.Slerp (myTransform.rotation, lookRotation, 0.25f);
		myTransform.rotation = turn;
		myTransform.Translate (direction.normalized * speed * Time.deltaTime, Space.World);

		if(Vector3.Distance(myTransform.position, target.position) <= closeEnoughToWP)
		{
			GetNextWayPoint ();
		}
	} 

	public void GetNextWayPoint()
	{
		if(wayPointIndex >= WayPointManager.points.Length - 1)
		{
			this.gameObject.SetActive (false);
			return;
		}
		wayPointIndex++;
		target = WayPointManager.points [wayPointIndex];
	}

}
