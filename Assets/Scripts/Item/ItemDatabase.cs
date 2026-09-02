using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ItemDatabase : ScriptableObject
{
    public ItemStats[] itemStats;

    public ItemStats GetItem(int id)
    {
        return itemStats[id];
    }
    public int GetDatabaseLength()
    {
        return itemStats.Length;
    }
}
