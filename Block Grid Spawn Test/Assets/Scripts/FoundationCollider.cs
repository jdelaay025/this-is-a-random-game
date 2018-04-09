using UnityEngine;
using System.Collections;

public class FoundationCollider : MonoBehaviour 
{

    #region Global Variable Declaration

    Foundation foundationScript;
	Vector3 sizeOfFoundation;

	float yOffset = 32.54f;

    #endregion

    void Start () 
	{
		foundationScript = transform.parent.parent.GetComponent<Foundation> ();
		sizeOfFoundation = transform.parent.parent.GetComponent<Collider> ().bounds.size;
	}
	void OnTriggerEnter(Collider other)
	{
		if (BuildingManager.building && other.tag == "Foundation" && foundationScript.isPlaced && !other.GetComponent<Foundation> ().isSnapped) 
		{
			Foundation foundation = other.GetComponent<Foundation>();

			foundation.isSnapped = true;
			foundation.mousePosX = Input.GetAxis ("horRot");
			foundation.mousePosX = Input.GetAxis ("verRot");

			float sizeX = sizeOfFoundation.x;
			float sizeZ = sizeOfFoundation.z;

			#region Case statement for which collider we hit

			switch (this.transform.tag) 
			{
				case "West Collider":
					other.transform.position = new Vector3 (transform.parent.parent.position.x - sizeX, yOffset, transform.parent.position.z);
					break;
				case "East Collider":
					other.transform.position = new Vector3 (transform.parent.parent.position.x + sizeX, yOffset, transform.parent.position.z);
					break;
				case "North Collider":
					other.transform.position = new Vector3 (transform.parent.parent.position.x, yOffset, transform.parent.position.z + sizeZ);
					break;
				case "South Collider":
					other.transform.position = new Vector3 (transform.parent.parent.position.x, yOffset, transform.parent.position.z - sizeZ);
					break;
			}

			#endregion
		}
					
	}

}
