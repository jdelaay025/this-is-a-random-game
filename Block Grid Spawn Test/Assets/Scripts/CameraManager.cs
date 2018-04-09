using UnityEngine;
using System.Collections;

public class CameraManager : MonoBehaviour 
{
    #region Global Variable Declaration

    public Camera TheCamera;
    public float orbitSensitivity = 10f;
    public bool HoldToOrbit;

    public float zoomMultiplier = 2f;
    public float minDistance = 2f;
    public float maxDistance = 30f;
    public bool InvertZoomDirection = false;
    float panSpeed = 0.25f;

    Transform cameraRig;
    Vector3 lastMousePos;

    Vector3 initialPosition;
    Quaternion initialRigRotation;

    #endregion

    void Start () 
	{
        if (TheCamera == null)
        {
            TheCamera = GetComponent<Camera>();            
        }
        if (TheCamera == null)
        {
            TheCamera = Camera.main;
        }
        if (TheCamera == null)
        {
            Debug.LogError("DUDE!!! THERE IS NO CAMERA!!!");
            return;
        }

        if (TheCamera != null)
        {
            initialPosition = TheCamera.transform.position;
        }

        cameraRig = TheCamera.transform.parent;
        if (cameraRig != null)
        {
            initialRigRotation = cameraRig.rotation;
        }
    }

    void Update()
    {
        OrbitCamera();
        DollyCamera();
        PanCamera();
    }

    void PanCamera()
    {
        Vector3 input = new Vector3(Input.GetAxis("horizontal"), 0f, Input.GetAxis("vertical"));

        Vector3 actualChange = input * panSpeed;

        actualChange = Quaternion.Euler(0f, TheCamera.transform.rotation.eulerAngles.y, 0f) * actualChange;      

        Vector3 newPosition = cameraRig.transform.position + actualChange;        

        cameraRig.transform.position = newPosition;
    }

    void DollyCamera()
    {
        float delta = -Input.GetAxis("Mouse ScrollWheel");
        if (InvertZoomDirection)
            delta = -delta;

        Vector3 actualChange = TheCamera.transform.localPosition * zoomMultiplier * delta;

        Vector3 newPosition = TheCamera.transform.localPosition + actualChange;

        newPosition = newPosition.normalized * Mathf.Clamp(newPosition.magnitude, minDistance, maxDistance);

        TheCamera.transform.localPosition = newPosition;

    }

	void OrbitCamera () 
	{
        if (Input.GetMouseButtonDown(1) == true)
        {
            lastMousePos = Input.mousePosition;
        }

        if (Input.GetMouseButton(1) == true)
        {
            Vector3 currentmousePosition = Input.mousePosition;

            //In pixels!
            Vector3 mouseMovement = currentmousePosition - lastMousePos;

            #region explanation of camera orbit           
            // let's orbit the camera rig with our actual camera object
            // when we orbit, the distance from the rig stays the same, but
            // the angle changes. Or another way to put it, we want to rotate
            // the vector indicating the relative position of our camera from
            // the rig.
            #endregion

            Vector3 posRelativeToRig = TheCamera.transform.localPosition;

            Vector3 rotationAngles = mouseMovement / orbitSensitivity;

            if (HoldToOrbit)
            {
                rotationAngles *= Time.deltaTime;
            }

            // TODO: Fix me
            // Quaternion theOrbitalRotation = Quaternion.Euler(rotationAngles.y, rotationAngles.x, 0);
            // posRelativeToRig = theOrbitalRotation * posRelativeToRig;

            TheCamera.transform.RotateAround( cameraRig.position, TheCamera.transform.right, -rotationAngles.y );
            TheCamera.transform.RotateAround( cameraRig.position, TheCamera.transform.up, rotationAngles.x );

            // cameraRig.Rotate(theOrbitalRotation.eulerAngles, Space.Self);
            // make sure the camera is still looking at our focal point (i.e. the rig)

            Quaternion lookRotation = Quaternion.LookRotation(-TheCamera.transform.localPosition);
            TheCamera.transform.localRotation = lookRotation;

            // if you want to keep the spinning going even without 
            // moving the mouse more ... don't do this.
            if (HoldToOrbit == false)
            {
                lastMousePos = currentmousePosition;
            }
        }
	}
}
