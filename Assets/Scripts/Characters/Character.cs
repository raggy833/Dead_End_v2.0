using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Character
{
    [Space(20)]
    public bool unlocked;
    public int unlockCost;
    [Header("Base Info")]
    public int id;
    public string name;
    [TextArea(3, 10)]
    public string description;
    [TextArea(3, 10)]
    public string skillDescription;
    public Sprite icon;

    [Header("Stats")]
    public float knifeDamageBuff = 1f;
    public float handgunDamageBuff = 1f;

    [Header("Saved data")]
    public int highestWave;
}
