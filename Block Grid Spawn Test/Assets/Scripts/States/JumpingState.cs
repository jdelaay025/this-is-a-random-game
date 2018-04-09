using UnityEngine;
using System.Collections;

public class JumpingState : BaseState 
{
	public override void Construct()
	{
		base.Construct();

		motor.VerticalVelocity = motor.JumpForce;
	}

	public override Vector3 ProcessMotion (Vector3 input)
	{
		ApplySpeed (ref input, motor.Speed);

		ApplyGravity (ref input, motor.Gravity);

		return input;
	}

	public override void Transition()
	{
		if(motor.VerticalVelocity < 0.0f)
		{
			motor.ChangeState ("FallingState");
		}
	}
}
