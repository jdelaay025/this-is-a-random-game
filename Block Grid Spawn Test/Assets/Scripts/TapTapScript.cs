using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
public class TapTapScript : MonoBehaviour
{
    #region Global Variable Declaration

    public static TapTapScript Instance;

    public List<string> quantityGroups;
    public bool buyingMax = false;
    public bool collect = false;
    public GameObject managerButton;
    public GameObject axeButton;
    public DateTime gameStartingTime;

    [Header("Public Saved Items")]

    public int CurrentLevelCost = 0;
    public int currentValueIncrease = 0;
    public string currentStreamerGroup = "TimTheTatMan";
    public int currentLevel = 1;
    public double totalMoney = 1.0;
    public bool hasManager = false;
    public int currentNumOfStrikes = 0;
    public int totalClicks = 0;
    public int currentInstructionsThreshold = 0;
    public int numNecessaryForDrop = 30;
    public double currentMoneyIncrease = 1.0;
    public string whichAxe = "plastic";
    public string whichStep = "First";
    public int currentAxePrice = 0;
    public int whichStepNum = 0;
    public double upgradeIncomePrice = 100000;
    public float mineSpeedMultiplier = 1f;
    public bool firstTime = true;
    public int amountToBuy = 1;

    public TextMeshProUGUI timerText;

    public bool readyToPlay = false;
    public bool dontSave = false;

    [SerializeField]
    float durationOfReturnMessage = 1.5f;
    [SerializeField]
    float amp = 0.2f;
    [SerializeField]
    float dur = 1f;

    [SerializeField]
    float countingValue = 0f;
    [SerializeField]
    int secondsNeededToStartAd = 30;

    #region UI Objects

    [SerializeField]
    GameObject totalMoneyObj;
    [SerializeField]
    GameObject currentLevelTextObj;
    [SerializeField]
    GameObject amountToButTextObj;
    [SerializeField]
    GameObject currentPayoutAmountObj;
    [SerializeField]
    Button buyMoreValueButton;
    [SerializeField]
    GameObject upgradeIncomeButton;
    [SerializeField]
    GameObject instructionsTextObj;
    [SerializeField]
    GameObject returningMessageObjs;
    [SerializeField]
    GameObject ContinuePanel;
    [SerializeField]
    Slider markerSlider;
    [SerializeField]
    GameObject TestLootButton;
    [SerializeField]
    Transform playerTransform;

    TextMeshProUGUI currentLevelText;
    TextMeshProUGUI amountToBuyText;
    TextMeshProUGUI currentlyBuyingText;
    TextMeshProUGUI currentPriceOfUpgradeText;
    TextMeshProUGUI totalMoneyText;
    TextMeshProUGUI currentPayoutAmountText;
    TextMeshProUGUI currentAxeButtonText;
    TextMeshProUGUI upgradeIncomeButtonText;
    TextMeshProUGUI instructionsText;

    [SerializeField]
    TextMeshProUGUI returningMessage;
    [SerializeField]
    GameObject collectButton;
    [SerializeField]
    GameObject collectDoubleButton;

    CanvasFloating instructCanvasScript;

    Color buttonNormal = new Color(96f, 96f, 96f, 255f);
    Color buttonHighlighted = new Color(80f, 255f, 47f, 255f);
    Color buttonPressed = new Color(0f, 221f, 231f, 255f);
    Color buttonDisabled = new Color(105f, 105f, 105f, 128f);

    string timeAwayMessage = string.Empty;

    #endregion

    [SerializeField]
    int numberOfMineLevels = 500;
    [SerializeField]
    float maxCountingValue = 1f;
    [SerializeField]
    float startingSpeedMultiplier = 1f;
    [SerializeField]
    double currentCount = 1.0;
    [SerializeField]
    double currentPriceOfUpgrade = 1.0;
    [SerializeField]
    double currentIncreaseMultiplier = 1.0;

    [SerializeField]
    List<double> costPerBuys;
    [SerializeField]
    List<double> valueIncreases;
    
    double costForDisplay = 0.0;

    [SerializeField]
    int managerPrice = 10000;

    [SerializeField]
    Animator anim;

