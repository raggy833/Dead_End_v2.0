using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ShootBtn : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private bool btnDown;
    private PlayerGunControl playerGunControl;
    private void Start()
    {
        playerGunControl = FindObjectOfType<PlayerGunControl>();
    }
    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        btnDown = true;
    }

    void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
    {
        btnDown = false;
    }
    private void Update()
    {
        if (btnDown)
        {
            playerGunControl.Shoot();
        }
    }
}
