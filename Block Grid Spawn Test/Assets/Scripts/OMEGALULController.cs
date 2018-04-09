using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OMEGALULController : MonoBehaviour 
{
    #region Global Variable Declaration

    [SerializeField]
    float growthAmount = 1.0125f;
    
    [SerializeField]
    Transform playerOMEGALUL;
    [SerializeField]
    Transform enemyOMEGALUL;

    [SerializeField]
    int difficultyLevel = 0;
    [SerializeField]
    float rateOfGrowth = 0f;

    float nextEnemyGrowth = 0f;

    [SerializeField]
    float timesUp = 0f;
    [SerializeField]
    float timeCap = 15f;

    [SerializeField]
    bool building = false;
    bool fired = false;

    [SerializeField]
    TextMeshProUGUI timerText;
    [SerializeField]
    TextMeshProUGUI countdownText;

    float timeFromStart = 0f;
    float cooldown = 0f;

    [SerializeField]
    Transform MeetingPoint;
    bool clashing = false;

    [SerializeField]
    float distanceFromMeetingPoint = 1f;

    [SerializeField]
    float moveSpeed = 5f;

    bool playerWon = false;
    bool playerMet = false;
    bool enemyMet = false;
    bool combined = false;

    #endregion

    void Awake () 
	{
	}
	void Start () 
	{
        switch (difficultyLevel)
        {
            case 0:
                rateOfGrowth = 0.75f;
                break;
            case 1:
                rateOfGrowth = 0.65f;
                break;
            case 2:
                rateOfGrowth = 0.55f;
                break;
            case 3:
                rateOfGrowth = 0.35f;
                break;
            case 4:
                rateOfGrowth = 0.225f;
                break;
            default:
                rateOfGrowth = 0.1f;
                break;                
        }

        nextEnemyGrowth = rateOfGrowth;
    }
    void Update () 
	{
        #region setting timer text

        float timerSeconds = 0.0f;
        float timerMinutes = 0.0f;
        timerSeconds = Time.time - timeFromStart;
        timerMinutes = Time.time - timeFromStart;
        string minutes = Mathf.Floor(timerMinutes / 60).ToString("00");
        string seconds = Mathf.Floor(timerSeconds % 60).ToString("00");
        
        if (timerText != null)
        {
            timerText.text = minutes + ":" + seconds;
        }

        #endregion

        if (Input.GetButtonDown("Interact") && 
            !building &&
            !fired)
        {
            StartBuilding();
        }

        if (building)
        {
            #region setting cooldown text
            
            if (cooldown > 0)
            {
                cooldown -= Time.deltaTime;
            }

            if (countdownText != null)
            {
                countdownText.text = Mathf.Floor(cooldown).ToString("00");
            }

            #endregion

            if (Time.time >= timesUp)
            {
                StopBuilding();
            }
            else
            {
                if (Input.GetButtonDown("Jump"))
                {
                    if (playerOMEGALUL != null)
                    {
                        Vector3 newScale = (playerOMEGALUL.localScale * growthAmount);

                        playerOMEGALUL.localScale = newScale;
                    }
                }

                if (Time.time >= nextEnemyGrowth)
                {
                    if (enemyOMEGALUL != null)
                    {
                        Vector3 newScale = (enemyOMEGALUL.localScale * growthAmount);

                        enemyOMEGALUL.localScale = newScale;

                        nextEnemyGrowth = Time.time + rateOfGrowth;
                    }
                }
            }
        }
        else
        {
            if(clashing)
            {
                if (Vector3.Distance(playerOMEGALUL.position, MeetingPoint.position) > distanceFromMeetingPoint)
                {
                    playerOMEGALUL.Translate(playerOMEGALUL.forward * Time.deltaTime * moveSpeed, Space.World);
                }
                else
                {
                    playerMet = true;
                }
                if (Vector3.Distance(enemyOMEGALUL.position, MeetingPoint.position) > distanceFromMeetingPoint)
                {
                    enemyOMEGALUL.Translate(enemyOMEGALUL.forward * Time.deltaTime * moveSpeed, Space.World);
                }
                else
                {
                    enemyMet = true;
                }

                if (!combined)
                {
                    if (playerWon)
                    {
                        if (playerMet && enemyMet)
                        {
                            playerOMEGALUL.localScale = (playerOMEGALUL.localScale * enemyOMEGALUL.localScale.x);
                            combined = true;
                            enemyOMEGALUL.gameObject.SetActive(false);
                        }
                    }
                    else
                    {
                        if (playerMet && enemyMet)
                        {
                            enemyOMEGALUL.localScale = (enemyOMEGALUL.localScale * playerOMEGALUL.localScale.x);
                            combined = true;
                            playerOMEGALUL.gameObject.SetActive(false);
                        }
                    }
                }
            }
        }

    }

    void StartBuilding()
    {
        timeFromStart = Time.time;

        timesUp = Time.time + timeCap;
        cooldown = timeCap;

        building = true;
    }

    void StopBuilding()
    {
        fired = true;
        building = false;

        timerText.text = "";
        countdownText.text = "Time's Up!!";

        Invoke("FireBombs", 3);
    }
    void FireBombs()
    {
        if(playerOMEGALUL != null &&
            enemyOMEGALUL != null)
        {
            clashing = true;

            if(playerOMEGALUL.localScale.x > enemyOMEGALUL.localScale.x)
            {
                countdownText.text = "Player Wins!!!";
                playerWon = true;
            }
            else
            {
                if(playerOMEGALUL.localScale.x < enemyOMEGALUL.localScale.x)
                {
                    countdownText.text = "Enemy Wins!!!";
                    playerWon = false;
                }
                else
                {
                    countdownText.text = "Somehow we tied!!";
                }
            }
        }
    }

}