    [SerializeField]
    Transform lootSpawn;
    [SerializeField]
    GameObject lootPrefab;

    [SerializeField]
    List<GameObject> goldDrops;
    [SerializeField]
    List<GameObject> goldDropsMedium;
    [SerializeField]
    int numOfMediumGolds = 3;
    [SerializeField]
    List<GameObject> goldDropsLarge;
    [SerializeField]
    int numOfLargeGolds = 7;
    [SerializeField]
    List<GameObject> goldDropsGiant;
    [SerializeField]
    int numOfGiantGolds = 2;

    [SerializeField]
    float dropSpeed = 10f;

    [SerializeField]
    List<Transform> instructionPositions;
    [SerializeField]
    List<GameObject> testLoot;
    [SerializeField]
    GameObject gamepalyCanvas;

    public DateTime lastEndingTime;

    GameSaveLoadWorker saveAndLoadObj;

    double amountGatheredWhileIdle;

    [SerializeField]
    List<double> payouts;
    [SerializeField]
    float payoutTimerIncrement = 10f;
    float payoutTimer = 10f;
    double totalPerSegmentOfTime = 0;

    bool hasDoctor = false;
    bool hitMaxLevel = false;

    string whichDoctor = string.Empty;

    #endregion

    public static TapTapScript GetInstance()
    {
        if (Instance != null)
        {
            return Instance;
        }
        else
        {
            return new TapTapScript();
        }
    }

