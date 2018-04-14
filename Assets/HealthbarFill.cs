using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthbarFill : MonoBehaviour {
    public RectTransform lifeRect;
    private int currentHealth;
    private int maxHealth;

    private float maxWidth;
    RectTransform rect;

    public void Initialize(int _maxHealth)
    {
        maxHealth = _maxHealth;
        rect = GetComponent<RectTransform>();
        maxWidth = lifeRect.sizeDelta.x;
    }

    public void SetHealth(int _currentHealth)
    {
        currentHealth = _currentHealth;
        lifeRect.sizeDelta = new Vector2((maxWidth * currentHealth) / maxHealth, lifeRect.sizeDelta.y);
    }
}
