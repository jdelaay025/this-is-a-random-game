using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnManager : MonoSingleton<SpawnManager> 
{
	public List<Transform> spawnPoints = new List<Transform>();
	public List <GameObject> spawnPrefabs = new List<GameObject>();

	public void Spawn(int spawnPrefabIndex)
	{
		Spawn (spawnPrefabIndex, 0);
	}
	public void Spawn (int spawnPrefabIndex, int spawnPointIndex) 
	{
		Instantiate (spawnPrefabs [spawnPrefabIndex], spawnPoints [spawnPointIndex].position, spawnPoints [spawnPointIndex].rotation);
	}
	private void Update()
	{
		if(Input.GetKeyDown(KeyCode.Alpha1))
		{
		    Spawn(0);
		}
		if(Input.GetKeyDown(KeyCode.Alpha2))
		{
			Spawn(1);
		}
		if(Input.GetKeyDown(KeyCode.Alpha3))
		{
			Spawn(2);
		}
	}
}
