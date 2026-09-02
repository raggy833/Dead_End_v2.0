using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    private Camera cam;
    [SerializeField]
    private float distance = 3f;
    [SerializeField]
    private LayerMask mask;
    private PlayerUI playerUI;
    private Interactable interactable;
    void Start()
    {
        cam = GetComponent<PlayerLook>().cam;
        playerUI = GetComponent<PlayerUI>();
    }

    // Update is called once per frame
    void Update()
    {
        interactable = null;  // TODO change onTrigger or something less heavy
        playerUI.UpdateText(string.Empty);
        playerUI.interactIcon.SetActive(false);
        // Create a ray at the center of the camera, shooting outwards

        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * distance);
        RaycastHit hitInfo; // variable to store our collision information
        if (Physics.Raycast(ray, out hitInfo, distance, mask))
        {
            Debug.Log(hitInfo.collider.gameObject.name);
            if (hitInfo.collider.GetComponent<Interactable>() != null && hitInfo.collider.GetComponent<Interactable>().promptMessage != "")
            {
                interactable = hitInfo.collider.GetComponent<Interactable>();
                playerUI.UpdateText(interactable.promptMessage);
                playerUI.interactIcon.SetActive(true);
            }
        }
    }
    public void TapInteractIcon()
    {
        if (interactable != null)
        {
            interactable.BaseInteract();
        }
    }
}
