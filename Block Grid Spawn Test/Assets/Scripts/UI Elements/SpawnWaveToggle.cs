using UnityEngine;
using UnityEngine.UI;

public class SpawnWaveToggle : MonoBehaviour 
{
	Toggle myToggle;
	public static bool timeToSpawn;

	void Awake () 
	{
		myToggle = GetComponent<Toggle>();
	}

	void Start () 
	{
		timeToSpawn = myToggle.isOn;
	}

	void Update () 
	{
		timeToSpawn = myToggle.isOn;
	}
}
