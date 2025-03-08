using System;
using Script.Manager;
using Script.UI;
using UnityEngine;
using UnityEngine.UI;

public class UIApplication : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private Image _bg;
    [SerializeField] private DTButton _button;
    
    [SerializeField] public RectTransform rect;
    public void SetImage(string imgName) //, string bgName)
    {
        _image.sprite = ResourceImporter.GetImage(imgName);
    }
    
    public void SetOnClickButton(Action action)
    {
        _button.onClick.RemoveAllListeners();
        _button.onClick.AddListener(() => action());
    }
    public void SetHoverEvent(Action OnAction, Action OffAction)
    {
        _button.SetHoverEvent(OnAction, OffAction);
    }
}
