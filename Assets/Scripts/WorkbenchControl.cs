using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WorkbenchControl : MonoBehaviour
{
    [SerializeField] GunDatabase gunDatabase;
    [SerializeField] GameObject wbContentPrefab;
    [SerializeField] GameObject layoutParent;
    [SerializeField] List<int> weaponLv = new List<int>();
    [SerializeField] private int primaryGunId;
    [SerializeField] private int secondaryGunId;
    [SerializeField] private Sprite coinIcon;
    [SerializeField] private Sprite gearIcon;
    [SerializeField] private GameObject playerStoragePanel;
    [SerializeField] private List<GameObject> contentCloneList = new List<GameObject>();
    [SerializeField] private Gun newGunToReplace;
    [Header("In Hand Guns")]
    [SerializeField] private GameObject primaryGun_InHandPanel_GO;
    [SerializeField] private Image primaryGun_InHandPanel_Image;
    [SerializeField] private TextMeshProUGUI primaryGun_InHandPanel_Name;
    [SerializeField] private TextMeshProUGUI primaryGun_InHandPanel_Mag;
    [SerializeField] private TextMeshProUGUI primaryGun_InHandPanel_TotalAmmo;
    [SerializeField] private TextMeshProUGUI primaryGun_InHandPanel_TotalLv;
    [SerializeField] private GameObject secondaryGun_InHandPanel_GO;
    [SerializeField] private Image secondaryGun_InHandPanel_Image;
    [SerializeField] private TextMeshProUGUI secondaryGun_InHandPanel_Name;
    [SerializeField] private TextMeshProUGUI secondaryGun_InHandPanel_Mag;
    [SerializeField] private TextMeshProUGUI secondaryGun_InHandPanel_TotalAmmo;
    [SerializeField] private TextMeshProUGUI secondaryGun_InHandPanel_TotalLv;
    [Header("Gun details")]
    [SerializeField] private int upgrade_priceMultiplier = 250;
    [SerializeField] private int currentlySelectedGunId;
    [SerializeField] private Color content_DefaultColor = new Color(255f, 255f, 255f, 255f);
    [SerializeField] private Color content_SelectedColor = new Color(45f, 45f, 45f, 200f);
    [SerializeField] private TextMeshProUGUI upgradePanel_GunName;
    [SerializeField] private Image upgradePanel_GunImage;
    [SerializeField] private TextMeshProUGUI upgradePanel_Damage;
    [SerializeField] private TextMeshProUGUI upgradePanel_DamageLv;
    [SerializeField] private TextMeshProUGUI upgradePanel_DamageIncreasePlus;
    [SerializeField] private TextMeshProUGUI upgradePanel_DamageIncreaseValue;
    [SerializeField] private GameObject upgradePanel_DamageBtn;
    [SerializeField] private TextMeshProUGUI upgradePanel_Mag;
    [SerializeField] private TextMeshProUGUI upgradePanel_MagLv;
    [SerializeField] private TextMeshProUGUI upgradePanel_MagIncreasePlus;
    [SerializeField] private TextMeshProUGUI upgradePanel_MagIncreaseValue;
    [SerializeField] private GameObject upgradePanel_MagBtn;
    [SerializeField] private TextMeshProUGUI upgradePanel_AmmoTotal;
    [SerializeField] private TextMeshProUGUI upgradePanel_AmmoTotalLv;
    [SerializeField] private TextMeshProUGUI upgradePanel_AmmoTotalIncreasePlus;
    [SerializeField] private TextMeshProUGUI upgradePanel_AmmoTotalIncreaseValue;
    [SerializeField] private GameObject upgradePanel_AmmoTotalBtn;
    [SerializeField] private TextMeshProUGUI upgradePanel_Description;
    [SerializeField] private GameObject upgradePanel_BuyBtn;

    [SerializeField] private PlayerGunControl playerGunControl;
    private StageControl stageControl;

    // Replace Weapon 
    [SerializeField] GameObject replaceWeaponPanel;

    // Start is called before the first frame update
    void Start()
    {
        stageControl = FindObjectOfType<StageControl>();
        playerGunControl = FindObjectOfType<PlayerGunControl>();
        UpdateWeaponList();
        UpdateCurPoints();
        // Display primary gun in details panel by default
        DisplayInHandGunInfo(0);
    }


    //==============================================
    //-----UpdateWeaponList-----
    //Description: Update all the gun details on 'In Hand' panel and 'Craft' panel
    //
    //----Parameters-----------------
    // None
    //----Return---------------------
    // None
    //==============================================
    public void UpdateWeaponList()
    {
        playerGunControl = FindObjectOfType<PlayerGunControl>();
        Debug.Log("Start UpdateWeaponList");

        //---------------------
        //Clean current content list
        //---------------------
        foreach (GameObject content in contentCloneList)
        {
            // contentCloneList.Remove(content);
            GameObject.Destroy(content);
        }
        contentCloneList.Clear();

        primaryGunId = playerGunControl.primaryWeaponId;
        secondaryGunId = playerGunControl.secondaryWeaponId;

        //---------------------
        //Update in hand primary gun details
        //---------------------
        primaryGun_InHandPanel_Image.sprite = gunDatabase.GetGun(primaryGunId).sprite_white;
        primaryGun_InHandPanel_Name.text = gunDatabase.GetGun(primaryGunId).name;
        primaryGun_InHandPanel_Mag.text = (gunDatabase.GetGun(primaryGunId).mag_current_value_InGame).ToString();
        primaryGun_InHandPanel_TotalAmmo.text = (gunDatabase.GetGun(primaryGunId).ammoTotal_current_value_InGame).ToString();
        primaryGun_InHandPanel_TotalLv.text = ((gunDatabase.GetGun(primaryGunId).damage_currentLv + gunDatabase.GetGun(primaryGunId).mag_currentLv + gunDatabase.GetGun(primaryGunId).ammoTotal_currentLv)).ToString();

        //---------------------
        //Check and update in hand secondary gun details
        //---------------------
        if (playerGunControl.secondaryWeaponId != 99)
        {
            secondaryGun_InHandPanel_GO.SetActive(true);
            secondaryGun_InHandPanel_Image.sprite = gunDatabase.GetGun(secondaryGunId).sprite_white;
            secondaryGun_InHandPanel_Name.text = gunDatabase.GetGun(secondaryGunId).name;
            secondaryGun_InHandPanel_Mag.text = (gunDatabase.GetGun(secondaryGunId).mag_current_value_InGame).ToString();
            secondaryGun_InHandPanel_TotalAmmo.text = (gunDatabase.GetGun(secondaryGunId).ammoTotal_current_value_InGame).ToString();
            secondaryGun_InHandPanel_TotalLv.text = ((gunDatabase.GetGun(secondaryGunId).damage_currentLv + gunDatabase.GetGun(secondaryGunId).mag_currentLv + gunDatabase.GetGun(secondaryGunId).ammoTotal_currentLv)).ToString();
        }
        else
        {
            secondaryGun_InHandPanel_GO.SetActive(false);
        }

        //---------------------
        //Update craft panel gun list
        //---------------------
        try
        {
            for (int i = 0; i < gunDatabase.GetDatabaseLength(); i++)
            {
                int index = i;
                //---------------------
                //Create contentClone and add it to content list
                //---------------------
                GameObject contentClone = Instantiate(wbContentPrefab, layoutParent.transform) as GameObject;
                contentCloneList.Add(contentClone);
                contentClone.name = "contentClone" + (index + 1);
                contentClone.transform.SetParent(layoutParent.transform);

                // Update content gun image
                GameObject contentChild = contentClone.transform.GetChild(0).GetChild(0).gameObject;
                Image gunImage = contentChild.GetComponentInChildren<Image>();
                Sprite newGunImage = gunDatabase.GetGun(index).sprite_white;
                gunImage.sprite = newGunImage;

                // Update content gun name
                contentClone.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = gunDatabase.GetGun(index).name;

                if (primaryGunId == gunDatabase.GetGun(index).id || secondaryGunId == gunDatabase.GetGun(index).id)
                {
                    contentClone.transform.GetChild(2).gameObject.SetActive(true);
                    contentClone.GetComponent<Image>().enabled = false;
                    contentClone.GetComponent<Button>().enabled = false;
                }
                else
                {
                    contentClone.transform.GetChild(2).gameObject.SetActive(false);
                    contentClone.GetComponent<Image>().color = content_DefaultColor;
                    contentClone.GetComponent<Image>().enabled = true;
                    contentClone.GetComponent<Button>().enabled = true;
                }
                contentClone.GetComponent<Button>().onClick.AddListener(delegate { DisplayCraftableGunInfo(index); });
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error: " + e.Message);
        }
    }
    private void UpdateCurPoints()
    {
        // update gear amount
        playerStoragePanel.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = stageControl.gears.ToString();
        // update points
        playerStoragePanel.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = stageControl.points.ToString();
    }


    //==============================================
    //-----DisplayInHandGunInfo-----
    //Description: Called when 'in hand' gun is clicked
    //          - Display gun info in details panel
    //          - Update upgrade points text
    //          - Show upgrade buttons
    //          - Hide craft button
    //----Parameters-----------------
    // primaryOrSecondaryID 0 : primary
    // primaryOrSecondaryID 1 : secondary
    //----Return---------------------
    // None
    //==============================================
    public void DisplayInHandGunInfo(int primaryOrSecondaryID)
    {
        if (primaryOrSecondaryID == 0)
        {
            currentlySelectedGunId = primaryGunId;
        }
        else if (primaryOrSecondaryID == 1)
        {
            currentlySelectedGunId = secondaryGunId;
        }

        // Get gun stats
        int tempDamage = gunDatabase.GetGun(currentlySelectedGunId).damage_lv1_value;
        int tempDamageLv = gunDatabase.GetGun(currentlySelectedGunId).damage_currentLv;
        int tempMag = gunDatabase.GetGun(currentlySelectedGunId).mag_lv1_value;
        int tempMagLv = gunDatabase.GetGun(currentlySelectedGunId).mag_currentLv;
        int tempAmmoTotal = gunDatabase.GetGun(currentlySelectedGunId).ammoTotal_lv1_value;
        int tempAmmoTotalLv = gunDatabase.GetGun(currentlySelectedGunId).ammoTotal_currentLv;
        int tempDamageIncreasePerUpgrade = gunDatabase.GetGun(currentlySelectedGunId).damage_IncreasePerUpgrade;
        int tempMagIncreasePerUpgrade = gunDatabase.GetGun(currentlySelectedGunId).mag_IncreasePerUpgrade;
        int totalAmmoIncreasePerUpgrade = gunDatabase.GetGun(currentlySelectedGunId).ammoTotal_IncreasePerUpgrade;

        // Update info panel gun stats
        upgradePanel_GunName.text = gunDatabase.GetGun(currentlySelectedGunId).name;
        upgradePanel_GunImage.sprite = gunDatabase.GetGun(currentlySelectedGunId).sprite_white;
        upgradePanel_Damage.text = (tempDamage + tempDamageIncreasePerUpgrade * tempDamageLv).ToString();
        upgradePanel_DamageLv.text = tempDamageLv.ToString();
        upgradePanel_Mag.text = (tempMag + tempMagIncreasePerUpgrade * tempMagLv).ToString();
        upgradePanel_MagLv.text = tempMagLv.ToString();
        upgradePanel_AmmoTotal.text = (tempAmmoTotal + totalAmmoIncreasePerUpgrade * tempAmmoTotalLv).ToString();
        upgradePanel_AmmoTotalLv.text = tempAmmoTotalLv.ToString();
        upgradePanel_Description.text = gunDatabase.GetGun(currentlySelectedGunId).description;

        // Update upgrade increase value
        upgradePanel_DamageIncreasePlus.gameObject.SetActive(true);
        upgradePanel_DamageIncreaseValue.text = gunDatabase.GetGun(currentlySelectedGunId).damage_IncreasePerUpgrade.ToString();
        upgradePanel_MagIncreasePlus.gameObject.SetActive(true);
        upgradePanel_MagIncreaseValue.text = gunDatabase.GetGun(currentlySelectedGunId).mag_IncreasePerUpgrade.ToString();
        upgradePanel_AmmoTotalIncreasePlus.gameObject.SetActive(true);
        upgradePanel_AmmoTotalIncreaseValue.text = gunDatabase.GetGun(currentlySelectedGunId).ammoTotal_IncreasePerUpgrade.ToString();

        // Update upgrade points text
        upgradePanel_DamageBtn.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = (tempDamageLv * upgrade_priceMultiplier).ToString();
        upgradePanel_MagBtn.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = (tempMagLv * upgrade_priceMultiplier).ToString();
        upgradePanel_AmmoTotalBtn.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = (tempAmmoTotalLv * upgrade_priceMultiplier).ToString();

        // Show upgrade buttons
        upgradePanel_DamageBtn.SetActive(true);
        upgradePanel_MagBtn.SetActive(true);
        upgradePanel_AmmoTotalBtn.SetActive(true);

        // Hide craft button
        upgradePanel_BuyBtn.SetActive(false);

        HighlightSelectedContent(true, primaryOrSecondaryID);
    }


    //==============================================
    //-----DisplayCraftableGunInfo-----
    //Description: Called when craftable gun content is clicked
    //          - Display gun info in details panel
    //          - Update craft btn gear amount text
    //          - Hide upgrade buttons
    //          - Show craft button
    //
    //----Parameters-----------------
    // gunId
    //----Return---------------------
    // None
    //==============================================
    public void DisplayCraftableGunInfo(int gunId)
    {
        Debug.Log("Display craftable gun info");
        if (gunId == primaryGunId || gunId == secondaryGunId)
        {
            DisplayInHandGunInfo(gunId);
        }
        else
        {
            currentlySelectedGunId = gunId;
            // Get gun stats
            int tempDamage = gunDatabase.GetGun(currentlySelectedGunId).damage_lv1_value;
            int tempDamageLv = gunDatabase.GetGun(currentlySelectedGunId).damage_currentLv;
            int tempMag = gunDatabase.GetGun(currentlySelectedGunId).mag_lv1_value;
            int tempMagLv = gunDatabase.GetGun(currentlySelectedGunId).mag_lv1_value;
            int tempAmmoTotal = gunDatabase.GetGun(currentlySelectedGunId).ammoTotal_lv1_value;
            int tempAmmoTotalLv = gunDatabase.GetGun(currentlySelectedGunId).ammoTotal_currentLv;
            int tempDamageIncreasePerUpgrade = gunDatabase.GetGun(currentlySelectedGunId).damage_IncreasePerUpgrade;
            int tempMagIncreasePerUpgrade = gunDatabase.GetGun(currentlySelectedGunId).mag_IncreasePerUpgrade;
            int totalAmmoIncreasePerUpgrade = gunDatabase.GetGun(currentlySelectedGunId).ammoTotal_IncreasePerUpgrade;

            // Update info panel gun stats
            upgradePanel_GunName.text = gunDatabase.GetGun(currentlySelectedGunId).name;
            upgradePanel_GunImage.sprite = gunDatabase.GetGun(currentlySelectedGunId).sprite_white;
            upgradePanel_Damage.text = (tempDamage + tempDamageIncreasePerUpgrade * tempDamageLv).ToString();
            upgradePanel_DamageLv.text = tempDamageLv.ToString();
            upgradePanel_Mag.text = (tempMag + tempMagIncreasePerUpgrade * tempMagLv).ToString();
            upgradePanel_MagLv.text = tempMagLv.ToString();
            upgradePanel_AmmoTotal.text = (tempAmmoTotal + totalAmmoIncreasePerUpgrade * tempAmmoTotalLv).ToString();
            upgradePanel_AmmoTotalLv.text = tempAmmoTotalLv.ToString();
            upgradePanel_Description.text = gunDatabase.GetGun(currentlySelectedGunId).description;

            // upgradePanel_GunName.text = gunDatabase.GetGun(currentlySelectedGunId).name;
            // upgradePanel_GunImage.sprite = gunDatabase.GetGun(currentlySelectedGunId).sprite_white;
            // upgradePanel_Damage.text = (gunDatabase.GetGun(currentlySelectedGunId).damage) + (gunDatabase.GetGun(currentlySelectedGunId).damageIncreasePerUpgrade * gunDatabase.GetGun(currentlySelectedGunId).damage_currentLv).ToString();
            // upgradePanel_Mag.text = (gunDatabase.GetGun(currentlySelectedGunId).mag) + (gunDatabase.GetGun(currentlySelectedGunId).magIncreasePerUpgrade * gunDatabase.GetGun(currentlySelectedGunId).mag_currentLv).ToString();
            // upgradePanel_AmmoTotal.text = (gunDatabase.GetGun(currentlySelectedGunId).ammoTotal) + (gunDatabase.GetGun(currentlySelectedGunId).totalAmmoIncreasePerUpgrade * gunDatabase.GetGun(currentlySelectedGunId).ammoTotal_currentLv).ToString();
            // upgradePanel_Description.text = gunDatabase.GetGun(currentlySelectedGunId).description;

            // Hide upgrade increase value
            upgradePanel_DamageIncreasePlus.gameObject.SetActive(false);
            upgradePanel_DamageIncreaseValue.gameObject.SetActive(false);
            upgradePanel_MagIncreasePlus.gameObject.SetActive(false);
            upgradePanel_MagIncreaseValue.gameObject.SetActive(false);
            upgradePanel_AmmoTotalIncreasePlus.gameObject.SetActive(false);
            upgradePanel_AmmoTotalIncreaseValue.gameObject.SetActive(false);

            // Update craft btn gear amount text
            upgradePanel_BuyBtn.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = gunDatabase.GetGun(currentlySelectedGunId).gears.ToString();

            // Show upgrade buttons
            upgradePanel_DamageBtn.SetActive(false);
            upgradePanel_MagBtn.SetActive(false);
            upgradePanel_AmmoTotalBtn.SetActive(false);

            // Hide craft button
            upgradePanel_BuyBtn.SetActive(true);
        }
        HighlightSelectedContent(false, gunId);
    }


    //==============================================
    //-----HighlightSelectedContent-----
    //Description: Change all the content color to the default color and highlight the clicked content.
    //
    //----Parameters-----------------
    // bool in_hand - True if in hand gun
    // int index - gunId
    //----Return---------------------
    // None
    //==============================================
    private void HighlightSelectedContent(bool in_hand, int index)
    {
        //---------------------
        //Change all content to default color
        //---------------------
        primaryGun_InHandPanel_GO.GetComponent<Image>().color = content_DefaultColor;
        secondaryGun_InHandPanel_GO.GetComponent<Image>().color = content_DefaultColor;
        foreach (GameObject content in contentCloneList)
        {
            content.GetComponent<Image>().color = content_DefaultColor;
        }

        //---------------------
        //Highlight the selected content
        //---------------------
        if (in_hand)
        {
            if (index == 0)
            {
                primaryGun_InHandPanel_GO.GetComponent<Image>().color = content_SelectedColor;
            }
            else
            {
                secondaryGun_InHandPanel_GO.GetComponent<Image>().color = content_SelectedColor;
            }

        }
        else
        {
            // If craft gun
            GameObject targetGameObject = contentCloneList[index];
            Image image = targetGameObject.GetComponent<Image>();
            image.color = content_SelectedColor;
        }
    }


    //==============================================
    //-----UpgradeWeapon-----
    //Description: Upgrade the currently selected gun. Upgrade function will depend on the id parameter.
    //----Parameters-----------------
    // id - function to upgrade
    //      id 0 : damage
    //      id 1 : mag
    //      id 2 : total ammo
    //----Return---------------------
    // None
    //==============================================
    public void UpgradeWeaponBtn(int id)
    {
        if (id == 0 && stageControl.points >= gunDatabase.GetGun(currentlySelectedGunId).damage_currentLv * upgrade_priceMultiplier)
        {
            AudioManager.instance.Play("purchase");
            // Decrease points
            stageControl.points -= gunDatabase.GetGun(currentlySelectedGunId).damage_currentLv * upgrade_priceMultiplier;
            // Damage lv increase
            gunDatabase.GetGun(currentlySelectedGunId).damage_currentLv++;
        }
        else if (id == 1 && stageControl.points >= gunDatabase.GetGun(currentlySelectedGunId).mag_currentLv * upgrade_priceMultiplier)
        {
            AudioManager.instance.Play("purchase");
            // Decrease points
            stageControl.points -= gunDatabase.GetGun(currentlySelectedGunId).mag_currentLv * upgrade_priceMultiplier;
            // Mag lv increase
            gunDatabase.GetGun(currentlySelectedGunId).mag_currentLv++;
        }
        else if (id == 2 && stageControl.points >= gunDatabase.GetGun(currentlySelectedGunId).ammoTotal_currentLv * upgrade_priceMultiplier)
        {
            AudioManager.instance.Play("purchase");
            // Decrease points
            stageControl.points -= gunDatabase.GetGun(currentlySelectedGunId).ammoTotal_currentLv * upgrade_priceMultiplier;
            // Mag lv increase
            gunDatabase.GetGun(currentlySelectedGunId).ammoTotal_currentLv++;
        }
        else
        {
            AudioManager.instance.Play("boop");
        }

        if (currentlySelectedGunId == primaryGunId)
        {
            DisplayInHandGunInfo(0);
        }
        else
        {
            DisplayInHandGunInfo(1);
        }

        UpdateWeaponList();
        UpdateCurPoints();
    }


    //==============================================
    //-----BuyWeapon-----
    //Description: 
    //----Parameters-----------------
    // None
    //----Return---------------------
    // None
    //==============================================
    public void BuyWeapon()
    {
        Debug.Log("Start BuyWeapon");
        // Check enough gears
        if (stageControl.gears >= gunDatabase.GetGun(currentlySelectedGunId).gears)
        {
            // Check if currently holding gun or not
            if (playerGunControl.secondaryWeaponId != 99)
            {
                Debug.Log("carrying two weapons");
                newGunToReplace = gunDatabase.GetGun(currentlySelectedGunId);
                replaceWeaponPanel.SetActive(true);
                UpdateReplaceWeaponPanel();
            }
            else
            {
                AudioManager.instance.Play("purchase");
                Debug.Log("carrying one weapon");
                // Gun to secondary
                playerGunControl.NewGunToSecondary(gunDatabase.GetGun(currentlySelectedGunId));
                stageControl.gears -= gunDatabase.GetGun(currentlySelectedGunId).gears;
                UpdateWeaponList();
                UpdateCurPoints();
            }
        }
        else
        {
            // Not enough gears
            AudioManager.instance.Play("boop");
        }
        Debug.Log("End BuyWeapon");
    }

    void UpdateReplaceWeaponPanel()
    {
        // Update new gun image
        replaceWeaponPanel.transform.GetChild(1).GetComponentInChildren<Image>().sprite = newGunToReplace.sprite_white;
        // Update primary gun image
        replaceWeaponPanel.transform.GetChild(2).GetComponentInChildren<Image>().sprite = playerGunControl.primaryGun.sprite_white;
        // Update secondary gun image
        replaceWeaponPanel.transform.GetChild(3).GetComponentInChildren<Image>().sprite = playerGunControl.secondaryGun.sprite_white;
    }

    public void ReplacePrimaryGun()
    {
        stageControl.gears -= gunDatabase.GetGun(currentlySelectedGunId).gears;
        AudioManager.instance.Play("purchase");
        playerGunControl.NewGunToPrimary(newGunToReplace);
        UpdateWeaponList();
        UpdateCurPoints();
        DisplayCraftableGunInfo(currentlySelectedGunId);
        HighlightSelectedContent(true, 0);
        CloseReplaceWeaponPanel();
    }

    public void ReplaceSecondaryGun()
    {
        stageControl.gears -= gunDatabase.GetGun(currentlySelectedGunId).gears;
        AudioManager.instance.Play("purchase");
        playerGunControl.NewGunToSecondary(newGunToReplace);
        UpdateWeaponList();
        UpdateCurPoints();
        DisplayCraftableGunInfo(currentlySelectedGunId);
        HighlightSelectedContent(true, 1);
        CloseReplaceWeaponPanel();
    }

    public void CloseReplaceWeaponPanel()
    {
        replaceWeaponPanel.SetActive(false);
    }

    // public void RefillPrimaryGunAmmo()
    // {
    //     // Boop sound if already max ammo
    //     if (playerGunControl.PrimaryGunAmmoIsMax() && stageControl.points >= playerGunControl.primaryGun.refillAmmoPrice)
    //     {
    //         AudioManager.instance.Play("boop");
    //     }
    //     else
    //     {
    //         AudioManager.instance.Play("purchase");
    //         stageControl.points -= playerGunControl.primaryGun.refillAmmoPrice;
    //         UpdateWeaponList();
    //         UpdateCurPoints();
    //     }
    // }

    public void CloseWorkbenchPanel()
    {
        stageControl.UpdateUI();
        this.gameObject.SetActive(false);
        // End pause
        Time.timeScale = 1;
    }
}
