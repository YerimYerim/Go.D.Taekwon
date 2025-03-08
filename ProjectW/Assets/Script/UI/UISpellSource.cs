using System;
using Script.Manager;
using Script.UI;
using UnityEngine;
using UnityEngine.UI;

public class UISpellSource : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private Image _bg;
    [SerializeField] private DTButton _button;
    [SerializeField] private Transform _isSelected;

    [SerializeField] public RectTransform rect;
    public void SetImage(string imgName, string bgName)
    {
        _image.sprite = ResourceImporter.GetImage(imgName);
        _bg.sprite = ResourceImporter.GetImage(bgName);
        
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
    public void SetSelected(bool isSelected)
    {
        _isSelected.gameObject.SetActive(isSelected);
    }
}
