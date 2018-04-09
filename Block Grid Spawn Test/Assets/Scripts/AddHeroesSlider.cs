using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AddHeroesSlider : MonoBehaviour 
{
	public Slider thisSlider;

	void Awake () 
	{
		thisSlider = GetComponent<Slider> ();
	}

	void Start () 
	{
		GameMasterObject.heroesSlider = thisSlider;
	}
}
