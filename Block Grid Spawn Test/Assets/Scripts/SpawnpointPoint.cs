using UnityEngine;
using System.Collections;

public class SpawnpointPoint : MonoBehaviour 
{
	void Start () 
	{
        GameMasterObject.enemySpawnPoints.Add(transform);
	}	
}
