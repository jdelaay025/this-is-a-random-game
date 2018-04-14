using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ControlNinjaDash : MonoBehaviour
{
    #region Global Variable Declaration

    public static ControlNinjaDash Instance { get; set; }

    public Camera cam;

    public float coolDown = 0f;
    public bool startChecking = false;
    public float tickPerSecond = 75f;
    public List<Transform> poorTargets;
    public int poorTargetsCap = 25;

    [SerializeField]
    Camera secondaryCam;
    [SerializeField]
    float findTargetsCap = 4f;
    [SerializeField]
    float seenForLogEnough = 3f;
    [SerializeField]
    List<Transform> poorOrderedTargets;
    [SerializeField]
    float nextAttackCap = 0.125f;

    [SerializeField]
    Transform playerModel;
    Transform myTransform;
    Animator playerAnim;
    bool abilityStarted = false;


    #region attack section

    [SerializeField]
    float movementSpeed = 1f;
    [SerializeField]
    float closeEnough = 0.5f;
    [SerializeField]
    float increaseSpeedBy = 8f;
    [SerializeField]
    float movementSpeedCap = 500f;
    Vector3 startPos;
    Vector3 endPos;
    float startLerpTime;
    float dist = 0f;
    int currentTargetAttacking = 0;
    float nextAttack = 0.125f;

    bool attacking = false;
    int Combo = 0;

    #endregion

    #endregion

    void Awake()
    {
        myTransform = transform;

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }

        poorTargets = new List<Transform>();
        poorOrderedTargets = new List<Transform>();
    }
    void Start()
    {
        playerAnim = playerModel.GetComponent<Animator>();
    }
    void Update()
    {
        if (Input.GetButtonDown("Interact") &&
            coolDown <= 0)
        {
            startChecking = true;
            coolDown = findTargetsCap;
            if (secondaryCam != null)
            {
                secondaryCam.enabled = true;
            }
            abilityStarted = true;
        }

        if (startChecking)
        {
            cam.farClipPlane = 60;

            if (coolDown > 0)
            {
                coolDown -= Time.deltaTime;
            }
            else
            {
                startChecking = false;
            }
        }
        else
        {
            if (abilityStarted == true)
            {
                cam.farClipPlane = 1000;
                if (secondaryCam != null)
                {
                    secondaryCam.enabled = false;
                }

                OrderTargets();

                abilityStarted = false;
                attacking = true;
            }

            if (attacking == true)
            {
                if (nextAttack > 0)
                {
                    nextAttack -= Time.deltaTime;
                }
                else
                {
                    MoveToTarget();
                }
            }
        }
    }

    void OrderTargets()
    {
        poorOrderedTargets = poorTargets.OrderBy(t => Vector3.Distance(playerModel.position, t.position)).ToList();

        startPos = playerModel.position;
        endPos = poorOrderedTargets[currentTargetAttacking].position;

        dist = Vector3.Distance(startPos, endPos);

        currentTargetAttacking++;
    }

    void MoveToTarget()
    {
        float curLerpTime = (Time.time - startLerpTime) * movementSpeed;
        float journeyFraction = curLerpTime / dist;

        if (startPos != Vector3.zero && endPos != Vector3.zero)
        {
            if (startPos != endPos)
            {
                playerModel.position = Vector3.Lerp(startPos, endPos, journeyFraction);
            }

            if (Vector3.Distance(playerModel.position, endPos) <= closeEnough)
            {
                if (endPos == poorOrderedTargets[poorOrderedTargets.Count - 1].position)
                {
                    poorOrderedTargets[poorOrderedTargets.Count - 1].gameObject.SetActive(false);
                    attacking = false;
                }
                else
                {
                    if ((currentTargetAttacking % 2) > 0)
                    {
                        playerAnim.SetTrigger("Attack");
                    }
                    else
                    {
                        playerAnim.SetTrigger("Attack2");
                    }

                    nextAttack = nextAttackCap;
                    poorOrderedTargets[currentTargetAttacking - 1].gameObject.SetActive(false);
                    SetNewTarget(poorOrderedTargets[currentTargetAttacking].position);
                }
            }
        }
    }
    void SetNewTarget(Vector3 endPos)
    {
        startPos = Vector3.zero;
        this.endPos = Vector3.zero;

        startLerpTime = Time.time;

        startPos = playerModel.position;
        this.endPos = endPos;

        dist = Vector3.Distance(startPos, endPos);

        if (playerModel != null)
        {
            Vector3 dir = endPos - playerModel.position;

            Quaternion lookRotation = Quaternion.LookRotation(dir);

            playerModel.rotation = lookRotation;
        }

        currentTargetAttacking++;
        if (movementSpeed < movementSpeedCap)
        {
            movementSpeed += increaseSpeedBy;
        }
    }

}
