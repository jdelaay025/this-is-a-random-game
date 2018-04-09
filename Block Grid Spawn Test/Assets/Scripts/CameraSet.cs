using UnityEngine;
using System.Collections;

public class CameraSet : MonoBehaviour 
{
	public Camera cam;

	void Awake()
	{
		cam = GetComponent<Camera> ();	
	}

	void OnEnable () 
	{
		GameMasterObject.camInUse = cam;
	}
}