    void OnEnable()
    {
        if (readyToPlay)
        {
            if (saveAndLoadObj != null)
            {
                saveAndLoadObj.LoadGame();
            }
        }
    }
    void OnDisable()
    {
        if (saveAndLoadObj != null)
        {
            saveAndLoadObj.SaveGame();
        }
    }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }

        if(gamepalyCanvas != null)
        {
            gamepalyCanvas.SetActive(true);
        }

        readyToPlay = false;

        saveAndLoadObj = GetComponent<GameSaveLoadWorker>();

        #region Setting Quantity Group



        #endregion

        LoadCost();

        LoadValues();

        #region Setting up the prefab pool List
        
        for (int i = 0; i < numOfMediumGolds; i++)
        {
            GameObject mediumGold = (GameObject)Instantiate(goldDrops[0], lootSpawn.position, lootSpawn.rotation);

            mediumGold.SetActive(false);
            goldDropsMedium.Add(mediumGold);
        }
        for (int i = 0; i < numOfLargeGolds; i++)
        {
            GameObject largeGold = (GameObject)Instantiate(goldDrops[1], lootSpawn.position, lootSpawn.rotation);

            largeGold.SetActive(false);
            goldDropsLarge.Add(largeGold);
        }
        for (int i = 0; i < numOfGiantGolds; i++)
        {
            GameObject giantGold = (GameObject)Instantiate(goldDrops[2], lootSpawn.position, lootSpawn.rotation);

            giantGold.SetActive(false);
            goldDropsGiant.Add(giantGold);
        }

        payouts = new List<double>();

        #endregion
    }
    void Start()
    {
        #region Getting access to the textmeshpros

        if (markerSlider != null)
        {
            markerSlider.maxValue = maxCountingValue;
        }
        if (currentLevelTextObj != null)
        {
            currentLevelText = currentLevelTextObj.GetComponent<TextMeshProUGUI>();
        }
        if (amountToButTextObj != null)
        {
            amountToBuyText = amountToButTextObj.GetComponent<TextMeshProUGUI>();
        }
        if (totalMoneyObj != null)
        {
            totalMoneyText = totalMoneyObj.GetComponent<TextMeshProUGUI>();
        }
        if (currentPayoutAmountObj != null)
        {
            currentPayoutAmountText = currentPayoutAmountObj.GetComponent<TextMeshProUGUI>();
        }
        if (axeButton != null)
        {
            currentAxeButtonText = axeButton.GetComponentInChildren<TextMeshProUGUI>();
        }
        if (upgradeIncomeButton != null)
        {
            upgradeIncomeButtonText = upgradeIncomeButton.GetComponentInChildren<TextMeshProUGUI>();
        }
        if (instructionsTextObj != null)
        {
            instructionsText = instructionsTextObj.GetComponentInChildren<TextMeshProUGUI>();
            instructCanvasScript = instructionsTextObj.GetComponent<CanvasFloating>();
        }

        currentlyBuyingText = GameObject.Find("Buy Text").GetComponent<TextMeshProUGUI>();
        currentPriceOfUpgradeText = GameObject.Find("Cost").GetComponent<TextMeshProUGUI>();

        #endregion

        currentPriceOfUpgrade = 1;

        CalculatePrice();

        FindNextInstruction();

        readyToPlay = true;
        dontSave = false;

        OnEnable();

        SetAxeType();

        if (firstTime)
        {
            firstTime = false;
            gameStartingTime = DateTime.Now;
            mineSpeedMultiplier = startingSpeedMultiplier;
        }

        SetUpReturnMessage();
    }
    void Update()
    {
        // Debug.Log(countingValue);

        #region setting timer text

        float timerSeconds = 0.0f;
        float timerMinutes = 0.0f;
        timerSeconds = Time.time;
        timerMinutes = Time.time;
        string minutes = Mathf.Floor(timerMinutes / 60).ToString("00");
        string seconds = Mathf.Floor(timerSeconds % 60).ToString("00");

        if (timerText != null)
        {
            timerText.text = minutes + ":" + seconds;
        }

        #endregion

        #region Tracking how much payout money per set amount of time commented out

        //if (payoutTimer > 0)
        //{
        //    payoutTimer -= Time.deltaTime;
        //}
        //else
        //{
        //    payouts.Add(totalPerSegmentOfTime);
        //    totalPerSegmentOfTime = 0;
        //    payoutTimer = payoutTimerIncrement;
        //}

        #endregion

        if (totalMoneyText != null)
        {
            totalMoneyText.text = string.Format("{0:C}", totalMoney);
        }
        if (Input.GetButtonDown("Jump"))
        {
            collect = true;
            if (anim != null)
            {
                anim.SetBool("Swing Axe", collect);
            }
        }
        if (markerSlider != null)
        {
            if (mineSpeedMultiplier < 50)
            {
                markerSlider.value = (float)countingValue;
            }
            else
            {
                markerSlider.value = markerSlider.maxValue;
            }

            if (collect)
            {
                if (countingValue < maxCountingValue)
                {
                    countingValue += Time.deltaTime * mineSpeedMultiplier;
                }
                else
                {
                    PayOut();
                }
            }
        }
        if (currentLevelText != null)
        {
            currentLevelText.text = currentLevel.ToString();
        }
        if (currentPriceOfUpgradeText != null)
        {
            currentPriceOfUpgradeText.text = string.Format("{0:C}", currentPriceOfUpgrade);

            if (buyingMax)
            {
                amountToBuy = CalculateMax();

                currentlyBuyingText.text = "Buy " + amountToBuy;
            }
            else
            {
                currentlyBuyingText.text = "Buy " + amountToBuy;
            }
        }
        if (currentPayoutAmountText != null)
        {
            currentPayoutAmountText.text = string.Format("{0:C}", currentMoneyIncrease);
        }

        if (TestLootButton != null)
        {
            if (totalMoney > 10)
            {
                TestLootButton.SetActive(false);

                TestLootButton = null;
            }
        }
    }
    
    public void SetUpReturnMessage()
    {
        #region Calculate Money Gathered Since Last Play Session
        
        if (hasManager)
        {
            collect = true;
            if (anim != null)
            {
                anim.SetBool("Swing Axe", collect);
            }

            if (lastEndingTime != null)
            {
                TimeSpan timeSinceLastPlayed = (DateTime.Now - lastEndingTime);

                amountGatheredWhileIdle = (timeSinceLastPlayed.TotalSeconds * (currentMoneyIncrease * (mineSpeedMultiplier / 4)));

                if (timeSinceLastPlayed.TotalSeconds < secondsNeededToStartAd)
                {
                    GetGatheredMoney();
                }
                else
                {
                    if (collectButton != null)
                    {
                        collectButton.SetActive(true);
                    }
                    if (collectDoubleButton != null)
                    {
                        collectDoubleButton.SetActive(true);
                    }

                    if (returningMessageObjs != null)
                    {
                        returningMessageObjs.SetActive(true);

                        timeAwayMessage = "While you were away, you got:" + Environment.NewLine + Environment.NewLine +
                                          string.Format("{0:C}!", amountGatheredWhileIdle) + Environment.NewLine + Environment.NewLine +
                                          "Would you like to Double it!!!";
                        if (returningMessage != null)
                        {
                            returningMessage.text = timeAwayMessage;
                        }
                    }
                }
            }
        }
        else
        {
            if (returningMessageObjs != null)
            {
                returningMessageObjs.SetActive(false);
            }
        }

        #endregion
    }

    void PayOut()
    {
        totalMoney += currentMoneyIncrease;
        
        // totalPerSegmentOfTime += currentMoneyIncrease; - this is for when I'm tracking the amount of payout per set amount of time

        countingValue = 0;

        if (currentNumOfStrikes < numNecessaryForDrop)
        {
            ClickGold();
        }

        if (hasManager)
        {
            collect = true;
        }
        else
        {
            collect = false;
            if (anim != null)
            {
                anim.SetBool("Swing Axe", collect);
            }
        }

        if (buyingMax)
        {
            amountToBuy = CalculateMax();
        }

        CalculatePrice();

        SetButtons();
    }

    void ClickGold()
    {
        currentNumOfStrikes++;
        totalClicks++;

        if (totalClicks >= currentInstructionsThreshold)
        {
            FindNextInstruction();

            if (ContinuePanel != null)
            {
                // StopTime();
            }
        }
    }

    public void SetButtons()
    {
        #region Set manager button

        if (managerButton != null)
        {
            if (managerButton.activeSelf == false
                && hasManager == false)
            {
                if (totalMoney >= managerPrice)
                {
                    managerButton.SetActive(true);
                }
                else
                {
                    managerButton.SetActive(false);
                }
            }
            else
            {
                if (!hasManager)
                {
                    if (totalMoney >= managerPrice)
                    {
                        managerButton.SetActive(true);
                    }
                    else
                    {
                        managerButton.SetActive(false);
                    }
                }
                else
                {
                    managerButton.SetActive(false);
                }
            }
        }

        #endregion

        #region Set Axe Button

        if (axeButton != null)
        {
            if (axeButton.activeSelf == false)
            {
                if (totalMoney >= currentAxePrice && whichAxe != "obsidian")
                {
                    axeButton.SetActive(true);
                }
                else
                {
                    axeButton.SetActive(false);
                }
            }
            else
            {
                if (totalMoney >= currentAxePrice && whichAxe != "obsidian")
                {
                    axeButton.SetActive(true);
                }
                else
                {
                    axeButton.SetActive(false);
                }
            }
        }

        #endregion

        #region Set Upgrade Button

        if (upgradeIncomeButton != null)
        {
            if (upgradeIncomeButton.activeSelf == false)
            {
                if (totalMoney >= upgradeIncomePrice)
                {
                    upgradeIncomeButton.SetActive(true);
                }
                else
                {
                    upgradeIncomeButton.SetActive(false);
                }
            }
            else
            {
                if (totalMoney >= upgradeIncomePrice)
                {
                    upgradeIncomeButton.SetActive(true);
                }
                else
                {
                    upgradeIncomeButton.SetActive(false);
                }
            }
        }

        #endregion
    }
    
    public int CalculateMax()
    {
        int tempamount = 0;
        if (costPerBuys.Count > CurrentLevelCost)
        {
            double tempvalue = costPerBuys[CurrentLevelCost];
            if (totalMoney < tempvalue)
            {
                return 0;
            }
            else
            {
                tempamount++;

                for (int i = (CurrentLevelCost + 1); i < (costPerBuys.Count + CurrentLevelCost); i++)
                {
                    if (i < costPerBuys.Count)
                    {
                        if (totalMoney >= (tempvalue + costPerBuys[i]))
                        {
                            tempamount++;

                            tempvalue += costPerBuys[i];
                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }
                }

                return tempamount;
            }
        }
        else
        {
            return tempamount;
        }
    }

    public void CalculatePrice()
    {
        costForDisplay = 0;

        if ((costPerBuys.Count - CurrentLevelCost) >= amountToBuy)
        {
            for (int i = CurrentLevelCost; i < (amountToBuy + CurrentLevelCost); i++)
            {
                costForDisplay += costPerBuys[i];
            }
        }
        else
        {
            amountToBuy = (costPerBuys.Count - CurrentLevelCost);

            for (int i = CurrentLevelCost; i < (amountToBuy + CurrentLevelCost); i++)
            {
                costForDisplay += costPerBuys[i];
            }
        }

        currentPriceOfUpgrade = costForDisplay;

        if (totalMoney >= costForDisplay)
        {
            if (buyMoreValueButton != null)
            {
                buyMoreValueButton.enabled = true;
                currentlyBuyingText.color = Color.white;
                currentPriceOfUpgradeText.color = Color.white;
            }
        }
        else
        {
            if (buyMoreValueButton != null)
            {
                buyMoreValueButton.enabled = false;
                currentlyBuyingText.color = Color.black;
                currentPriceOfUpgradeText.color = Color.black;
            }
        }
    }

    public void InitiateValueIncrease()
    {
        totalMoney -= currentPriceOfUpgrade;

        currentMoneyIncrease += (valueIncreases[currentValueIncrease] * amountToBuy);

        if (costPerBuys.Count >= amountToBuy)
        {
            currentLevel += amountToBuy;

            CurrentLevelCost += amountToBuy;
        }

        if (CurrentLevelCost >= costPerBuys.Count)
        {
            hitMaxLevel = true;
        }
        else
        {
            hitMaxLevel = false;
        }

        if (buyingMax)
        {
            amountToBuy = CalculateMax();
        }
        
        CalculatePrice();

        EventSystem.current.SetSelectedGameObject(null);
    }

    public void UpgradePayout()
    {
        mineSpeedMultiplier *= 2;

        if (mineSpeedMultiplier > 100)
        {
            mineSpeedMultiplier = 100;
        }

        upgradeIncomePrice *= 5;

        hasManager = false;

        currentValueIncrease++;

        CurrentLevelCost = 0;

        currentLevel = 1;

        LoadCost();

        amountToBuy = 1;

        CalculatePrice();

        currentMoneyIncrease = 1;

        totalMoney = 0;
    }

    public void SetAmountToBuyText()
    {
        if (amountToBuyText != null && currentlyBuyingText != null)
        {
            switch (amountToBuyText.text.ToLower())
            {
                case "buy 1":
                    buyingMax = false;
                    amountToBuyText.text = "Buy 10";
                    amountToBuy = 10;
                    currentlyBuyingText.text = "Buy 10";
                    break;
                case "buy 10":
                    buyingMax = false;
                    amountToBuyText.text = "Buy 100";
                    amountToBuy = 100;
                    currentlyBuyingText.text = "Buy 100";
                    break;
                case "buy 100":
                    buyingMax = true;
                    amountToBuyText.text = "Buy Max";
                    amountToBuy = CalculateMax();
                    currentlyBuyingText.text = "Buy " + amountToBuy;
                    break;
                case "buy max":
                    buyingMax = false;
                    amountToBuyText.text = "Buy 1";
                    amountToBuy = 1;
                    currentlyBuyingText.text = "Buy 1";
                    break;
                default:
                    amountToBuyText.text = "Buy 1";
                    amountToBuy = 1;
                    currentlyBuyingText.text = "Buy 1";
                    break;
            }

            CalculatePrice();

            EventSystem.current.SetSelectedGameObject(null);
        }
    }

    public void HireManager()
    {
        hasManager = true;

        totalMoney -= managerPrice;

        collect = true;
        if (anim != null)
        {
            anim.SetBool("Swing Axe", collect);
        }

        SetButtons();
    }

    public void SetAxeType()
    {
        if (totalMoney >= currentAxePrice ||
            whichAxe == "obsidian")
        {
            switch (whichAxe)
            {
                case "none":
                    numNecessaryForDrop = 30;
                    currentAxePrice = 2000;
                    whichAxe = "plastic";
                    currentAxeButtonText.text = "Buy" + Environment.NewLine + "Wooden" + Environment.NewLine + "Axe";
                    mineSpeedMultiplier = (mineSpeedMultiplier * 1f);
                    break;
                case "plastic":
                    numNecessaryForDrop = 20;
                    totalMoney -= currentAxePrice;
                    currentAxePrice = 7000;
                    whichAxe = "wooden";
                    mineSpeedMultiplier = (mineSpeedMultiplier * 1f);
                    currentAxeButtonText.text = "Buy" + Environment.NewLine + "Stone" + Environment.NewLine + "Axe";
                    break;
                case "wooden":
                    numNecessaryForDrop = 15;
                    totalMoney -= currentAxePrice;
                    currentAxePrice = 25000;
                    whichAxe = "stone";
                    mineSpeedMultiplier = (mineSpeedMultiplier * 1.5f);
                    currentAxeButtonText.text = "Buy" + Environment.NewLine + "Metal" + Environment.NewLine + "Axe";
                    break;
                case "stone":
                    numNecessaryForDrop = 7;
                    totalMoney -= currentAxePrice;
                    currentAxePrice = 500000;
                    whichAxe = "metal";
                    mineSpeedMultiplier = (mineSpeedMultiplier * 5f);
                    currentAxeButtonText.text = "Buy" + Environment.NewLine + "Obsidian" + Environment.NewLine + "Axe";
                    break;
                case "metal":
                    numNecessaryForDrop = 1;
                    totalMoney -= currentAxePrice;
                    currentAxePrice = 2000000000;
                    whichAxe = "obsidian";
                    mineSpeedMultiplier = (mineSpeedMultiplier * 10f);
                    break;
                case "obsidian":
                    numNecessaryForDrop = 1;
                    whichAxe = "obsidian";
                    mineSpeedMultiplier = (mineSpeedMultiplier * 10f);
                    break;
            }

            if (mineSpeedMultiplier > 100)
            {
                mineSpeedMultiplier = 100;
            }
        }
        else
        {
            // Debug.Log("Not ready to buy yet. Current Money : " + totalMoney + " : " + whichAxe + " axe cost : " + currentAxePrice);
        }
    }

    public void DropLoot()
    {
        ShakeCamera.InstanceSM1.ShakeSM1(amp,dur);
        switch (whichAxe)
        {
            case "plastic":
                break;
            case "wooden":
                DropSpecificLootType(goldDropsMedium);
                break;
            case "stone":
                DropSpecificLootType(goldDropsLarge);
                break;
            case "metal":
                DropSpecificLootType(goldDropsLarge);
                break;
            case "obsidian":
                DropSpecificLootType(goldDropsLarge);
                break;
            default:
                break;
        }
    }

    void DropSpecificLootType(List<GameObject> lootList)
    {
        for (int i = 0; i < lootList.Count; i++)
        {
            GameObject loot = lootList[i];
            if (!loot.activeInHierarchy)
            {
                loot.transform.position = lootSpawn.position;
                loot.SetActive(true);
                Rigidbody rb = loot.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    Vector3 force = new Vector3(1f, 1f, 0f);
                    rb.AddForce(force * dropSpeed, ForceMode.Impulse);
                }
                break;
            }
        }

        currentNumOfStrikes = 0;
    }

    // (speech on TODO list)
    /// <summary>
    /// This function sets the tutorial text and plays the tutorial speech
    /// </summary>
    public void FindNextInstruction()
    {
        if (currentInstructionsThreshold <= 30)
        {
            Vector3 instructionsPos = Vector3.zero;
            string instrText = string.Empty;

            #region Switch based on which instructions step you're on

            switch (whichStep)
            {
                case "First":
                    if (instructionPositions.Count > whichStepNum)
                    {
                        instructionsPos = instructionPositions[whichStepNum].position;
                    }
                    instrText = "Repeatedly Click" + Environment.NewLine +
                                "on Gold";
                    currentInstructionsThreshold = 5;
                    whichStepNum++;
                    whichStep = "Second";
                    break;
                case "Second":
                    if (instructionPositions.Count > whichStepNum)
                    {
                        instructionsPos = instructionPositions[whichStepNum].position;
                    }
                    instrText = "In the tatmanarmy Our main focus " + Environment.NewLine +
                                "is to get as much money as possible" + Environment.NewLine +
                                "so we can defend our base";
                    currentInstructionsThreshold = 10;
                    whichStep = "Third";
                    break;
                case "Third":
                    if (instructionPositions.Count > whichStepNum)
                    {
                        instructionsPos = instructionPositions[whichStepNum].position;
                    }
                    instrText = "Eventually, you will want to" + Environment.NewLine +
                                 "Hire a manager to take over mining" + Environment.NewLine +
                                 "the gold";
                    currentInstructionsThreshold = 15;
                    whichStep = "Forth";
                    break;
                case "Forth":
                    if (instructionPositions.Count > whichStepNum)
                    {
                        instructionsPos = instructionPositions[whichStepNum].position;
                    }
                    instrText = "Gold can also be applied to purchasing" + Environment.NewLine +
                                "training equipement and fortifications";
                    currentInstructionsThreshold = 20;
                    whichStep = "Last";
                    break;
                case "Last":
                    if (instructionPositions.Count > whichStepNum)
                    {
                        instructionsPos = instructionPositions[whichStepNum].position;
                    }
                    instrText = "Keep Getting Money" + Environment.NewLine +
                                currentStreamerGroup + Environment.NewLine +
                                "Needs The Support!!";
                    currentInstructionsThreshold = 29;
                    whichStep = "Close";
                    break;
                case "Close":
                    if (instructionPositions.Count > whichStepNum)
                    {
                        instructionsPos = instructionPositions[whichStepNum].position;
                    }
                    instrText = "";
                    break;
            }

            #endregion

            if (instructionsTextObj != null)
            {
                if (instructCanvasScript != null)
                {
                    instructCanvasScript.initialPosition = instructionsPos;
                }
                if (instructionsText != null)
                {
                    instructionsText.text = instrText;
                }
            }
            if (ContinuePanel != null)
            {
                // ContinuePanel.SetActive(true);
            }
        }
    }

    void LoadCost()
    {
        costPerBuys = new List<double>();

        double tempCostPerBuy = 1.0;

        for (int i = 0; i < numberOfMineLevels; i++)
        {
            if (i <= 0)
            {
                costPerBuys.Add(1);
            }
            else
            {
                costPerBuys.Add(costPerBuys[i - 1] + (tempCostPerBuy *= currentIncreaseMultiplier));

                #region Setting Cost to Buy

                switch (i)
                {
                    case 25:
                        currentIncreaseMultiplier = 2.0;
                        break;
                    case 50:
                        currentIncreaseMultiplier = 3.0;
                        break;
                    case 75:
                        currentIncreaseMultiplier = 3.0;
                        break;
                    case 100:
                        currentIncreaseMultiplier = 4.0;
                        break;
                    case 150:
                        currentIncreaseMultiplier = 4.0;
                        break;
                    case 200:
                        currentIncreaseMultiplier = 4.0;
                        break;
                    case 220:
                        currentIncreaseMultiplier = 5.0;
                        break;
                    case 225:
                        currentIncreaseMultiplier = 5.0;
                        break;
                    case 250:
                        currentIncreaseMultiplier = 5.0;
                        break;
                    case 300:
                        currentIncreaseMultiplier = 5.0;
                        break;
                    case 325:
                        currentIncreaseMultiplier = 3.0;
                        break;
                    case 350:
                        currentIncreaseMultiplier = 3.0;
                        break;
                    case 375:
                        currentIncreaseMultiplier = 3.0;
                        break;
                    case 400:
                        currentIncreaseMultiplier = 4.0;
                        break;
                    case 420:
                        currentIncreaseMultiplier = 4.0;
                        break;
                    case 450:
                        currentIncreaseMultiplier = 4.0;
                        break;
                    case 475:
                        currentIncreaseMultiplier = 4.0;
                        break;
                    case 500:
                        currentIncreaseMultiplier = 10.0;
                        break;
                    default:
                        currentIncreaseMultiplier = 1.0;
                        break;
                }

                #endregion
            }
        }
    }

    void LoadValues()
    {
        valueIncreases = new List<double>();

        double tempValueIncrease = 1.0;

        valueIncreases.Add(tempValueIncrease);

        for (int i = 1; i < 100; i++)
        {
            #region Setting Increase Values

            switch (i)
            {
                case 28:
                    valueIncreases.Add((valueIncreases[i - 1] + 28));
                    break;
                case 2:
                case 12:
                case 22:
                case 20:
                case 32:
                case 42:
                case 52:
                case 62:
                case 72:
                case 82:
                case 92:
                    valueIncreases.Add((valueIncreases[i - 1] * 2));
                    break;
                case 3:
                case 13:
                case 23:
                case 30:
                case 33:
                case 43:
                case 53:
                case 63:
                case 73:
                case 93:
                    valueIncreases.Add((valueIncreases[i - 1] * 3));
                    break;
                case 24:
                case 34:
                case 40:
                case 44:
                case 54:
                case 64:
                case 74:
                case 84:
                case 94:
                    valueIncreases.Add((valueIncreases[i - 1] + 48));
                    break;
                case 83:
                    valueIncreases.Add((valueIncreases[i - 1] * 4));
                    break;
                case 5:
                case 15:
                case 25:
                case 35:
                case 50:
                case 45:
                case 55:
                case 65:
                case 75:
                case 85:
                case 95:
                    valueIncreases.Add((valueIncreases[i - 1] * 5));
                    break;
                case 29:
                case 39:
                case 90:
                case 49:
                case 59:
                case 69:
                case 79:
                case 89:
                    valueIncreases.Add((valueIncreases[i - 1] + 81));
                    break;
                case 99:
                    valueIncreases.Add((valueIncreases[i - 1] * 10));
                    break;
                default:
                    valueIncreases.Add((valueIncreases[i - 1] + 2));
                    break;
            }

            #endregion
        }
    }

    public void StopTime()
    {
        Time.timeScale = 0;
    }
    public void KeepGoing()
    {
        Time.timeScale = 1;
    }

    public void DropTestLoot()
    {
        for (int i = 0; i < testLoot.Count; i++)
        {
            testLoot[i].SetActive(true);
        }
    }

    public void ResetEverything()
    {
        CurrentLevelCost = 0;
        currentValueIncrease = 0;
        currentLevel = 1;
        totalMoney = 1.0;
        hasManager = false;
        currentNumOfStrikes = 0;
        totalClicks = 0;
        currentInstructionsThreshold = 0;
        numNecessaryForDrop = 30;
        currentMoneyIncrease = 1.0;
        whichAxe = "plastic";
        whichStep = "First";
        whichStepNum = 0;
    }

    public void GetGatheredMoney()
    {
        if (collectButton != null)
        {
            collectButton.SetActive(false);
        }
        if (collectDoubleButton != null)
        {
            collectDoubleButton.SetActive(false);
        }

        totalMoney += amountGatheredWhileIdle;

        Invoke("CloseReturningMessage", durationOfReturnMessage);

        timeAwayMessage = "You got:" + Environment.NewLine + Environment.NewLine +
                                          string.Format("{0:C}!", (amountGatheredWhileIdle));
        if (returningMessage != null)
        {
            returningMessage.text = timeAwayMessage;
        }
    }
    public void GetDoubleGatheredMoney()
    {
        if (collectButton != null)
        {
            collectButton.SetActive(false);
        }
        if (collectDoubleButton != null)
        {
            collectDoubleButton.SetActive(false);
        }

        totalMoney += (amountGatheredWhileIdle * 2);

        Invoke("CloseReturningMessage", durationOfReturnMessage);

        timeAwayMessage = "You got:" + Environment.NewLine + Environment.NewLine +
                                          string.Format("{0:C}!", (amountGatheredWhileIdle * 2));
        if (returningMessage != null)
        {
            returningMessage.text = timeAwayMessage;
        }
    }

    public void CloseReturningMessage()
    {
        if(returningMessageObjs != null)
        {
            returningMessageObjs.SetActive(false);
        }
    }

    public void GetDoctor(string docType)
    {
        hasDoctor = true;

        whichDoctor = docType;

        switch(docType)
        {
            case "dealer":
                break;
            case "genetic":
                break;
            case "mechanical":
                break;
            default:
                break;
        }
    }

}