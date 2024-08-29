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
        txtTitle.SetText(GameUtil.GetString(titleStringKey));
        txtDesc.SetText(GameUtil.GetString(descStringKey));

        // 화면 중앙 좌표
        Vector2 screenCenter = new(Screen.width * 0.5f , Screen.height * 0.5f);

        // 아이콘의 스크린 좌표
        //Vector2 parentsScreenPoint = RectTransformUtility.WorldToScreenPoint(Camera.current, parentsRect.position);

        // 아이콘이 화면 중앙보다 오른쪽에 있는지 확인
        var parentsRectPosition = parentsRect.position;
        bool isParentsOnRight = parentsRectPosition.x > screenCenter.x;
        Vector3[] parentsCorners = new Vector3[4];
        parentsRect.GetWorldCorners(parentsCorners);
        var parentsRectWidth = (parentsCorners[2].x - parentsCorners[1].x) * 0.5f;
        var parentsRectHeight = (parentsCorners[1].y - parentsCorners[0].y) * 0.5f;
        
        Vector3[] tooltipCorners = new Vector3[4];
        toolTipRectTransform.GetWorldCorners(tooltipCorners);
        var tooltipRectWidth = (tooltipCorners[2].x - tooltipCorners[1].x) * 0.5f;
        var tooltipRectHeight =(tooltipCorners[1].y - tooltipCorners[0].y) * 0.5f;

        // 툴팁의 위치 설정
        if (isParentsOnRight)
        {
            Vector2 resultPosition = new Vector2(parentsRectPosition.x - parentsRectWidth - tooltipRectWidth, parentsRectPosition.y - (tooltipRectHeight - parentsRectHeight));
            toolTipRectTransform.position =resultPosition;
        }
        else
        {
            Vector2 resultPosition = new Vector2(parentsRectPosition.x + parentsRectWidth + tooltipRectWidth,parentsRectPosition.y - (tooltipRectHeight - parentsRectHeight));
            toolTipRectTransform.position = resultPosition;
        }

    }
    
    public override void Show()
    {
        base.Show();
        OnShow();
    }
}
