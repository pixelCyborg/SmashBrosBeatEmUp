using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour {
    public static DialogManager instance;
    public RectTransform speechBubble;
    public Text dialogText;
    private CanvasGroup group;
    private Vector3 bubblePos;

	// Use this for initialization
	void Start () {
        instance = this;
        group = GetComponent<CanvasGroup>();
        HideDialog();
	}

    private void Update()
    {
        if(bubblePos != Vector3.zero)
        {
            transform.position = bubblePos;
        }
    }

    public void SetDialog(Dialog newLog)
    {
        dialogText.text = newLog.dialogString;
        bubblePos = (newLog.transform.position + Vector3.up * newLog.rend.bounds.extents.y * 1.5f);
        group.alpha = 1;
        group.interactable = true;
        group.blocksRaycasts = true;
        StartCoroutine(DialogTimeout(newLog.dialogTime));
    }

    IEnumerator DialogTimeout(float dialogTime)
    {
        yield return new WaitForSeconds(dialogTime);
        HideDialog();
    }

    public void HideDialog()
    {
        group.alpha = 0;
        group.interactable = false;
        group.blocksRaycasts = false;
        bubblePos = Vector3.zero;
    }
}
