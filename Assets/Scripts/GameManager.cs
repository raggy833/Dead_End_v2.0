using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public int characterId;
    private int goldAmount;
    private int silverAmount;
    [SerializeField] private LoadingScene loadingScene;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);

        Setup();
    }
    private void Setup()
    {
        loadingScene = FindObjectOfType<LoadingScene>();
    }

    private void Start()
    {
        (silverAmount, goldAmount) = SaveSystem.LoadData();
        AudioManager.instance.Play("bgm_mainMenu");
    }
    public void ToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
        AudioManager.instance.Play("bgm_mainMenu");
    }
    public void StartCutscene()
    {
        SceneManager.LoadScene("GameIntroScene");
    }
    public void StartDefendMode()
    {
        loadingScene = FindObjectOfType<LoadingScene>();
        // StartCoroutine(loadingScene.LoadSceneAsync("Game_DefendMode"));
        loadingScene.ShowLoadScreen();
        SceneManager.LoadScene("Game_DefendMode");
        AudioManager.instance.Stop("bgm_mainMenu");
        AudioManager.instance.Stop("bgm_game");
    }
    public void QuitGame()
    {
        Debug.Log("Quit button pressed");
        Application.Quit();
    }
    public int Get_current_silver_amount()
    {
        return silverAmount;
    }
    public void SaveData_silver_gold(int silver_change_value, int gold_change_value)
    {
        silverAmount += silver_change_value;
        goldAmount += gold_change_value;
        SaveSystem.SaveData(silverAmount, goldAmount);
    }
    public bool SaveData_HighestWave(int newWave)
    {
        Debug.Log("Save new wave called");
        // Check if newWave is more than current wave, if so save the newWave as the highest wave
        int currentHighestWave = PlayerPrefs.GetInt("HighScoreForCharacter" + characterId, 0);
        if (newWave > currentHighestWave)
        {
            PlayerPrefs.SetInt("HighScoreForCharacter" + characterId, newWave);
            return true;
        }
        else
        {
            return false;
        }
    }
}
