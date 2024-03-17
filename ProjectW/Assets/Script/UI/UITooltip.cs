using System;
using System.Collections;
using System.Collections.Generic;
using Script.UI;
using TMPro;
using Unity.VisualScripting;
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

        // 화면 중앙 좌표
        Vector2 screenCenter = new(Screen.width * 0.5f , Screen.height * 0.5f);

        // 아이콘의 스크린 좌표
        Vector2 iconScreenPosition = RectTransformUtility.WorldToScreenPoint(Camera.current, parentsRect.position);

        // 아이콘이 화면 중앙보다 오른쪽에 있는지 확인
        bool isIconOnRight = iconScreenPosition.x > screenCenter.x;
        Vector3[] parentscorners = new Vector3[4];
        parentsRect.GetWorldCorners(parentscorners);
        var parentsRectWidth = (parentscorners[2].x - parentscorners[1].x) * 0.5f;
        var parentsRectHeight = (parentscorners[1].y - parentscorners[0].y) * 0.5f;
        
        Vector3[] tooltipcorners = new Vector3[4];
        toolTipRectTransform.GetWorldCorners(tooltipcorners);
        var tooltipRectWidth = (tooltipcorners[2].x - tooltipcorners[1].x) * 0.5f;
        var tooltipRectHeight =(tooltipcorners[1].y - tooltipcorners[0].y) * 0.5f;
        
        // 툴팁의 위치 설정
        if (isIconOnRight)
        {
            Vector2 curScreenPosition = new Vector2(iconScreenPosition.x - parentsRectWidth - tooltipRectWidth, iconScreenPosition.y - (tooltipRectHeight - parentsRectHeight));
            toolTipRectTransform.position =curScreenPosition;
        }
        else
        {
            Vector2 curScreenPosition = new Vector2(iconScreenPosition.x + parentsRectWidth + tooltipRectWidth,iconScreenPosition.y - (tooltipRectHeight - parentsRectHeight));
            toolTipRectTransform.position = curScreenPosition;
        }

    }
    
    public override void Show()
    {
        base.Show();
        OnShow();
    }
}
