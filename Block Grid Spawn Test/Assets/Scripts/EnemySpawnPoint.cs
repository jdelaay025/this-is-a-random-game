using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class EnemySpawnPoint : MonoBehaviour 
{
    #region Global Variable Declaration
    public GameObject enemyToSpawn;
    public List<Transform> enemies;
    public bool spawnNow = false;
    public float timeToSpawn = 3f;    
    public bool dead = false;

    float timer = 0f;
    int nextPoint = 0;
    int spawnAmount = 5;
    int enemyNumbers = 0;
    Rigidbody rb;
    #endregion

    void Start () 
	{
        
	}

	void Update () 
	{
        if (spawnNow)
        {
            if (timer <= 0)
            {
                if (enemyNumbers <= spawnAmount)
                {
                    SpawnLionShark();
                }
                
                timer = timeToSpawn;
            } 
            else if (timer > 0)
            {
                timer -= Time.deltaTime;
            }
        }

        if (dead)
        {
            ChangePositions();
            dead = false;
        }

        switch (nextPoint)
        {
            case 1:
                spawnAmount = 3;
                break;
            case 2:
                spawnAmount = 10;
                break;
            case 3:
                spawnAmount = 13;
                break;
            case 4:
                spawnAmount = 16;
                break;
            case 5:
                spawnAmount = 19;
                break;
            case 6:
                spawnAmount = 22;
                break;
            case 7:
                spawnAmount = 25;
                break;
            case 8:
                spawnAmount = 28;
                break;
            case 9:
                spawnAmount = 31;
                break;
            case 10:
                spawnAmount = 35;
                break;
            default:
                break;
        }
	}

    void SpawnLionShark()
    {
        enemyToSpawn = enemies[0].gameObject;
        SpawnEnemy(enemyToSpawn);
    }

    void SpawnEnemy(GameObject enemy)
    {
        GameObject go = (GameObject)Instantiate(enemy, transform.position, transform.rotation);
        enemies.Add(go.transform);
        enemyNumbers++;

        rb = go.GetComponent<Rigidbody>();
        Vector3 force = new Vector3(20f, 50f, 10f);
        if (rb != null)
        {
            rb.AddForce(force, ForceMode.Impulse);
        }
        
        rb = null;
    }

    public void ChangePositions()
    {
        //		Debug.Log ("Change Position");
        nextPoint++;

        if (nextPoint < GameMasterObject.enemySpawnPoints.Count)
        {
            transform.position = GameMasterObject.enemySpawnPoints[nextPoint].position;
            this.gameObject.SetActive(true);
        }
        else if (nextPoint >= GameMasterObject.enemySpawnPoints.Count)
        {
            //Debug.Log("Free to cap");
            //Debug.Log("sike");
            nextPoint = 0;
        }
    }
}
