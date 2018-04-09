 using UnityEngine;
using System.Collections;

public class MouseManager : MonoBehaviour 
{
    #region Global Variable Declaration

    public GameObject prefabToSpawn;

    public LayerMask snapPointLayerMask;

    public ComponentKeybindDialog componentKeybindDialog;

    public GameObject shipRoot;

    #endregion

    void Start () 
	{
	
	}

	void Update () 
	{
        if (Input.GetMouseButtonDown(0))
        {
            CheckLeftClick();
        }

        if (Input.GetMouseButtonDown(1))
        {
            CheckRightClick();
        }
    }

    Collider DoRaycast()
    {
        Camera theCamera = Camera.main;

        Ray ray = theCamera.ScreenPointToRay(Input.mousePosition);

        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo, 500f))
        {
            int maskForThisHitObject = 1 << hitInfo.collider.gameObject.layer;
            //Debug.Log(hitInfo.collider);
            return hitInfo.collider;
            
        }
        return null;

    }

    void CheckRightClick()
    {
        //Debug.Log("right click");
        Collider theCollider = DoRaycast();
        if (theCollider == null)
        {            
            return;
        }
        
        GameObject shipPart = FindShipPart(theCollider);        
        if (shipPart == null)
        {
            return;
        }

        KeybindableComponent kc = shipPart.GetComponent<KeybindableComponent>();
        if (kc == null)
        {
            return;
        }
        //Debug.Log(kc);
        componentKeybindDialog.OpenDialog(kc);
    }

    GameObject FindShipPart(Collider collider)
    {
        Transform curr = collider.transform;
        while (curr != null)
        {
            if (curr.gameObject.tag == "Ship Part")
            {
                return curr.gameObject;
            }
            curr = curr.parent;
        }
        return null;
    }

    void CheckLeftClick()
    {       
        Camera theCamera = Camera.main;

        Collider theCollider = DoRaycast();
        if (theCollider == null)
        {
            return;
        }
                
        int maskForThisHitObject = 1 << theCollider.gameObject.layer;

        if ((maskForThisHitObject & snapPointLayerMask) != 0)
        {
            Vector3 spawnSpot = theCollider.transform.position;

            Quaternion spawnRotation = theCollider.transform.rotation; //Quaternion.FromToRotation(Vector3.forward, hitInfo.normal);             

            GameObject go = (GameObject)Instantiate(prefabToSpawn, spawnSpot, spawnRotation);
            go.transform.parent = theCollider.transform;

            if (theCollider.GetComponent<Renderer>() != null)
            {
                theCollider.GetComponent<Renderer>().enabled = false;
            }
            if (theCollider.GetComponent<Collider>() != null)
            {
                theCollider.GetComponent<Collider>().enabled = false;
            }
        }
                
    }

    void RemovePart(GameObject go)
    {
        go.transform.parent.GetComponent<Renderer>().enabled = true;
        go.transform.parent.GetComponent<Collider>().enabled = true;

        Destroy(go);
    }

    public void SetMode_Edit()
    {
        SetSnapPointEnable(shipRoot.transform, true);

        Camera.main.transform.parent.SetParent(null);
    }

    public void SetMode_Flight()
    {
        SetSnapPointEnable(shipRoot.transform, false);

        Camera.main.transform.parent.SetParent(shipRoot.transform);
        Camera.main.transform.parent.localPosition = Vector3.zero;
    }

    void SetSnapPointEnable(Transform t, bool setToActive)
    {
        int maskForThisHitObject = 1 << t.gameObject.layer;

        if ((maskForThisHitObject & snapPointLayerMask) != 0)
        {
            if (setToActive)
            {
                t.gameObject.SetActive(true);
            }
            else
            {
                if (t.childCount <= 0)
                {
                    t.gameObject.SetActive(false);
                    return;
                }
            }
        }

        for (int i = 0; i < t.childCount; i++)
        {
            SetSnapPointEnable(t.GetChild(i), setToActive);
        }    
    }
}
