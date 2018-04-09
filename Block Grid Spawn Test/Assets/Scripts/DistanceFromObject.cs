using UnityEngine;
using System.Collections;

public class DistanceFromObject : MonoBehaviour 
{
    public Transform target;

    Vector3 originalPos;
    
    float dist;

	void Awake () 
	{
        originalPos = transform.position;
	}

    void Start()
    {

    }

    void Update () 
	{
        dist = Vector3.Distance(target.position, transform.position);
        Debug.Log(dist);
	}
}
