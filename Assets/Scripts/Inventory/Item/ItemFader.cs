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
    /// �𽥻ָ���ɫ
    /// </summary>
    public void FadeIn()
    {
        Color targetColor = new Color(1, 1, 1, 1);
        spriteRenderer.DOColor(targetColor, Settings.itemfadeDuration);
    }
    /// <summary>
    /// �𽥰�͸��
    /// </summary>
    public void FadeOut()
    {
        Color targetcolor = new Color(1, 1,1,Settings.targetAlpha);
        spriteRenderer.DOColor(targetcolor, Settings.itemfadeDuration);
    }
     
}
