using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HoverBox : MonoBehaviour {
    public static HoverBox instance;
    RectTransform rect;
    bool shown;
    Canvas myCanvas;
    CanvasGroup group;
    
    public Text itemName;
    public Text itemDescription;

    void Start()
    {
        rect = GetComponent<RectTransform>();
        instance = this;
        myCanvas = GetComponentInParent<Canvas>();
        group = GetComponent<CanvasGroup>();
        group.alpha = 0;
        group.interactable = false;
        group.blocksRaycasts = true;
    }

    void Update()
    {
        if(group.alpha == 1)
        {
            Vector2 pos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(myCanvas.transform as RectTransform, Input.mousePosition, myCanvas.worldCamera, out pos);
            transform.position = myCanvas.transform.TransformPoint(pos) - new Vector3(rect.rect.width / 2, -rect.rect.height / 2, 0) * myCanvas.transform.localScale.x;
        }
    }
	
    public void ShowDescription(Item item)
    {
        itemName.text = item.name;
        itemDescription.text = item.description;

        group.alpha = 1;
    }

    public void HideDescription()
    {
        group.alpha = 0;
    }
}
