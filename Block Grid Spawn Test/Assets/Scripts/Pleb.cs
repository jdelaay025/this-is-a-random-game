using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Pleb : MonoBehaviour
{
    #region Global Variable Declaration

    public Transform target;

    public CreepState state;

    Transform myTransform;
    NavMeshAgent agent;

    Vector3 untouchablePos = new Vector3(0f, 3000f, 0f);

    [SerializeField]
    float closeEnough = 3f;

    [SerializeField]
    LayerMask lookLayerMask;
    [SerializeField]
    float sphereRadius = 2f;
    float lookRayDistance = 3f;

    bool somethingInFront = false;

    Pleb opponent;

    #endregion

    void Awake()
    {
        myTransform = transform;
        state = CreepState.Running;
        agent = GetComponent<NavMeshAgent>();
    }
    void Start()
    {
        if (agent.destination != untouchablePos)
        {
            agent.SetDestination(target.position);
        }
    }
    void Update()
    {
        RaycastHit opponentHit;
        if (state == CreepState.Running)
        {

        }
        else
        {
            agent.isStopped = true;
        }

        if (opponent == null)
        {
            Ray middleMiddleRay = new Ray(myTransform.position, myTransform.forward);
            if (Physics.SphereCast(middleMiddleRay, sphereRadius, out opponentHit, lookRayDistance, lookLayerMask))
            {
                state = CreepState.Idle;
                UpdateTarget(opponentHit.collider.transform);
            }
        }

        #region Switch statment To check states

        switch (state)
        {
            case CreepState.Idle:
                break;
            case CreepState.Searching:
                break;
            case CreepState.Running:
                break;
            case CreepState.Fighting:
                break;
            case CreepState.Dead:
                break;
            default:
                break;
        }

        #endregion
    }
    void FixedUpdate()
    {
        if(opponent != null)
        {
            Attack();
        }
    }

    void UpdateTarget(Transform targ)
    {
        target = targ;
        opponent = targ.GetComponent<Pleb>();

        if(Vector3.Distance(myTransform.position, targ.position) <= closeEnough)
        {
            state = CreepState.Fighting;
        }
    }
    void Attack()
    {

    }
    public void DisconnectOpponent()
    {
        target = null;
        opponent = null;
    }

}
