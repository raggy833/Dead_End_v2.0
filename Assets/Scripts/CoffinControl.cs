using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoffinControl : Interactable
{
    public int coffinCost;
    public GameObject coffinLidGO;
    public GameObject WeaponDropGO;
    public Vector3 weaponDropOffset;


    [SerializeField] private int weaponDropRarity_normal;
    [SerializeField] private int weaponDropRarity_rare;
    [SerializeField] private int weaponDropRarity_epic;
    private string currentWeaponDropRarityString;
    private int currentWeaponRarity;
    private int percentageMax = 100;
    private Vector3 weaponDropPos;
    private bool isOpen = false;
    private StageControl stageControl;

    // Start is called before the first frame update
    void Start()
    {
        stageControl = FindObjectOfType<StageControl>();
        this.GetComponent<BoxCollider>().enabled = true;
    }

    // Update is called once per frame
    void Update()
    {

    }

    protected override void Interact()
    {

        if (isOpen)
        {

        }
        else
        {
            if (stageControl.OpenCoffin(coffinCost))
            {
                // Set current rarity
                SetCurrentRarity();
                // Play animation for coffin door

                // Disable coffin door
                coffinLidGO.SetActive(false);
                // Spawn WeaponDropGO GameObject
                Vector3 spawnPosition = transform.position + transform.forward * weaponDropOffset.z +
                                       transform.up * weaponDropOffset.y;
                GameObject WeaponDropClone = Instantiate(WeaponDropGO, spawnPosition, Quaternion.identity) as GameObject;
                WeaponDropClone.GetComponent<WeaponDropControl>().SetWeaponAndBubbleColor(currentWeaponDropRarityString);
                WeaponDropClone.GetComponent<Interactable>().promptMessage = "Pick up";
                // Change weaponDrop gun


                // Change coffin layer to default
                this.gameObject.layer = LayerMask.NameToLayer("Default");
                promptMessage = "";
                isOpen = true;

                // AudioManager.instance.Play("chaching");
            }
            else
            {
                // Not enough points
            }
        }
    }
    private void SetCurrentRarity()
    {
        int randomRarity = Random.Range(1, percentageMax + 1);
        Debug.Log("Rarity is: " + randomRarity);
        if (randomRarity > weaponDropRarity_epic)
        {
            Debug.Log("Gun is epic rarity");
            currentWeaponRarity = weaponDropRarity_epic;
            currentWeaponDropRarityString = "epic";
        }
        else if (randomRarity > weaponDropRarity_rare)
        {
            Debug.Log("Gun is rare rarity");
            currentWeaponRarity = weaponDropRarity_rare;
            currentWeaponDropRarityString = "rare";
        }
        else
        {
            Debug.Log("Gun is epic rarity");
            currentWeaponRarity = weaponDropRarity_normal;
            currentWeaponDropRarityString = "normal";
        }
    }
}
