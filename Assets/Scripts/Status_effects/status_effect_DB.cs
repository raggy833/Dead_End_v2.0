using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class status_effect_DB : ScriptableObject
{
    public status_effect[] status_e;

    public status_effect GetStatus_Effect(int id)
    {
        return status_e[id];
    }
    public int GetDatabaseLength()
    {
        return status_e.Length;
    }
}

