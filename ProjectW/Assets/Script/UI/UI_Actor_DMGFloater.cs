using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_Actor_DMGFloater : UIBase
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private RectTransform moveRectTransform;
    private Vector3 startPostion;
    private Vector3 targetPosition;

    private void Awake()
    {
        ResetValue();
    }

    public void ShowDamage(int damage)
    {
        ResetValue();
        Show();
        SetStringFloater(damage);
        MoveStart();
    }
    public void SetPosition(Transform gameTransform)
    {
        if (Camera.main != null)
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(gameTransform.position);
            startPostion = screenPos + Vector3.down*5;
            targetPosition = screenPos;
            moveRectTransform.transform.position = screenPos;
        }
    }
    private void ResetValue()
    {
        text.text = "";
        canvasGroup.alpha = 0;
        moveRectTransform.transform.position = startPostion;
    }

    private void MoveStart()
    {
        LeanTween.alphaCanvas(canvasGroup, 1, 0.1f).setOnComplete( ()=>
        {
            LeanTween.move(moveRectTransform.transform.gameObject, targetPosition, 0.5f).setOnComplete(() =>
            {
                LeanTween.alphaCanvas(canvasGroup, 0, 0.2f).setOnComplete(ResetValue);
            });
        });
    }

    private void SetStringFloater(int damage)
    {
        if(damage > 0)
        {
            text.color = Color.green;
            text.text = "+" + damage;
        }
        else if(damage < 0)
        {
            text.color = Color.red;
            text.text = damage.ToString();
        }
        else
        {
            text.text = "";
        }
    }
}
