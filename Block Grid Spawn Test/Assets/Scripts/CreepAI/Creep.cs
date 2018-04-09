using UnityEngine;
using System.Collections;

public class Creep : MonoBehaviour 
{
    #region Global Variable Declaration

    [Header("Targeting Factors")]
    public float turnSpeed = 10f;
    public float sightRange = 50f;
    public float targetingDuration = 2.5f;
    public float stoppingDistance = 5f;

    [Header("Attacking and Damaging Factors")]
    public float minDamage = 5f;
    public float maxDamage = 10f;
    public float timeTillAttack = 3f;

    [Header("Sets Creep Vision Location")]
    public Transform eyes;   
    
    [Header("Flags for Types of Creep")]
    public bool heroCreep = false;
    public LayerMask layerMask;
    public GameObject stateIdentifier;
    
    /*[HideInInspector]*/ public Transform chaseTarget;

    [HideInInspector] public IDamageable damageableObject;
    [HideInInspector] public ICreepAI currentState;

    [HideInInspector] public AttackState attackState;
    [HideInInspector] public TargetDamageableObjectState targetState;
    [HideInInspector] public TargetWaypointState targetWayPoint;
    [HideInInspector] public UnityEngine.AI.NavMeshAgent agent;
    [HideInInspector] public Animator anim;
    [HideInInspector] public Renderer rend;
    
    #endregion

    void Awake()
    {
        attackState = new AttackState(this);
        targetState = new TargetDamageableObjectState(this);
        targetWayPoint = new TargetWaypointState(this);

        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        anim = GetComponent<Animator>();
        rend = stateIdentifier.GetComponent<Renderer>();
    }

	void Start ()
	{
        currentState = targetWayPoint;
	}

	void FixedUpdate ()
    {
        //Debug.Log(currentState);
        //Debug.Log(agent.destination);
        //Debug.Log(damageableObject);
        //Debug.Log(agent.remainingDistance);

        currentState.UpdateStates();

        if (currentState == attackState)
        {
            if (chaseTarget == null)
            {
                currentState = targetWayPoint;
            }
            else if (chaseTarget != null)
            {
                float dist = Vector3.Distance(chaseTarget.position, transform.position);

                if (dist >= 1500)
                {
                    chaseTarget = null;
                }
            }
        }
        //else if (currentState != attackState)
        //{
        //}
	}

    void OnTriggerEnter(Collider other)
    {
        currentState.OnTriggerEnter(other);
    }

    void OnTriggerStay(Collider other)
    {
        currentState.OnTriggerStay(other);
    }

    void OnTriggerExit(Collider other)
    {
        currentState.OnTriggerExit(other);
    }

    void OnCollisionEnter(Collision col)
    {
        currentState.OnCollisionEnter(col);
    }

    public void Attack()
    {
        if (damageableObject != null)
        {
            damageableObject.TakeDamage(Random.Range(minDamage, maxDamage), Vector3.zero);
        }
    }

}
