using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EffectIconController : MonoBehaviour
{
    public List<Sprite> effectIcons;
    public float effectDuration;
    public float flickerDuration;
    public float flickerSpeed;
    [SerializeField] private GameObject stats_parent;
    [SerializeField] private PlayerGunControl playerGunControl;
    [SerializeField] private PlayerMotor playerMotor;
    [SerializeField] private ItemDatabase itemDatabase;

    [SerializeField] private Dictionary<int, Image> effectIconDict = new Dictionary<int, Image>();
    private Dictionary<int, float> effectTimeDict = new Dictionary<int, float>();
    private Dictionary<int, bool> effectFlickerDict = new Dictionary<int, bool>();

    private void Start()
    {
        playerGunControl = FindObjectOfType<PlayerGunControl>();
        playerMotor = FindObjectOfType<PlayerMotor>();

        for (int i = 0; i < effectIcons.Count; i++)
        {
            Sprite icon = effectIcons[i];
            GameObject iconObj = new GameObject("EffectIcon_" + icon.name);
            iconObj.transform.SetParent(transform);
            Image iconImage = iconObj.AddComponent<Image>();
            iconImage.sprite = icon;
            iconImage.enabled = false;
            effectIconDict.Add(i, iconImage);
        }
    }


    public void ActivateEffect(int index, float duration)
    {
        if (effectIconDict.ContainsKey(index))
        {
            Image effectIcon = effectIconDict[index];
            effectIcon.enabled = true;

            if (!effectTimeDict.ContainsKey(index))
            {
                effectTimeDict.Add(index, duration);
                effectFlickerDict.Add(index, false);
                effectIcon.transform.SetParent(stats_parent.transform, false);
                StartCoroutine(EffectCountdown(index));
            }
            else
            {
                effectTimeDict[index] = duration;
                effectFlickerDict[index] = false;
            }

            // Modify player's damage or speed based on the power-up index
            switch (index)
            {
                case 0: // Power-up 1: Increase damage
                    playerGunControl.damageBuff = itemDatabase.GetItem(1).damageBuff;
                    break;
                case 1: // Power-up 2: Increase speed
                    playerMotor.speedBuff = itemDatabase.GetItem(2).speedBuff;
                    break;
                default:
                    break;
            }
            // Reorganize the effect icons
            ReorganizeEffectIcons();
        }
    }


    private IEnumerator EffectCountdown(int index)
    {
        float effectTime;
        while (effectTimeDict.TryGetValue(index, out effectTime) && effectTime > 0)
        {
            effectTimeDict[index] -= Time.deltaTime;

            bool shouldFlicker;
            if (effectTimeDict[index] <= flickerDuration && effectFlickerDict.TryGetValue(index, out shouldFlicker) && !shouldFlicker)
            {
                effectFlickerDict[index] = true;
                StartCoroutine(FlickerEffect(index));
            }

            yield return null;
        }

        effectIconDict[index].enabled = false;
        effectTimeDict.Remove(index);
        effectFlickerDict.Remove(index);

        // Reset player's damage or speed when the power-up effect ends
        switch (index)
        {
            case 0: // Power-up 1: Increase damage
                playerGunControl.damageBuff = 1f;
                break;
            case 1: // Power-up 2: Increase speed
                playerMotor.speedBuff = 1f;
                break;
            default:
                break;
        }
        // Reorganize the effect icons
        ReorganizeEffectIcons();
    }

    private IEnumerator FlickerEffect(int index)
    {
        bool shouldFlicker;
        while (effectFlickerDict.TryGetValue(index, out shouldFlicker) && shouldFlicker)
        {
            Image effectIcon;
            if (effectIconDict.TryGetValue(index, out effectIcon))
            {
                effectIcon.enabled = !effectIcon.enabled;
            }
            yield return new WaitForSeconds(flickerSpeed);
        }
    }
    private void ReorganizeEffectIcons()
    {
        // Get the active effect icons in order
        List<Image> activeIcons = new List<Image>();
        foreach (KeyValuePair<int, Image> kvp in effectIconDict)
        {
            if (kvp.Value.enabled)
            {
                activeIcons.Add(kvp.Value);
            }
        }
        activeIcons.Sort((a, b) => a.transform.GetSiblingIndex().CompareTo(b.transform.GetSiblingIndex()));

        // Reorganize the effect icons to the left
        for (int i = 0; i < activeIcons.Count; i++)
        {
            activeIcons[i].transform.SetSiblingIndex(i);
        }
    }
}
