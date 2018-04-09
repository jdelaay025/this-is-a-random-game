using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuildingManager : MonoBehaviour 
{

    #region Global Variable Declaration

    public static bool building = false;

	public GameObject foundationPrefab;

	Transform objectToBuild; 
	float yOffset = 32.54f;
	Vector3 mousePosition;

    #endregion
    
	void Update()
	{

		if(!building)
		{
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit hit;

			if(Physics.Raycast(ray.origin, ray.direction, out hit, 200f))
			{			
				mousePosition = new Vector3(hit.point.x, 0, hit.point.z);
			}

			if(Input.GetButtonDown("Build"))
			{
				building = true;

				BuildFoundation ();
			}
		}
	}

	void BuildFoundation () 
	{
		Transform go = Instantiate (foundationPrefab.transform, new Vector3 (mousePosition.x, yOffset, mousePosition.z), Quaternion.identity);
		GameMasterObject.foundations.Add (go);
	}
	void Build (GameObject obj) 
	{
		Transform go = Instantiate (obj.transform, transform.position, Quaternion.identity);
		GameMasterObject.foundations.Add (go);
	}

}
