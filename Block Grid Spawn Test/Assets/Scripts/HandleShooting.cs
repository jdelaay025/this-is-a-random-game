using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HandleShooting : MonoBehaviour
{
    #region Global Variable Declaration

    StateManager states;
    public Animator weaponAnim;
    public float fireRate;
    public float reloadTime = 2;

    [SerializeField]
    float timer;

    public Transform bulletSpawnPoint;
    public Transform tracerBulletSpawn;
    public GameObject smokeParticle;
    public ParticleSystem[] muzzle;
    public GameObject casingPrefab;
    public Transform caseSpawn;
    public LayerMask layerMask;
    public float damage = 30;
    public float shootDistance = 250f;

    public Image ridicule;
    public Text bulletText;

    public int maxBullets = 45;
    public int curBullets = 0;

    public float amplitude;
    public float duration;

    public float hitForce = 50f;

    bool shoot;
    bool dontShoot;
    bool reloading = false;

    bool emptyGun;
    IKHandler ikhandler;

    Color initialColor;
    GameMasterObject gmobj;

    #endregion

    void Start()
    {
        gmobj = GameMasterObject.GetInstance();
        if (gmobj != null)
        {
            ridicule = gmobj.circleOne.GetComponent<Image>();
            bulletText = gmobj.bulletsLeft.GetComponent<Text>();
        }

        states = GetComponent<StateManager>();
        ikhandler = GetComponent<IKHandler>();

        curBullets = maxBullets;

        initialColor = ridicule.color;
    }
    void Update()
    {
        if (!GameMasterObject.isPlayerActive)
        {
            return;
        }

        shoot = states.shoot;

        if (states.handleAnim != null)
        {
            reloading = states.handleAnim.anim.GetBool("Reloading");
        }

        if (bulletText != null)
        {
            bulletText.text = (curBullets + " / " + maxBullets);
        }
        if (Input.GetButtonDown("Reload") && curBullets < maxBullets)
        {
            // Debug.Log("reload");
            Reload();
        }

        if (shoot && !ikhandler.notFacing)
        {
            if (timer <= 0 && !reloading)
            {
                weaponAnim.SetBool("Shoot", false);

                if (curBullets > 0)
                {
                    emptyGun = false;
                    states.audioManager.PlayGunSound();

                    ShakeCamera.InstanceSM1.ShakeSM1(amplitude, duration);

                    GameObject go = Instantiate(casingPrefab, caseSpawn.position, caseSpawn.rotation) as GameObject;
                    Rigidbody rig = go.GetComponent<Rigidbody>();
                    rig.AddForce(transform.right.normalized * 2 + Vector3.up * 1.3f, ForceMode.Impulse);
                    rig.AddRelativeTorque(go.transform.right * 1.5f, ForceMode.Impulse);

                    for (int i = 0; i < muzzle.Length; i++)
                    {
                        muzzle[i].Emit(1);
                    }

                    RaycastShoot();

                    curBullets = curBullets - 1;

                    timer = fireRate;
                }
                else if (curBullets <= 0)
                {
                    if (emptyGun)
                    {
                        states.handleAnim.StartReload();
                        curBullets = maxBullets;
                        timer = reloadTime;
                    }
                    else
                    {
                        states.audioManager.PlayEffect("Dry Fire");
                        emptyGun = true;
                    }
                }
            }
            else if (!reloading)
            {
                weaponAnim.SetBool("Shoot", true);

                timer -= Time.deltaTime;
            }
        }
        else if (!reloading)
        {
            timer = -1;
            weaponAnim.SetBool("Shoot", false);
        }
    }
    void FixedUpdate()
    {
        RaycastHit hit;
        Transform raySpawnPoint = bulletSpawnPoint;

        if (Physics.Raycast(raySpawnPoint.position, raySpawnPoint.forward, out hit))
        {
            #region rotates Cube around barrel of the gun for tracer bullets commented out
            //Vector3 dir = hit.point - tracerBulletSpawn.position;
            //Quaternion newRotation = Quaternion.LookRotation(dir);

            //tracerBulletSpawn.rotation = newRotation;
            #endregion

            if (hit.collider.gameObject.tag == "Enemy")
            {
                ridicule.color = Color.red;
            }
            else
            {
                ridicule.color = initialColor;
            }
        }

    }
    private void OnEnable()
    {
        timer = 2f;
    }

    void Reload()
    {
        states.handleAnim.StartReload();
        curBullets = maxBullets;
        timer = reloadTime;
    }
    void RaycastShoot()
    {
        Vector3 direction = states.lookHitPosition - bulletSpawnPoint.position;
        RaycastHit hit;
        //		            Ray ray = new Ray ();

        if (Physics.Raycast(bulletSpawnPoint.position, direction, out hit, shootDistance, layerMask))
        {
            if (smokeParticle != null)
            {
                GameObject go = Instantiate(smokeParticle, hit.point, Quaternion.identity) as GameObject;
                go.transform.LookAt(bulletSpawnPoint.position);
            }
            else if (hit.collider.gameObject.tag == "Enemy")
            {
                IDamageable enemy = hit.transform.GetComponent<IDamageable>();
                if (enemy != null)
                {
                    enemy.TakeDamage(damage, hit.point);
                }
            }
            else if (hit.collider.gameObject.tag == "Foe")
            {
                IDamageable enemy = hit.transform.GetComponent<IDamageable>();
                if (enemy != null)
                {
                    enemy.TakeDamage(damage, hit.point);
                }
            }
            else if (hit.collider.gameObject.tag == "Enemy Tower")
            {
                IDamageable enemy = hit.transform.GetComponent<IDamageable>();
                if (enemy != null)
                {
                    enemy.TakeDamage(damage, hit.point);
                }
            }
            else if (hit.collider.gameObject.tag == "Head")
            {
                IDamageable enemy = hit.transform.GetComponentInParent<IDamageable>();
                if (enemy != null)
                {
                    //Debug.Log("Head");
                    enemy.TakeDamage(damage, hit.point);
                }
            }
            else if (hit.collider.gameObject.tag == "Damageable Prop")
            {
                IDamageable prop = hit.transform.GetComponent<IDamageable>();
                Rigidbody propBody = hit.transform.GetComponent<Rigidbody>();
                if (prop != null)
                {
                    prop.TakeDamage(damage, hit.point);
                }
                if (propBody != null)
                {
                    propBody.AddForce(-hit.normal * hitForce, ForceMode.Impulse);
                }
            }
        }
    }

}
