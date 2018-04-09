using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickMe : MonoBehaviour
{
    #region Global Variable Declaration

    [SerializeField]
    Transform impactSpawnLocation;
    [SerializeField]
    GameObject goldImpactPrefab;
    [SerializeField]
    List<GameObject> goldShards;
    [SerializeField]
    float numOfGoldShards = 20f;

    TapTapScript tapMaster;
    Animator anim;

    int currentImpact = 0;
    int maxImpacts = 20;

    #endregion

    void Awake()
    {
        tapMaster = TapTapScript.GetInstance();
        anim = GetComponent<Animator>();

        goldShards = new List<GameObject>();

        for (int i = 0; i < numOfGoldShards; i++)
        {
            GameObject shard = (GameObject)Instantiate(goldImpactPrefab, impactSpawnLocation.position, impactSpawnLocation.rotation);

            goldShards.Add(shard);
        }
    }
    void OnMouseDown()
    {
        TapTapScript.Instance.collect = true;
        if (anim != null)
        {
            anim.SetBool("Swing Axe", true);
        }
    }

    public void SpawnImpact()
    {
        if (goldImpactPrefab != null)
        {
            goldShards[currentImpact].transform.position = impactSpawnLocation.position;
            goldShards[currentImpact].GetComponent<ParticleSystem>().Play();

            if (++currentImpact >= maxImpacts)
            {
                currentImpact = 0;
            }
        }

        if (TapTapScript.Instance.currentNumOfStrikes >= TapTapScript.Instance.numNecessaryForDrop)
        {
            TapTapScript.Instance.DropLoot();
        }
    }

}
