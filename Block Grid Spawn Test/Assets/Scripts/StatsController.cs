using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatsController : MonoBehaviour
{
    #region Global Variable Declaration

    public static StatsController Instance;

    [SerializeField]
    Slider prestigeSlider;
    [SerializeField]
    TextMeshProUGUI totalMoneyText;
    [SerializeField]
    TextMeshProUGUI prestigeNumText;
    [SerializeField]
    bool showing = false;

    #endregion

    void Awake () 
	{
		if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
	}
	void Start () 
	{
		if(prestigeSlider != null)
        {
            prestigeSlider.maxValue = (int)TapTapScript.Instance.upgradeIncomePrice;
            prestigeSlider.value = (int)TapTapScript.Instance.totalMoney;
        }
	}

    private void Update()
    {
        if (this.gameObject.activeInHierarchy)
        {
            UpdatePrestigeValues();
        }
    }

    public void UpdatePrestigeValues () 
	{
		if(prestigeSlider != null)
        {
            prestigeSlider.maxValue = (int)TapTapScript.Instance.upgradeIncomePrice;
            prestigeSlider.value = (int)TapTapScript.Instance.totalMoney;
        }
        if(totalMoneyText != null)
        {
            totalMoneyText.text = string.Format("{0:C}", TapTapScript.Instance.totalMoney);
        }
        if(prestigeNumText != null)
        {
            prestigeNumText.text = string.Format("{0:C}", TapTapScript.Instance.upgradeIncomePrice);
        }
	}

}
