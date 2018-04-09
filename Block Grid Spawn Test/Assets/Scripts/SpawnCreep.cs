using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class SpawnCreep : MonoBehaviour 
{
    #region Global Variable Declaration

    public GameObject creepToSpawn;
    public List<GameObject> possibleCreeps;
    public List<GameObject> creeps;
    public Transform spawnPoint;
    public float numberOfCreeps = 10f;
    public float timeTillSpawn = 10f;

    public bool spawnNow = false;
    
    float ReadyToSpawn = 0f;
    float nextSpawn = 0f;

    #endregion

    void Awake()
    {
        creeps = new List<GameObject>();
    }

    void Start () 
	{
        for (int i = 0; i < numberOfCreeps; i++)
        {

            GameObject go = (GameObject)Instantiate(creepToSpawn, transform.position, transform.rotation);
            creeps.Add(go);
            go.SetActive(false);

        }
	}

	void Update () 
	{
        //Debug.Log(nextSpawn);

        if (Time.time >= nextSpawn)
        {
            if (spawnNow)
            {
                Spawn();
            }

            nextSpawn = Time.time + timeTillSpawn;
        }
    }

    void Spawn()
    {
        for (int i = 0; i < creeps.Count; i++)
        {
            if (!creeps[i].activeInHierarchy)
            {

                creeps[i].transform.position = spawnPoint.position;
                creeps[i].SetActive(true);

                return;

            }
        }
    }
}
