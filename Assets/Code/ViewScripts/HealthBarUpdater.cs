using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarUpdater : MonoBehaviour
{
    [SerializeField] private RectTransform _healthBarFill;
    [SerializeField] private RectTransform _healthBarParent;

    public void UpdateHealthBarFill(float percentToFill)
    {
        _healthBarFill.sizeDelta = new Vector2(_healthBarParent.rect.width * percentToFill, _healthBarFill.rect.height);
    }
}
