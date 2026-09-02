using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StageControl : MonoBehaviour
{
    // Survival mode = 1
    // Defence mode = 2
    [Header("Game mode")]
    public int gamemode;

    [Header("Defense Mode")]
    [SerializeField] private Transform pumpkinPosition;
    [SerializeField] private Transform workbenchPosition;
    [SerializeField] private Transform fuseBoxPosition;

    public PlayerGunControl playerGunControl;
    [Header("Pumpkin")]
    [SerializeField] private GameObject pumpkinGO;

    [Header("Fusebox")]
    [SerializeField] private GameObject groundFogGO;
    private bool fuseboxIsBroken;

    [Header("Spawn Enemy Status")]
    public int spawnedEnemyCounter;

    [Header("Points")]
    public TextMeshProUGUI pointsNumTxt;
    public int points;
    public TextMeshProUGUI addPointsTxt;
    public Vector3 addPointsOffset;
    [Header("Gears")]
    public int gears;
    [Header("Boss")]
    public bool isBossBattle = false;
    public GameObject bossPrefab;
    public Transform bossSpawnPos;
    public GameObject bossDoorPrefab;
    public Transform bossDoorPos;
    public GameObject bossHealthUi;

    [Header("Spawn")]
    public List<Transform> activeSpawnList = new List<Transform>();
    public List<Transform> activeSpawnTargetList = new List<Transform>();

    //Walking zombie stats---------------------
    public float walking_animationSpeed;
    public float walking_walkSpeed;

    //Fast walking zombie stats---------------------
    public float fastWalk_animationSpeed;
    public float fastWalk_walkSpeed;

    // Running zombie stats---------------------
    public float run_animationSpeed;
    public float run_walkSpeed;

    [Header("Enemy")]
    [SerializeField] private List<GameObject> allEnemyList = new List<GameObject>();
    private int enemyNameCounter = 1;

    public float spawnCooldown = 7;
    public float spawnTimer;
    public List<GameObject> zombiePrefab = new List<GameObject>();
    public List<GameObject> spiderPrefab = new List<GameObject>();
    public GameObject spawnEffect;
    public int spawnCurrentListNum; // Add after opening doors
    [Header("EnemySpeed")]
    private float lastZombieMaxSpeed = 1.5f;

    [Header("Skull item")]
    [SerializeField] private List<GameObject> skullPositions = new List<GameObject>();
    [SerializeField] private GameObject skullPrefab;

    [Header("Panels")]
    public GameObject pausePanel;
    public GameObject checkLeavePanel;
    public GameObject mainCanvas;
    public GameObject resultPanel;
    public GameObject gameOverPanel;
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI pausePointsNum;
    [SerializeField] private TextMeshProUGUI pauseGearsNum;
    public GameObject pauseBtn;
    public TextMeshProUGUI waveNumText;

    [Header("Character")]
    [SerializeField] private CharacterDB characterDB;
    [SerializeField] private int currentCharacterId;

    [Header("Character buff")]
    public bool decreaseDoorPrice = false;
    public bool decreaseFuseboxRepairPrice = false;
    public bool increaseFoundGearAmount = false;
    public bool increaseHandgunDamage = false;
    public bool increaseShotgunDamage = false;
    public bool increaseHealth = false;


    [Header("Stage Clear")]
    private bool stageClear;
    public float stageTimer;
    public TextMeshProUGUI completeTimeTxt;
    public TextMeshProUGUI itemsFound;

    [Header("Timer")]
    public float msgPanelTimer;
    public float msgPanelTime;
    [Header("Items")]
    public int itemsAcquiredNum;
    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private Transform[] itemAllPos = new Transform[3];
    [SerializeField] private PotionSpawnControl potionSpawnControl;
    [Header("Key")]
    public bool hasKey1 = false;
    public bool hasKey2 = false;

    [Header("Mission")]
    public TextMeshProUGUI missionText;
    public TextMeshProUGUI mainMenuMissionText;
    [Header("MsgPanel")]
    public MsgPanel_System msgPanel_System;
    [Header("Gun database")]
    [SerializeField] GunDatabase gunDatabase;

    [Header("Waves")]
    [SerializeField] private TextMeshProUGUI currentWaveText;
    [SerializeField] private TextMeshProUGUI zombiesLeftNumText;
    [SerializeField] private GameObject waveTextParent;

    [Header("GameOver Panel")]
    [SerializeField] private GameObject NewHighscore;


    public int INIT_ENEMY_NUM = 3;
    public float nextEnemySpawnTimer = 0f;
    public float nextZombieSpawnCoolTime = 6f;
    public int currentWaveZombiesLeft = 3;
    public int currentWaveZombiesToSpawn = 3;
    public int totalDefeatedEnemies = 0;
    public int totalSpawnedEnemies = 0;
    private int currentWave = 1;                                // Init wave num
    private WAVE_STATUS wave_status;
    public enum WAVE_STATUS
    {
        UPDATE,
        WAIT
    }

    private void Awake()
    {
        UpdateCharacterId(GameManager.instance.characterId);
    }

    void Start()
    {
        Setup();
    }

    private void Setup()
    {
        Time.timeScale = 1;
        // TODO: fix to 0
        // points = 1000;
        UpdateUI();

        playerGunControl = FindObjectOfType<PlayerGunControl>();

        pauseBtn.SetActive(true);
        pausePanel.SetActive(false);
        checkLeavePanel.SetActive(false);
        resultPanel.SetActive(false);
        gameOverPanel.SetActive(false);

        msgPanel_System.OutputMsg("Protect the pumpkin and survive!");

        fuseboxIsBroken = false;
        // Wave values reset
        currentWaveZombiesToSpawn = INIT_ENEMY_NUM;
        currentWaveZombiesLeft = currentWaveZombiesToSpawn;
        wave_status = WAVE_STATUS.UPDATE;
        totalSpawnedEnemies = 0;
        totalDefeatedEnemies = 0;
        currentWaveText.text = currentWave.ToString();
        zombiesLeftNumText.text = currentWaveZombiesLeft.ToString();

        itemsAcquiredNum = 0;
        UpdateMission();
        spawnedEnemyCounter = 0;

        // Spawn enemy after cooldown time
        spawnTimer = 2;

        stageClear = false;
        // Stage time
        stageTimer = 0;
        ResetGunLevels();
    }

    //==============================================
    //-----ResetGunLevels-----
    //Description: Reset all levels of the guns in the database
    //
    //----Parameters-----------------
    // None
    //----Return---------------------
    // None
    //==============================================
    private void ResetGunLevels()
    {
        for (int i = 0; i < gunDatabase.GetDatabaseLength(); i++)
        {
            gunDatabase.GetGun(i).damage_currentLv = 1;
            gunDatabase.GetGun(i).mag_currentLv = 1;
            gunDatabase.GetGun(i).ammoTotal_currentLv = 1;
            gunDatabase.GetGun(i).mag_current_value_InGame = gunDatabase.GetGun(i).mag_lv1_value + (gunDatabase.GetGun(i).mag_currentLv * gunDatabase.GetGun(i).mag_IncreasePerUpgrade);
            gunDatabase.GetGun(i).ammoTotal_current_value_InGame = gunDatabase.GetGun(i).ammoTotal_lv1_value + (gunDatabase.GetGun(i).ammoTotal_currentLv * gunDatabase.GetGun(i).ammoTotal_IncreasePerUpgrade);
            gunDatabase.GetGun(i).mag_current_max_InGame = gunDatabase.GetGun(i).mag_lv1_value + (gunDatabase.GetGun(i).mag_currentLv * gunDatabase.GetGun(i).mag_IncreasePerUpgrade);
            gunDatabase.GetGun(i).ammoTotal_current_max_InGame = gunDatabase.GetGun(i).ammoTotal_lv1_value + (gunDatabase.GetGun(i).ammoTotal_currentLv * gunDatabase.GetGun(i).ammoTotal_IncreasePerUpgrade);
        }
    }
    public int GetCharacterId()
    {
        return currentCharacterId;
    }

    void Update()
    {
        // Stage timer counter
        if (!stageClear)
        {
            stageTimer += Time.deltaTime;
        }
        if (isBossBattle)
        {
            // Custom spawn
        }
        else
        {
            // If nextEnemySpawnTimer is more than 0
            if (nextEnemySpawnTimer > 0)
            {
                nextEnemySpawnTimer -= Time.deltaTime;
            }
            // If zombieSpawn Timer is less or equal to 0
            else
            {
                // If zombies wave num is more than 0
                if (currentWaveZombiesToSpawn > 0)
                {
                    ChooseSpawn();
                    totalSpawnedEnemies += 1;
                    currentWaveZombiesToSpawn -= 1;
                    nextEnemySpawnTimer = nextZombieSpawnCoolTime;
                }
                else if (totalDefeatedEnemies == totalSpawnedEnemies)
                {
                    // Start next wave
                    StartCoroutine(NextWave());

                }
            }
        }
    }
    public void UpdateWaveEnemyNum(int zombieDefeated, string enemyName)
    {
        // Remove the defeated enemy from the list
        GameObject zombieToRemove = allEnemyList.Find(zombie => zombie.name == enemyName);
        allEnemyList.Remove(zombieToRemove);

        totalDefeatedEnemies += zombieDefeated;
        currentWaveZombiesLeft -= zombieDefeated;
        UpdateUI();
        // if only one zombie left in wave
        if (currentWaveZombiesLeft == 1)
        {
            Debug.Log("Last enemy!" + allEnemyList.Count);
            GameObject lastZombie = allEnemyList[0];
            lastZombie.GetComponent<Enemy>().walkSpeed = lastZombieMaxSpeed;
            lastZombie.GetComponent<Enemy>().currentTarget = playerGunControl.gameObject.transform;
        }
    }

    private IEnumerator NextWave()
    {
        // Reset timer
        nextEnemySpawnTimer = nextZombieSpawnCoolTime * 3;              // multiply by 3 for break between waves
        // Reset zombiesLeftNumber
        currentWaveZombiesToSpawn = INIT_ENEMY_NUM + (currentWave * 2);
        currentWaveZombiesLeft = currentWaveZombiesToSpawn;
        // Increase wave number
        currentWave += 1;

        // Add new enemy type at a specific wave
        // TODO : Add spider at wave 3
        if (currentWave == 3)
        {
            Debug.Log("Add spider");
        }

        waveTextParent.GetComponent<Animator>().SetTrigger("next_wave");
        yield return new WaitForSeconds(2.6f);
        // Refill player grenade
        playerGunControl.WaveClear_RefillGrenade();
        // Spawn potion at random position
        potionSpawnControl.RespawnPotion();
        UpdateUI();
    }

    public void UpdateUI()
    {
        pointsNumTxt.text = points.ToString();
        currentWaveText.text = currentWave.ToString();
        zombiesLeftNumText.text = currentWaveZombiesLeft.ToString();
    }

    private void ChooseSpawn()
    {
        int rand = Random.Range(0, activeSpawnList.Count);
        Debug.Log("Rand num is: " + rand);
        Instantiate(spawnEffect, activeSpawnList[rand].position, activeSpawnList[rand].rotation);
        StartCoroutine(SpawnEnemy(rand));
        spawnTimer = spawnCooldown;
    }
    public void BossRoarSpawnZombie()
    {
        // for (int i = 0; i < room5Spawns.Length; i++)
        // {
        //     Instantiate(spawnEffect, activeSpawnList[i].position, activeSpawnList[i].rotation);
        //     StartCoroutine(SpawnEnemy(i));
        // }
    }
    private IEnumerator SpawnEnemy(int pos)
    {
        int enemyType = Random.Range(0, zombiePrefab.Count + spiderPrefab.Count);
        yield return new WaitForSeconds(1.5f);
        GameObject enemyPrefab;
        if (enemyType < zombiePrefab.Count)
        {
            enemyPrefab = zombiePrefab[enemyType];
        }
        else
        {
            enemyPrefab = spiderPrefab[enemyType - zombiePrefab.Count];
        }
        GameObject enemy = Instantiate(enemyPrefab, activeSpawnList[pos].position, activeSpawnList[pos].rotation) as GameObject;
        enemy.name = "Enemy_prefab_" + enemyNameCounter;
        enemyNameCounter += 1;
        ChooseTarget(enemy);
        allEnemyList.Add(enemy);

        //==============================================
        // Update spawned enemy status
        //==============================================
        float speedRandomizer = Random.Range(0.5f, 1.5f);
        if (enemy.GetComponent<Enemy>().zombie)
        {
            // Enemy speed is base speed * current wave * speed randomizer
            enemy.GetComponent<Enemy>().walkSpeed = (0.75f * (1 + (currentWave * 0.1f)) * speedRandomizer);
            // Change enemy max health
            enemy.GetComponent<EnemyHealth>().maxHealth = enemy.GetComponent<EnemyHealth>().zombieBaseHealth * (1 + (currentWave * 0.5f));
            enemy.GetComponent<EnemyHealth>().curHealth = enemy.GetComponent<EnemyHealth>().maxHealth;

        }
        else if (enemy.GetComponent<Enemy>().spider)
        {
            // Enemy speed is base speed * current wave * speed randomizer
            enemy.GetComponent<Enemy>().walkSpeed = (1.15f * (1 + (currentWave * 0.1f)) * speedRandomizer);
            // Change enemy max health
            enemy.GetComponent<EnemyHealth>().maxHealth = enemy.GetComponent<EnemyHealth>().spiderBaseHealth * (1 + (currentWave * 0.5f));
            enemy.GetComponent<EnemyHealth>().curHealth = enemy.GetComponent<EnemyHealth>().maxHealth;
        }
        spawnedEnemyCounter++;
    }

    private void ChooseTarget(GameObject z_go)
    {
        // Check if there is a third target
        int tempRand;
        tempRand = Random.Range(0, 3);
        // Choose target
        if (tempRand == 0)
        {
            z_go.GetComponent<Enemy>().currentTarget = pumpkinPosition;
        }
        // else if (tempRand == 1)
        // {
        //     //Debug.Log("target 2: workbench");
        //     z_go.GetComponent<Enemy>().currentTarget = workbenchPosition;
        // }
        // else if (tempRand == 2)
        // {
        //     //Debug.Log("target 3: fuse box");
        //     z_go.GetComponent<Enemy>().currentTarget = fuseBoxPosition;
        // }
        else
        {
            z_go.GetComponent<Enemy>().currentTarget = playerGunControl.gameObject.transform;
        }
    }
    public void SpawnEnemyTrigger(Transform pos)
    {
        int z_type = Random.Range(0, zombiePrefab.Count);
        Instantiate(zombiePrefab[z_type], pos.position, pos.rotation);
    }

    public void AddPoints(int addingPoints)
    {
        points += addingPoints;
        UpdateUI();
        AddPointsEffect(addingPoints);
    }
    public void AddPointsEffect(int addingPoints)
    {
        addPointsTxt.text = "+ " + addingPoints.ToString();
        addPointsTxt.color = Color.white;
        Instantiate(addPointsTxt, pointsNumTxt.transform.position + addPointsOffset, Quaternion.identity, pointsNumTxt.transform);
    }
    public void MinusPoints(int minusPoints)
    {
        points -= minusPoints;
        UpdateUI();
        MinusPointsEffect(minusPoints);
    }
    public void MinusPointsEffect(int minusPoints)
    {
        addPointsTxt.text = "- " + minusPoints.ToString();
        addPointsTxt.color = Color.red;
        Instantiate(addPointsTxt, pointsNumTxt.transform.position + addPointsOffset, Quaternion.identity, pointsNumTxt.transform);
    }
    public bool OpenDoor(int cost, List<Transform> newSpawn = null, List<Transform> newSpawnTarget = null)
    {
        // Enough points to open door
        if (points >= cost)
        {
            activeSpawnList.AddRange(newSpawn);
            activeSpawnTargetList.AddRange(newSpawnTarget);
            MinusPoints(cost);
            UpdateUI();
            return true;
        }
        // Not enough points
        else
        {
            Debug.Log("Not enough");
            return false;
        }
    }
    public bool OpenCoffin(int cost, List<Transform> newSpawn = null, List<Transform> newSpawnTarget = null)
    {
        // Enough points to open door
        if (points >= cost)
        {
            // activeSpawnList.AddRange(newSpawn);
            // activeSpawnTargetList.AddRange(newSpawnTarget);
            MinusPoints(cost);
            UpdateUI();
            return true;
        }
        // Not enough points
        else
        {
            Debug.Log("Not enough");
            return false;
        }
    }

    public bool WallGunCost(int cost)
    {
        if (cost <= points)
        {
            MinusPoints(cost);
            UpdateUI();
            return true;
        }
        else
        {
            return false;
        }
    }

    public int FindItem()
    {
        itemsAcquiredNum++;
        UpdateMission();
        return itemsAcquiredNum;
    }
    public void UpdateMission()
    {
        switch (itemsAcquiredNum)
        {
            case 0:
                mainMenuMissionText.text = "0/3 skulls found";
                missionText.text = "0/3 skulls found";
                break;
            case 1:
                mainMenuMissionText.text = "1/3 skulls found";
                missionText.text = "1/3 skulls found";
                break;
            case 2:
                mainMenuMissionText.text = "2/3 skulls found";
                missionText.text = "2/3 skulls found";
                break;
            case 3:
                mainMenuMissionText.text = "Find the gravestone";
                missionText.text = "Find the gravestone";
                break;
            default:
                Debug.Log("Error mission text");
                break;
        }
    }
    private void UnspawnZombies()
    {
        foreach (GameObject enemy in allEnemyList)
        {
            // Delete enemy
            enemy.SetActive(false);
            // Instantiate effect
            Instantiate(spawnEffect, enemy.transform.position, enemy.transform.rotation);
        }
    }
    public void BossFightTrigger()
    {
        if (isBossBattle)
        {
            return;
        }
        StartCoroutine(BossFightSetup());
    }
    IEnumerator BossFightSetup()
    {
        // Boss battle trigger to true
        isBossBattle = true;
        // close door
        GameObject bossDoorCloneGo = Instantiate(bossDoorPrefab, bossDoorPos.position, bossDoorPos.rotation) as GameObject;
        bossDoorCloneGo.GetComponentInChildren<Door>().enabled = false;
        bossDoorCloneGo.layer = LayerMask.NameToLayer("Default");
        yield return new WaitForSeconds(2f);
        // Delete current enemies
        UnspawnZombies();
        // replace spawn pos
        activeSpawnList.Clear();
        //this.ExpandSpawnArea(5);
        yield return new WaitForSeconds(1.5f);
        bossHealthUi.SetActive(true);
        // Spawn boss
        GameObject bossCloneGo = Instantiate(bossPrefab, bossSpawnPos.position, bossSpawnPos.rotation) as GameObject;
        bossHealthUi.SetActive(true);
    }
    public void UpdateCharacterId(int id)
    {
        currentCharacterId = id;
    }
    public void PauseGame()
    {
        pausePointsNum.text = points.ToString();
        pauseGearsNum.text = gears.ToString();
        Time.timeScale = 0;
        pausePanel.SetActive(true);
        mainCanvas.SetActive(false);
    }
    public void ResumeGame()
    {
        Time.timeScale = 1;
        pausePanel.SetActive(false);
        mainCanvas.SetActive(true);
    }
    public void EndBtnPress()
    {
        pausePanel.SetActive(false);
        checkLeavePanel.SetActive(true);
    }
    public void BackBtnPress()
    {
        pausePanel.SetActive(true);
        checkLeavePanel.SetActive(false);
    }
    public void ShowResultPanel()
    {
        Time.timeScale = 0;
        pauseBtn.SetActive(true);
        completeTimeTxt.text = Mathf.Round(stageTimer).ToString();
        stageClear = true;
        resultPanel.SetActive(true);
    }
    public void LeaveResultPanel()
    {
        GameManager.instance.ToMainMenu();
    }
    // Called when player health is 0 or pumpkin health is 0
    public void GameOver()
    {
        // Save wave if more then current highest wave
        Debug.Log("Gameover in stagecontrol");
        bool isNewHighScore = GameManager.instance.SaveData_HighestWave(currentWave - 1);
        if (isNewHighScore)
        {

        }
        else
        {

        }
        waveNumText.text = (currentWave - 1).ToString();
        gameOverPanel.SetActive(true);
        Time.timeScale = 0;
    }
    public void FuseboxBroken()
    {
        fuseboxIsBroken = true;
        groundFogGO.SetActive(true);
        groundFogGO.GetComponent<ParticleSystem>().Play();
    }
    public void FuseboxFixed()
    {
        fuseboxIsBroken = false;
        groundFogGO.SetActive(false);
    }
}
