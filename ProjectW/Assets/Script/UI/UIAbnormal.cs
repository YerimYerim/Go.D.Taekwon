using Script.Manager;
using Script.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIAbnormal : MonoBehaviour
{
    [SerializeField] private Image BG;
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private DTButton button;
    
    public void SetImage(string imageName)
    {
        image.sprite = ResourceImporter.GetImage(imageName);
    }

    public void SetText(string str)
    {
        text.SetText(str);
    }
    
    public void SetBgColor(string str)
    {
        BG.sprite = ResourceImporter.GetImage(str);
    }
    
    public void SetData(AbnormalTableData data)
    {
        button.SetHoverEvent(
            () =>
            {
                if (GameUIManager.Instance.TryGetOrCreate<UITooltip>(true, UILayer.LEVEL_4, out var ui))
                {
                    ui.CreateInfo(data?.abnormal_name?? "알수 없음", data?.abnormal_desc ?? "알수없음", rectTransform);
                    ui.Show();
                }
            } , 
            () =>
            {
                if (GameUIManager.Instance.TryGetOrCreate<UITooltip>(true, UILayer.LEVEL_4, out var ui))
                {
                    ui.Hide();
                }
            });
    }
    
}
