using UnityEngine;
using System.Collections;

public class ChaseState : IEnemyState 
{
    #region Global Variable Declaration
    private readonly StatePatternCreep enemy;
    #endregion

    public ChaseState(StatePatternCreep statePatternCreep)
    {
        enemy = statePatternCreep;
    }

    public void UpdateStates()
    {
        Look();
        Chase();
    }

    public void OnTriggerEnter(Collider other)
    {

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

    }

    private void Look()
    {
        RaycastHit hit;

        if (Physics.Raycast(enemy.eyes.transform.position, enemy.eyes.transform.forward, out hit, enemy.sightRange) &&
            hit.collider.CompareTag("Enemy"))
        {
            enemy.chaseTarget = hit.transform;

        }
        else
        {
            ToAlertState();
        }
    }

    private void Chase()
    {
        enemy.meshRendererFlag.material.color = Color.red;
        enemy.navMeshAgent.destination = enemy.chaseTarget.position;
        enemy.navMeshAgent.Resume();
    }
}
