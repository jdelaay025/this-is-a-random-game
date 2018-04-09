using UnityEngine;
using System.Collections;

public class DeactivateThis : MonoBehaviour 
{
    public float minTime = 2f;
    public float maxTime = 10f;
	void OnEnable()
	{
		Invoke ("DeactivateThisItem", Random.Range(minTime, maxTime));
	}

	void DeactivateThisItem () 
	{
		this.gameObject.SetActive(false);
	}
}
