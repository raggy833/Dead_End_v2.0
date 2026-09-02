using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GunRarity
{
    Common,
    Rare,
    Epic
};

[System.Serializable]
public class Gun
{
    [Space(20)]
    [Header("Base Info")]
    public int id;
    public string name;
    [Multiline(3)]
    public string description;
    [Header("Mag")]
    public int mag_currentLv = 1;
    public int mag_lv1_value;
    public int mag_current_value_InGame;
    public int mag_current_max_InGame;
    public int mag_IncreasePerUpgrade;
    [Header("Total ammo")]
    public int ammoTotal_currentLv = 1;
    public int ammoTotal_lv1_value;
    public int ammoTotal_current_value_InGame;
    public int ammoTotal_current_max_InGame;
    public int ammoTotal_IncreasePerUpgrade;
    [Header("Damage")]
    public int damage_currentLv = 1;
    public int damage_lv1_value;
    public int damage_IncreasePerUpgrade;
    [Header("Other stats")]
    public int bullets_per_shot;
    public int price;
    public int gears;
    public float shootLag;
    public GameObject gameObject;
    public GameObject holdPos;
    public Sprite sprite_white;
    public string shootSound;
    public string reloadSound;
    public GunType gunType = new GunType();
    public GunRarity rarity = new GunRarity();
    public enum GunType
    {
        HandGun,
        SubMachineGun,
        AssultRifle,
        LMG,
        Sniper,
        ShotGun
    };


    public GunRarity GetRarity()
    {
        return rarity;
    }

    [Header("Details")]
    // Hipfire Recoil
    [SerializeField] public float recoilX = -2;
    [SerializeField] public float recoilY = 2;
    [SerializeField] public float recoilZ = 0.35f;

    // ADS Recoil
    [SerializeField] public float aimRecoilX = -1.5f;
    [SerializeField] public float aimRecoilY = 1;
    [SerializeField] public float aimRecoilZ = .3f;

    // Settings
    [SerializeField] public float snappiness = 6;
    [SerializeField] public float returnSpeed = 2;
}
