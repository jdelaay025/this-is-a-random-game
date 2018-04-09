using UnityEngine;
using System.Collections;

public class PatrolState : IEnemyState 
{
    #region Global Variable Declaration

    private readonly StatePatternCreep enemy;
    private int nextWayPoint;    

    #endregion

    public PatrolState(StatePatternCreep statePatterCreep)
    {
        enemy = statePatterCreep;
    }

    public void UpdateStates()
    {
        Look();
        Patrol();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            ToAlertState();
        }
        else if (other.gameObject.CompareTag("Tower"))
        {

        }
    }

    public void ToPatrolState()
    {

    }

    public void ToAlertState()
    {
        enemy.currentState = enemy.alertState;
    }

    public void ToChaseState()
    {
        enemy.currentState = enemy.chaseState;
    }

    private void Look()
    {
        RaycastHit hit;
        
        if (Physics.Raycast(enemy.eyes.transform.position, enemy.eyes.transform.forward, out hit, enemy.sightRange) && 
            hit.collider.CompareTag("Enemy"))
        {
            enemy.chaseTarget = hit.transform;
            ToChaseState();
        }
    }

    void Patrol()
    {
        enemy.meshRendererFlag.material.color = Color.green;
        enemy.navMeshAgent.destination = enemy.wayPoints[nextWayPoint].position;
        enemy.navMeshAgent.Resume();

        if (enemy.navMeshAgent.remainingDistance <= enemy.navMeshAgent.stoppingDistance && !enemy.navMeshAgent.pathPending)
        {
            nextWayPoint = (nextWayPoint + 1) % enemy.wayPoints.Length;
        }

    }
}
