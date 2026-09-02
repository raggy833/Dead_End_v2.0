using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuManager : MonoBehaviour
{
    public GameObject zombie_prefab;
    public GameObject zombie_clone;
    public Transform zombie_startPos;
    public Transform zombie_endPos;
    public GameObject mainMenu_panel;
    public GameObject chooseLevel_panel;
    public GameObject menuCamera;
    public GameObject fadeoutPanel;

    private float showMenuLag = 1.8f;

    [SerializeField] private TextMeshProUGUI silverAmount;

    [Header("Character")]
    public int selectedCharacterId;
    public GameObject selectCharacter_panel;
    public CharacterDB characterDB;
    [SerializeField] private Image characterImage;
    [SerializeField] private TextMeshProUGUI characterNameText;
    [SerializeField] private TextMeshProUGUI characterHighestWave_UI;
    [SerializeField] private int characterHighestWave_Num;

    [Header("Character Detail")]
    [SerializeField] private GameObject characterContent;
    [SerializeField] private GameObject contentParentGO;
    [SerializeField] private Image detail_characterImage;
    [SerializeField] private TextMeshProUGUI detail_characterName;
    [SerializeField] private TextMeshProUGUI detail_characterDescription;
    [SerializeField] private TextMeshProUGUI detail_characterSkill;
    [SerializeField] private int currentlySelectedCharacterId;
    [SerializeField] private Color32 defaultColor = new Color32(0, 0, 0, 100);
    [SerializeField] private Color32 selectedColor = new Color32(0, 0, 255, 100);
    [SerializeField] private List<GameObject> contentCloneList = new List<GameObject>();
    [SerializeField] private GameObject characterSelectBtn;
    public Button select_unlock_btn;
    public TextMeshProUGUI select_unlock_text;
    public GameObject characterUnlockPanel;
    public Image toUnlockCharacterImage;
    public TextMeshProUGUI toUnlockCharacterName;
    [SerializeField] private TextMeshProUGUI unlockPanel_silverAmount;
    [SerializeField] private TextMeshProUGUI unlockPanel_characterUnlockAmount;

    [Header("HowToPlay")]
    [SerializeField] private GameObject howToPlayPanel;
    [SerializeField] private Image description_image;
    [SerializeField] private TextMeshProUGUI description_text;
    [SerializeField] private List<GameObject> howToPlayCategoryList = new List<GameObject>();
    [SerializeField] private List<GameObject> howToPlayDescriptionPanelList = new List<GameObject>();

    [Header("Settings")]
    [SerializeField] private GameObject settings_panel;

    // Start is called before the first frame update
    void Start()
    {
        fadeoutPanel.SetActive(false);
        SpawnZombie();
        Setup();
    }

    void Setup()
    {
        Time.timeScale = 1;
        mainMenu_panel.SetActive(true);
        menuCamera.GetComponent<Animator>().SetBool("ChooseLevel", false);

        characterUnlockPanel.SetActive(false);
        // TODO: Load selected character
        selectedCharacterId = 0;
        GameManager.instance.characterId = selectedCharacterId;
        UpdateLevelSelect_characterHighestWave();
        CheckCharacterUnlock();
        UpdateSilverAmount();
    }
    private void UpdateSilverAmount()
    {
        silverAmount.text = GameManager.instance.Get_current_silver_amount().ToString();
    }
    public void UpdateLevelSelect_characterHighestWave()
    {
        characterHighestWave_Num = GetHighScoreForCharacter(selectedCharacterId);
        characterHighestWave_UI.text = characterHighestWave_Num.ToString();
    }
    public void CheckCharacterUnlock()
    {
        if (characterDB.GetCharacter(currentlySelectedCharacterId).unlocked)
        {
            Debug.Log("Character is unlocked");
            // Character is unlocked
            select_unlock_text.text = "Select";
            select_unlock_btn.onClick.RemoveAllListeners();
            select_unlock_btn.onClick.AddListener(ChangeToSelectedCharacter);

        }
        else
        {
            Debug.Log("Character is locked");
            // Character is locked
            select_unlock_text.text = "Unlock";
            select_unlock_btn.onClick.RemoveAllListeners();
            select_unlock_btn.onClick.AddListener(ToUnlockPanel);
        }
    }
    public void ToUnlockPanel()
    {
        characterUnlockPanel.SetActive(true);
        toUnlockCharacterImage.sprite = characterDB.GetCharacter(currentlySelectedCharacterId).icon;
        toUnlockCharacterName.text = characterDB.GetCharacter(currentlySelectedCharacterId).name;
        unlockPanel_characterUnlockAmount.text = characterDB.GetCharacter(currentlySelectedCharacterId).unlockCost.ToString();
        unlockPanel_silverAmount.text = GameManager.instance.Get_current_silver_amount().ToString();
    }
    public void UnlockCharacter()
    {
        int selectedCharacterCost = characterDB.GetCharacter(currentlySelectedCharacterId).unlockCost;
        int currentSilverAmount = GameManager.instance.Get_current_silver_amount();
        if (currentSilverAmount >= selectedCharacterCost)
        {
            // Play purchase sound

            // Unlock character
            characterDB.GetCharacter(currentlySelectedCharacterId).unlocked = true;
            // Decrease and save new silver amount
            GameManager.instance.SaveData_silver_gold(-selectedCharacterCost, 0);
            // Leave unlock panel
            characterUnlockPanel.SetActive(false);
            ChangeToSelectedCharacter();

            UpdateLevelSelect_characterHighestWave();
            CheckCharacterUnlock();
        }
        else
        {
            // Play boop sound
        }
    }
    public void LeaveUnlockCharacterPanel()
    {
        characterUnlockPanel.SetActive(false);
    }

    public void ToHowToPlayPanel()
    {
        AudioManager.instance.Play("click_horror_select");
        howToPlayPanel.SetActive(true);
        mainMenu_panel.SetActive(false);
        HowToPlay_DefaultDisplay();
    }
    private void HowToPlay_DefaultDisplay()
    {
        HowToPlay_DisplaySelectedCategory(0);
    }
    public void HowToPlay_DisplaySelectedCategory(int categoryId)
    {
        // Disable all description panel
        foreach (GameObject panel in howToPlayDescriptionPanelList)
        {
            panel.SetActive(false);
        }
        // Display details for selected category
        howToPlayDescriptionPanelList[categoryId].SetActive(true);

        // Reset category color to default
        foreach (GameObject category in howToPlayCategoryList)
        {
            // Change color to default
            category.GetComponent<Image>().color = Color.white;
        }

        // Highlight selected category
        howToPlayCategoryList[categoryId].GetComponent<Image>().color = Color.yellow;

    }
    public void CloseHowToPlayPanel()
    {
        AudioManager.instance.Play("click_horror_select");
        howToPlayPanel.SetActive(false);
        mainMenu_panel.SetActive(true);
    }
    public void ToChooseLevel()
    {
        mainMenu_panel.SetActive(false);
        menuCamera.GetComponent<Animator>().SetBool("ChooseLevel", true);
        AudioManager.instance.Play("click_horror_select");
        Invoke("DestroyZombie", 0.5f);
        // Update character UI
        characterImage.sprite = characterDB.GetCharacter(selectedCharacterId).icon;
        characterNameText.text = characterDB.GetCharacter(selectedCharacterId).name;

        Invoke("ShowChooseLevelPanel", showMenuLag);
    }
    public void ToDefendMode()
    {
        AudioManager.instance.Play("click_horror_select");
        chooseLevel_panel.SetActive(false);
        menuCamera.GetComponent<Animator>().SetBool("StartDefendMode", true);
        StartCoroutine(StartDefendMode());
    }
    private IEnumerator StartDefendMode()
    {
        yield return new WaitForSeconds(2.2f);
        GameManager.instance.StartDefendMode();
    }
    public void ToCharacterSelectPanel()
    {
        chooseLevel_panel.SetActive(false);
        currentlySelectedCharacterId = selectedCharacterId;
        // Instantiate character content
        InstantiateCharacterContent();
        // Highlight selected character
        HighlightSelectedContent(currentlySelectedCharacterId);
        // Update UI with selected character details
        UpdateSelectedCharacterContent(currentlySelectedCharacterId);
        selectCharacter_panel.SetActive(true);
    }
    private void InstantiateCharacterContent()
    {
        foreach (GameObject content in contentCloneList)
        {
            Destroy(content);
        }
        contentCloneList.Clear();
        for (int i = 0; i < characterDB.GetDatabaseLength(); i++)
        {
            int index = i;
            GameObject contentClone = Instantiate(characterContent, contentParentGO.transform) as GameObject;
            // TODO: check if needed
            contentCloneList.Add(contentClone);
            contentClone.name = "contentClone" + (index + 1);
            // contentClone.transform.SetParent(contentParentGO.transform);

            // Update character image
            GameObject contentChild = contentClone.transform.GetChild(0).GetChild(0).gameObject;
            Image charImage = contentChild.GetComponentInChildren<Image>();
            Sprite newGunImage = characterDB.GetCharacter(index).icon;
            charImage.sprite = newGunImage;

            // Update character name
            contentClone.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = characterDB.GetCharacter(index).name;

            // Update high score text
            int highScore = GetHighScoreForCharacter(index);
            contentClone.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = highScore.ToString();

            // Update button text depending on the current holding gun
            Button button = contentClone.GetComponentInChildren<Button>();
            //GameObject pricePanel = button.transform.GetChild(1).gameObject;

            button.onClick.AddListener(delegate { DisplaySelectedCharacterDetails(index); });
        }
    }
    private void HighlightSelectedContent(int selectedId)
    {
        // Change all to default color 
        foreach (GameObject content in contentCloneList)
        {
            content.GetComponent<Image>().color = defaultColor;
        }
        // Only highlight the selected content
        contentCloneList[selectedId].GetComponent<Image>().color = selectedColor;
    }
    private void UpdateSelectedCharacterContent(int selectedId)
    {
        detail_characterImage.sprite = characterDB.GetCharacter(selectedId).icon;
        detail_characterName.text = characterDB.GetCharacter(selectedId).name;
        detail_characterDescription.text = characterDB.GetCharacter(selectedId).description;
        detail_characterSkill.text = characterDB.GetCharacter(selectedId).skillDescription;
    }
    public void DisplaySelectedCharacterDetails(int id)
    {
        AudioManager.instance.Play("click_horror_select");
        // If current character, set select btn to not active
        // if (id == currentlySelectedCharacterId)
        // {
        //     characterSelectBtn.SetActive(false);
        // }
        // else
        // {
        //     characterSelectBtn.SetActive(true);
        // }
        // Highlight selected character
        HighlightSelectedContent(id);
        // Update UI with selected character details
        UpdateSelectedCharacterContent(id);
        currentlySelectedCharacterId = id;

        CheckCharacterUnlock();
        Debug.Log("Display character " + id);
    }
    public void ChangeToSelectedCharacter()
    {
        selectedCharacterId = currentlySelectedCharacterId;
        Debug.Log("Current selected character is :" + selectedCharacterId);
        AudioManager.instance.Play("click_horror_select");
        // Update character UI
        characterImage.sprite = characterDB.GetCharacter(selectedCharacterId).icon;
        characterNameText.text = characterDB.GetCharacter(selectedCharacterId).name;
        selectCharacter_panel.SetActive(false);
        chooseLevel_panel.SetActive(true);
        GameManager.instance.characterId = selectedCharacterId;
        UpdateLevelSelect_characterHighestWave();
    }
    public void LeaveCharacterSelectPanel()
    {

    }
    public void BackToMainMenu()
    {
        AudioManager.instance.Play("click_horror_select");
        chooseLevel_panel.SetActive(false);
        menuCamera.GetComponent<Animator>().SetBool("ChooseLevel", false);
        Invoke("SpawnZombie", showMenuLag);
        Invoke("ShowMenuPanel", showMenuLag);
        UpdateSilverAmount();
    }
    void SpawnZombie()
    {
        zombie_clone = Instantiate(zombie_prefab, zombie_startPos.position, zombie_startPos.rotation) as GameObject;
        zombie_clone.GetComponent<MainMenu_Enemy>().startPosition = zombie_startPos;
        zombie_clone.GetComponent<MainMenu_Enemy>().target = zombie_endPos;
    }
    void DestroyZombie()
    {
        Destroy(zombie_clone);
    }
    void ShowMenuPanel()
    {
        mainMenu_panel.SetActive(true);
    }
    void ShowChooseLevelPanel()
    {
        chooseLevel_panel.SetActive(true);
    }
    public void Fadeout()
    {
        fadeoutPanel.SetActive(true);
        StartCoroutine(FadeoutScreen());
    }
    private IEnumerator FadeoutScreen()
    {
        int tempNum = 0;
        while (fadeoutPanel.GetComponent<Image>().color.a != 255)
        {
            tempNum += 1;
            fadeoutPanel.GetComponent<Image>().color = new Color32(0, 0, 0, ((byte)tempNum));
            yield return null;
        }
    }

    public void OpenSettingsPanel()
    {
        AudioManager.instance.Play("click_horror_select");
        settings_panel.SetActive(true);
    }
    public void CloseSettingsPanel()
    {
        AudioManager.instance.Play("click_horror_select");
        settings_panel.SetActive(false);
    }

    private int GetHighScoreForCharacter(int characterIndex)
    {
        // TODO: Replace this with your own code to retrieve the high score data from your database
        // For example, you can use PlayerPrefs to store and retrieve the high score data
        return PlayerPrefs.GetInt("HighScoreForCharacter" + characterIndex, 0);
    }

}
