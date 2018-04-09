using UnityEngine;
using UnityEngine.UI;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Collections.Generic;

public enum CreepState
{
    Idle = 0,
    Searching = 1,
    Running = 2,
    Jump = 3,
    Fighting = 4,
    Dead = 5
}

public class GameMasterObject : MonoBehaviour
{
    #region Global Variable Declaration

    #region static members

    public static GameMasterObject Instance;
    public static List<Transform> enemies;
    public static List<Transform> heroes;
    public static List<Transform> heroCreeps;
    public static List<Transform> enemyCreeps;
    public static List<Transform> dragonsMonsters;
    public static List<Transform> defenseItems;
    public static List<GameObject> impacts;
    public static List<Transform> towerMounts;
    public static Transform[] tempTowers = new Transform[8];

    public static List<Transform> foundations;
    public static List<Transform> walls;
    public static List<Transform> floors;
    public static List<Transform> doors;
    public static List<Transform> roofs;

    public static List<Transform> capPointPositions;

    public static GameObject playerUse;
    public static Camera camInUse;

    public static bool allPointsCapped = false;

    public static Transform CapturePoint;
    public static int heroPoints = 0;
    public static int enemyPoints = 0;
    public static bool changePostion = false;

    public static List<Transform> enemySpawnPoints;

    public static Slider heroesSlider;
    public static Slider enemiesSlider;

    public static Inventory inventory;

    public static Transform heroCore;
    public static Transform enemyCore;

    public static int playerLevel = 0;
    public static int enemyLevel = 0;

    public static bool isPlayerActive = true;

    public static bool isTPSState = false;
    public static bool isTowerState = false;
    public static bool isFreeMoveCamState = false;

    public static bool setPlayerPosition = false;

    #endregion

    #region nonstatic public members
    public GameObject inventoryCanvas;

    [Header("Players and Cameras")]
    public GameObject dannyMoba;
    public GameObject mobaCam;
    public GameObject dannyMobaPanel;
    public GameObject dannyTPS;
    public GameObject tpsCam;
    public GameObject towerCam;
    public GameObject topDownCam;

    public bool isTPS = false;
    public bool isTowerCam = false;
    public bool isTopDown = false;

    public bool canBuild = false;
    public bool needImpacts = false;

    public GameObject impactPrefab;
    public float pooledImpactsAmount = 100;

    public Text timerText;
    public Text timerText2;

    public Text PlayerPoints;
    public Text EnemyPoints;

    public Text PlayerLevels;
    public Text EnemyLevels;

    public GameObject ridicule;
    public GameObject circleOne;
    public GameObject bulletsLeft;

    public bool lockCursor = false;

    #endregion

    #region nonstatic private members

    bool tps = false;
    bool moba = false;
    bool towerCamera = false;
    bool topDownCamera = false;

    #endregion

    #endregion

    public static GameMasterObject GetInstance()
    {
        return Instance;
    }

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        enemies = new List<Transform>();
        heroes = new List<Transform>();
        defenseItems = new List<Transform>();
        capPointPositions = new List<Transform>();
        enemySpawnPoints = new List<Transform>();
        heroCreeps = new List<Transform>();
        enemyCreeps = new List<Transform>();
        dragonsMonsters = new List<Transform>();
        towerMounts = new List<Transform>();

        if (needImpacts)
        {
            impacts = new List<GameObject>();
        }

        if (canBuild)
        {
            foundations = new List<Transform>();
            walls = new List<Transform>();
            floors = new List<Transform>();
            doors = new List<Transform>();
            roofs = new List<Transform>();
        }

        string todaysDate = DateTime.Now.ToString("MM-dd-yyyy");

        //		Debug.Log (inventory + " awake");

        //DataTable table = RetrieveData();

        //for (int i = 0; i < table.Rows.Count; i++)
        //{
        //    string username = table.Rows[i]["Username"].ToString();
        //    string linkTitle = table.Rows[i]["Link Title"].ToString();
        //    string actLink = table.Rows[i]["Actual Link"].ToString();
        //    string model = table.Rows[i]["Model"].ToString();

