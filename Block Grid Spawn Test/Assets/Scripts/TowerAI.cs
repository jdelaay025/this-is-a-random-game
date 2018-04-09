using UnityEngine;
using System.Collections;

public class TowerAI : MonoBehaviour 
{
    #region Global Variable Declaration

    [Header("Right Turret Info")]   
    public Transform rightCanonBase;
    public Transform rightCanonHead;
    public Transform rightBulletSpawnPoint;

    [Header("Left Turret Info")]
    public Transform leftCanonBase;
    public Transform leftCanonHead;
    public Transform leftBulletSpawnPoint;

    public Transform target;

    [Header("Type of Turret")]
    public bool heroTurret = false;
    public bool enemyTurret = false;

    [Header("For Raycast")]
    public float lengthOfFire = 100f;
    public LayerMask layerMask;
    public float minDamage = 30f;
    public float maxDamage = 50f;
    public float timeTillFire = 2.5f;

    public string typeOfCore = string.Empty;

    Transform myTransform;
    Animator animLeftCanon;
    Animator animRightCanon;
    UnityEngine.AI.NavMeshObstacle obstacle;

    float shootTimer = 0f;    
    float turnSpeed = 7f;
    
    #endregion

    void Awake () 
	{

        myTransform = transform;

        animLeftCanon = GetComponent<Animator>();
        animRightCanon = GetComponent<Animator>();

        obstacle = GetComponentInChildren<UnityEngine.AI.NavMeshObstacle>();

        switch (typeOfCore)
        {
            case "hero":
                heroTurret = true;
                enemyTurret = false;
                break;
            case "Hero":
                heroTurret = true;
                enemyTurret = false;
                break;
            case "enemy":
                heroTurret = false;
                enemyTurret = true;
                break;
            case "Enemy":
                heroTurret = false;
                enemyTurret = true;
                break;
            case "tower":
                break;
            default:
                break;
        }
    }

    void Start()
    {

    }

    void Update () 
	{
        if (target != null)
        {
            RotateToTarget(target);

            if (shootTimer > 0)
            {
                shootTimer -= Time.deltaTime;
            }
            else if (shootTimer <= 0)
            {
                RightRaycastShoot();
                LeftRaycastShoot();

                shootTimer = timeTillFire;
            }
        }
	}

    void RightRaycastShoot()
    {
        //Debug.Log("Right Fire");

        Vector3 dirToShoot = rightBulletSpawnPoint.forward;
        RaycastHit hit;

        if (Physics.Raycast(rightBulletSpawnPoint.position, dirToShoot, out hit, lengthOfFire, layerMask))
        {
            //Debug.Log(hit.collider.gameObject);

            if (hit.collider.gameObject.tag == "Player")
            {
                IDamageable target = hit.collider.transform.GetComponent<IDamageable>();
                if (target != null)
                {
                    target.TakeDamage(Random.Range(minDamage, maxDamage), hit.point);
                }
            }
            if (hit.collider.gameObject.tag == "Ally")
            {
                IDamageable target = hit.collider.transform.GetComponent<IDamageable>();
                if (target != null)
                {
                    target.TakeDamage(Random.Range(minDamage, maxDamage), hit.point);
                }
            }
            if (hit.collider.gameObject.tag == "Foe")
            {
                IDamageable target = hit.collider.transform.GetComponent<IDamageable>();
                if (target != null)
                {
                    target.TakeDamage(Random.Range(minDamage, maxDamage), hit.point);
                }
            }
            if (hit.collider.gameObject.tag == "Enemy")
            {
                IDamageable target = hit.collider.transform.GetComponent<IDamageable>();
                if (target != null)
                {
                    target.TakeDamage(Random.Range(minDamage, maxDamage), hit.point);
                }
            }
        }

    }

    void LeftRaycastShoot()
    {
        //Debug.Log("Left Fire");

        Vector3 dirToShoot = leftBulletSpawnPoint.forward;
        RaycastHit hit;

        if (Physics.Raycast(rightBulletSpawnPoint.position, dirToShoot, out hit, lengthOfFire, layerMask))
        {
            //Debug.Log(hit.collider.gameObject);

            if (hit.collider.gameObject.tag == "Player")
            {
                IDamageable target = hit.collider.transform.GetComponent<IDamageable>();
                if (target != null)
                {
                    target.TakeDamage(Random.Range(minDamage, maxDamage), hit.point);
                }
            }
            if (hit.collider.gameObject.tag == "Ally")
            {
                IDamageable target = hit.collider.transform.GetComponent<IDamageable>();
                if (target != null)
                {
                    target.TakeDamage(Random.Range(minDamage, maxDamage), hit.point);
                }
            }
            if (hit.collider.gameObject.tag == "Foe")
            {
                IDamageable target = hit.collider.transform.GetComponent<IDamageable>();
                if (target != null)
                {
                    target.TakeDamage(Random.Range(minDamage, maxDamage), hit.point);
                }
            }
            if (hit.collider.gameObject.tag == "Enemy")
            {
                IDamageable target = hit.collider.transform.GetComponent<IDamageable>();
                if (target != null)
                {
                    target.TakeDamage(Random.Range(minDamage, maxDamage), hit.point);
                }
            }
        }

    }

