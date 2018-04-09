using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasFloating : MonoBehaviour
{
    #region Global Variable Declaration

    public Vector3 initialPosition;

    [SerializeField]
    float spinSpeed = 1f;
    [SerializeField]
    float floatSpeed = 1f;
    [SerializeField]
    float circleDiameter = 10f;
    [SerializeField]
    float floatVerticalMultiplier = 1f;

    [SerializeField]
    bool useSine = false;
    [SerializeField]
    bool useCos = false;

    [SerializeField]
    int numTillStopShowing = 10;

    Transform myTransform;

    #endregion

    void Awake()
    {
        myTransform = transform;
    }

    void Start()
    {
        initialPosition = myTransform.position;
    }

    void Update()
    {
        if (useSine)
        {
            float x = initialPosition.x;
            float y = initialPosition.y;
            float z = initialPosition.z;

            if (useSine && !useCos)
            {
                y = y + Mathf.Sin(Time.timeSinceLevelLoad * floatSpeed) / floatVerticalMultiplier;
            }

            myTransform.position = new Vector3(x, y, z);
        }
    }

}
