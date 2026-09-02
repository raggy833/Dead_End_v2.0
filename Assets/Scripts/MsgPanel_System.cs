using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MsgPanel_System : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private MsgPanelControl msgPanelPrefab = null;
    [SerializeField] private RectTransform holder = null;

    [SerializeField] private MsgPanelControl topMsgPanelPrefab = null;

    public void OutputMsg(string msg)
    {
        MsgPanelControl msgPanelClone = Instantiate(msgPanelPrefab, holder);
        msgPanelClone.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = msg;
    }
    public void OutputTopMsg(string msg)
    {
        MsgPanelControl msgPanelClone = Instantiate(topMsgPanelPrefab, holder);
        msgPanelClone.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = msg;
    }
}