        //    Debug.Log("Username: " + username + " Title: " + linkTitle + " Link: " + actLink + " Enemy Type: " + model);
        //}
    }
    void Start()
    {
        camInUse = Camera.main;

        #region pooled impact prefabs

        if (needImpacts)
        {
            for (int i = 0; i < pooledImpactsAmount; i++)
            {
                GameObject impact = (GameObject)Instantiate(impactPrefab);
                impact.SetActive(false);
                impacts.Add(impact);
            }
        }

        #endregion

        if (isTowerCam)
        {
            SetToTowerCam();
        }
        else if (!isTowerCam)
        {
            if (isTPS && !isTopDown)
            {
                SetToTPS();
            }
            else if (!isTPS && isTopDown)
            {
                SetToTopDown();
            }
        }

        for (int a = 0; a < tempTowers.Length; a++)
        {
            towerMounts.Add(tempTowers[a]);
        }

        isTPSState = isTPS;
        isTowerState = isTowerCam;
        isFreeMoveCamState = topDownCamera;
    }
    void Update()
    {
        // Debug.Log(playerLevel);
        // Debug.Log (capPointPositions.Count);
        // Debug.Log(dragonsMonsters.Count);

        isTPSState = isTPS;
        isTowerState = isTowerCam;
        isFreeMoveCamState = topDownCamera;

        playerUse = dannyTPS;

        if (tps && !moba && !towerCamera)
        {
            if (Input.GetButtonDown("Chat"))
            {
                lockCursor = !lockCursor;
            }

            if (lockCursor)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                isPlayerActive = true;

                if (inventoryCanvas != null)
                {
                    inventoryCanvas.SetActive(false);
                }
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                isPlayerActive = false;

                if (inventoryCanvas != null)
                {
                    inventoryCanvas.SetActive(true);
                }
            }
        }
        else if (moba && !tps && !towerCamera)
        {
            if (Input.GetButtonDown("Chat"))
            {
                lockCursor = !lockCursor;
            }

            if (lockCursor)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                isPlayerActive = true;

                if (inventoryCanvas != null)
                {
                    inventoryCanvas.SetActive(false);
                }
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                isPlayerActive = false;

                if (inventoryCanvas != null)
                {
                    inventoryCanvas.SetActive(true);
                }
            }
        }
        else if (towerCamera && !moba && !tps)
        {
            if (Input.GetButtonDown("Chat"))
            {
                lockCursor = !lockCursor;
            }

            if (lockCursor)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                isPlayerActive = true;

                if (inventoryCanvas != null)
                {
                    inventoryCanvas.SetActive(false);
                }
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                isPlayerActive = false;

                if (inventoryCanvas != null)
                {
                    inventoryCanvas.SetActive(true);
                }
            }
        }

        if (EnemyLevels != null)
        {
            EnemyLevels.text = enemyLevel.ToString("00");
        }
        if (PlayerLevels != null)
        {
            PlayerLevels.text = playerLevel.ToString("00");
        }

        float timerSeconds = 0.0f;
        float timerMinutes = 0.0f;
        timerSeconds = Time.time;
        timerMinutes = Time.time;
        string minutes = Mathf.Floor(timerMinutes / 60).ToString("00");
        string seconds = Mathf.Floor(timerSeconds % 60).ToString("00");

        if (timerText != null)
        {
            timerText.text = seconds;
        }
        if (timerText2 != null)
        {
            timerText2.text = minutes;
        }

        #region Switch Camera orientation commented out
        //if(Input.GetButtonDown("Switch") && tps)
        //{
        //	SetToMoba ();
        //}
        //else if(Input.GetButtonDown("Switch") && moba)
        //{
        //	SetToTPS ();
        //}
        #endregion

        if (CapturePoint != null)
        {
            if (!CapturePoint.gameObject.activeInHierarchy && !allPointsCapped)
            {
                if (changePostion)
                {
                    CapturePoint.GetComponent<ControlPointScript>().ChangePositions();
                }
            }
            else if (!CapturePoint.gameObject.activeInHierarchy && allPointsCapped)
            {
                //Debug.Log("Game Over");
            }
        }

        if (PlayerPoints != null)
        {
            PlayerPoints.text = heroPoints.ToString();
        }
        if (EnemyPoints != null)
        {
            EnemyPoints.text = enemyPoints.ToString();
        }

        if (setPlayerPosition)
        {
            //dannyTPS.transform.position = 
        }

    }

    public void SetToMoba()
    {
        dannyMoba.transform.position = dannyTPS.transform.position;

        tps = false;
        moba = true;
        towerCamera = false;
        topDownCamera = false;

        dannyMoba.SetActive(true);
        mobaCam.SetActive(true);
        dannyMobaPanel.SetActive(true);

        dannyTPS.SetActive(false);
        tpsCam.SetActive(false);
        if (topDownCam != null)
        {
            towerCam.SetActive(false);
        }
        topDownCam.SetActive(false);

        ridicule.SetActive(false);

        //lockCursor = true;
    }
    public void SetToTPS()
    {
        //if (dannyMoba != null)
        //{
        //    dannyTPS.transform.position = dannyMoba.transform.position/* + new Vector3 (0f, 20f, 0f)*/;
        //}

        tps = true;
        moba = false;
        towerCamera = false;
        topDownCamera = false;

        dannyTPS.SetActive(true);
        tpsCam.SetActive(true);

        if (dannyMoba != null)
        {
            dannyMoba.SetActive(false);
            mobaCam.SetActive(false);
            dannyMobaPanel.SetActive(false);
        }

        towerCam.SetActive(false);
        if (topDownCam != null)
        {
            topDownCam.SetActive(false);
        }
        ridicule.SetActive(true);

        //lockCursor = true;
    }
    public void SetToTowerCam()
    {
        tps = false;
        moba = false;
        towerCamera = true;
        topDownCamera = false;

        dannyTPS.SetActive(false);
        tpsCam.SetActive(false);

        if (dannyMoba != null)
        {
            dannyMoba.SetActive(false);
            mobaCam.SetActive(false);
            dannyMobaPanel.SetActive(false);
        }

        towerCam.SetActive(true);
        if (topDownCam != null)
        {
            topDownCam.SetActive(false);
        }
        ridicule.SetActive(false);

        lockCursor = true;
    }
    public void SetToTopDown()
    {
        tps = false;
        moba = false;
        towerCamera = false;
        topDownCamera = true;

        dannyTPS.SetActive(false);
        tpsCam.SetActive(false);

        if (dannyMoba != null)
        {
            dannyMoba.SetActive(false);
            mobaCam.SetActive(false);
            dannyMobaPanel.SetActive(false);
        }

        towerCam.SetActive(false);
        if (topDownCam != null)
        {
            topDownCam.SetActive(true);
        }
        ridicule.SetActive(false);

        lockCursor = true;
    }

    DataTable RetrieveData()
    {
        #region Sql Statements

        string sql = "select * from Characters";
        string truncateSQL = "truncate table Characters";

        #endregion

        #region Actual Select and Truncate

        SqlConnection conn = new SqlConnection(@"Server= 127.0.0.1,1433;User ID=sa;Password=ispJdelaay025;Database=LinkStorm;");

        SqlDataAdapter sda = new SqlDataAdapter();
        sda.SelectCommand = new SqlCommand();
        sda.SelectCommand.Connection = conn;
        sda.SelectCommand.CommandType = CommandType.Text;
        sda.SelectCommand.CommandText = sql;

        SqlDataAdapter sda2 = new SqlDataAdapter();
        sda2.SelectCommand = new SqlCommand();
        sda2.SelectCommand.Connection = conn;
        sda2.SelectCommand.CommandType = CommandType.Text;
        sda2.SelectCommand.CommandText = truncateSQL;

        DataTable dt = new DataTable();

        try
        {
            conn.Open();

            sda.Fill(dt);

            if (dt.Rows.Count > 0)
            {
                // sda2.SelectCommand.ExecuteNonQuery();
            }
        }
        catch (Exception ex)
        {

        }
        finally
        {
            sda.Dispose();
            sda2.Dispose();

            conn.Close();
            conn.Dispose();
        }

        #endregion

        if (dt.Rows.Count > 0)
        {
            return dt;
        }
        else
        {
            return new DataTable();
        }
    }

}
