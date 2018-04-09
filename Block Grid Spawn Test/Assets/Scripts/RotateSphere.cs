using UnityEngine;
using System.Collections;

public class RotateSphere : MonoBehaviour 
{
	public float rotationMultiplier = 0.5f;

	Transform myTransform;

	void Awake () 
	{
		myTransform = transform;
	}

	void Update () 
	{
		transform.rotation *= Quaternion.Euler (0f, 90f * Time.deltaTime * rotationMultiplier, 0f);
	}
}
