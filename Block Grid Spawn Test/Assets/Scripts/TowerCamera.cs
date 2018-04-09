using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class TowerCamera : MonoBehaviour
{
    #region Global Variable Declaration

    public float rotateSpeed = 0f;
       
    public float xMinRotation = 40.0F;
    public float xMaxRotation = 240.0F;
    public float lookSmoothDamp = 0.03f;

    public Image ridicule;
    public Transform bulletSpawnPoint;
    public float maxBullets = 100f;
    public float minDamage = 20f;
    public float maxDamage = 30f;
    public float nextFireTime = 1.6f;
    public float amplitude = 0f;
    public float duration = 0f;
    public float rayDistance = 350f;
    public LayerMask layerMask;
    public GameObject impactPrefab;
    public int pooledImpactsAmount = 10;
    public bool needImpacts = false;

    private float horRot = 0f;
    private float verRot = 0f;
    private float fire = 0f;
    private float aim = 0f;
    private float currentX = 0.0f;
    private float currentY = 0.0f;
    private float sensitivityX = 3.5f;
    private float sensitivityY = 2.0f;
    private float xRotationV = 0f;
    private float yRotationV = 0f;

    Transform holder;
    Transform myTransform;
    Transform target;
    Quaternion lookDirection;
    Color initialColor;
    float currentBullets;
    float fireTime = 0f;
    int currentMountPlacement = 1;

    List<GameObject> impacts;
    
    #endregion

    void Awake()
    {
        myTransform = transform;
        holder = transform.parent.transform;       
    }

    void Start()
    {
        initialColor = ridicule.color;
        currentBullets = maxBullets;

        #region pooled impact prefabs
        if (needImpacts)
        {
            impacts = new List<GameObject>();

            for (int i = 0; i < pooledImpactsAmount; i++)
            {
                GameObject impact = (GameObject)Instantiate(impactPrefab);
                impact.SetActive(false);
                impacts.Add(impact);
            }
        }
        #endregion
    }

    void FixedUpdate()
    {
        if (!GameMasterObject.isPlayerActive)
        {
            return;
        }

        HandleInput();
        SmoothLook();
        ScoutRayCast();
        //HandleRotation(horRot, verRot);
    }

    void Update()
    {
        //Debug.Log(fireTime);

        if (!GameMasterObject.isPlayerActive)
        {
            return;
        }

        if (fireTime > 0)
        {
            fireTime -= Time.deltaTime;
        }

        if (currentBullets > 0 && fireTime <= 0)
        {
            if (fire > 0)
            {
                RaycastShoot();
                fireTime = nextFireTime;
            }
        }

        if (Input.GetButtonDown("Alt Fire"))
        {
            currentMountPlacement++;

            if (currentMountPlacement > (GameMasterObject.towerMounts.Count - 1))
            {
                currentMountPlacement = 0;
            }

            holder.position = GameMasterObject.towerMounts[currentMountPlacement].position;
        }
        if (Input.GetButtonDown("Ultimate"))
        {
            currentMountPlacement--;

            if (currentMountPlacement < 0)
            {
                currentMountPlacement = (GameMasterObject.towerMounts.Count - 1);
            }

            holder.position = GameMasterObject.towerMounts[currentMountPlacement].position;
        }
    }

    void HandleInput()
    {
        horRot = Input.GetAxis("horRot");
        verRot = Input.GetAxis("verRot");
        fire = Input.GetAxis("Fire");
        aim = Input.GetAxisRaw("Aim");
    }

    void SmoothLook()
    {
        Vector3 dir = Vector3.zero;

        currentY += Input.GetAxis("horRot") * sensitivityY;
        currentX += Input.GetAxis("verRot") * sensitivityX;
       
        currentX = Mathf.Clamp(currentX, xMinRotation, xMaxRotation);

        dir.x = Mathf.SmoothDamp(dir.x, currentX, ref xRotationV, lookSmoothDamp);
        dir.y = Mathf.SmoothDamp(dir.y, currentY, ref yRotationV, lookSmoothDamp);

        transform.rotation = Quaternion.Euler(-dir.x, dir.y, 0);
    }

    void ScoutRayCast()
    {
        RaycastHit scoutHit;

        if (Physics.Raycast(bulletSpawnPoint.position, bulletSpawnPoint.forward, out scoutHit, rayDistance, layerMask))
        {
            if (scoutHit.collider.gameObject.CompareTag("Enemy")  || scoutHit.collider.gameObject.CompareTag("Foe"))
            {
               ridicule.color = Color.red;
            }
            else
            {
                ridicule.color = initialColor;
            }
        }
        else
        {
            ridicule.color = initialColor;
        }
    }

    void RaycastShoot()
    {
        RaycastHit hit;
        
        ShakeCamera.InstanceSM1.ShakeSM1(amplitude, duration);

        if (Physics.Raycast(bulletSpawnPoint.position, bulletSpawnPoint.forward, out hit, rayDistance, layerMask))
        {

            #region Group Attack
            ExplosionDamage(hit.point, 3);
            #endregion
            #region Single Enemy Attack commented out
            //IDamageable enemy = hit.collider.gameObject.GetComponentInParent<IDamageable>();
            //if (enemy != null)
            //{
            //    enemy.TakeDamage(Random.Range(minDamage, maxDamage), Vector3.zero);

            //    GameObject thisImpact;

            //    for (int i = 0; i < impacts.Count; i++)
            //    {
            //        if (!impacts[i].activeInHierarchy)
            //        {
            //            impacts[i].transform.position = hit.point;
            //            impacts[i].transform.rotation = Quaternion.identity;
            //            thisImpact = impacts[i];
            //            thisImpact.SetActive(true);
            //            break;
            //        }
            //    }
            //}
            #endregion

            GameObject thisImpact;

            for (int i = 0; i < impacts.Count; i++)
            {
                if (!impacts[i].activeInHierarchy)
                {
                    impacts[i].transform.position = hit.point;
                    impacts[i].transform.rotation = Quaternion.identity;
                    thisImpact = impacts[i];
                    thisImpact.SetActive(true);
                    break;
                }
            }
        }
    }

    void ExplosionDamage(Vector3 center, float radius)
    {
        Collider[] hitColliders = Physics.OverlapSphere(center, radius);
        for (int i = 0; i < hitColliders.Length; i++)
        {
            //Debug.Log(hitColliders[i].gameObject.tag);
            
            if (hitColliders[i].gameObject.CompareTag("Enemy") || hitColliders[i].gameObject.CompareTag("Foe"))
            {
                //Debug.Log("explosive hit");

                IDamageable e = hitColliders[i].gameObject.GetComponentInParent<IDamageable>();
                if (e != null)
                {
                    e.TakeDamage(Random.Range(minDamage, maxDamage), Vector3.zero);
                }
            }
        }
    }
}
