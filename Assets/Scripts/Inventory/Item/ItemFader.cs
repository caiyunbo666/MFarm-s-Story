using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class ItemFader : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
       spriteRenderer = GetComponent<SpriteRenderer>();
    }
    /// <summary>
    /// Öð½¥»Ö¸´ÑÕÉ«
    /// </summary>
    public void FadeIn()
    {
        Color targetColor = new Color(1, 1, 1, 1);
        spriteRenderer.DOColor(targetColor, Settings.itemfadeDuration);
    }
    /// <summary>
    /// Öð½¥°ëÍ¸Ã÷
    /// </summary>
    public void FadeOut()
    {
        Color targetcolor = new Color(1, 1,1,Settings.targetAlpha);
        spriteRenderer.DOColor(targetcolor, Settings.itemfadeDuration);
    }
     
}
