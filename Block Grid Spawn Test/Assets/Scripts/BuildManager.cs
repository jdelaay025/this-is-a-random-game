using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BuildManager : MonoBehaviour 
{
    #region Global Variable Declaration

    public static BuildManager Instance;
	public Text warningText;
	public Color lerpedColor = Color.red;
	public bool turretWarning = false;

	public GameObject standardTurret;
	public GameObject upgradedTurretLv2;
	public GameObject upgradedTurretLv3;
    public GameObject tpsPlayer;

	GameObject turretToBuild;

    #endregion

    void Awake () 
	{
		if(Instance != null)
		{
			Destroy(this.gameObject);
			return;
		}
		Instance = this;
	}

	void Update () 
	{
        //Debug.Log(turretToBuild);

		warningText.enabled = true;
		warningText.color = lerpedColor;
		if (turretWarning) 
		{
			lerpedColor = Color.Lerp (Color.red, new Color(1f, 0f, 0f, .02f),Mathf.PingPong(Time.time, 1));
		} 
		else 
		{
			warningText.enabled = false;
		}
	}

	public GameObject GetTurretToBuild()
	{
		return turretToBuild;
	}

	public void SetLv1Turret()
	{
		if(standardTurret != null)
		{
			turretToBuild = standardTurret;
		}
	}

	public void SetLv2Turret()
	{
		if(upgradedTurretLv2 != null)
		{
			turretToBuild = upgradedTurretLv2;
		}
	}

	public void SetLv3Turret()
	{
		if(upgradedTurretLv3 != null)
		{
			turretToBuild = upgradedTurretLv3;
		}
	}

    public void SetTPSPlayer()
    {        
        turretToBuild = tpsPlayer;
        GameMasterObject.setPlayerPosition = true;
    }

	public void EmptyTurret()
	{
		turretToBuild = null;
        GameMasterObject.setPlayerPosition = false;
    }

    public void SetPurchaseWarning()
	{
		StartCoroutine (SetPurchaseWarningNow ());
	}

	public IEnumerator SetPurchaseWarningNow()
	{		
		turretWarning = true;
		warningText.transform.position = Input.mousePosition;
		yield return new WaitForSeconds (3f);
		turretWarning = false;
//		yield return new WaitForSeconds (0.01f);
	}
}
