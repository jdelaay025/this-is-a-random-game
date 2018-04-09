using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldDrops : MonoBehaviour
{
    #region Global Variable Declaration

    [SerializeField]
    GameObject PickupBurst;

    [SerializeField]
    string typeOfGold = "";
    [SerializeField]
    bool destoryThis = false;

    [SerializeField]
    int deleteTimer = 5;

    float collectionTime = 0f;
    bool pickedUp = false;

    #endregion

    void OnEnable()
    {
        pickedUp = false;

        if (TapTapScript.Instance.hasManager == false)
        {
            DestroyLootKinda(deleteTimer);
        }
        else
        {
            collectionTime = Time.time + 3f;
        }
    }
    void Update()
    {
        if (TapTapScript.Instance.hasManager == true)
        {
            if (Time.time >= collectionTime && pickedUp == false)
            {
                pickedUp = true;

                Pickup();
            }
        }
    }

    void OnMouseEnter()
    {
        if (TapTapScript.Instance.hasManager == false)
        {
            Pickup();
        }
    }

    public void Pickup()
    {
        if (PickupBurst != null)
        {
            Instantiate(PickupBurst, transform.position, transform.rotation);
        }

        switch (typeOfGold)
        {
            case "medium gold":
                TapTapScript.Instance.totalMoney += 200;
                break;
            case "large gold":
                TapTapScript.Instance.totalMoney += 5000;
                break;
            case "Giant":
                TapTapScript.Instance.totalMoney += 150000;
                break;
            case "diamond":
                TapTapScript.Instance.totalMoney += 150000000;
                break;
            default:
                TapTapScript.Instance.totalMoney += 1;
                break;
        }

        if (TapTapScript.Instance.buyingMax)
        {
            TapTapScript.Instance.amountToBuy = TapTapScript.Instance.CalculateMax();
        }

        TapTapScript.Instance.CalculatePrice();

        TapTapScript.Instance.SetButtons();

        this.gameObject.SetActive(false);
    }

    void DestroyLootKinda(int timer)
    {
        Invoke("Deactivate", timer);
    }
    void Deactivate()
    {
        if (this.gameObject.activeInHierarchy)
        {
            this.gameObject.SetActive(false);
        }
    }

}
