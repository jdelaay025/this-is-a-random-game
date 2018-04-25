using UnityEngine;
using System.Collections.Generic;
//using UnityEditor;

public class FreeCameraLook : Pivot
{
    #region Global Variable Declaration

    public static FreeCameraLook Instance { get; set; }

    [SerializeField]
    private float moveSpeed = 5f;
    [SerializeField]
    public float turnSpeed = 3.5f;
    [SerializeField]
    public float turnSmoothing = .1f;
    [SerializeField]
    private float tiltMax = 75f;
    [SerializeField]
    private float tiltMin = 45f;

    private float lookAngle;
    private float tiltAngle;

    private const float LookDistance = 100f;
    public ParticleSystem burst;
    public ParticleSystem burst2;

    private float smoothX = 0;
    private float smoothY = 0;
    private float smoothXVelocity = 0;
    private float smoothYVelocity = 0;
    public GameObject player;
    public float setTurnSpeed;
    public bool controllerInverted = false;

    public bool controllingWithMouse = false;
    public int whichTarget = 0;
    public Transform focusTarget
    {
        get {return focustarget; }
        set
        {
            focustarget = value;
            if (targets.Count > 1)
            {
                targets.RemoveAt(1);
                targets.Add(GameMasterObject.enemies[whichTarget]);
            }
        ; }
    }
    private Transform focustarget;

    public Transform pivotTarget;
    public List<Transform> targets;

    StateManager playerStates;

    public enum CameraState
    {
        FreeRun = 0,
        MultiTarget = 1,
        FirstPerson = 2,
        ThirdPerson = 3
    }

    public CameraState state;

    public bool playing = false;

    public Transform cameraHolderPos;

    #endregion

    protected override void Awake()
    {
        base.Awake();

        playing = false;

        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }

        Cursor.lockState = CursorLockMode.Confined;

        cam = GetComponentInChildren<Camera>().transform;
        if(cam != null)
        {
            camHolder = cam.parent;
        }

        setTurnSpeed = turnSpeed;
    }
    protected override void Start()
    {
        base.Start();

        pivot = GameObject.Find("Pivot").transform;
        player = GameMasterObject.playerUse;
        if(player != null)
        {
            playerStates = player.GetComponent<StateManager>();
        }

        state = CameraState.MultiTarget;

        playing = true;
    }

    protected override void Update()
    {
        base.Update();

        if (playing == true)
        {
            HandleRotationMovement();
        }

        // controllerInverted = InvertedControlsScript.isInverted;

        /*if (lockCursor && Input.GetMouseButtonUp (0)) 
		{
			Cursor.lockState = CursorLockMode.Locked;
		}*/
    }
    void LateUpdate()
    {
    }
    void OnDisable()
    {
        //Cursor.lockState = CursorLockMode.None;
    }

    protected override void Follow(float deltaTime)
    {
        transform.position = Vector3.Lerp (transform.position, target.position, deltaTime * moveSpeed);
        if (state == CameraState.MultiTarget)
        {
            if (playing == true)
            {
                camHolder.position = Vector3.Lerp(camHolder.position, cameraHolderPos.position, deltaTime * moveSpeed);
            }
        }
    }

    void HandleRotationMovement()
    {
        if (state == CameraState.ThirdPerson)
        {
            #region Third Person Functionality

            pivot.position = pivotTarget.position;
            pivot.localRotation = pivotTarget.rotation;

            camHolder.localRotation = Quaternion.Euler(10f, 0f, 0f);

            float x = Input.GetAxis("horRot");
            float y = Input.GetAxis("verRot");

            if (turnSmoothing > 0)
            {
                smoothX = Mathf.SmoothDamp(smoothX, x, ref smoothXVelocity, turnSmoothing);
                smoothY = Mathf.SmoothDamp(smoothY, y, ref smoothYVelocity, turnSmoothing);
            }
            else
            {
                smoothX = x;
                smoothY = y;
            }

            lookAngle += smoothX * turnSpeed;

            transform.rotation = Quaternion.Euler(0f, lookAngle, 0f);

            tiltAngle -= smoothY * turnSpeed;
            tiltAngle = Mathf.Clamp(tiltAngle, -tiltMin, tiltMax);

            if (controllerInverted == true)
            {
                pivot.localRotation = Quaternion.Euler(-tiltAngle, 0, 0);
            }
            else if (controllerInverted == false)
            {
                pivot.localRotation = Quaternion.Euler(tiltAngle, 0, 0);
            }

            #endregion
        }
        if (state == CameraState.FirstPerson)
        {
            #region First Person Functionality



            #endregion
        }
        if (state == CameraState.MultiTarget)
        {
            #region MultiTarget Functionality

            if (playing == true)
            {
                playerStates.lookPosition = focustarget.position;
                pivot.position = GetCenterPoint();

                Vector3 dir = pivot.position - camHolder.position;

                Quaternion lookRotation = Quaternion.LookRotation(dir);

                camHolder.rotation = lookRotation;
            }
            #endregion
        }
        if (state == CameraState.FreeRun)
        {
            #region FreeRun Functionality



            #endregion
        }
    }

    Vector3 GetCenterPoint()
    {
        if (targets.Count == 1)
        {
            return targets[0].position;
        }

        var bounds = new Bounds(targets[0].position, Vector3.zero);
        for (int i = 0; i < targets.Count; i++)
        {
            bounds.Encapsulate(targets[i].position);
        }

        return bounds.center;
    }

}