    void RotateToTarget(Transform _target)
    {
        #region Right Canon Rotations

        Vector3 rightCanonDir = (_target.position + new Vector3(0f, 5f, 0f)) - rightCanonHead.position;
        Quaternion rcLookDirection = Quaternion.LookRotation(rightCanonDir);

        Quaternion rcCanonBaseDir = Quaternion.Slerp(rightCanonBase.rotation, rcLookDirection, Time.deltaTime * turnSpeed);
        Quaternion rcCanonHeadDir = Quaternion.Slerp(rightCanonHead.rotation, rcLookDirection, Time.deltaTime * turnSpeed);

        Quaternion rightCanonBaseRotation = Quaternion.Euler(0f, rcCanonBaseDir.eulerAngles.y, 0f);
        Quaternion rightCanonHeadRotation = Quaternion.Euler(rcCanonHeadDir.eulerAngles.x, rcCanonBaseDir.eulerAngles.y, 0f);

        rightBulletSpawnPoint.rotation = Quaternion.Euler(rcCanonHeadDir.eulerAngles.x, (rcCanonBaseDir.eulerAngles.y - 3f), 0f);
        //Debug.Log(rightCanonHeadRotation.eulerAngles);

        rightCanonBase.rotation = rightCanonBaseRotation;
        rightCanonHead.rotation = rightCanonHeadRotation;

        Vector3 rSpawnDir = (_target.position - rightBulletSpawnPoint.position);
        Quaternion rSpawnPointLookRotation = Quaternion.LookRotation(rSpawnDir);

        Quaternion rSpawnPointDir = Quaternion.Euler(rSpawnPointLookRotation.eulerAngles.x, rSpawnPointLookRotation.eulerAngles.y, 0f);

        rightBulletSpawnPoint.rotation = rSpawnPointDir;

        #endregion

        #region Left Canon Rotations

        Vector3 leftCanonDir = (_target.position + new Vector3(0f, 5f, 0f)) - leftCanonHead.position;
        Quaternion lcLookDirection = Quaternion.LookRotation(leftCanonDir);

        Quaternion lcCanonBaseDir = Quaternion.Slerp(leftCanonBase.rotation, lcLookDirection, Time.deltaTime * turnSpeed);
        Quaternion lcCanonHeadDir = Quaternion.Slerp(leftCanonHead.rotation, lcLookDirection, Time.deltaTime * turnSpeed);

        Quaternion leftCanonBaseRotation = Quaternion.Euler(0f, lcCanonBaseDir.eulerAngles.y, 0f);
        Quaternion leftCanonHeadRotation = Quaternion.Euler(lcCanonHeadDir.eulerAngles.x, lcCanonBaseDir.eulerAngles.y, 0f);

        leftBulletSpawnPoint.rotation = Quaternion.Euler(lcCanonHeadDir.eulerAngles.x, (lcCanonBaseDir.eulerAngles.y - 3f), 0f);
        //Debug.Log(leftCanonHeadRotation.eulerAngles);

        leftCanonBase.rotation = leftCanonBaseRotation;
        leftCanonHead.rotation = leftCanonHeadRotation;

        Vector3 lSpawnDir = (_target.position - rightBulletSpawnPoint.position);
        Quaternion lSpawnPointLookRotation = Quaternion.LookRotation(lSpawnDir);

        Quaternion lSpawnPointDir = Quaternion.Euler(lSpawnPointLookRotation.eulerAngles.x, lSpawnPointLookRotation.eulerAngles.y, 0f);

        rightBulletSpawnPoint.rotation = lSpawnPointDir;

        #endregion
    }

    void OnTriggerEnter(Collider other)
    {
        if (heroTurret)
        {
            if (other.transform.tag == "Foe")
            {
                target = other.transform;
            }
            if (other.transform.tag == "Enemy")
            {
                target = other.transform;
            }

            if (other.transform.tag == "Ally")
            {
                obstacle.enabled = false;
            }
            if (other.transform.tag == "Player")
            {
                obstacle.enabled = false;
            }
        }
        else if (enemyTurret)
        {
            if (other.transform.tag == "Ally")
            {
                target = other.transform;
            }
            if (other.transform.tag == "Player")
            {
                target = other.transform;
            }

            if (other.transform.tag == "Foe")
            {
                obstacle.enabled = false;
            }
            if (other.transform.tag == "Enemy")
            {
                obstacle.enabled = false;
            }
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (heroTurret)
        {
            if (other.transform.tag == "Foe")
            {
                target = other.transform;
            }
            if (other.transform.tag == "Enemy")
            {
                target = other.transform;
            }

            if (other.transform.tag == "Ally")
            {
                obstacle.enabled = false;
            }
            if (other.transform.tag == "Player")
            {
                obstacle.enabled = false;
            }
        }
        else if (enemyTurret)
        {
            if (other.transform.tag == "Ally")
            {
                target = other.transform;
            }
            if (other.transform.tag == "Player")
            {
                target = other.transform;
            }

            if (other.transform.tag == "Foe")
            {
                obstacle.enabled = false;
            }
            if (other.transform.tag == "Enemy")
            {
                obstacle.enabled = false;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.transform == target)
        {
            target = null;
        }

        if (heroTurret)
        {
            if (other.transform.tag == "Ally")
            {
                obstacle.enabled = true;
            }
            if (other.transform.tag == "Player")
            {
                obstacle.enabled = true;
            }
        }
        else if (enemyTurret)
        {
            if (other.transform.tag == "Foe")
            {
                obstacle.enabled = true;
            }
            if (other.transform.tag == "Enemy")
            {
                obstacle.enabled = true;
            }
        }
    }

}
