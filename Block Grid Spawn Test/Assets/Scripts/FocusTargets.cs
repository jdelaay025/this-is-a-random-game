using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusTargets : MonoBehaviour
{
    #region Global Variable Declaration



    #endregion

    void Awake()
    {

    }
    void Start()
    {
        GameMasterObject.enemies.Add(this.transform);
    }
    void Update ()
    {
		
	}

}
