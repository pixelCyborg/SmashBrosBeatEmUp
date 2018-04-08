using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapSelector : MonoBehaviour {
	private static List<Transform> TargetQueue;
	private static Transform Target;
	private static Image TargetSprite;
	private static Image Rend;
	private static Sprite OrigSprite;

    public Text titleText;
    private static Text TitleText;
    public Text labelText;
    private static Text LabelText;
    public Text interactionText;
	private static Text InteractionText;


	private static RectTransform Rect;
    private static Image TextBackground;

	private void Start()
	{
		TargetQueue = new List<Transform>();
		Rend = GetComponent<Image>();
        TextBackground = transform.GetChild(0).GetComponent<Image>();
        LabelText = labelText;
		InteractionText = interactionText;
        TitleText = titleText;
		OrigSprite = Rend.sprite;
		Deselect ();
	}

	public static void Select(Transform newTarget, string labels = "", string text = "")
	{
		Rend.enabled = true;
        TextBackground.enabled = true;
		Target = newTarget;
		TargetSprite = Target.GetComponent<Image> ();

        TitleText.text = newTarget.gameObject.name;
        InteractionText.text = text;
        LabelText.text = labels;

        TitleText.color = TargetSprite.color;
        InteractionText.color = TargetSprite.color;
        LabelText.color = TargetSprite.color;
    }

    public static void Deselect()
    {
        Target = null;
        TargetSprite = null;

        TitleText.text = "";
        InteractionText.text = "";
        LabelText.text = "";

        Rend.enabled = false;
        TextBackground.enabled = false;
    }

	private void Update()
	{
		if (Target != null && TargetSprite != null) {

			transform.position = Target.position;
			transform.localScale = Target.localScale;
		}
	}
}
