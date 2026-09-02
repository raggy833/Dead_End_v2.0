using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemStats
{
    [Header("Base Info")]
    public int itemId;
    public string name;
    public int price;
    public float effectDuration;
    public string details;
    public Sprite item_sprite;
    public GameObject item_prefab;

    public float healAmount;
    public float damageBuff;
    public float speedBuff;

}
