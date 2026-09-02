using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class CharacterDB : ScriptableObject
{
    public Character[] character;

    public Character GetCharacter(int id)
    {
        return character[id];
    }
    public int GetDatabaseLength()
    {
        return character.Length;
    }
}

