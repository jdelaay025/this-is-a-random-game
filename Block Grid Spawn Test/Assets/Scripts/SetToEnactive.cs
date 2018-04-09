using UnityEngine;
using System.Collections;

public class SetToEnactive : MonoBehaviour 
{
	public float timer = 2f;

	void OnEnable () 
	{		
		Invoke ("Deactive", timer);
	}

	void Deactive () 
	{
		this.gameObject.SetActive (false);
	}
}
