using System;
using System.Collections;
using System.Collections.Generic;
using Script.Manager;
using Script.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIPopUpMapSelectObject : MonoBehaviour
{
    [SerializeField] private DTButton button;
    [SerializeField] private Image BG;
    [SerializeField] private Image mainImage;
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private TextMeshProUGUI desc;
    
    public void SetData(Sprite mainImage, string title, string desc, MAP_TYPE type)
    {
        this.mainImage.sprite = mainImage;
        this.title.text = GameUtil.GetString(title);
        this.desc.text = GameUtil.GetString(desc);
        SetBGColor(type);
    }
    
    public void SetButtonEvent(System.Action action)
    {
        button.onClick.AddListener(() => action());
    }
    
    public void SetBGColor(MAP_TYPE type)
    {
        BG.sprite = type switch
        {
            MAP_TYPE.MAP_TYPE_BATTLE_NORMAL => GameResourceManager.Instance.GetImage("map_select_bg_battle_normal"),
            MAP_TYPE.MAP_TYPE_BATTLE_ELITE => GameResourceManager.Instance.GetImage("map_select_bg_battle_elite"),
            MAP_TYPE.MAP_TYPE_BATTLE_BOSS => GameResourceManager.Instance.GetImage("map_select_bg_battle_boss"),
            _ => GameResourceManager.Instance.GetImage("map_select_bg_battle_normal")
        };
    }
    
    
}
