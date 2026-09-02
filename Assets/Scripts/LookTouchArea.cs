using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookTouchArea : MonoBehaviour
{
    public GameObject LookJoystick;
    private Vector2 touchPos;
    private Camera mainCam;
    private PlayerLook playerLook;

    void Start()
    {
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        playerLook = GetComponent<PlayerLook>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            touchPos = mainCam.ScreenToViewportPoint(Input.mousePosition);
            // Touch right area
            if (touchPos.x > 0.5)
            {
                // Debug.Log("Touch right");
            }
            else
            {
                // Debug.Log("Touch left");
            }
        }
    }
}
