using UnityEngine;

public class TargetWaypointState : ICreepAI 
{
    #region Global Variable Declaration
    public Transform wayPoint;

    private float searchTimer;
    private float sightRange = 50f;

    private readonly Creep thisCreep;
    #endregion

    public TargetWaypointState(Creep creep)
    {
        thisCreep = creep;
    }
    public void UpdateStates()
    {
        //Look();

        NavigateWayPoints();

    }
    public void ToAttack(Transform target)
    {
        thisCreep.damageableObject = target.GetComponentInParent<IDamageable>();

        thisCreep.agent.Stop();
        thisCreep.rend.material.color = Color.red;

        thisCreep.currentState = thisCreep.attackState;
    }
    public void ToWayPoint()
    {
        // thisCreep.currentState = thisCreep.targetWayPoint;

        Debug.Log("Currently In This State");
    }
    public void ToFindTargetEnemy()
    {
        thisCreep.currentState = thisCreep.targetState;
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "WayPoint")
        {
            if (thisCreep.heroCreep)
            {
                wayPoint = other.gameObject.GetComponent<WayPoint>().nextHeroWayPoint;
                
                if (other.gameObject.CompareTag("Enemy Tower"))
                {
                    //Debug.Log("enemy waypoint enter");

                    thisCreep.chaseTarget = other.transform;
                    thisCreep.stoppingDistance = 5;

                    if (thisCreep.agent.remainingDistance <= thisCreep.stoppingDistance)
                    {
                        ToAttack(other.transform);
                    }
                }
            }
            else if (!thisCreep.heroCreep)
            {
                wayPoint = other.gameObject.GetComponent<WayPoint>().nextEnemyWayPoint;
                
                if (other.gameObject.CompareTag("Hero Tower"))
                {
                    Debug.Log("hero waypoint enter");

                    thisCreep.chaseTarget = other.transform;
                    thisCreep.stoppingDistance = 5;

                    if (thisCreep.agent.remainingDistance <= thisCreep.stoppingDistance)
                    {
                        ToAttack(other.transform);
                    }
                }
            }
        }
        if (thisCreep.heroCreep)
        {
            wayPoint = other.gameObject.GetComponent<WayPoint>().nextHeroWayPoint;

            if (other.gameObject.CompareTag("Enemy Tower"))
            {
                //Debug.Log("enemy waypoint enter");

                thisCreep.chaseTarget = other.transform;
                thisCreep.stoppingDistance = 5;

                if (thisCreep.agent.remainingDistance <= thisCreep.stoppingDistance)
                {
                    ToAttack(other.transform);
                }
            }
        }
        else if (!thisCreep.heroCreep)
        {
            if (other.gameObject.CompareTag("Hero Tower"))
            {
                //Debug.Log("hero waypoint enter");

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
        if (thisCreep.heroCreep)
        {
            if (other.gameObject.CompareTag("WayPoint"))
            {
                wayPoint = other.gameObject.GetComponent<WayPoint>().nextHeroWayPoint;
            }
        }
        else if (!thisCreep.heroCreep)
        {
            if (other.gameObject.CompareTag("WayPoint"))
            {
                wayPoint = other.gameObject.GetComponent<WayPoint>().nextEnemyWayPoint;
            }
        }
    }
    public void OnTriggerExit(Collider other)
    {

    }
    public void OnCollisionEnter(Collision other)
    {
        //if (thisCreep.heroCreep)
        //{
        //    if (other.gameObject.CompareTag("Enemy Tower"))
        //    {              
        //        if (thisCreep.agent.remainingDistance <= thisCreep.stoppingDistance)
        //        {
        //            ToAttack(other.transform);
        //        }
        //    }
        //}
        //else if (!thisCreep.heroCreep)
        //{
        //    if (other.gameObject.CompareTag("Hero Tower"))
        //    {
        //        Debug.Log("hero collision enter");
        //        if (thisCreep.agent.remainingDistance <= thisCreep.stoppingDistance)
        //        {
        //            ToAttack(other.transform);
        //        }
        //    }
        //}
    }
    private void Look()
    {
        // Debug.Log("About to Look...");
        RaycastHit hit;

        if (Physics.Raycast(thisCreep.eyes.position, thisCreep.eyes.forward, out hit, sightRange, thisCreep.layerMask))
        {
            // Debug.Log("Looking... waypoints");

            //Debug.Log(hit.collider.gameObject.name);

            if (thisCreep.heroCreep)
            {
                if (hit.collider.CompareTag("Enemy Tower"))
                {
                    //Debug.Log("waypoint detection");
                    thisCreep.chaseTarget = hit.collider.transform;
                    //ToFindTargetEnemy();
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
                    //ToFindTargetEnemy();
                }
                else
                {
                    thisCreep.chaseTarget = null;
                }
            }            
        }
        else
        {
            //Debug.Log("We hit nothing! - waypoint");
            //thisCreep.chaseTarget = null;
        }
    }
    private void NavigateWayPoints()
    {
        if (wayPoint != null)
        {
            thisCreep.agent.destination = wayPoint.position;
            thisCreep.anim.SetBool("Running", true);
            thisCreep.agent.Resume();
        }
    }    
}
