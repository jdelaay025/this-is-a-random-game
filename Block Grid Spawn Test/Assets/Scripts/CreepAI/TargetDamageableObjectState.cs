using UnityEngine;

public class TargetDamageableObjectState : ICreepAI
{
    #region Global Variable Declaration
   

    private readonly Creep thisCreep;
    #endregion

    public TargetDamageableObjectState(Creep creep)
    {
        thisCreep = creep;
    }
    public void UpdateStates()
    {
        thisCreep.rend.material.color = Color.yellow;

        if (thisCreep.chaseTarget != null)
        {
            if (!thisCreep.chaseTarget.gameObject.activeSelf)
            {
                thisCreep.chaseTarget = null;
                ToWayPoint();
            }
        }
        else if (thisCreep.chaseTarget == null)
        {
            ToWayPoint();
        }

        Chase();
        //Target();
    }
    public void ToAttack(Transform target)
    {
        thisCreep.damageableObject = target.GetComponentInParent<IDamageable>();

        thisCreep.currentState = thisCreep.attackState;
    }
    public void ToWayPoint()
    {
        thisCreep.currentState = thisCreep.targetWayPoint;
    }
    public void ToFindTargetEnemy()
    {
        // thisCreep.currentState = thisCreep.targetState;

        Debug.Log("Currently In This State");
    }
    public void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.gameObject.tag);
        if (thisCreep.heroCreep)
        {
            if (other.gameObject.CompareTag("Enemy"))
            {
                thisCreep.chaseTarget = other.transform;
                thisCreep.stoppingDistance = 5;

                if (thisCreep.agent.remainingDistance <= thisCreep.stoppingDistance)
                {
                    ToAttack(other.transform);
                }
            }
            else if (other.gameObject.CompareTag("Foe"))
            {
                thisCreep.chaseTarget = other.transform;
                thisCreep.stoppingDistance = 5;

                if (thisCreep.agent.remainingDistance <= thisCreep.stoppingDistance)
                {
                    ToAttack(other.transform);
                }
            }
            else if (other.gameObject.CompareTag("WayPoint"))
            {
                thisCreep.targetWayPoint.wayPoint = other.gameObject.GetComponent<WayPoint>().nextHeroWayPoint;
            }
            else if (other.gameObject.CompareTag("Enemy Tower"))
            {
                
                thisCreep.chaseTarget = other.transform;
                thisCreep.stoppingDistance = 5;

                if (thisCreep.agent.remainingDistance <= thisCreep.stoppingDistance)
                {
                    //Debug.Log("hero stop");
                    ToAttack(other.transform);
                }
            }
        }
        else if (!thisCreep.heroCreep)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                thisCreep.chaseTarget = other.transform;
                thisCreep.stoppingDistance = 5;

                if (thisCreep.agent.remainingDistance <= thisCreep.stoppingDistance)
                {
                    ToAttack(other.transform);
                }
            }
            else if (other.gameObject.CompareTag("Ally"))
            {
                thisCreep.chaseTarget = other.transform;
                thisCreep.stoppingDistance = 5;

                if (thisCreep.agent.remainingDistance <= thisCreep.stoppingDistance)
                {
                    ToAttack(other.transform);
                }
            }
            else if (other.gameObject.CompareTag("WayPoint"))
            {
                thisCreep.targetWayPoint.wayPoint = other.gameObject.GetComponent<WayPoint>().nextEnemyWayPoint;
            }
            else if (other.gameObject.CompareTag("Hero Tower"))
            {
                //Debug.Log("hero targeting enter");
                thisCreep.chaseTarget = other.transform;
                thisCreep.stoppingDistance = 5;

                if (thisCreep.agent.remainingDistance <= thisCreep.stoppingDistance)
                {
                    ToAttack(other.transform);
                }
            }
        }
    }
    public void OnTriggerStay(Collider other)
    {
        //if (other.transform != thisCreep.chaseTarget && !other.gameObject.CompareTag("WayPoint") && 
        //    !other.gameObject.CompareTag("Environment") && !other.gameObject.CompareTag("Tower"))
        //{
        //    thisCreep.chaseTarget = other.transform;
        //    thisCreep.agent.destination = thisCreep.chaseTarget.position;
        //}
    }
    public void OnTriggerExit(Collider other)
    {
       
    }
    public void OnCollisionEnter(Collision other)
    {
        //if (thisCreep.heroCreep)
        //{
        //    if (other.gameObject.CompareTag("Foe"))
        //    {
        //        thisCreep.chaseTarget = other.transform;
        //        ToAttack(other.transform);
        //    }
        //}
        //else if (!thisCreep.heroCreep)
        //{
        //    if (other.gameObject.CompareTag("Ally"))
        //    {
        //        thisCreep.chaseTarget = other.transform;
        //        ToAttack(other.transform);

        //    }
        //}
    }
    private void Target()
    {        
        if (thisCreep.chaseTarget == null)
        {
            RaycastHit hit;

            if (Physics.Raycast(thisCreep.eyes.position, thisCreep.eyes.forward, out hit, thisCreep.sightRange, thisCreep.layerMask))
            {
                // Debug.Log("Looking...targeting");

                // Debug.Log(hit.collider.tag);

                if (thisCreep.heroCreep)
                {
                    Debug.Log("targeting detection");
                    if (hit.collider.CompareTag("Enemy Tower"))
                    {
                        thisCreep.chaseTarget = hit.collider.transform;
                        thisCreep.stoppingDistance = 5f;                      

                        if (thisCreep.agent.remainingDistance <= thisCreep.stoppingDistance)
                        {
                            thisCreep.anim.SetBool("Running", false);
                            ToAttack(thisCreep.chaseTarget);
                        }
                    }
                    else
                    {
                        thisCreep.chaseTarget = null;
                    }
                }
                else if (!thisCreep.heroCreep)
                {                    
                    if (hit.collider.CompareTag("Hero Tower"))
                    {
                        thisCreep.chaseTarget = hit.collider.transform;
                        thisCreep.stoppingDistance = 5f;                    

                        if (thisCreep.agent.remainingDistance <= thisCreep.stoppingDistance)
                        {                        
                            thisCreep.anim.SetBool("Running", false);
                            ToAttack(thisCreep.chaseTarget);
                        }
                    }
                    else
                    {
                        thisCreep.chaseTarget = null;
                    }
                }
            }
            else
            {
                //Debug.Log("We hit nothing! - targeting");

                thisCreep.chaseTarget = null;
            }
        }    
    }
    private void Chase()
    {
        //if (thisCreep.chaseTarget != null)
        //{
            //Debug.Log("Chasing");
            thisCreep.agent.destination = thisCreep.chaseTarget.position;
            thisCreep.anim.SetBool("Running", true);
            thisCreep.agent.Resume();
        //}
        //else
        //{
        //    Debug.Log("Stopped");
        //}
    }
}
