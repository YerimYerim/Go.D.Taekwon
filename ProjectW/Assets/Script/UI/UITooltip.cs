using System;
using System.Collections;
using System.Collections.Generic;
using Script.UI;
using TMPro;
using UnityEngine;

public class UITooltip : UIBase
{
    [SerializeField] private TextMeshProUGUI txtTitle;
    [SerializeField] private TextMeshProUGUI txtDesc;
    [SerializeField] private RectTransform toolTipRectTransform;
    [SerializeField] private DTButton Dim;


    private void Awake()
    {
        Dim.onClick.AddListener(Hide);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="titleStringKey"></param>
    /// <param name="descStringKey"></param>
    /// <param name="parentsRect"></param>
    public void CreateInfo(string titleStringKey, string descStringKey, RectTransform parentsRect)
    {
        txtTitle.text = GameUtil.GetString(titleStringKey);
        txtDesc.text = GameUtil.GetString(descStringKey);

        //var tooltipPosition = toolTipRectTransform.anchoredPosition3D;
        var parentsRectPosition = parentsRect.position;

        float x = 0f;
        float y = 0f;
        
        
        // 해당 아이템이 화면의 왼쪽에 있을경우 오른쪽에

        x = parentsRectPosition.x + Math.Abs(parentsRect.rect.x * 0.5f) + Math.Abs(toolTipRectTransform.rect.x * 0.5f);
        

        // 해당 아이템이 화면하단에 있을경우 위쪽에 
        if (parentsRect.position.y < 0)
        {
            y = parentsRectPosition.y;// + Math.Abs(parentsRect.rect.y * 0.5f) + Math.Abs(toolTipRectTransform.rect.y * 0.5f);
        }
        else
        {
            y = parentsRectPosition.y; //  - Screen.height * 0.5f;
        }

        toolTipRectTransform.position = new Vector3(parentsRectPosition.x +  Math.Abs(toolTipRectTransform.rect.x * 0.5f), y - Math.Abs(toolTipRectTransform.rect.y * 0.5f) +  Math.Abs(parentsRect.rect.y * 0.5f) + 10 );
    }
    
    public override void Show()
    {
        base.Show();
        OnShow();
    }
}
