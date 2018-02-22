using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapSelector : MonoBehaviour {
	private static List<Transform> targetQueue;
	private static Transform target;
	private static Image targetSprite;
	private static Image rend;
	private static Sprite origSprite;
	private static Text interactionText;
	private static RectTransform rect;

	private void Start()
	{
		targetQueue = new List<Transform>();
		rend = GetComponent<Image>();
		interactionText = GetComponentInChildren<Text>();
			origSprite = rend.sprite;
		Deselect ();
	}

	public static void Select(Transform newTarget, string text = "")
	{
		rend.enabled = true;
		target = newTarget;
		targetSprite = target.GetComponent<Image> ();

		if (text != "") {
			interactionText.text = text;
//			interactionText.rectTransform.localPosition = Vector3.up * rend.sprite.bounds.extents.y;

		}
	}

		public static void Deselect()
		{
			target = null;
			targetSprite = null;
//			rend.sprite = origSprite;
			interactionText.text = "";
		rend.enabled = false;
		}

	private void Update()
	{
		if (target != null && targetSprite != null) {
	//		rend.sprite = targetSprite.sprite;
			transform.position = target.position;
			transform.localScale = target.localScale;
		}
	}
}
