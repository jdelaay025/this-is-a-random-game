using UnityEngine;
using System.Collections;

public class towerMountPlacement : MonoBehaviour 
{
    public int numberMountPlacement = 0;
    void Awake () 
	{
        GameMasterObject.tempTowers[numberMountPlacement] = transform;
    }
}
