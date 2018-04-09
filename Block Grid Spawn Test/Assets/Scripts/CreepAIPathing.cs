using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreepAIPathing : MonoBehaviour
{
    #region Global Variable Declaration

    public CreepState state;

    [SerializeField]
    Transform myTransform;

    [SerializeField]
    Transform player;
    [SerializeField]
    Transform target;
    [SerializeField]
    LayerMask lookLayerMask;
    [SerializeField]
    float sphereRadius = 2f;

    [SerializeField]
    Transform leftTurnTarget;
    [SerializeField]
    Transform rightTurnTarget;
    [SerializeField]
    float turnSpeed = 0.5f;
    [SerializeField]
    float moveSpeed = 3f;

    public Transform targetToUse;

    float lookRayDistance = 3f;

    [SerializeField]
    bool somethingInFront = false;
    [SerializeField]
    bool somethingOnLeft = false;
    [SerializeField]
    bool somethingOnRight = false;

    Transform parentTransform;

    #endregion

    void Awake()
    {
        parentTransform = transform;
    }
    void Start()
    {
        state = CreepState.Running;

        if (GameMasterObject.playerUse != null)
        {
            player = GameMasterObject.playerUse.transform;
        }
    }
    void FixedUpdate()
    {
        if ((int)state == 2)
        {
            LookForward();
            
            RotateFromObstacle();

            if (somethingInFront != true)
            {
                MoveForward();
            }
        }
    }

    void LookForward()
    {
        #region setting up vision rays

        // head
        Ray upperMiddleRay = new Ray(myTransform.position + myTransform.up, myTransform.forward);
        if (Physics.SphereCast(upperMiddleRay, sphereRadius, lookRayDistance, lookLayerMask))
        {
            // Debug.Log("hit head");
        }

        // gut
        Ray middleMiddleRay = new Ray(myTransform.position, myTransform.forward);
        somethingInFront = Physics.SphereCast(middleMiddleRay, sphereRadius, lookRayDistance, lookLayerMask);

        // left hip left
        Ray middleLeftRay = new Ray(myTransform.position + -myTransform.right, -myTransform.right);
        somethingOnLeft = Physics.SphereCast(middleLeftRay, sphereRadius, lookRayDistance, lookLayerMask);

        // right hip right
        Ray middleRighttRay = new Ray(myTransform.position + myTransform.right, myTransform.right);
        somethingOnRight = Physics.SphereCast(middleRighttRay, sphereRadius, lookRayDistance, lookLayerMask);

        // left leg
        Ray lowerLeftRay = new Ray(myTransform.position + new Vector3(-1f, -1f, 0f), myTransform.forward);
        if (Physics.Raycast(lowerLeftRay, lookRayDistance, lookLayerMask))
        {
            // Debug.Log("hit left leg");
        }

        // right leg
        Ray lowerRighttRay = new Ray(myTransform.position + new Vector3(1f, -1f, 0f), myTransform.forward);
        if (Physics.Raycast(lowerRighttRay, lookRayDistance, lookLayerMask))
        {
            // Debug.Log("hit right leg");
        }

        #endregion
    }
    void MoveForward()
    {
        parentTransform.Translate(myTransform.forward * moveSpeed * Time.deltaTime);
    }
    void RotateFromObstacle()
    {
        Vector3 dir;

        if (somethingInFront)
        {
            dir = (leftTurnTarget.position - myTransform.position);
        }
        else
        {
            if (somethingOnRight ||
                somethingOnLeft)
            {
                dir = (leftTurnTarget.position - myTransform.position);
            }
            else
            {
                dir = (target.position - myTransform.position);
            }
        }

        Quaternion lookDirection = Quaternion.LookRotation(dir);
        lookDirection = Quaternion.Euler(0f, lookDirection.eulerAngles.y, 0f);

        Quaternion turn = Quaternion.Slerp(myTransform.rotation, lookDirection, turnSpeed);

        myTransform.rotation = turn;
    }

}
