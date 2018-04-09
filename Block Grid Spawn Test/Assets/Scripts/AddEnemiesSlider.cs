using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AddEnemiesSlider : MonoBehaviour 
{
	public Slider thisSlider;

	void Awake () 
	{
		thisSlider = GetComponent<Slider> ();
	}

	void Start () 
	{
		GameMasterObject.enemiesSlider = thisSlider;
	}
}
