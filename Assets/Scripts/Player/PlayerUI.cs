using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI promptText;

    public GameObject interactIcon;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void UpdateText(string promptMessage)
    {
        promptText.text = promptMessage;
    }
}
