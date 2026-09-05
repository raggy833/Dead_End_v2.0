using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class PlayerLook : MonoBehaviour
{
    public Camera cam;
    public Transform playerRoot;
    private float xRotation = 0f;
    public float xSensitivity = 10f;
    public float ySensitivity = 10f;
    public float minClamp = -200f;
    public float maxClamp = 200f;

    private float mouseX;
    private float mouseY;

    private Touch initTouch = new Touch();
    private int fingerCount = 0;
    private Vector2 touchPos;
    private Vector3 lastRotation;

    public void ProcessLook(Vector2 input)
    {
        // float mouseX = input.x;
        // float mouseY = input.y;
        // // Calculate camera roatation for looking up and down
        // xRotation -= (mouseY * Time.deltaTime) * ySensitivity;
        // xRotation = Mathf.Clamp(xRotation, minClamp, maxClamp);
        // // Apply this to our camera transform
        // cam.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        // // Rotate player to look left and right
        // transform.Rotate(Vector3.up * (mouseX * Time.deltaTime) * xSensitivity);

        // ANDROID TOUCH SYSTEM
        // Reset values when there is no touch
        mouseX = 0f;
        mouseY = 0f;

        // ANDROID TOUCH SYSTEM
        foreach (Touch touch in Input.touches)
        {
            if (touch.position.x > (Screen.width / 2))
            {
                mouseX = touch.deltaPosition.x;
                mouseY = touch.deltaPosition.y;
            }
        }

        // Calculate camera rotation for looking up and down
        xRotation -= mouseY * Time.deltaTime * ySensitivity;
        xRotation = Mathf.Clamp(xRotation, minClamp, maxClamp);

        // Apply this to our camera transform
        cam.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);

        // Rotate player to look left and right
        playerRoot.Rotate(Vector3.up * (mouseX * Time.deltaTime) * xSensitivity);
    }


    public void Update()
    {

        // Use the last known rotation when there is no touch
        // cam.transform.localRotation = Quaternion.Euler(lastRotation.x, 0, 0);
        // transform.rotation = Quaternion.Euler(0, lastRotation.y, 0);
    }

    //     if (Application.isMobilePlatform)
    //     {
    //         // ANDROID TOUCH SYSTEM
    //         foreach (Touch touch in Input.touches)
    //         {
    //             if (touch.position.x > (Screen.width / 2))
    //             {
    //                 mouseX = touch.deltaPosition.x;
    //                 mouseY = touch.deltaPosition.y;
    //             }
    //         }
    //     }
    //     else
    //     {
    //         // PC MOUSE INPUT
    //         mouseX = Input.GetAxis("Mouse X");
    //         mouseY = Input.GetAxis("Mouse Y");
    //     }

    //     Debug.Log("Mouse X: " + mouseX);
    //     Debug.Log("Mouse Y: " + mouseY);

    //     // Calculate camera rotation for looking up and down
    //     xRotation -= mouseY * Time.deltaTime * ySensitivity;
    //     xRotation = Mathf.Clamp(xRotation, minClamp, maxClamp);

    //     // Apply this to our camera transform
    //     cam.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);

    //     // Rotate player to look left and right
    //     transform.Rotate(Vector3.up * (mouseX * Time.deltaTime) * xSensitivity);
    // }

}
