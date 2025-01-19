using System;
using System.Collections.Generic;
using Script.Manager;
using UnityEngine;
using UnityEngine.UI;

public abstract class UIAPGaugeIconBase : MonoBehaviour
{
    [SerializeField] protected Image _image;
    protected bool IsShow = true;
    private readonly List<LTDescr> _tweens = new();
    private readonly List<LTDescr> _tweensToRemove = new();
    
    public abstract int GetRemainSpellAP();
    public abstract int GetResetAp();

    protected void SetImage(string image)
    {
        _image.sprite = GameResourceManager.Instance.GetImage(image);
    }
    public void SetPosition(Vector3 position)
    {
        transform.gameObject.SetActive(GetRemainSpellAP() <= 5 && IsShow);
        transform.SetPositionAndRotation(position, Quaternion.identity);
    }

    public void AddPositionSmooth(Vector3 position)
    {
        transform.gameObject.SetActive(GetRemainSpellAP() <= 5 && IsShow);
        _tweens.Add(LeanTween.move(gameObject, position, 0.1f).pause());
    }

    private void Update()
    {
        if(_tweens.Count > 0)
        {
            foreach (var tween in _tweens)
            {
                if (LeanTween.isTweening(tween.id) == false)
                {
                    _tweensToRemove.Add(tween);
                }
            }
            foreach (var tween in _tweensToRemove)
            {
                _tweens.Remove(tween);
            }
            
            if(_tweens.Count > 0)
            {
                LeanTween.resume(_tweens[0].id);
            }
            _tweensToRemove.Clear();
        }
    }
}
