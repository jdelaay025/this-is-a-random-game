using UnityEngine;
using System.Collections;

public class AIFallingState : FallingState 
{
	public override void Transition () 
	{
		if(motor.Grounded())
		{
			motor.ChangeState ("AIWalkingState");
		}
	}
}
