using UnityEngine;
using System.Collections;

public class FrontFacingBillboard : MonoBehaviour 
{
    #region Global Variable Declaration

    public Camera m_Camera;

    #endregion

    void start()
	{

    }
	void Update () 
	{
        if (m_Camera == null)
        {
            m_Camera = GameMasterObject.camInUse;
        }
		else		 
		{
			transform.LookAt (transform.position + m_Camera.transform.rotation * Vector3.forward, m_Camera.transform.rotation * Vector3.up);
		}
	}

}
