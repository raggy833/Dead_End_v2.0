using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class status_effect
{
    [Space(20)]
    [Header("Base Info")]
    public int id;
    public string name;
    public float duration;
    public Sprite icon;
    [TextArea(3, 10)]
    public string description;
}
