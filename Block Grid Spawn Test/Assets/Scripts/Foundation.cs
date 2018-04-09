using UnityEngine;
using System.Collections;

public class Foundation : MonoBehaviour 
{

    #region Global Variable Declaration

    public bool isPlaced = false;
	public bool isSnapped = false;

	public float mousePosX;
	public float mousePosY;

	float yOffset = 32.54f;

    #endregion

    void Update () 
	{
		if(!isPlaced && !isSnapped)
		{
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit hit;

			if(Physics.Raycast(ray.origin, ray.direction, out hit, 200f))
			{
				this.transform.position = new Vector3(hit.point.x, yOffset, hit.point.z);
			}
		}
		if(Input.GetButtonDown("Fire"))
		{
			isPlaced = true;
			BuildingManager.building = false;
		}

		if(isSnapped && !isPlaced && Mathf.Abs(mousePosX - Input.GetAxis("horRot")) > 1f || 
                                     Mathf.Abs(mousePosY - Input.GetAxis("verRot")) > 1f)
		{
			isSnapped = false;
		}
	}

}
