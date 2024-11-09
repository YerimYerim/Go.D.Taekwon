using System;
using System.Collections;
using System.Collections.Generic;
using Script.Manager;
using Script.UI;
using UnityEngine;
using UnityEngine.UI;

public class UISupportModule : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private Image bg;
    [SerializeField] private DTButton _button;
    [SerializeField] public RectTransform rect;
    
    public void SetImage(string iconName)
    {
        icon.sprite = GameResourceManager.Instance.GetImage(iconName);
        bg.sprite = GameResourceManager.Instance.GetImage("ui_bg_supportmodule");
    }

    public void SetHoverEvent(Action OnAction, Action OffAction)
    {
        _button.SetHoverEvent(OnAction, OffAction);
    }
}
