using UnityEngine;
using System.Collections;

public class FirePoint : MonoBehaviour 
{
	Transform myTransform;

	void Awake()
	{
		myTransform = transform;
	}

	public Transform GetFirePoint()
	{
		return myTransform;
	}
}
