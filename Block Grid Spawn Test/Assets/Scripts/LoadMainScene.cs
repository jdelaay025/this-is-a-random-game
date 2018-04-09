using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadMainScene : MonoBehaviour 
{
	#region Global Variable Declaration



	#endregion

	void Awake () 
	{
		
	}
	void Start () 
	{
		
	}	
	public void GoToMainScene () 
	{
        SceneManager.LoadScene("Tap Booster");
	}

}
