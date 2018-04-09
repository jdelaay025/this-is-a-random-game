using UnityEngine;
using System.Collections;

public class FirstPersonCamera : BaseCameraState 
{
	private const float Y_ANGLE_MIN = -75.0F;
	private const float Y_ANGLE_MAX = 50.0F;

	private float offset = 1.0f;

	private float currentX = 0.0f;
	private float currentY = 0.0f;
	private float sensitivityX = 3.5f;
	private float sensitivityY = 2.0f;

	public override Vector3 ProcessMotion (Vector3 input)
	{
		return transform.position + (transform.up * offset);
	}

	public override Quaternion ProcessRotation (Vector3 input)
	{
		currentX += input.x * sensitivityX;
		currentY += input.z * sensitivityY;

		currentY = ClampAngle (currentY, Y_ANGLE_MIN, Y_ANGLE_MAX);

		return Quaternion.Euler(currentY, currentX,0);
	}
}
