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

    public DialogAudioHandler audioHandler;
    public float textWriteDelay = 0.1f;

	// Use this for initialization
	void Start () {
        audioHandler.Initialize(GetComponent<AudioSource>());
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
        bubblePos = (newLog.transform.position + Vector3.up * newLog.rend.bounds.extents.y * 1.5f);
        group.alpha = 1;
        group.interactable = true;
        group.blocksRaycasts = true;
        StartCoroutine(WriteDialog(newLog));
    }

    IEnumerator WriteDialog(Dialog newLog)
    {
        dialogText.text = "";
        while(dialogText.text != newLog.dialogString)
        {
            audioHandler.PlayDialog();
            dialogText.text = newLog.dialogString.Substring(0, dialogText.text.Length + 1);
            yield return new WaitForSeconds(textWriteDelay);
        }
        StartCoroutine(DialogTimeout(newLog.dialogTime));
    }

    IEnumerator DialogTimeout(float dialogTime)
    {
        yield return new WaitForSeconds(dialogTime);
        HideDialog();
    }

    public void HideDialog()
    {
        if (group.alpha == 0) return;
        audioHandler.PlayCloseDialog();
        group.alpha = 0;
        group.interactable = false;
        group.blocksRaycasts = false;
        bubblePos = Vector3.zero;
    }
}
