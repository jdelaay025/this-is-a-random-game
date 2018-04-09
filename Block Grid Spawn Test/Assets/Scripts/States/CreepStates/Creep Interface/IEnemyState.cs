using UnityEngine;
using System.Collections;

public interface IEnemyState  
{
    void UpdateStates();

    void OnTriggerEnter(Collider other); 

    void ToPatrolState();

    void ToAlertState();

    void ToChaseState();
	
}
