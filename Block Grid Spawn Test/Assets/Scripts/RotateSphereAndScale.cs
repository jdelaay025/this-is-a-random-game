using UnityEngine;
using System.Collections;

public class RotateSphereAndScale : MonoBehaviour 
{
	Transform myTransform;

	public Vector3 initalScale;
	public Vector3 otherScale;
	public Vector3 sizedScale;

	public bool resize = false;

	public float length = 5f;

	public float scaleOffsetStart = 49f;
	public float scaleOffset = 35f;

	public float lerpNum = 0f;
	public float timerOffset = 1f;
	public float minLerpValue = 0f;
	public float maxLerpValue = 1f;

	bool ascend = false;
	bool descend = false;


	void Awake () 
	{
		myTransform = transform;

	}

	void Start () 
	{
//		InvokeRepeating ("SizeUpdate", 0, 0.5f);
	}



	void Update () 
	{
		initalScale = new Vector3 (scaleOffsetStart, scaleOffsetStart, scaleOffsetStart);
		otherScale = new Vector3 (scaleOffset, scaleOffset, scaleOffset);

		if(lerpNum <= minLerpValue)
		{
			ascend = true;
			descend = false;
		}
		else if(lerpNum > maxLerpValue)
		{
			ascend = false;
			descend = true;
		}
		if(ascend)
		{
			lerpNum += Time.deltaTime * timerOffset;
		}
		else if(descend)
		{
			lerpNum -= Time.deltaTime * timerOffset;
		}

//		Debug.Log (lerpNum);

		transform.rotation *= Quaternion.Euler (0f, 90f * Time.deltaTime * 3f, 0f);
		sizedScale = Vector3.Lerp (initalScale, otherScale, lerpNum/*Mathf.PingPong (Time.time, length)*/);

		myTransform.localScale = sizedScale;
	}
}
