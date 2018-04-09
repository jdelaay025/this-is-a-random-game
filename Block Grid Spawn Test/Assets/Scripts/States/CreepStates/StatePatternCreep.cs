using UnityEngine;
using System.Collections;

public class StatePatternCreep : MonoBehaviour 
{

    #region Global Variable Declaration

    public float searchingTurnSpeed = 120f;
    public float searchingDuration = 4f;
    public float sightRange = 20f;
    public Transform[] wayPoints;
    public Transform eyes;
    public Vector3 offset = new Vector3(0f, 0.5f, 0f);
    public MeshRenderer meshRendererFlag;
    public string typeOfCreep;

    [HideInInspector] public Transform chaseTarget;
    [HideInInspector] public IEnemyState currentState;
    [HideInInspector] public ChaseState chaseState;
    [HideInInspector] public AlertState alertState;
    [HideInInspector] public PatrolState patrolState;
    [HideInInspector] public UnityEngine.AI.NavMeshAgent navMeshAgent;

    #endregion

    void Awake()
    {
        chaseState = new ChaseState(this);
        alertState = new AlertState(this);
        patrolState = new PatrolState(this);

        navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }

    void Start () 
	{
        currentState = patrolState;
	}

	void Update () 
	{
        Debug.Log("");

        currentState.UpdateStates();       
	}

    private void OnTriggerEnter(Collider other)
    {
        currentState.OnTriggerEnter(other);
    }
}
