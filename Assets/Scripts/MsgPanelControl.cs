using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MsgPanelControl : MonoBehaviour
{
    private const float maxTimer = 2.0f;
    private float timer = maxTimer;

    private TextMeshProUGUI msg;
    private CanvasGroup canvasGroup = null;
    protected CanvasGroup CanvasGroup
    {
        get
        {
            if (canvasGroup == null)
            {
                canvasGroup = GetComponent<CanvasGroup>();
                if (canvasGroup == null)
                {
                    canvasGroup = gameObject.AddComponent<CanvasGroup>();
                }
            }
            return canvasGroup;
        }
    }

    private void OnEnable()
    {
        Debug.Log("MsgPanelEnabled");
        msg = this.GetComponentInChildren<TextMeshProUGUI>();
        StartCoroutine(Countdown());
    }
    private void OnDisable()
    {

    }

    private IEnumerator Countdown()
    {
        while (CanvasGroup.alpha < 1.0f)
        {
            CanvasGroup.alpha += 4 * Time.deltaTime;
            yield return null;
        }
        while (timer > 0)
        {
            timer--;
            yield return new WaitForSeconds(1);
        }
        while (canvasGroup.alpha > 0.0f)
        {
            CanvasGroup.alpha -= 2 * Time.deltaTime;
            yield return null;
        }
        Destroy(gameObject);
    }
}
