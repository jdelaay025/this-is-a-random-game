using UnityEngine;
using System.Collections;

public class MobaStyleCamera : BaseCameraState 
{
	public float minScrollDistance = 27.0f;
	public float maxScrollDistance = 57.0f;

	private const float Y_ANGLE_MIN = 45.0F;
	private const float Y_ANGLE_MAX = 45.0F;
	private const float X_ANGLE_MIN = 240.0F;
	private const float X_ANGLE_MAX = 240.0F;

	private Transform lookAt; 
	private Transform cameraContainer;

	private Vector3 offset = Vector3.up;
	private float scrollNum = 0.0f;
	private float scrollValue = 40.0f;
	private float distance = 40.0f;
	private float currentX = 0.0f;
	private float currentY = 0.0f;
	private float sensitivityX = 3.5f;
	private float sensitivityY = 2.0f;

	public override void Construct()
	{
		base.Construct ();

		lookAt = transform;
		cameraContainer = motor.CameraContainer;
	}

	public override Vector3 ProcessMotion (Vector3 input)
	{
		currentX += input.x * sensitivityX;
		currentY += input.z * sensitivityY;

		currentY = ClampAngle (currentY, Y_ANGLE_MIN, Y_ANGLE_MAX);
		currentX = ClampAngle (currentX, X_ANGLE_MIN, X_ANGLE_MAX);

		return CalculatePosition();
	}

	public override Quaternion ProcessRotation (Vector3 input)
	{
		cameraContainer.LookAt(lookAt.position + offset);

		return cameraContainer.rotation;
	}

	private Vector3 CalculatePosition()
	{
		Vector3 dir = new Vector3 (0, 0, -distance);
		Quaternion rotation = Quaternion.Euler (currentY, currentX, 0);

		return (lookAt.position + offset) + rotation * dir;
	}

	protected void Update()
	{
		scrollNum = Input.GetAxis ("Mouse ScrollWheel");
		scrollValue -= scrollNum * 350 * 10 * Time.deltaTime;
		scrollValue = Mathf.Clamp (scrollValue, minScrollDistance, maxScrollDistance);
//		Debug.Log (scrollValue);
		distance = scrollValue;
	}
}
