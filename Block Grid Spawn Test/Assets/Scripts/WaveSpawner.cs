using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class WaveSpawner : MonoBehaviour 
{
	public Transform enemyToSpawn;
	public int enemyLevelNumber = 1;
	public Transform enemyLv1;
	public Transform enemyLv2;
	public Transform enemyLv3;
	public Transform enemyBoss;
	public float timeBetweenSpawns = 5f;
	public Transform spawnPoints;
	public Text timerText;
	public float timeBeforeNextEnemy = 0.7f;
	public bool spawnNow = true;

	float CountDown = 5f;
	int waveNumber = 1;

	void Start () 
	{
		CountDown = 1;

		if (enemyLevelNumber == 1) 
		{
			enemyToSpawn = enemyLv1;
		}
		else if (enemyLevelNumber == 2) 
		{
			enemyToSpawn = enemyLv2;
		}
		else if (enemyLevelNumber == 3) 
		{
			enemyToSpawn = enemyLv3;
		}
		spawnNow = SpawnWaveToggle.timeToSpawn;
	}

	void Update () 
	{
		spawnNow = SpawnWaveToggle.timeToSpawn;
		if(spawnNow)
		{
			if(CountDown <= 0 )
			{
				StartCoroutine(SpawnWave ());
				CountDown = timeBetweenSpawns;
			}

			CountDown -= Time.deltaTime;

			if(timerText != null)
			{
				timerText.text = "Timer : " + ((int)Time.time).ToString ();
			}

			if (enemyLevelNumber == 1) 
			{
				enemyToSpawn = enemyLv1;
				timeBeforeNextEnemy = 0.5f;
				timeBetweenSpawns = 7;
			}
			else if (enemyLevelNumber == 2) 
			{
				enemyToSpawn = enemyLv2;
				timeBeforeNextEnemy = 1.5f;
				timeBetweenSpawns = 10;
			}
			else if (enemyLevelNumber == 3) 
			{
				enemyToSpawn = enemyLv3;
				timeBeforeNextEnemy = 3f;
				timeBetweenSpawns = 12;
			}
		}
	}

	IEnumerator SpawnWave()
	{
		if (waveNumber <= 12) 
		{
			for (int i = 0; i < waveNumber; i++) 
			{
				SpawnEnemies ();
				yield return new WaitForSeconds (timeBeforeNextEnemy);
			}

			waveNumber++;
		} 
		else 
		{
//			Debug.Log ("Congratulations, You Win!!");
		}
	}

	public void SpawnEnemies()
	{
		Instantiate (enemyToSpawn, spawnPoints.position, spawnPoints.rotation);
	}

	public void IncrementLv()
	{
		enemyLevelNumber++;
		if(enemyLevelNumber > 3)
		{
			enemyLevelNumber = 1;
		}
	}
	public void DecrementLv()
	{
		enemyLevelNumber--;
		if(enemyLevelNumber < 1)
		{
			enemyLevelNumber = 3;
		}
	}
}
