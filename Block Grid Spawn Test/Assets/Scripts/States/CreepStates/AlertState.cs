using UnityEngine;
using System.Collections;

public class AlertState : IEnemyState 
{
    #region Global Variable Declaration
    private readonly StatePatternCreep enemy;
    private float searchTimer;
    #endregion

    public AlertState(StatePatternCreep statePatternCreep)
    {
        enemy = statePatternCreep;
    }

    public void UpdateStates()
    {
        Look();
        Search();
    }

    public void OnTriggerEnter(Collider other)
    {

    }

    public void ToPatrolState()
    {
        enemy.currentState = enemy.patrolState;
        searchTimer = 0f;
    }

    public void ToAlertState()
    {

    }

    public void ToChaseState()
    {
        enemy.currentState = enemy.chaseState;
        searchTimer = 0f;
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

    void Search()
    {
        enemy.meshRendererFlag.material.color = Color.yellow;
        enemy.navMeshAgent.Stop();
        enemy.transform.Rotate(0f, enemy.searchingTurnSpeed * Time.deltaTime, 0f);
        searchTimer += Time.deltaTime;

        if (searchTimer >= enemy.searchingDuration)
        {
            ToPatrolState();
        }

    }
}
