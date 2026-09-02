using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDropControl : Interactable
{
    public float hoverSpeed = 0.5f;
    public float hoverHeight = 0.5f;
    private Vector3 initialPosition;

    public GameObject currentWeapon;
    public Gun currentGun;
    public GameObject weaponDrop_weaponPos;
    public GameObject weaponDrop_bubble;
    public Material bubbleMaterial;

    private PlayerGunControl playerGunControl;
    [SerializeField] private Color32 commonBubble;
    [SerializeField] private Color32 rareBubble;
    [SerializeField] private Color32 epicBubble;
    public GunDatabase gunDb;

    public float rotationSpeed = 30f;

    // Start is called before the first frame update
    void Start()
    {
        initialPosition = transform.position;
        // weaponDrop_weaponPos.transform.position = initialPosition;
        playerGunControl = FindObjectOfType<PlayerGunControl>();
    }

    // Update is called once per frame
    void Update()
    {
        WeaponBubble_Hover();
        WeaponBubble_WeaponRotate();

    }
    protected override void Interact()
    {
        playerGunControl.NewGunToPrimary(currentGun);

        // TODO pick up animation
        // TODO destroy object

    }

    //---------------------
    // Hover the Weapon Drop Bubble
    //---------------------
    public void WeaponBubble_Hover()
    {
        float newY = initialPosition.y + Mathf.Sin(Time.time * hoverSpeed) * hoverHeight;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }

    //---------------------
    // Rotate the gun in the bubble
    //---------------------
    public void WeaponBubble_WeaponRotate()
    {
        transform.GetChild(0).Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
    }

    //---------------------
    //Using the input string, call the corresponding function
    //---------------------
    public void SetWeaponAndBubbleColor(string rarity)
    {
        // Set default bubble color to commonBubble
        Color32 bubbleColor = commonBubble;
        if (rarity == "common")
        {
            ChooseCommonWeapon();
        }
        else if (rarity == "rare")
        {
            bubbleColor = rareBubble;
            ChooseRareWeapon();
        }
        else if (rarity == "epic")
        {
            bubbleColor = epicBubble;
            ChooseEpicWeapon();
        }

        // Set the color of the bubbleMaterial to the chosen color
        bubbleMaterial.color = bubbleColor;
    }

    //---------------------
    //Set normal gun in the bubble
    //---------------------
    public void ChooseCommonWeapon()
    {
        ChooseGun(GunRarity.Common);
    }
    //---------------------
    //Set rare gun in the bubble
    //---------------------
    public void ChooseRareWeapon()
    {
        ChooseGun(GunRarity.Rare);
    }

    //---------------------
    //Set epic gun in the bubble
    //---------------------
    public void ChooseEpicWeapon()
    {
        ChooseGun(GunRarity.Epic);
    }

    private void ChooseGun(GunRarity rarity)
    {
        // Get all guns of the specified rarity from the database
        Gun[] gunsOfRarity = gunDb.GetGunsByRarity(rarity);

        // Randomly choose one gun from the array
        Gun chosenGun = gunsOfRarity[Random.Range(0, gunsOfRarity.Length)];
        currentGun = chosenGun;

        // Instantiate the chosen gun and set its position to the gunPosition
        if (chosenGun != null)
        {
            currentWeapon = Instantiate(chosenGun.gameObject, weaponDrop_weaponPos.transform.position, weaponDrop_weaponPos.transform.rotation, weaponDrop_weaponPos.transform);
        }
        else
        {
            Debug.LogWarning("No gun of the specified rarity found.");
        }
    }

}
