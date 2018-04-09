using UnityEngine;

public class AttackState : ICreepAI 
{
    #region Global Variable Declaration

    float attackTimer = 0f;
        
    private readonly Creep thisCreep;
    
    #endregion

    public AttackState(Creep creep)
    {
        thisCreep = creep;
    }    
    public void UpdateStates()
    {
        thisCreep.anim.SetBool("Running", false);
        if (attackTimer <= 0)
        {
            thisCreep.anim.SetTrigger("Attack");
            attackTimer = thisCreep.timeTillAttack;
        }
        else if (attackTimer > 0)
        {
            attackTimer -= Time.deltaTime;
        }

        if (thisCreep.agent.remainingDistance > thisCreep.stoppingDistance)
        {
            thisCreep.chaseTarget = null;
        }
    }
    public void ToAttack(Transform target)
    {
        // thisCreep.damageableObject = target.GetComponent<IDamageable>();
        // thisCreep.currentState = thisCreep.attackState;

        Debug.Log("Currently In This State");
    }
    public void ToWayPoint()
    {
        thisCreep.rend.material.color = Color.green;

        thisCreep.chaseTarget = null;

        thisCreep.currentState = thisCreep.targetWayPoint;
    }
    public void ToFindTargetEnemy()
    {        
        thisCreep.currentState = thisCreep.targetState;
    }
    public void OnTriggerEnter(Collider other)
    {

    }
    public void OnTriggerStay(Collider other)
    {
        if (other.transform != thisCreep.chaseTarget && !other.gameObject.CompareTag("WayPoint") && 
            !other.gameObject.CompareTag("Environment") && !other.gameObject.CompareTag("Tower"))
        {
            thisCreep.chaseTarget = other.transform;
            thisCreep.agent.destination = thisCreep.chaseTarget.position;
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.transform == thisCreep.chaseTarget)
        {
            thisCreep.chaseTarget = null;
            thisCreep.damageableObject = null;

            ToWayPoint();
        }
    }
    public void OnCollisionEnter(Collision other)
    {

    }
}
