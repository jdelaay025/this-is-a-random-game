using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldScript : MonoBehaviour 
{
    #region Global Variable Declaration

    public GameObject goldParticlePrefab;

	#endregion

	void Awake () 
	{
		
	}

	void Start () 
	{
		
	}
	
    public void SpawnGold()
    {
        if(goldParticlePrefab != null)
        {
            Instantiate(goldParticlePrefab, transform.position, transform.rotation);
        }
    }

}
