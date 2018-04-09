using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormyBlockCharacter : MonoBehaviour 
{
    #region Global Variable Declaration

    Transform myTransform;
    [SerializeField] float turnSpeed = 10f;
    [SerializeField] float movementSpeed = 5f;
    [SerializeField] Transform target;
    [SerializeField] float closeEnough = 5f;
    #endregion

    void Awake () 
	{
        myTransform = transform;
	}

	void Start () 
	{
		
	}
	
	void Update () 
	{
		if(target != null)
        {
            RotateToTarget(target);

            if(Vector3.Distance(myTransform.position, target.position) > closeEnough)
            {
                MoveTowardsTarget(target);
            }
        }
	}

    void RotateToTarget(Transform _target)
    {
        #region Left Canon Rotations

        Vector3 leftCanonDir = _target.position - myTransform.position;

        Quaternion lookDirection = Quaternion.LookRotation(leftCanonDir);
        Quaternion turn = Quaternion.Slerp(myTransform.rotation, lookDirection, turnSpeed * Time.deltaTime);

        myTransform.rotation = turn;

        #endregion
    }

    void MoveTowardsTarget(Transform _target)
    {
        myTransform.Translate(myTransform.forward * movementSpeed * Time.deltaTime, Space.World);
    }

}
