using UnityEngine;
using System.Collections;

public interface ICreepAI 
{
    void UpdateStates();
    void ToAttack(Transform target);
    void ToWayPoint();
    void ToFindTargetEnemy();
    void OnTriggerEnter(Collider other);
    void OnTriggerStay(Collider other);    
    void OnTriggerExit(Collider other);
    void OnCollisionEnter(Collision other);
}
