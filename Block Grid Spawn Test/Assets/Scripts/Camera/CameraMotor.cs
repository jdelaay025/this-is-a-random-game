using UnityEngine;
using System.Collections;

public class CameraMotor : MonoBehaviour 
{
	public Camera cam;
	private BaseCameraState state;

	public Transform CameraContainer{ get; set;}
	public Vector3 InputVector{ get; set;}

	Vector3 velocity;
	public float smoothDampTime = 0.5f;

	public void Init()
	{
		if (Camera.main == null) 
		{
			CameraContainer = new GameObject ("Camera Container").transform;
			cam = CameraContainer.gameObject.AddComponent<Camera> ();
			CameraContainer.gameObject.AddComponent<AudioListener> ();
			CameraContainer.tag = "MainCamera";
//			Debug.Log ("Init");
		} 
		else 
		{
			Camera.main.gameObject.name = "Camera Container";
			cam = Camera.main;
			CameraContainer = Camera.main.transform;
//			Debug.Log ("Init");
		}


		state = gameObject.AddComponent<MobaStyleCamera> () as BaseCameraState;
		state.Construct ();
	}

	public Camera GetCameraObject()
	{
		
		return cam;
	}

	private void Update()
	{
		Vector3 dir = Vector3.zero;

//		dir.x = Input.GetAxis ("Horrot");
//		dir.z = Input.GetAxis ("Verrot");

		if(dir.magnitude > 1)
		{
			dir.Normalize ();
		}

		InputVector = dir;
	}

	private void LateUpdate()
	{
//		CameraContainer.position = Vector3.SmoothDamp (CameraContainer.position, InputVector, ref velocity, smoothDampTime);
		CameraContainer.position = state.ProcessMotion (Vector3.SmoothDamp (CameraContainer.position, InputVector, ref velocity, smoothDampTime));
		CameraContainer.rotation = state.ProcessRotation (InputVector);
	}

	public void ChangeState(string stateName)
	{
		System.Type t = System.Type.GetType (stateName);

		state.Destruct ();
		state = gameObject.AddComponent (t) as BaseCameraState;
		state.Construct ();
	}

}
