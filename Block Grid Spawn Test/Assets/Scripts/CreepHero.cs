using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CreepHero : MonoBehaviour
{
    #region Global variable Declaration

    public float timeTillAttack = 5f;
    public float minDamage = 5f;
    public float maxDamage = 10f;
    public float acceptableSDistance = 5f;
    public float canAgroEnemy = 50f;
    public float outLimitsOfDetection = 1500f;
    public float distanceFromEnemy = 5f;

    public float movementSpeed = 4f;
    public float turnSpeed = 3f;
    public float distToEnemy;
    public float distToTower;

    public Transform target = null;
    public Transform wayPoint;
    public int wayPointIndex = 0; 
    public Transform targetEnemy;
    public Transform tower;

    Transform myTransform;
    
    float attackTimer = 5f;
    Animator anim;
    bool running = false;
    bool canAttack = false;
    bool goForTower = false;

    float targetingTimer = 2.5f;
    float targetingTimerLimit = 2.5f;

    
    float closeEnoughToWP = 0.5f;

    #endregion
    void Awake()
    {
        myTransform = transform;
        anim = GetComponent<Animator>();
    }
    void Start() 
	{
        GameMasterObject.heroCreeps.Add(myTransform);
        wayPointIndex = WayPointManager.points.Length - 1;
                
    }
    void Update()
    {
        MoveToTarget();
        //Debug.Log(WayPointManager.points.Length - 1);
        #region set attack state       

        if (attackTimer > 0)
        {
            attackTimer -= Time.deltaTime;
        }
        else if (attackTimer <= 0 && canAttack)
        {
            if (anim != null)
            {
                anim.SetTrigger("Attack");
            }
            attackTimer = timeTillAttack;
        }
        #endregion
        FindWayPoints:
        #region move towards a target

        #region if targetEnemy is not null

        if (targetEnemy != null)
        {
            try
            {
                distToEnemy = Vector3.Distance(targetEnemy.position, myTransform.position);
                if (distToEnemy > acceptableSDistance && distToEnemy > canAgroEnemy && distToEnemy < outLimitsOfDetection)
                {
                    RotateToTarget(wayPoint);
                    canAttack = false;

                    #region check for Tower

                    if (tower == null)
                    {
                        try
                        {
                            RotateToTarget(wayPoint);
                        }
                        catch (System.Exception e)
                        {

                        }
                    }
                    if (tower != null)
                    {
                        try
                        {
                            RotateToTarget(tower);

                            distToTower = Vector3.Distance(tower.position, myTransform.position);

                            if (distToTower <= acceptableSDistance)
                            {
                                canAttack = true;
                            }
                            else
                            {
                                canAttack = false;
                            }
                        }
                        catch (System.Exception e)
                        {

                        }
                    }

                    #endregion

                    
                }
                else if (distToEnemy > acceptableSDistance && distToEnemy <= canAgroEnemy)
                {
                    RotateToTarget(targetEnemy);
                    canAttack = false;

                    //Debug.Log("canAgro");
                    #region check for Tower

                    if (tower == null)
                    {
                        try
                        {
                            RotateToTarget(wayPoint);
                        }
                        catch (System.Exception e)
                        {

                        }
                    }
                    if (tower != null)
                    {
                        try
                        {
                            RotateToTarget(tower);

                            distToTower = Vector3.Distance(tower.position, myTransform.position);

                            if (distToTower <= acceptableSDistance)
                            {
                                canAttack = true;
                            }
                            else
                            {
                                canAttack = false;
                            }
                        }
                        catch (System.Exception e)
                        {

                        }
                    }

                    #endregion

                }
                else if (distToEnemy <= acceptableSDistance)
                {
                    //Debug.Log("canAttack");
                    RotateToTarget(targetEnemy);
                    canAttack = true;
                }
                else if (distToEnemy > outLimitsOfDetection)
                {
                    canAttack = false;
                    targetEnemy = null;
                }
            }
            catch (System.Exception e)
            {
                goto FindWayPoints;
            }
        }

        #endregion

        #region if targetEnemy is null       

        else if (targetEnemy == null)
        {
            if (tower == null)
            {
                try
                {
                    RotateToTarget(wayPoint);
                }
                catch (System.Exception e)
                {

                }
            }
            if (tower != null)
            {
                try
                {
                    RotateToTarget(tower);

                    distToTower = Vector3.Distance(tower.position, myTransform.position);

                    if (distToTower <= acceptableSDistance)
                    {
                        canAttack = true;
                    }
                    else
                    {
                        canAttack = false;
                    }
                }
                catch (System.Exception e)
                {

                }
            }

            if (targetingTimer <= 0)
            {
                targetEnemy = FindTargetEnemy();
                targetingTimer = targetingTimerLimit;
            }
            else if (targetingTimer > 0)
            {
                targetingTimer -= Time.deltaTime;
            }
        }

        #endregion

        #region set states

        if (target != null)
        {
            if (!canAttack)
            {
                running = true;

            }
            else if (canAttack)
            {
                running = false;                
            }
        }        
        else if (target == null)
        {
            canAttack = false;
            running = false;
        }

        #endregion

        #endregion

        #region Check if there are any Dragon Monsters on the Field
        if (GameMasterObject.dragonsMonsters.Count > 0)
        {
            if (targetingTimer <= 0)
            {
                targetEnemy = FindTargetEnemy();
                targetingTimer = targetingTimerLimit;
            }
            else if (targetingTimer > 0)
            {
                targetingTimer -= Time.deltaTime;
            }
        }
        #endregion
    }
    void Attack()
    {
        if (target == null)
            return;

        IDamageable enemy = target.GetComponent<IDamageable>();
        if (enemy != null)
        {
            enemy.TakeDamage(Random.Range(minDamage, maxDamage), target.position);
        }
    }
    void RotateToTarget(Transform _target)
    {
        target = _target;

        Vector3 dir = target.position - myTransform.position;
        Quaternion lookDirection = Quaternion.LookRotation(dir);
        Quaternion facingDir = Quaternion.Slerp(myTransform.rotation, lookDirection, Time.deltaTime * turnSpeed);
        Quaternion faceNow = Quaternion.Euler(0f, facingDir.eulerAngles.y, 0f);

        myTransform.rotation = faceNow;
    }
    void MoveToTarget()
    {
        if (anim != null)
        {
            anim.SetBool("Running", running);
        }

        if (running)
        {
            myTransform.Translate(Vector3.forward * Time.deltaTime * movementSpeed);
        }
    }
    Transform FindTargetEnemy()
    {
        //Debug.Log("Checking for enemies");

        Transform tempTarget = null;
        float lastEnemyDist = Mathf.Infinity;

        if (GameMasterObject.dragonsMonsters.Count > 0)
        {
            for (int j = 0; j < GameMasterObject.dragonsMonsters.Count; j++)
            {
                float dist = Vector3.Distance(myTransform.position, GameMasterObject.dragonsMonsters[j].position);
                if (dist < lastEnemyDist && dist < outLimitsOfDetection && GameMasterObject.dragonsMonsters[j].gameObject.activeInHierarchy)
                {

                    tempTarget = GameMasterObject.dragonsMonsters[j];
                    lastEnemyDist = dist;
                }
            }
            acceptableSDistance = 9f;
        }
        else if (GameMasterObject.dragonsMonsters.Count <= 0)
        {       
            tempTarget = GameMasterObject.enemyCreeps[Random.Range(0, GameMasterObject.enemyCreeps.Count - 1)];
            return tempTarget;                      
        }
        return tempTarget;
    }      
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "WayPoint")
        {
            wayPoint = other.gameObject.GetComponent<WayPoint>().nextHeroWayPoint;
        }

        if (other.gameObject.tag == "Enemy Tower")
        {
            goForTower = true;
            if (other.gameObject != null)
            {
                tower = other.gameObject.GetComponentInChildren<TowerHealth>().transform;

                acceptableSDistance = other.gameObject.GetComponentInChildren<TowerHealth>().distanceFromMe;
            }
        }
        if (other.gameObject.tag == "Enemy")
        {
            targetEnemy = other.gameObject.transform;

            acceptableSDistance = distanceFromEnemy;
        }
    }
    void OnCollisionEnter(Collision collision)
    {            
        if (collision.gameObject.tag == "Foe")
        {                
            //Debug.Log("Foe");
            targetEnemy = collision.gameObject.transform;
            acceptableSDistance = distanceFromEnemy;
        }        
    }
    void OncollsionStay(Collision collision)
    {
        //Debug.Log("Foe");
        targetEnemy = collision.gameObject.transform;
        acceptableSDistance = distanceFromEnemy;

        if (targetEnemy == null && collision.gameObject.tag == "Foe")
        {
            //Debug.Log("Foe");
            targetEnemy = collision.gameObject.transform;
            acceptableSDistance = distanceFromEnemy;
        }
    }
}
