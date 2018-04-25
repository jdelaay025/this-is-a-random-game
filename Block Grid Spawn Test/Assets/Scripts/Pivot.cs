using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class Pivot : FollowTarget 
{
    #region Global Variable Declaration

    protected Transform cam;
    protected Transform camHolder;
    protected Transform pivot;
	protected Vector3 lastTargetPosition;

    #endregion

    protected virtual void Awake()
	{
		cam = GetComponentInChildren<Camera> ().transform;
		pivot = cam.parent.Find("Pivot");
	}

	protected override void Start () 
	{
		base.Start ();	
	}

    protected virtual void Update () 
	{
		if (Application.isPlaying) 
		{
			if(target != null)
			{
				Follow (999);
				lastTargetPosition = target.position;
			}

			if(Mathf.Abs(cam.localPosition.x) > .5f || Mathf.Abs(cam.localPosition.y) > .5f)
			{
				cam.localPosition = Vector3.Scale(cam.localPosition, Vector3.forward);
			}

			// cam.localPosition = Vector3.Scale (cam.localPosition, Vector3.forward);
		}
	}

	protected override void Follow(float deltaTime)
	{

	}

}
