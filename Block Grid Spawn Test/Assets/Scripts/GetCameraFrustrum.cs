using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetCameraFrustrum : MonoBehaviour
{
    #region Global Variable Declaration

    public GameObject anObject;
    public Collider anObjCollider;

    [SerializeField]
    float seenForLogEnough = 3f;
    [SerializeField]
    float currentHealth = 100f;

    Camera cam;
    Plane[] planes;
    Renderer rend;
    bool startChecking = false;
    float coolDown = 0f;
    Color initialColor;

    bool seen = false;
    float timeSeen = 0f;
    Transform myTransform;

    #endregion

    void Awake()
    {
        myTransform = transform;
        anObject = this.gameObject;
        anObjCollider = GetComponent<Collider>();
        rend = anObject.GetComponent<Renderer>();
    }
    void Start()
    {
        cam = ControlNinjaDash.Instance.cam;

        if (rend != null)
        {
            initialColor = rend.material.color;
        }

        seenForLogEnough = (currentHealth / ControlNinjaDash.Instance.tickPerSecond);
    }
    void Update()
    {
        if (ControlNinjaDash.Instance.startChecking)
        {
            planes = GeometryUtility.CalculateFrustumPlanes(cam);
            timeSeen += Time.deltaTime;

            if (timeSeen >= seenForLogEnough
                && !seen)
            {
                seen = true;
                if (!ControlNinjaDash.Instance.poorTargets.Contains(myTransform) &&
                    ControlNinjaDash.Instance.poorTargets.Count < ControlNinjaDash.Instance.poorTargetsCap)
                {
                    ControlNinjaDash.Instance.poorTargets.Add(myTransform);
                }
            }

            if (GeometryUtility.TestPlanesAABB(planes, anObjCollider.bounds))
            {
                if (rend != null)
                {
                    if (seen)
                    {
                        rend.material.color = Color.green;
                    }
                    else
                    {
                        rend.material.color = Color.yellow;
                    }
                }
            }
            else
            {
                if (rend != null && !seen)
                {
                    rend.material.color = Color.red;
                    timeSeen = 0;
                }
            }
        }
        else
        {
            rend.material.color = initialColor;
        }
    }

}

