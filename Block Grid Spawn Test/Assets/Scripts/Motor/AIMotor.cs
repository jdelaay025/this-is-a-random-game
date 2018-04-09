using UnityEngine;
using System.Collections;

public class AIMotor : BaseMotor 
{
	private Vector3 destination = Vector3.zero;

	protected override void Start()
	{
		base.Start();

		state = GetComponent<AIWalkingState> ();
		if(state == null)
		{
			state = this.gameObject.AddComponent<AIWalkingState> ();	
		}
		state.Construct ();	
	}

	protected override void UpdateMotor()
	{
		// take in user input
		MoveVector = Direction();

		// send the input to a filter
		MoveVector = state.ProcessMotion(MoveVector);
		RotationQuaternion = state.ProcessRotation (MoveVector);

		// check if need to change current state
		state.Transition();

		// Move character
		Move();
		Rotate ();
	}

	public Vector3 Direction()
	{
		if(destination == Vector3.zero)
		{
			return destination;
		}

		Vector3 dir = destination - myTransform.position;
		dir.Set (dir.x, 0, dir.z);

		return dir.normalized;
	}

	public void SetDestination(Transform t)
	{
		destination = t.position;
	}

}
