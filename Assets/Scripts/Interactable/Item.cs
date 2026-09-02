using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : Interactable
{
    /*
    -- Useable item --
    Item id 0 - null
    Item id 1 - health pot : heal 30 hp
    Item id 2 - 
    
    -- Pouch item --
    Item id 20 - gear
    Item id 21 - 
    */

    //public GameObject getItemEffect;
    private StageControl stageControl;
    private PlayerGunControl playerGunControl;
    private MsgPanel_System msgPanel_System;
    private ItemControl itemControl;
    private PouchItemSpawnControl pouchItemSpawnControl;
    [SerializeField] private bool isGlow;
    [SerializeField] private int itemId;
    [SerializeField] private bool useableItem;

    [SerializeField] private float blackDuration = 1.0f;
    private float yellowDuration;

    [SerializeField] private Color glowColor = Color.yellow;
    [SerializeField] private float transitionDuration = 2.0f;
    [SerializeField] private float glowInterval = 2.0f;

    private Material originalMaterial;
    private Material glowMaterial;
    private bool isGlowing = false;
    private float currentLerpTime = 0f;

    void Start()
    {
        pouchItemSpawnControl = FindObjectOfType<PouchItemSpawnControl>();
        stageControl = FindObjectOfType<StageControl>();
        playerGunControl = FindObjectOfType<PlayerGunControl>();
        msgPanel_System = FindObjectOfType<MsgPanel_System>();
        itemControl = FindObjectOfType<ItemControl>();

        // Get the original material of the game object
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            originalMaterial = renderer.material;
            glowMaterial = new Material(originalMaterial);
        }
        else
        {
            Debug.LogError("Item script requires a Renderer component on the game object!");
            this.enabled = false;
        }
    }

    private void Update()
    {
        if (isGlow)
        {
            Glow();
        }
    }

    private void Glow()
    {
        if (!isGlowing)
        {
            StartCoroutine(GlowRoutine());
        }
    }

    private IEnumerator GlowRoutine()
    {
        isGlowing = true;

        Color startColor = originalMaterial.color;
        Color targetColor = glowColor;
        Color reverseTargetColor = Color.black;

        yellowDuration = transitionDuration - blackDuration;

        while (true)
        {
            // Black to Yellow transition
            currentLerpTime = 0f;
            while (currentLerpTime < yellowDuration)
            {
                currentLerpTime += Time.deltaTime;

                // Calculate the t parameter for the color Lerp
                float t = currentLerpTime / yellowDuration;
                t = Mathf.Clamp01(t);

                // Lerp the color from the start color to the target color
                Color lerpedColor = Color.Lerp(startColor, targetColor, t);

                // Lerp the color from the start color to the reverse target color
                Color reverseLerpedColor = Color.Lerp(startColor, reverseTargetColor, t);

                // Set the glow material color to the lerped color
                glowMaterial.color = lerpedColor + reverseLerpedColor;

                // Apply the glow material to the game object
                Renderer renderer = GetComponent<Renderer>();
                renderer.material = glowMaterial;

                yield return null;
            }

            // Wait for the blackDuration before the next glow
            yield return new WaitForSeconds(blackDuration);

            // Yellow to Black transition
            currentLerpTime = 0f;
            while (currentLerpTime < yellowDuration)
            {
                currentLerpTime += Time.deltaTime;

                // Calculate the t parameter for the color Lerp
                float t = currentLerpTime / yellowDuration;
                t = Mathf.Clamp01(t);

                // Lerp the color from the target color to the reverse target color
                Color lerpedColor = Color.Lerp(targetColor, reverseTargetColor, t);

                // Lerp the color from the reverse target color to the start color
                Color reverseLerpedColor = Color.Lerp(reverseTargetColor, startColor, t);

                // Set the glow material color to the lerped color
                glowMaterial.color = lerpedColor + reverseLerpedColor;

                // Apply the glow material to the game object
                Renderer renderer = GetComponent<Renderer>();
                renderer.material = glowMaterial;

                yield return null;
            }
        }
    }

    protected override void Interact()
    {
        if (this.useableItem)
        {
            ToUseableItemBag();
        }
        else
        {
            ToPouchBag();
        }
    }
    private void ToUseableItemBag()
    {
        // If item is full
        if (!itemControl.PickupItem(itemId))
        {
            msgPanel_System.OutputMsg("Item is full");
        }
        // If item is not full
        else
        {

            AudioManager.instance.Play("get_item");
            // Check total items 
            //int totalItem = stageControl.FindItem();
            // Use returned value for prompt message
            msgPanel_System.OutputMsg("Aquired item");
            //Instantiate(getItemEffect, this.transform.position, this.transform.rotation);
            Destroy(gameObject);
        }
    }
    private void ToPouchBag()
    {
        if (itemId == 20)
        {
            int randomIncrease = Random.Range(5, 21); // Random value between 5 and 20 (inclusive)
            stageControl.gears += randomIncrease;
            msgPanel_System.OutputMsg("Picked up " + randomIncrease + " gears");
            pouchItemSpawnControl.RespawnGear(this.transform);
            Destroy(gameObject);
        }
    }
}
