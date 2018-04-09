using UnityEngine;
using System.Collections;

public class AddToCapPoints : MonoBehaviour 
{
	Transform myTransform;

	void Awake()
	{
		myTransform = transform;	
	}

	void Start () 
	{
		GameMasterObject.capPointPositions.Add (myTransform);
	}
}
