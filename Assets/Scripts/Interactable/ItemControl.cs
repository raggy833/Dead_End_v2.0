using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemControl : MonoBehaviour
{
    /*
    Item id 1 - health pot : heal 30 hp
    Item id 2 - damage buff : increase damage by 15%
    Item id 3 - speed buff : increase move speed by 10%

    */

    [SerializeField] private ItemDatabase itemDatabase;
    [SerializeField] private List<GameObject> itemPrefabs = new List<GameObject>();
    [SerializeField] private GameObject mainItemParent;
    [SerializeField] private GameObject subItemParent;
    [SerializeField] private GameObject mainItemGo;
    [SerializeField] private GameObject subItemGo;
    [SerializeField] private int curMainItemId;
    [SerializeField] private int curSubItemId;

    [SerializeField] private PlayerGunControl playerGunControl;
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private EffectIconController effectIconController;

    void Start()
    {
        mainItemGo = null;
        subItemGo = null;
        curMainItemId = 0;
        curSubItemId = 0;
        playerGunControl = FindObjectOfType<PlayerGunControl>();
        playerHealth = FindObjectOfType<PlayerHealth>();
        effectIconController = FindObjectOfType<EffectIconController>();
    }

    public void ClickItemButton()
    {
        if (curMainItemId == 0)
        {
            // No item
        }
        else if (curMainItemId == 1)
        {
            AudioManager.instance.Play("heal_player");
            // Get heal amount from database
            float healAmount = itemDatabase.GetItem(0).healAmount;
            // Heal player
            playerHealth.RestoreHealth(healAmount);
            Destroy(mainItemGo);
            // check if sub item exists
            SubItemToMain();
        }
        else if (curMainItemId == 2)
        {
            AudioManager.instance.Play("powerup_1");
            float duration = itemDatabase.GetItem(1).effectDuration;
            effectIconController.ActivateEffect(0, duration);
            Destroy(mainItemGo);
            // check if sub item exists
            SubItemToMain();
        }
        else if (curMainItemId == 3)
        {
            AudioManager.instance.Play("powerup_1");
            float duration = itemDatabase.GetItem(2).effectDuration;
            effectIconController.ActivateEffect(1, duration);
            Destroy(mainItemGo);
            // check if sub item exists
            SubItemToMain();
        }
    }
    void SubItemToMain()
    {
        if (curSubItemId == 0)
        {
            curMainItemId = 0;
            return;
        }
        else
        {
            // sub item id to main item id
            curMainItemId = curSubItemId;
            curSubItemId = 0;
            // sub gameobject to main item gameobject
            Destroy(subItemGo);
            mainItemGo = Instantiate(itemPrefabs[curMainItemId], mainItemParent.transform) as GameObject;
            mainItemGo.transform.localPosition = new Vector3(0, 0, 0);
            mainItemGo.transform.SetParent(mainItemParent.transform);
        }
    }
    public void SwitchButton()
    {
        if (curMainItemId == 0 || curSubItemId == 0)
        {
            // do nothin if either items are empty
        }
        else
        {
            // Switch id
            int tempId = curMainItemId;
            curMainItemId = curSubItemId;
            curSubItemId = tempId;
            // Destory current item
            Destroy(mainItemGo);
            Destroy(subItemGo);
            // Instantiate switched items
            mainItemGo = Instantiate(itemPrefabs[curMainItemId], mainItemParent.transform) as GameObject;
            mainItemGo.transform.localPosition = new Vector3(0, 0, 0);
            mainItemGo.transform.SetParent(mainItemParent.transform);
            subItemGo = Instantiate(itemPrefabs[curSubItemId], subItemParent.transform) as GameObject;
            subItemGo.transform.localPosition = new Vector3(0, 0, 0);
            subItemGo.transform.SetParent(subItemParent.transform);
        }
    }
    public bool PickupItem(int newItemId)
    {
        if (curMainItemId == 0)
        {
            // Add to main item
            curMainItemId = newItemId;
            mainItemGo = Instantiate(itemPrefabs[newItemId], mainItemParent.transform) as GameObject;
            mainItemGo.transform.localPosition = new Vector3(0, 0, 0);
            mainItemGo.transform.SetParent(mainItemParent.transform);
            return true;
        }
        else if (curSubItemId == 0)
        {
            // Add to sub item
            curSubItemId = newItemId;
            subItemGo = Instantiate(itemPrefabs[newItemId], subItemParent.transform) as GameObject;
            subItemGo.transform.localPosition = new Vector3(0, 0, 0);
            subItemGo.transform.SetParent(subItemParent.transform);
            return true;
        }
        else
        {
            return false;
        }
    }
}
