using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Node : MonoBehaviour 
{
	public Color hoverColor;
	public Text warningText;

	Transform myTransform;
	Renderer rend;
	GameObject turret;
	Color initialColor;
	GameObject turretToDestroy;
	bool isSelected = false;
	BuildManager buildManager;
    GameMasterObject obj;

	void Awake () 
	{

		myTransform = transform;
		rend = GetComponent<Renderer> ();
		initialColor = rend.material.color;
	}

	void Start()
	{
		buildManager = BuildManager.Instance;
        obj = GameMasterObject.GetInstance();
    }

    void Update()
	{
        if (!GameMasterObject.isFreeMoveCamState)
        {
            return;
        }
        if (isSelected)
		{
			if(Input.GetMouseButtonDown(1))
			{
				Destroy (turret);
				return;
			}
		}
	}

	void OnMouseDown () 
	{
        if (!GameMasterObject.isFreeMoveCamState)
        {
            return;
        }
		if (turret != null) 
		{
			Debug.Log ("A TURRET IS ALREADY IN PLACE HERE!\nPLEASE SELECT ANOTHER SPACE!");
			return;
		}
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        else
        {
            if (GameMasterObject.setPlayerPosition)
            {
                if (buildManager.GetTurretToBuild() == null)
                {
                    // Debug.Log ("NO TURRET HAS BEEN PURCHASED! PLEASE PURCHASE A TURRET TO PLACE HERE!");
                    if (buildManager.warningText != null)
                        buildManager.SetPurchaseWarning();
                    return;
                }

                GameObject turretToBuild = buildManager.GetTurretToBuild();
                turretToBuild.transform.position = myTransform.position + new Vector3(0f, 0.2f, 0f);
                obj.SetToTPS();
                buildManager.EmptyTurret();
            }
            else if (!GameMasterObject.setPlayerPosition)
            {
                if (buildManager.GetTurretToBuild() == null)
                {
                    // Debug.Log ("NO TURRET HAS BEEN PURCHASED! PLEASE PURCHASE A TURRET TO PLACE HERE!");
                    if (buildManager.warningText != null)
                        buildManager.SetPurchaseWarning();
                    return;
                }

                GameObject turretToBuild = buildManager.GetTurretToBuild();
                turret = (GameObject)Instantiate(turretToBuild, myTransform.position + new Vector3(0f, 0.2f, 0f), myTransform.rotation);
                buildManager.EmptyTurret();
            }                       
        }
	}

	void OnMouseEnter () 
	{
        if (!GameMasterObject.isFreeMoveCamState)
        {
            return;
        }
        if (EventSystem.current.IsPointerOverGameObject())
		{
			return;
		}
		if(buildManager.GetTurretToBuild() == null && turret == null)
		{
			return;
		}

		rend.material.color = hoverColor;
		isSelected = true;
	}
    void OnMouseStay()
    {
        if (!GameMasterObject.isFreeMoveCamState)
        {
            return;
        }
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        if (buildManager.GetTurretToBuild() == null && turret == null)
        {
            return;
        }
    }
	void OnMouseExit () 
	{
        if (!GameMasterObject.isFreeMoveCamState)
        {
            return;
        }
        rend.material.color = initialColor;
		isSelected = false;
	}
}
