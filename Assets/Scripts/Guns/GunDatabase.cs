using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class GunDatabase : ScriptableObject
{
    public Gun[] allGuns;

    public Gun GetGun(int id)
    {
        return allGuns[id];
    }
    public int GetDatabaseLength()
    {
        return allGuns.Length;
    }
    // Function to get guns by rarity
    public Gun[] GetGunsByRarity(GunRarity rarity)
    {
        List<Gun> gunsOfRarity = new List<Gun>();

        foreach (Gun gun in allGuns)
        {
            if (gun.GetRarity() == rarity)
            {
                gunsOfRarity.Add(gun);
            }
        }

        return gunsOfRarity.ToArray();
    }
}
